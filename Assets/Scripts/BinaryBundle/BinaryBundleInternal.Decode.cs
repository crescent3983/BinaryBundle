using System;
using System.Text;

namespace BinaryBundle.Internal {
	public static partial class BinaryBundleInternal {

#if BINARY_BUNDLE_THREAD_SAFE
		[ThreadStatic]
		private static byte[] _tmp;
		private static byte[] tmp {
			get {
				if (_tmp == null) _tmp = new byte[8];
				return _tmp;
			}
		}
#else
        private static byte[] tmp = new byte[8];
#endif

#if BINARY_BUNDLE_DEBUG
		private static void DebugLog(string log) {
            UnityEngine.Debug.Log(log);
        }
#endif

        public static bool DecodeBool(byte[] bytes, ref int offset, ref bool data) {
            if (offset + 1 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode bool at {offset}");
#endif
            data = bytes[offset] > 0 ? true : false;
            offset += 1;
            return true;
        }

        public static bool DecodeByte(byte[] bytes, ref int offset, ref byte data) {
            if (offset + 1 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode byte at {offset}");
#endif
            data = bytes[offset];
            offset += 1;
            return true;
        }

        public static bool DecodeSByte(byte[] bytes, ref int offset, ref sbyte data) {
            if (offset + 1 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode sbyte at {offset}");
#endif
            data = (sbyte)bytes[offset];
            offset += 1;
            return true;
        }

        public static bool DecodeShort(byte[] bytes, ref int offset, ref short data) {
            if (offset + 2 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode short at {offset}");
#endif
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 1];
                tmp[1] = bytes[offset];
                data = BitConverter.ToInt16(tmp, 0);
            }
            else {
                data = BitConverter.ToInt16(bytes, offset);
            }
            offset += 2;
            return true;
        }

        public static bool DecodeUShort(byte[] bytes, ref int offset, ref ushort data) {
            if (offset + 2 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode ushort at {offset}");
#endif
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 1];
                tmp[1] = bytes[offset];
                data = BitConverter.ToUInt16(tmp, 0);
            }
            else {
                data = BitConverter.ToUInt16(bytes, offset);
            }
            offset += 2;
            return true;
        }

        public static bool DecodeInt(byte[] bytes, ref int offset, ref int data) {
            if (offset + 4 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode int at {offset}");
#endif
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 3];
                tmp[1] = bytes[offset + 2];
                tmp[2] = bytes[offset + 1];
                tmp[3] = bytes[offset];
                data = BitConverter.ToInt32(tmp, 0);
            }
            else {
                data = BitConverter.ToInt32(bytes, offset);
            }
            offset += 4;
            return true;
        }

        public static bool DecodeUInt(byte[] bytes, ref int offset, ref uint data) {
            if (offset + 4 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode uint at {offset}");
#endif
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 3];
                tmp[1] = bytes[offset + 2];
                tmp[2] = bytes[offset + 1];
                tmp[3] = bytes[offset];
                data = BitConverter.ToUInt32(tmp, 0);
            }
            else {
                data = BitConverter.ToUInt32(bytes, offset);
            }
            offset += 4;
            return true;
        }

        public static bool DecodeLong(byte[] bytes, ref int offset, ref long data) {
            if (offset + 8 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode long at {offset}");
#endif
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 7];
                tmp[1] = bytes[offset + 6];
                tmp[2] = bytes[offset + 5];
                tmp[3] = bytes[offset + 4];
                tmp[4] = bytes[offset + 3];
                tmp[5] = bytes[offset + 2];
                tmp[6] = bytes[offset + 1];
                tmp[7] = bytes[offset];
                data = BitConverter.ToInt64(tmp, 0);
            }
            else {
                data = BitConverter.ToInt64(bytes, offset);
            }
            offset += 8;
            return true;
        }

        public static bool DecodeULong(byte[] bytes, ref int offset, ref ulong data) {
            if (offset + 8 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode ulong at {offset}");
#endif
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 7];
                tmp[1] = bytes[offset + 6];
                tmp[2] = bytes[offset + 5];
                tmp[3] = bytes[offset + 4];
                tmp[4] = bytes[offset + 3];
                tmp[5] = bytes[offset + 2];
                tmp[6] = bytes[offset + 1];
                tmp[7] = bytes[offset];
                data = BitConverter.ToUInt64(tmp, 0);
            }
            else {
                data = BitConverter.ToUInt64(bytes, offset);
            }
            offset += 8;
            return true;
        }

        public static bool DecodeFloat(byte[] bytes, ref int offset, ref float data) {
            if (offset + 4 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode float at {offset}");
#endif
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 3];
                tmp[1] = bytes[offset + 2];
                tmp[2] = bytes[offset + 1];
                tmp[3] = bytes[offset];
                data = BitConverter.ToSingle(tmp, 0);
            }
            else {
                data = BitConverter.ToSingle(bytes, offset);
            }
            offset += 4;
            return true;
        }

        public static bool DecodeDouble(byte[] bytes, ref int offset, ref double data) {
            if (offset + 8 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode double at {offset}");
#endif
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 7];
                tmp[1] = bytes[offset + 6];
                tmp[2] = bytes[offset + 5];
                tmp[3] = bytes[offset + 4];
                tmp[4] = bytes[offset + 3];
                tmp[5] = bytes[offset + 2];
                tmp[6] = bytes[offset + 1];
                tmp[7] = bytes[offset];
                data = BitConverter.ToDouble(tmp, 0);
            }
            else {
                data = BitConverter.ToDouble(bytes, offset);
            }
            offset += 8;
            return true;
        }

        public static bool DecodeString(byte[] bytes, ref int offset, ref string data, Encoding encoding = null) {
            if (offset + 4 > bytes.Length) return false;
#if BINARY_BUNDLE_DEBUG
            DebugLog($"decode string at {offset}");
#endif
            int len;
            if (BitConverter.IsLittleEndian) {
                tmp[0] = bytes[offset + 3];
                tmp[1] = bytes[offset + 2];
                tmp[2] = bytes[offset + 1];
                tmp[3] = bytes[offset];
                len = BitConverter.ToInt32(tmp, 0);
            }
            else {
                len = BitConverter.ToInt32(bytes, offset);
            }

            if (len > 0) {
                if (offset + 4 + len > bytes.Length) return false;
                if (encoding == null) encoding = Encoding.UTF8;
                data = encoding.GetString(bytes, offset + 4, len);
                offset += len;
            }
            offset += 4;
            return true;
        }
    }
}
