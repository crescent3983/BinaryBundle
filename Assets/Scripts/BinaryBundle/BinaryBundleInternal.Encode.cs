using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryBundle.Internal {
	public static partial class BinaryBundleInternal {
		public static void EncodeBool(List<byte> bytes, bool data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode bool at {bytes.Count}");
#endif
			bytes.Add(data ? (byte)1 : (byte)0);
		}

		public static void EncodeByte(List<byte> bytes, byte data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode byte at {bytes.Count}");
#endif
			bytes.Add(data);
		}

		public static void EncodeSByte(List<byte> bytes, sbyte data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode sbyte at {bytes.Count}");
#endif
			bytes.Add((byte)data);
		}

		public static void EncodeShort(List<byte> bytes, short data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode short at {bytes.Count}");
#endif
			var b = BitConverter.GetBytes(data);
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			bytes.AddRange(b);
		}

		public static void EncodeUShort(List<byte> bytes, ushort data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode ushort at {bytes.Count}");
#endif
			var b = BitConverter.GetBytes(data);
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			bytes.AddRange(b);
		}

		public static void EncodeInt(List<byte> bytes, int data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode int at {bytes.Count}");
#endif
			var b = BitConverter.GetBytes(data);
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			bytes.AddRange(b);
		}

		public static void EncodeUInt(List<byte> bytes, uint data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode uint at {bytes.Count}");
#endif
			var b = BitConverter.GetBytes(data);
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			bytes.AddRange(b);
		}

		public static void EncodeLong(List<byte> bytes, long data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode long at {bytes.Count}");
#endif
			var b = BitConverter.GetBytes(data);
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			bytes.AddRange(b);
		}

		public static void EncodeULong(List<byte> bytes, ulong data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode ulong at {bytes.Count}");
#endif
			var b = BitConverter.GetBytes(data);
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			bytes.AddRange(b);
		}

		public static void EncodeFloat(List<byte> bytes, float data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode float at {bytes.Count}");
#endif
			var b = BitConverter.GetBytes(data);
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			bytes.AddRange(b);
		}

		public static void EncodeDouble(List<byte> bytes, double data) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode double at {bytes.Count}");
#endif
			var b = BitConverter.GetBytes(data);
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			bytes.AddRange(b);
		}

		public static void EncodeString(List<byte> bytes, string data, Encoding encoding = null) {
#if BINARY_BUNDLE_DEBUG
			DebugLog($"encode string at {bytes.Count}");
#endif
			if (string.IsNullOrEmpty(data)) {
				bytes.AddRange(BitConverter.GetBytes(0));
				return;
			}
			if (encoding == null) encoding = Encoding.UTF8;
			var b = encoding.GetBytes(data);
			var l = BitConverter.GetBytes(b.Length);
			if (BitConverter.IsLittleEndian) Array.Reverse(l);
			bytes.AddRange(l);
			bytes.AddRange(b);
		}
	}
}
