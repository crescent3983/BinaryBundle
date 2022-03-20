using System;
using System.Collections.Generic;

namespace BinaryBundle {
	public static partial class BinaryBundleSerializer {

		private const int DEFAULT_SIZE = 256;
		private const int TRIM_THRESHOLD = 8192;
#if BINARY_BUNDLE_THREAD_SAFE
		[ThreadStatic]
#endif
		private static List<byte> bytes;

#if BINARY_BUNDLE_THREAD_SAFE
		[ThreadStatic]
#endif
		private static object[] args;

		public static byte[] Encode<T>(T target) {
			var type = typeof(T);
			if (!typeof(IBinaryBundleSerializable).IsAssignableFrom(type)) {
				throw new EncodeBinaryBundleException($"{type} is not IBinaryBundleSerializable");
			}

			if (bytes == null) {
				bytes = new List<byte>(DEFAULT_SIZE);
			}

			if (type.IsClass && target == null) {
				return new byte[] { 0 };
			}
			else {
				bytes.Add(1);
			}

			((IBinaryBundleSerializable)target).__BinaryBundleSerialize(bytes);

			byte[] result = bytes.ToArray();

			bytes.Clear();
			if (bytes.Capacity > TRIM_THRESHOLD) {
				bytes.Capacity = DEFAULT_SIZE;
			}

			return result;
		}

		public static T Decode<T>(byte[] bytes) {
			var type = typeof(T);
			if (!typeof(IBinaryBundleSerializable).IsAssignableFrom(type)) {
				throw new DecodeBinaryBundleException($"{type} is not IBinaryBundleSerializable");
			}

			if (bytes == null || bytes.Length == 0 || bytes[0] == 0) {
				return default(T);
			}

			int offset = 1;
			if (args == null) {
				args = new object[2];
			}
			args[0] = bytes;
			args[1] = offset;	
			return (T)Activator.CreateInstance(type, args);
		}
	}
}
