#define USE_TYPE_TEST
#define USE_CALLBACK_TEST
#define USE_CUSTOM_TEST
#define USE_EMPTY_TEST
#define USE_BASE_CLASS
#define USE_INHERIT_TEST
#define USE_CHILD_CLASS
#define USE_NESTED_TEST
#define USE_PARTIAL_TEST
#define USE_PRIVATE_TEST
#define USE_PROPERTY_TEST
#define USE_STRUCT_TEST
#define USE_VERSION_TEST
#define USE_MIDDLE_CLASS

using System;
using System.Collections.Generic;
using BinaryBundle;
using BinaryBundle.Internal;

namespace BinaryBundleTest {
	partial class TypeTest : IBinaryBundleSerializable {
		public TypeTest(byte[] bytes, ref int offset)  {
#if USE_TYPE_TEST
			var __Boolean_1 = default(Boolean);
			var __Byte_1 = default(Byte);
			var __Int16_1 = default(Int16);
			var __UInt16_1 = default(UInt16);
			var __Int32_1 = default(Int32);
			var __Int32_2 = default(Int32);
			var __Int32_3 = default(Int32);
			var __Int32_4 = default(Int32);
			var __UInt32_1 = default(UInt32);
			var __Int64_1 = default(Int64);
			var __UInt64_1 = default(UInt64);
			var __Single_1 = default(Single);
			var __Double_1 = default(Double);
			var __String_1 = default(String);
			var __String_2 = default(String);
			var __Int32___1 = default(Int32[]);
			var __List_Int32__1 = default(List<Int32>);
			var __Dictionary_Int32_String__1 = default(Dictionary<Int32,String>);

			// fieldBool
			if(!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref __Boolean_1)) throw new DecodeBinaryBundleException("fieldBool", offset);
			fieldBool = __Boolean_1;
			// fieldByte
			if(!BinaryBundleInternal.DecodeByte(bytes, ref offset, ref __Byte_1)) throw new DecodeBinaryBundleException("fieldByte", offset);
			fieldByte = __Byte_1;
			// fieldShort
			if(!BinaryBundleInternal.DecodeShort(bytes, ref offset, ref __Int16_1)) throw new DecodeBinaryBundleException("fieldShort", offset);
			fieldShort = __Int16_1;
			// fieldUShort
			if(!BinaryBundleInternal.DecodeUShort(bytes, ref offset, ref __UInt16_1)) throw new DecodeBinaryBundleException("fieldUShort", offset);
			fieldUShort = __UInt16_1;
			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldUInt
			if(!BinaryBundleInternal.DecodeUInt(bytes, ref offset, ref __UInt32_1)) throw new DecodeBinaryBundleException("fieldUInt", offset);
			fieldUInt = __UInt32_1;
			// fieldLong
			if(!BinaryBundleInternal.DecodeLong(bytes, ref offset, ref __Int64_1)) throw new DecodeBinaryBundleException("fieldLong", offset);
			fieldLong = __Int64_1;
			// fieldULong
			if(!BinaryBundleInternal.DecodeULong(bytes, ref offset, ref __UInt64_1)) throw new DecodeBinaryBundleException("fieldULong", offset);
			fieldULong = __UInt64_1;
			// fieldFloat
			if(!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref __Single_1)) throw new DecodeBinaryBundleException("fieldFloat", offset);
			fieldFloat = __Single_1;
			// fieldDouble
			if(!BinaryBundleInternal.DecodeDouble(bytes, ref offset, ref __Double_1)) throw new DecodeBinaryBundleException("fieldDouble", offset);
			fieldDouble = __Double_1;
			// fieldString
			if(!BinaryBundleInternal.DecodeString(bytes, ref offset, ref __String_1)) throw new DecodeBinaryBundleException("fieldString", offset);
			fieldString = __String_1;
			// fieldEnum
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldEnum", offset);
			fieldEnum = (BinaryBundleTest.TestEnum)__Int32_1;
			// fieldIntArray
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldIntArray", offset);
			__Int32___1 = new Int32[__Int32_1];
			for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
				if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_3)) throw new DecodeBinaryBundleException("fieldIntArray", offset);
				__Int32___1[__Int32_2] = __Int32_3;
			}
			fieldIntArray = __Int32___1;
			// fieldIntList
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldIntList", offset);
			__List_Int32__1 = new List<Int32>(__Int32_1);
			for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
				if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_3)) throw new DecodeBinaryBundleException("fieldIntList", offset);
				__List_Int32__1.Add(__Int32_3);
			}
			fieldIntList = __List_Int32__1;
			// fieldDictIntString
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldDictIntString", offset);
			__Dictionary_Int32_String__1 = new Dictionary<Int32, String>(__Int32_1);
			for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
				if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_4)) throw new DecodeBinaryBundleException("fieldDictIntString", offset);
				__Int32_3 = __Int32_4;
				if(!BinaryBundleInternal.DecodeString(bytes, ref offset, ref __String_2)) throw new DecodeBinaryBundleException("fieldDictIntString", offset);
				__String_1 = __String_2;
				__Dictionary_Int32_String__1[__Int32_3] = __String_1;
			}
			fieldDictIntString = __Dictionary_Int32_String__1;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_TYPE_TEST
			var __Int32_1 = default(Int32);
			var __Int32_2 = default(Int32);
			var __Int32_3 = default(Int32);
			var __String_1 = default(String);

			// fieldBool
			BinaryBundleInternal.EncodeBool(bytes, fieldBool);
			// fieldByte
			BinaryBundleInternal.EncodeByte(bytes, fieldByte);
			// fieldShort
			BinaryBundleInternal.EncodeShort(bytes, fieldShort);
			// fieldUShort
			BinaryBundleInternal.EncodeUShort(bytes, fieldUShort);
			// fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldUInt
			BinaryBundleInternal.EncodeUInt(bytes, fieldUInt);
			// fieldLong
			BinaryBundleInternal.EncodeLong(bytes, fieldLong);
			// fieldULong
			BinaryBundleInternal.EncodeULong(bytes, fieldULong);
			// fieldFloat
			BinaryBundleInternal.EncodeFloat(bytes, fieldFloat);
			// fieldDouble
			BinaryBundleInternal.EncodeDouble(bytes, fieldDouble);
			// fieldString
			BinaryBundleInternal.EncodeString(bytes, fieldString);
			// fieldEnum
			BinaryBundleInternal.EncodeInt(bytes, (Int32)fieldEnum);
			// fieldIntArray
			__Int32_1 = fieldIntArray?.Length ?? 0;
			BinaryBundleInternal.EncodeInt(bytes, __Int32_1);
			if(__Int32_1 > 0){
				for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
					__Int32_3 = fieldIntArray[__Int32_2];
					BinaryBundleInternal.EncodeInt(bytes, __Int32_3);
				}
			}
			// fieldIntList
			__Int32_1 = fieldIntList?.Count ?? 0;
			BinaryBundleInternal.EncodeInt(bytes, __Int32_1);
			if(__Int32_1 > 0){
				for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
					__Int32_3 = fieldIntList[__Int32_2];
					BinaryBundleInternal.EncodeInt(bytes, __Int32_3);
				}
			}
			// fieldDictIntString
			__Int32_1 = fieldDictIntString?.Count ?? 0;
			BinaryBundleInternal.EncodeInt(bytes, __Int32_1);
			if(__Int32_1 > 0){
				foreach(var item1 in fieldDictIntString) {
					__Int32_2 = item1.Key;
					BinaryBundleInternal.EncodeInt(bytes, __Int32_2);
					__String_1 = item1.Value;
					BinaryBundleInternal.EncodeString(bytes, __String_1);
				}
			}
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class CallbackTest : IBinaryBundleSerializable {
		public CallbackTest(byte[] bytes, ref int offset)  {
#if USE_CALLBACK_TEST
			var __Int32_1 = default(Int32);
			var __Single_1 = default(Single);

			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldFloat
			if(!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref __Single_1)) throw new DecodeBinaryBundleException("fieldFloat", offset);
			fieldFloat = __Single_1;
			this.OnAfterDeserialize();
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_CALLBACK_TEST

			this.OnBeforeSerialize();
			// fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldFloat
			BinaryBundleInternal.EncodeFloat(bytes, fieldFloat);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class CustomTest : IBinaryBundleSerializable {
		public CustomTest(byte[] bytes, ref int offset)  {
#if USE_CUSTOM_TEST
			var __Int32_1 = default(Int32);

			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldColor
			fieldColor = BinaryBundleTest.ColorSerializer.Deserialize(bytes, ref offset);
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_CUSTOM_TEST

			// fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldColor
			BinaryBundleTest.ColorSerializer.Serialize(fieldColor, bytes);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class EmptyTest : IBinaryBundleSerializable {
		public EmptyTest(byte[] bytes, ref int offset)  {
#if USE_EMPTY_TEST
			var __Int32_1 = default(Int32);
			var __Int32_2 = default(Int32);
			var __Int32_3 = default(Int32);
			var __Int32___1 = default(Int32[]);
			var __List_Int32__1 = default(List<Int32>);

			// fieldIntArray
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldIntArray", offset);
			__Int32___1 = new Int32[__Int32_1];
			for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
				if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_3)) throw new DecodeBinaryBundleException("fieldIntArray", offset);
				__Int32___1[__Int32_2] = __Int32_3;
			}
			fieldIntArray = __Int32___1;
			// fieldIntList
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldIntList", offset);
			__List_Int32__1 = new List<Int32>(__Int32_1);
			for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
				if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_3)) throw new DecodeBinaryBundleException("fieldIntList", offset);
				__List_Int32__1.Add(__Int32_3);
			}
			fieldIntList = __List_Int32__1;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_EMPTY_TEST
			var __Int32_1 = default(Int32);
			var __Int32_2 = default(Int32);
			var __Int32_3 = default(Int32);

			// fieldIntArray
			__Int32_1 = fieldIntArray?.Length ?? 0;
			BinaryBundleInternal.EncodeInt(bytes, __Int32_1);
			if(__Int32_1 > 0){
				for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
					__Int32_3 = fieldIntArray[__Int32_2];
					BinaryBundleInternal.EncodeInt(bytes, __Int32_3);
				}
			}
			// fieldIntList
			__Int32_1 = fieldIntList?.Count ?? 0;
			BinaryBundleInternal.EncodeInt(bytes, __Int32_1);
			if(__Int32_1 > 0){
				for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
					__Int32_3 = fieldIntList[__Int32_2];
					BinaryBundleInternal.EncodeInt(bytes, __Int32_3);
				}
			}
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class BaseClass : IBinaryBundleSerializable {
		public BaseClass(byte[] bytes, ref int offset)  {
#if USE_BASE_CLASS
			var __Boolean_1 = default(Boolean);
			var __String_1 = default(String);

			// fieldBool
			if(!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref __Boolean_1)) throw new DecodeBinaryBundleException("fieldBool", offset);
			fieldBool = __Boolean_1;
			// fieldString
			if(!BinaryBundleInternal.DecodeString(bytes, ref offset, ref __String_1)) throw new DecodeBinaryBundleException("fieldString", offset);
			fieldString = __String_1;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_BASE_CLASS

			// fieldBool
			BinaryBundleInternal.EncodeBool(bytes, fieldBool);
			// fieldString
			BinaryBundleInternal.EncodeString(bytes, fieldString);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class InheritTest : IBinaryBundleSerializable {
		public InheritTest(byte[] bytes, ref int offset) : base(bytes, ref offset) {
#if USE_INHERIT_TEST
			var __Int32_1 = default(Int32);
			var __Single_1 = default(Single);

			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldFloat
			if(!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref __Single_1)) throw new DecodeBinaryBundleException("fieldFloat", offset);
			fieldFloat = __Single_1;
#endif
		}
		public override void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_INHERIT_TEST

			base.__BinaryBundleSerialize(bytes);
			// fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldFloat
			BinaryBundleInternal.EncodeFloat(bytes, fieldFloat);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class ChildClass : IBinaryBundleSerializable {
		public ChildClass(byte[] bytes, ref int offset)  {
#if USE_CHILD_CLASS
			var __Int32_1 = default(Int32);
			var __String_1 = default(String);

			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldString
			if(!BinaryBundleInternal.DecodeString(bytes, ref offset, ref __String_1)) throw new DecodeBinaryBundleException("fieldString", offset);
			fieldString = __String_1;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_CHILD_CLASS

			// fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldString
			BinaryBundleInternal.EncodeString(bytes, fieldString);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class NestedTest : IBinaryBundleSerializable {
		public NestedTest(byte[] bytes, ref int offset)  {
#if USE_NESTED_TEST
			var __Boolean_1 = default(Boolean);

			// fieldBool
			if(!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref __Boolean_1)) throw new DecodeBinaryBundleException("fieldBool", offset);
			fieldBool = __Boolean_1;
			// fieldChildClass
			if(!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref __Boolean_1)) throw new DecodeBinaryBundleException("fieldChildClass", offset);
			fieldChildClass = __Boolean_1 ? new BinaryBundleTest.ChildClass(bytes, ref offset) : null;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_NESTED_TEST

			// fieldBool
			BinaryBundleInternal.EncodeBool(bytes, fieldBool);
			// fieldChildClass
			if(fieldChildClass == null){
				BinaryBundleInternal.EncodeBool(bytes, false);
			}
			else {
				BinaryBundleInternal.EncodeBool(bytes, true);
				fieldChildClass.__BinaryBundleSerialize(bytes);
			}
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class PartialTest : IBinaryBundleSerializable {
		public PartialTest(byte[] bytes, ref int offset)  {
#if USE_PARTIAL_TEST
			var __Int32_1 = default(Int32);
			var __Single_1 = default(Single);

			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldFloat
			if(!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref __Single_1)) throw new DecodeBinaryBundleException("fieldFloat", offset);
			fieldFloat = __Single_1;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_PARTIAL_TEST

			// fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldFloat
			BinaryBundleInternal.EncodeFloat(bytes, fieldFloat);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class PrivateTest : IBinaryBundleSerializable {
		public PrivateTest(byte[] bytes, ref int offset)  {
#if USE_PRIVATE_TEST
			var __Int32_1 = default(Int32);
			var __Double_1 = default(Double);

			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldDouble
			if(!BinaryBundleInternal.DecodeDouble(bytes, ref offset, ref __Double_1)) throw new DecodeBinaryBundleException("fieldDouble", offset);
			fieldDouble = __Double_1;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_PRIVATE_TEST

			// fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldDouble
			BinaryBundleInternal.EncodeDouble(bytes, fieldDouble);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class PropertyTest : IBinaryBundleSerializable {
		public PropertyTest(byte[] bytes, ref int offset)  {
#if USE_PROPERTY_TEST
			var __Int32_1 = default(Int32);
			var __Double_1 = default(Double);

			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldDouble
			if(!BinaryBundleInternal.DecodeDouble(bytes, ref offset, ref __Double_1)) throw new DecodeBinaryBundleException("fieldDouble", offset);
			fieldDouble = __Double_1;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_PROPERTY_TEST

			// fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldDouble
			BinaryBundleInternal.EncodeDouble(bytes, fieldDouble);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial struct StructTest : IBinaryBundleSerializable {
#if USE_STRUCT_TEST
		public StructTest(byte[] bytes, ref int offset)  {
			var __Boolean_1 = default(Boolean);
			var __Single_1 = default(Single);

			// fieldBool
			if(!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref __Boolean_1)) throw new DecodeBinaryBundleException("fieldBool", offset);
			fieldBool = __Boolean_1;
			// fieldFloat
			if(!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref __Single_1)) throw new DecodeBinaryBundleException("fieldFloat", offset);
			fieldFloat = __Single_1;
			this.fieldInt = default(Single);
		}
#endif
		public  void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_STRUCT_TEST

			// fieldBool
			BinaryBundleInternal.EncodeBool(bytes, fieldBool);
			// fieldFloat
			BinaryBundleInternal.EncodeFloat(bytes, fieldFloat);
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class VersionTest : IBinaryBundleSerializable {
		public VersionTest(byte[] bytes, ref int offset)  {
#if USE_VERSION_TEST
			// __version
			ushort __version = 0;
			if(!BinaryBundleInternal.DecodeUShort(bytes, ref offset, ref __version)) throw new DecodeBinaryBundleException("__version", offset);
			if(__version < 1 || __version > 3) throw new VersionNotMatchException(__version, 1, 3);
			var __Int32_1 = default(Int32);
			var __Int32_2 = default(Int32);
			var __List_String__1 = default(List<String>);
			var __String_1 = default(String);

			// fieldInt
			if(__version >= 1 && __version <= 2){
				if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
				fieldInt = __Int32_1;
			}
			// fieldStringList
				if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldStringList", offset);
				__List_String__1 = new List<String>(__Int32_1);
				for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
					if(!BinaryBundleInternal.DecodeString(bytes, ref offset, ref __String_1)) throw new DecodeBinaryBundleException("fieldStringList", offset);
					__List_String__1.Add(__String_1);
				}
				fieldStringList = __List_String__1;
#endif
		}
		public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_VERSION_TEST
			var __Int32_1 = default(Int32);
			var __Int32_2 = default(Int32);
			var __String_1 = default(String);

			// __version
			BinaryBundleInternal.EncodeUShort(bytes, 3);
			// fieldStringList
			__Int32_1 = fieldStringList?.Count ?? 0;
			BinaryBundleInternal.EncodeInt(bytes, __Int32_1);
			if(__Int32_1 > 0){
				for(__Int32_2 = 0; __Int32_2 < __Int32_1; __Int32_2++) {
					__String_1 = fieldStringList[__Int32_2];
					BinaryBundleInternal.EncodeString(bytes, __String_1);
				}
			}
#endif
		}
	}
}

namespace BinaryBundleTest {
	partial class MiddleClass : IBinaryBundleSerializable {
		public MiddleClass(byte[] bytes, ref int offset) : base(bytes, ref offset) {
#if USE_MIDDLE_CLASS
			var __Int16_1 = default(Int16);
			var __Byte_1 = default(Byte);

			// fieldShort
			if(!BinaryBundleInternal.DecodeShort(bytes, ref offset, ref __Int16_1)) throw new DecodeBinaryBundleException("fieldShort", offset);
			fieldShort = __Int16_1;
			// fieldByte
			if(!BinaryBundleInternal.DecodeByte(bytes, ref offset, ref __Byte_1)) throw new DecodeBinaryBundleException("fieldByte", offset);
			fieldByte = __Byte_1;
#endif
		}
		public override void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_MIDDLE_CLASS

			base.__BinaryBundleSerialize(bytes);
			// fieldShort
			BinaryBundleInternal.EncodeShort(bytes, fieldShort);
			// fieldByte
			BinaryBundleInternal.EncodeByte(bytes, fieldByte);
#endif
		}
	}
}
