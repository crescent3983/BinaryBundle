#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace BinaryBundle {
	partial class BinaryBundleSerializer {

		public const string ENCODE_STATIC_METHOD_NAME = "EncodeToBinaryBundle";
		public const string DECODE_STATIC_METHOD_NAME = "DecodeFromBinaryBundle";

		public const string CUSTOM_SERIALIZE_METHOD_NAME = "Serialize";
		public const string CUSTOM_DESERIALIZE_METHOD_NAME = "Deserialize";

		public const string AFTER_DESERIALIZE_METHOD_NAME = "OnAfterDeserialize";
		public const string BEFORE_SERIALIZE_METHOD_NAME = "OnBeforeSerialize";

		private const string VERSION_NAME = "__version";

		private static readonly Dictionary<Type, (string encode, string decode)> SUPPORTED_TYPES = new Dictionary<Type, (string encode, string decode)>() {
			[typeof(bool)]		= ("EncodeBool", "DecodeBool"),
			[typeof(byte)]		= ("EncodeByte", "DecodeByte"),
			[typeof(sbyte)]		= ("EncodeSByte", "DecodeSBytel"),
			[typeof(short)]		= ("EncodeShort", "DecodeShort"),
			[typeof(ushort)]	= ("EncodeUShort", "DecodeUShort"),
			[typeof(int)]		= ("EncodeInt", "DecodeInt"),
			[typeof(uint)]		= ("EncodeUInt", "DecodeUInt"),
			[typeof(long)]		= ("EncodeLong", "DecodeLong"),
			[typeof(ulong)]		= ("EncodeULong", "DecodeULong"),
			[typeof(float)]		= ("EncodeFloat", "DecodeFloat"),
			[typeof(double)]	= ("EncodeDouble", "DecodeDouble"),
			[typeof(string)]	= ("EncodeString", "DecodeString"),
		};

		private static string[] NAMESPACES = new string[] {
			"System",
			"System.Collections.Generic",
			"BinaryBundle",
			"BinaryBundle.Internal",
		};

		private static readonly Dictionary<Type, Type> BUILTIN_SERIALIZER = new Dictionary<Type, Type>() {
			[typeof(Vector2Int)] = typeof(Vector2IntSerializer),
			[typeof(Vector2)] = typeof(Vector2Serializer),
		};

		private struct BinaryBundleFieldInfo {
			public int Order { get; private set; }
			public uint Min { get; private set; }
			public uint Max { get; private set; }
			public string Name => this._member.Name;
			public Type Type {
				get {
					if (this._member is FieldInfo) return ((FieldInfo)this._member).FieldType;
					else return ((PropertyInfo)this._member).PropertyType;
				}
			}

			private MemberInfo _member;

			public BinaryBundleFieldInfo(int order, uint min, uint max, FieldInfo field) {
				this.Order = order;
				this.Min = min;
				this.Max = max;
				this._member = field;
			}

			public BinaryBundleFieldInfo(int order, uint min, uint max, PropertyInfo prop) {
				this.Order = order;
				this.Min = min;
				this.Max = max;
				this._member = prop;
			}
		}

		private class TmpVarState {
			public int idx;
			public int max;
		}
		private static Dictionary<Type, TmpVarState> _tmpVariables;

		public static void GenerateCode(string path, Type[] types) {
			var sb = new StringBuilder();

			var filteredTypes = new List<Type>();
			for (int i = 0; i < types.Length; i++) {
				Type type = types[i];
				if (type.HasAttribute<BinaryBundleObjectAttribute>()) {
					filteredTypes.Add(type);
				}
			}

			for (int i = 0; i < filteredTypes.Count; i++) {
				sb.AppendLine("#define USE_" + filteredTypes[i].Name.ToDefineNamingConvention());
			}

			sb.AppendLine();
			for (int i = 0; i < NAMESPACES.Length; i++) {
				sb.AppendLine("using " + NAMESPACES [i] + ";");
			}

			for (int i = 0; i < filteredTypes.Count; i++) {
				sb.AppendLine();
				var t = filteredTypes[i];
				var err = GenerateOneType(t, sb, "\t");
				if (!string.IsNullOrEmpty(err)) {
					EditorUtility.DisplayDialog("BinaryBundleGenerator", $"Generate {t.Name} failed\n{err}", "OK");
				}
			}

			var dir = Path.GetDirectoryName(path);
			if (!Directory.Exists(dir)) {
				Directory.CreateDirectory(dir);
			}
			File.WriteAllText(path, sb.ToString());

			Debug.Log("BinaryBundle code generation finished: " + path);
		}

		private static string GenerateOneType(Type type, StringBuilder psb, string indent) {
			if (type.IsGenericType) {
				return "Not support generic type";
			}

			/* ----- check custom serializers ----- */
			BindingFlags flags = BindingFlags.Static | BindingFlags.Public;

			var serializers = new Dictionary<Type, Type>(BUILTIN_SERIALIZER);
			var customSerializers = type.GetCustomAttributes(typeof(BinaryBundleCustomSerializerAttribute), false);
			for(int i = 0; i < customSerializers.Length; i++) {
				var attr = (BinaryBundleCustomSerializerAttribute)customSerializers[i];
				var target = attr.Target;
				var serializer = attr.Serializer;
				// check serialize method
				if (!serializer.HasMethod(CUSTOM_SERIALIZE_METHOD_NAME, flags, new Type[] { attr.Target, typeof(List<byte>) })) {
					return $"Custom serializer {serializer} has no valid static method {CUSTOM_SERIALIZE_METHOD_NAME}";
				}
				// check deserialize method
				if (!serializer.HasMethod(CUSTOM_DESERIALIZE_METHOD_NAME, flags, new Type[] { typeof(byte[]), typeof(int).MakeByRefType() }, attr.Target)) {
					return $"Custom serializer {serializer} has no valid static method {CUSTOM_DESERIALIZE_METHOD_NAME}";
				}
				serializers[target] = serializer;
			}

			var sb = new StringBuilder();

			/* ----- get serializable fields ----- */
			flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

			var fields = new List<BinaryBundleFieldInfo>();
			var excludedFields = new List<MemberInfo>();
			foreach (var field in type.GetFields(flags)) {
				var attr = field.GetCustomAttribute<BinaryBundleFieldAttribute>(false);
				if (attr != null) {
					fields.Add(new BinaryBundleFieldInfo(attr.Order, attr.Min, attr.Max, field));
				}
				else {
					excludedFields.Add(field);
				}
			}
			foreach (var prop in type.GetProperties(flags)) {
				var attr = prop.GetCustomAttribute<BinaryBundleFieldAttribute>(false);
				if (attr != null) {
					if (!prop.CanRead || !prop.CanWrite) {
						return $"{type.Name}.{prop.Name} is a property but doesn't have both getter and setter";
					}
					fields.Add(new BinaryBundleFieldInfo(attr.Order, attr.Min, attr.Max, prop));
				}
				else {
					excludedFields.Add(prop);
				}
			}
			fields.Sort((x, y) => x.Order.CompareTo(y.Order));

			var hasInherited = type.BaseType.HasAttribute<BinaryBundleObjectAttribute>();
			var methodType = hasInherited ? "override" : type.IsSealed ? string.Empty : "virtual";
			var hasCallback = type.GetInterfaces().Contains(typeof(IBinaryBundleSerializationCallback));

			var objAttr = type.GetCustomAttribute<BinaryBundleObjectAttribute>(false);
			var version = objAttr?.Version ?? 0;
			var minimum = objAttr?.Minimum ?? 0;

			/* ----- start namespace & type ----- */
			sb.AppendLine($"namespace {type.Namespace} {{");
			if (type.DeclaringType != null) {
				sb.Append(indent).Append($"partial {(type.DeclaringType.IsClass ? "class" : "struct")} {type.DeclaringType.Name} : IBinaryBundleSerializable {{").AppendLine();
			}
			sb.Append(indent).Append($"partial {(type.IsClass ? "class" : "struct")} {type.Name} : IBinaryBundleSerializable {{").AppendLine();

			/* ----- add deserialization ----- */
			if (!type.IsClass) sb.AppendLine("#if USE_" + type.Name.ToDefineNamingConvention());
			sb.Append(indent).Append($"\tpublic {type.Name}(byte[] bytes, ref int offset) {(hasInherited ? ": base(bytes, ref offset)" : string.Empty)} {{").AppendLine();
			if (type.IsClass) sb.AppendLine("#if USE_" + type.Name.ToDefineNamingConvention());

			// decode version
			if (version > 0) {
				sb.AppendLine(indent + "\t\t// " + VERSION_NAME);
				sb.Append(indent).AppendLine($"\t\tushort {VERSION_NAME} = 0;");
				sb.Append(indent).AppendLine($"\t\tif(!BinaryBundleInternal.DecodeUShort(bytes, ref offset, ref {VERSION_NAME})) throw new DecodeBinaryBundleException(\"{VERSION_NAME}\", offset);");

				sb.Append(indent).AppendLine($"\t\tif({VERSION_NAME} < {minimum} || {VERSION_NAME} > {version}) throw new VersionNotMatchException({VERSION_NAME}, {minimum}, {version});");
			}

			ClearTmpVariable();
			for (int i = 0; i < fields.Count; i++) {
				var f = fields[i];
				GenerateOneDecodeType(serializers, 1, f.Type, f.Name, string.Empty, indent);
			}
			DeclareTmpVariable(sb, indent + "\t\t");

			for (int i = 0; i < fields.Count; i++) {
				var f = fields[i];
				(var decodeStr, var decodeErr) = GenerateOneDecodeType(serializers, 1, f.Type, f.Name, $"{f.Name} = {{0}};", indent + (version > 0 ? "\t\t\t" : "\t\t"));
				if (string.IsNullOrEmpty(decodeStr)) {
					return $"Generate decode {type.Name}.{f.Name} failed\n{decodeErr}";
				}
				else {
					sb.AppendLine(indent + "\t\t// " + f.Name);
					if (version > 0) {
						string condition = string.Empty;
						if (f.Min > UInt16.MinValue) {
							condition += $"{VERSION_NAME} >= {f.Min}";
						}
						if (f.Max < UInt16.MaxValue) {
							if (!string.IsNullOrEmpty(condition)) {
								condition += " && ";
							}
							condition += $"{VERSION_NAME} <= {f.Max}";
						}
						if (string.IsNullOrEmpty(condition)) {
							sb.Append(decodeStr);
						}
						else {
							sb.Append(indent).AppendLine($"\t\tif({condition}){{");
							sb.Append(decodeStr);
							sb.Append(indent).AppendLine($"\t\t}}");
							if (!type.IsClass) {
								sb.Append(indent).AppendLine($"\t\telse{{");
								sb.Append(indent).AppendLine($"\t\t\tthis.{f.Name} = default({f.Type.GetRealTypeName(TypeNameWithNamespace)});");
								sb.Append(indent).AppendLine($"\t\t}}");
							}
						}
					}
					else {
						sb.Append(decodeStr);
					}
				}
			}
			// add left fields if it is a struct
			if (!type.IsClass) {
				for (int i = 0; i < excludedFields.Count; i++) {
					sb.Append(indent).AppendLine($"\t\tthis.{excludedFields[i].Name} = default({excludedFields[i].GetUnderlyingType().GetRealTypeName(TypeNameWithNamespace)});");
				}
			}

			if (hasCallback) {
				sb.Append(indent).AppendLine($"\t\tthis.{AFTER_DESERIALIZE_METHOD_NAME}();");
			}

			if (type.IsClass) sb.AppendLine("#endif");
			sb.Append(indent).AppendLine("\t}");
			if (!type.IsClass) sb.AppendLine("#endif");

			/* ----- add serialization ----- */
			sb.Append(indent).AppendLine($"\tpublic {methodType} void __BinaryBundleSerialize(List<byte> bytes) {{");
			sb.AppendLine("#if USE_" + type.Name.ToDefineNamingConvention());

			ClearTmpVariable();
			for (int i = 0; i < fields.Count; i++) {
				var f = fields[i];
				GenerateOneEncodeType(serializers, 1, f.Type, null, indent);
			}
			DeclareTmpVariable(sb, indent + "\t\t");

			if (hasInherited) {
				sb.Append(indent).AppendLine("\t\tbase.__BinaryBundleSerialize(bytes);");
			}

			if (hasCallback) {
				sb.Append(indent).AppendLine($"\t\tthis.{BEFORE_SERIALIZE_METHOD_NAME}();");
			}

			// encode version
			if (version > 0) {
				sb.AppendLine(indent + "\t\t// " + VERSION_NAME);
				sb.Append(indent).AppendLine($"\t\tBinaryBundleInternal.EncodeUShort(bytes, {version});");
			}

			for(int i = 0; i < fields.Count; i++) {
				var f = fields[i];
				if(version < f.Min || version > f.Max) {
					continue;
				}
				(var encodeStr, var encodeErr) = GenerateOneEncodeType(serializers, 1, f.Type, f.Name, indent + "\t\t");
				if (string.IsNullOrEmpty(encodeStr)) {
					return $"Generate encode {type.Name}.{f.Name} failed\n{encodeErr}";
				}
				else {
					sb.AppendLine(indent + "\t\t// " + f.Name);
					sb.Append(encodeStr);
				}
			}

			sb.AppendLine("#endif");
			sb.Append(indent).AppendLine("\t}");

			/* ----- end namespace & type ----- */
			sb.Append(indent).AppendLine("}");
			if (type.DeclaringType != null) {
				sb.Append(indent).AppendLine("}");
			}
			sb.AppendLine("}");

			psb.Append(sb);

			return null;
		}

		#region Encode
		private static (string, string) GenerateOneEncodeType(Dictionary<Type, Type> serializers, int depth, Type type, string name, string indent = "") {
			string tmp1, tmp2, tmp3;
			if (serializers.TryGetValue(type, out Type serializer)) {
				return ($"{indent}{TypeNameWithNamespace(serializer)}.{CUSTOM_SERIALIZE_METHOD_NAME}({name}, bytes);{Environment.NewLine}", null);
			}
			else if (type.IsArray) {
				Type valueType = type.GetElementType();

				tmp1 = RequireTmpVariable(typeof(int));
				tmp2 = RequireTmpVariable(typeof(int));
				tmp3 = RequireTmpVariable(valueType);
				(string encodeValueStr, string encodeErr) = GenerateOneEncodeType(serializers, depth + 1, valueType, tmp3, indent + "\t\t");
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(valueType);

				if (string.IsNullOrEmpty(encodeValueStr)) {
					return (null, encodeErr);
				}
				else {
					StringBuilder sb = new StringBuilder();
					sb.Append(indent).AppendLine($"{tmp1} = {name}?.Length ?? 0;");
					sb.Append(indent).AppendLine($"BinaryBundleInternal.EncodeInt(bytes, {tmp1});");
					sb.Append(indent).AppendLine($"if({tmp1} > 0){{");
					sb.Append(indent).AppendLine($"\tfor({tmp2} = 0; {tmp2} < {tmp1}; {tmp2}++) {{");
					sb.Append(indent).AppendLine($"\t\t{tmp3} = {name}[{tmp2}];");
					sb.Append(encodeValueStr);
					sb.Append(indent).AppendLine("\t}");
					sb.Append(indent).AppendLine("}");
					return (sb.ToString(), null);
				}
			}
			else if (type.IsGenericList()) {
				Type valueType = type.GetGenericArguments()[0];

				tmp1 = RequireTmpVariable(typeof(int));
				tmp2 = RequireTmpVariable(typeof(int));
				tmp3 = RequireTmpVariable(valueType);
				(string encodeValueStr, string encodeErr) = GenerateOneEncodeType(serializers, depth + 1, valueType, tmp3, indent + "\t\t");
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(valueType);

				if (string.IsNullOrEmpty(encodeValueStr)) {
					return (null, encodeErr);
				}
				else {
					StringBuilder sb = new StringBuilder();
					sb.Append(indent).AppendLine($"{tmp1} = {name}?.Count ?? 0;");
					sb.Append(indent).AppendLine($"BinaryBundleInternal.EncodeInt(bytes, {tmp1});");
					sb.Append(indent).AppendLine($"if({tmp1} > 0){{");
					sb.Append(indent).AppendLine($"\tfor({tmp2} = 0; {tmp2} < {tmp1}; {tmp2}++) {{");
					sb.Append(indent).AppendLine($"\t\t{tmp3} = {name}[{tmp2}];");
					sb.Append(encodeValueStr);
					sb.Append(indent).AppendLine("\t}");
					sb.Append(indent).AppendLine("}");
					return (sb.ToString(), null);
				}
			}
			else if (type.IsGenericDictionary()) {
				Type keyType = type.GetGenericArguments()[0];
				Type valueType = type.GetGenericArguments()[1];

				tmp1 = RequireTmpVariable(typeof(int));
				tmp2 = RequireTmpVariable(keyType);
				tmp3 = RequireTmpVariable(valueType);
				(string encodeKeyStr, string encodeKeyErr) = GenerateOneEncodeType(serializers, depth + 1, keyType, tmp2, indent + "\t\t");
				(string encodeValueStr, string encodeValueErr) = GenerateOneEncodeType(serializers, depth + 1, valueType, tmp3, indent + "\t\t");
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(keyType);
				ReturnTmpVariable(valueType);

				if (string.IsNullOrEmpty(encodeKeyStr)) {
					return (null, encodeKeyErr);
				}
				else if (string.IsNullOrEmpty(encodeValueStr)) {
					return (null, encodeValueErr);
				}
				else {
					StringBuilder sb = new StringBuilder();
					sb.Append(indent).AppendLine($"{tmp1} = {name}?.Count ?? 0;");
					sb.Append(indent).AppendLine($"BinaryBundleInternal.EncodeInt(bytes, {tmp1});");
					sb.Append(indent).AppendLine($"if({tmp1} > 0){{");
					sb.Append(indent).AppendLine($"\tforeach(var item{depth} in {name}) {{");
					sb.Append(indent).AppendLine($"\t\t{tmp2} = item{depth}.Key;");
					sb.Append(encodeKeyStr);
					sb.Append(indent).AppendLine($"\t\t{tmp3} = item{depth}.Value;");
					sb.Append(encodeValueStr);
					sb.Append(indent).AppendLine("\t}");
					sb.Append(indent).AppendLine("}");
					return (sb.ToString(), null);
				}
			}
			else if (type.HasAttribute<BinaryBundleObjectAttribute>()) {
				StringBuilder sb = new StringBuilder();
				if (type.IsClass) {
					sb.Append(indent).AppendLine($"if({name} == null){{");
					sb.Append(indent).AppendLine($"\tBinaryBundleInternal.EncodeBool(bytes, false);");
					sb.Append(indent).AppendLine($"}}");
					sb.Append(indent).AppendLine($"else {{");
					sb.Append(indent).AppendLine($"\tBinaryBundleInternal.EncodeBool(bytes, true);");
					sb.Append(indent).AppendLine($"\t{name}.__BinaryBundleSerialize(bytes);");
					sb.Append(indent).AppendLine($"}}");
				}
				else {
					sb.Append(indent).AppendLine($"BinaryBundleInternal.EncodeBool(bytes, true);");
					sb.Append(indent).AppendLine($"{name}.__BinaryBundleSerialize(bytes);");
				}
				return (sb.ToString(), null);
			}
			else if (SUPPORTED_TYPES.TryGetValue(type, out (string encode, string decode) v)) {
				return ($"{indent}BinaryBundleInternal.{v.encode}(bytes, {name});{Environment.NewLine}", null);
			}
			else if (type.IsEnum) {
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (SUPPORTED_TYPES.TryGetValue(underlyingType, out (string encode, string decode) u)) {
					return ($"{indent}BinaryBundleInternal.{u.encode}(bytes, ({underlyingType.Name}){name});{Environment.NewLine}", null);
				}
				else {
					return (null, $"Enum underlyingType {underlyingType} is not supported");
				}
			}
			return (null, $"{type} is not supported");
		}
		#endregion

		#region Decode
		private static (string, string) GenerateOneDecodeType(Dictionary<Type, Type> serializers, int depth, Type type, string name, string assignFmt, string indent = "") {
			string tmp1, tmp2, tmp3, tmp4, tmp5;
			if (serializers.TryGetValue(type, out Type serializer)) {
				return (indent + string.Format(assignFmt, $"{TypeNameWithNamespace(serializer)}.{CUSTOM_DESERIALIZE_METHOD_NAME}(bytes, ref offset)") + Environment.NewLine, null);
			}
			else if (type.IsArray) {
				Type valueType = type.GetElementType();

				tmp1 = RequireTmpVariable(typeof(int));
				tmp2 = RequireTmpVariable(typeof(int));
				tmp3 = RequireTmpVariable(type);
				(string decodeValueStr, string decodeErr) = GenerateOneDecodeType(serializers, depth + 1, valueType, name, $"{tmp3}[{tmp2}] = {{0}};", indent + "\t");
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(type);

				if (string.IsNullOrEmpty(decodeValueStr)) {
					return (null, decodeErr);
				}
				else {
					StringBuilder sb = new StringBuilder();
					sb.Append(indent).AppendLine($"if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref {tmp1})) throw new DecodeBinaryBundleException(\"{name}\", offset);");
					sb.Append(indent).AppendLine($"{tmp3} = new {JaggedArrayDeclaration(type, tmp1)};");
					sb.Append(indent).AppendLine($"for({tmp2} = 0; {tmp2} < {tmp1}; {tmp2}++) {{");
					sb.Append(decodeValueStr);
					sb.Append(indent).AppendLine("}");
					sb.Append(indent).AppendLine(string.Format(assignFmt, tmp3));
					return (sb.ToString(), null);
				}
			}
			else if (type.IsGenericList()) {
				Type valueType = type.GetGenericArguments()[0];

				tmp1 = RequireTmpVariable(typeof(int));
				tmp2 = RequireTmpVariable(typeof(int));
				tmp3 = RequireTmpVariable(type);
				(string decodeValueStr, string decodeErr) = GenerateOneDecodeType(serializers, depth + 1, valueType, name, $"{tmp3}.Add({{0}});", indent + "\t");
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(type);

				if (string.IsNullOrEmpty(decodeValueStr)) {
					return (null, decodeErr);
				}
				else {
					StringBuilder sb = new StringBuilder();
					sb.Append(indent).AppendLine($"if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref {tmp1})) throw new DecodeBinaryBundleException(\"{name}\", offset);");
					sb.Append(indent).AppendLine($"{tmp3} = new List<{valueType.GetRealTypeName(TypeNameWithNamespace)}>({tmp1});");
					sb.Append(indent).AppendLine($"for({tmp2} = 0; {tmp2} < {tmp1}; {tmp2}++) {{");
					sb.Append(decodeValueStr);
					sb.Append(indent).AppendLine("}");
					sb.Append(indent).AppendLine(string.Format(assignFmt, tmp3));
					return (sb.ToString(), null);
				}
			}
			else if (type.IsGenericDictionary()) {
				Type keyType = type.GetGenericArguments()[0];
				Type valueType = type.GetGenericArguments()[1];

				tmp1 = RequireTmpVariable(typeof(int));
				tmp2 = RequireTmpVariable(typeof(int));
				tmp3 = RequireTmpVariable(keyType);
				tmp4 = RequireTmpVariable(valueType);
				tmp5 = RequireTmpVariable(type);
				(string decodeKeyStr, string decodeKeyErr) = GenerateOneDecodeType(serializers, depth + 1, keyType, name, $"{tmp3} = {{0}};", indent + "\t");
				(string decodeValueStr, string decodeValueErr) = GenerateOneDecodeType(serializers, depth + 1, valueType, name, $"{tmp4} = {{0}};", indent + "\t");
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(typeof(int));
				ReturnTmpVariable(keyType);
				ReturnTmpVariable(valueType);
				ReturnTmpVariable(type);

				if (string.IsNullOrEmpty(decodeKeyStr)) {
					return (null, decodeKeyErr);
				}
				else if (string.IsNullOrEmpty(decodeValueStr)) {
					return (null, decodeValueErr);
				}
				else {
					StringBuilder sb = new StringBuilder();
					sb.Append(indent).AppendLine($"if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref {tmp1})) throw new DecodeBinaryBundleException(\"{name}\", offset);");
					sb.Append(indent).AppendLine($"{tmp5} = new Dictionary<{keyType.GetRealTypeName(TypeNameWithNamespace)}, {valueType.GetRealTypeName(TypeNameWithNamespace)}>({tmp1});");
					sb.Append(indent).AppendLine($"for({tmp2} = 0; {tmp2} < {tmp1}; {tmp2}++) {{");
					sb.Append(decodeKeyStr);
					sb.Append(decodeValueStr);
					sb.Append(indent).AppendLine($"\t{tmp5}[{tmp3}] = {tmp4};");
					sb.Append(indent).AppendLine("}");
					sb.Append(indent).AppendLine(string.Format(assignFmt, tmp5));
					return (sb.ToString(), null);
				}
			}
			else if (type.HasAttribute<BinaryBundleObjectAttribute>() && !type.IsAbstract) {
				StringBuilder sb = new StringBuilder();
				tmp1 = RequireTmpVariable(typeof(bool));
				sb.Append(indent).AppendLine($"if(!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref {tmp1})) throw new DecodeBinaryBundleException(\"{name}\", offset);");
				if (type.IsClass) {
					sb.Append(indent).AppendLine(string.Format(assignFmt, $"{tmp1} ? new {TypeNameWithNamespace(type)}(bytes, ref offset) : null"));
				}
				else {
					sb.Append(indent).AppendLine(string.Format(assignFmt, $"{tmp1} ? new {TypeNameWithNamespace(type)}(bytes, ref offset) : default({TypeNameWithNamespace(type)})"));
				}
				ReturnTmpVariable(typeof(bool));
				return (sb.ToString(), null);
			}
			else if (SUPPORTED_TYPES.TryGetValue(type, out (string encode, string decode) v)) {
				StringBuilder sb = new StringBuilder();
				tmp1 = RequireTmpVariable(type);
				sb.Append(indent).AppendLine($"if(!BinaryBundleInternal.{v.decode}(bytes, ref offset, ref {tmp1})) throw new DecodeBinaryBundleException(\"{name}\", offset);");
				sb.Append(indent).AppendLine(string.Format(assignFmt, tmp1));
				ReturnTmpVariable(type);
				return (sb.ToString(), null);
			}
			else if (type.IsEnum) {
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (SUPPORTED_TYPES.TryGetValue(underlyingType, out (string encode, string decode) u)) {
					StringBuilder sb = new StringBuilder();
					tmp1 = RequireTmpVariable(underlyingType);
					sb.Append(indent).AppendLine($"if(!BinaryBundleInternal.{u.decode}(bytes, ref offset, ref {tmp1})) throw new DecodeBinaryBundleException(\"{name}\", offset);");
					sb.Append(indent).AppendLine(string.Format(assignFmt, $"({TypeNameWithNamespace(type)}){tmp1}"));
					ReturnTmpVariable(underlyingType);
					return (sb.ToString(), null);
				}
				else {
					return (null, $"Enum underlyingType {underlyingType} is not supported");
				}
			}
			return (null, $"{type} is not supported");
		}
		#endregion

		#region Temporarily Variable
		private static string GetTmpVariableName(Type type, int index) {
			return $"__{type.GetRealTypeName().Replace('<', '_').Replace('>', '_').Replace(',', '_').Replace('[', '_').Replace(']', '_')}_{index}";
		}

		private static void ClearTmpVariable() {
			if(_tmpVariables == null) {
				_tmpVariables = new Dictionary<Type, TmpVarState>();
			}
			else {
				_tmpVariables.Clear();
			}
		}

		private static string RequireTmpVariable(Type type) {
			TmpVarState state;
			if (!_tmpVariables.TryGetValue(type, out state)) {
				state = new TmpVarState() { idx = 0, max = 1 };
				_tmpVariables.Add(type, state);
			}
			if (state.idx == state.max) {
				state.max++;
			}
			state.idx++;
			return GetTmpVariableName(type, state.idx);
		}

		private static void ReturnTmpVariable(Type type) {
			TmpVarState state;
			if (_tmpVariables.TryGetValue(type, out state)) {
				state.idx--;
			}
		}

		private static void DeclareTmpVariable(StringBuilder sb, string indent) {
			foreach(var t in _tmpVariables) {
				int count = t.Value.max;
				for(int i = 1; i <= count; i++) {
					sb.Append(indent).AppendLine($"var {(GetTmpVariableName(t.Key, i))} = default({t.Key.GetRealTypeName(TypeNameWithNamespace)});");
				}
			}
			sb.AppendLine();
		}

		private static string TypeNameWithNamespace(Type type) {
			if (NAMESPACES.Contains(type.Namespace)) {
				return type.Name;
			}
			else {
				return type.FullName.Replace('+', '.');
			}
		}

		private static string JaggedArrayDeclaration(Type type, string length) {
			int count = 0;
			Type valueType = type.GetElementType();
			while (valueType.IsArray) {
				count++;
				valueType = valueType.GetElementType();
			}
			var sb = new StringBuilder();
			sb.Append(valueType.GetRealTypeName(TypeNameWithNamespace)).Append("[").Append(length).Append("]");
			for(int i = 0; i < count; i++) {
				sb.Append("[]");
			}
			return sb.ToString();
		}
		#endregion

		public static string ToDefineNamingConvention(this string s) {
			var r = new Regex(@"
            (?<=[A-Z])(?=[A-Z][a-z]) |
                (?<=[^A-Z])(?=[A-Z]) |
                (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

			return r.Replace(s, "_").ToUpper();
		}
	}

	public static class TypeExtension {
		public static bool HasAttribute<T>(this Type type) where T : Attribute {
			return type.GetCustomAttribute<T>(true) != null;
		}

		public static bool IsGenericList(this Type type) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
		}
		public static bool IsGenericDictionary(this Type type) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
		}

		public static string GetRealTypeName(this Type t, Func<Type, string> nameGetter = null) {
			string name = nameGetter == null ? t.Name : nameGetter(t);

			if (!t.IsGenericType)
				return name;

			StringBuilder sb = new StringBuilder();
			sb.Append(name.Substring(0, name.IndexOf('`')));
			sb.Append('<');
			bool appendComma = false;
			foreach (Type arg in t.GetGenericArguments()) {
				if (appendComma) sb.Append(',');
				sb.Append(GetRealTypeName(arg, nameGetter));
				appendComma = true;
			}
			sb.Append('>');
			return sb.ToString();
		}

		public static bool HasMethod(this Type type, string methodName, BindingFlags bindingFlags = BindingFlags.Default, Type[] parameterTypes = null, Type returnType = null) {
			if (parameterTypes == null) parameterTypes = Type.EmptyTypes;
			var method = type.GetMethod(methodName, bindingFlags, null, parameterTypes, null);
			if (method != null && (returnType == null || method.ReturnType == returnType)) {
				return true;
			}
			return false;
		}

		public static Type GetUnderlyingType(this MemberInfo member) {
			switch (member.MemberType) {
				case MemberTypes.Event:
					return ((EventInfo)member).EventHandlerType;
				case MemberTypes.Field:
					return ((FieldInfo)member).FieldType;
				case MemberTypes.Method:
					return ((MethodInfo)member).ReturnType;
				case MemberTypes.Property:
					return ((PropertyInfo)member).PropertyType;
				default:
					throw new ArgumentException
					(
					 "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
					);
			}
		}
	}
}
#endif
