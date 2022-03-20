using BinaryBundle.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace BinaryBundle {
	public static class Vector2IntSerializer {
		public static void Serialize(Vector2Int data, List<byte> bytes) {
			BinaryBundleInternal.EncodeBool(bytes, true);
			BinaryBundleInternal.EncodeInt(bytes, data.x);
			BinaryBundleInternal.EncodeInt(bytes, data.y);
		}

		public static Vector2Int Deserialize(byte[] bytes, ref int offset) {
			bool exist = false;
			int x = 0, y = 0;
			if (!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref exist)) Error(offset);
			if (exist) {
				if (!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref x)) Error(offset);
				if (!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref y)) Error(offset);
			}
			return new Vector2Int(x, y);
		}

		private static void Error(int offset) {
			throw new DecodeBinaryBundleException(typeof(Vector2Int).ToString(), offset);
		}
	}
}
