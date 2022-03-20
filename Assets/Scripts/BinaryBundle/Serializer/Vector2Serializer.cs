using BinaryBundle.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace BinaryBundle {
	public static class Vector2Serializer {
		public static void Serialize(Vector2 data, List<byte> bytes) {
			BinaryBundleInternal.EncodeBool(bytes, true);
			BinaryBundleInternal.EncodeFloat(bytes, data.x);
			BinaryBundleInternal.EncodeFloat(bytes, data.y);
		}

		public static Vector2 Deserialize(byte[] bytes, ref int offset) {
			bool exist = false;
			float x = 0, y = 0;
			if (!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref exist)) Error(offset);
			if (exist) {
				if (!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref x)) Error(offset);
				if (!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref y)) Error(offset);
			}
			return new Vector2(x, y);
		}

		private static void Error(int offset) {
			throw new DecodeBinaryBundleException(typeof(Vector2).ToString(), offset);
		}
	}
}
