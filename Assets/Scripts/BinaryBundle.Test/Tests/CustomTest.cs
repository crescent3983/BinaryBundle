using BinaryBundle;
using BinaryBundle.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace BinaryBundleTest {
    [BinaryBundleObject]
	[BinaryBundleCustomSerializer(typeof(Color), typeof(ColorSerializer))]
    public partial class CustomTest : IBinaryBundleTest {

        [BinaryBundleField(0)]
        public int fieldInt;

        [BinaryBundleField(1)]
        public Color fieldColor;

        public CustomTest() {
			fieldInt = 5;
			fieldColor = new Color(0.1f, 0.2f, 0.5f, 1);
		}

        public bool RunTest() {
			var bytes = BinaryBundleSerializer.Encode<CustomTest>(this);
			var inst = BinaryBundleSerializer.Decode<CustomTest>(bytes);

			return
				TestUtil.IsEqual(fieldInt, inst.fieldInt, "fieldInt") &&
				TestUtil.IsEqual(fieldColor, inst.fieldColor, "fieldColor");
		}
    }

	public static class ColorSerializer {
		public static void Serialize(Color data, List<byte> bytes) {
			BinaryBundleInternal.EncodeBool(bytes, true);
			BinaryBundleInternal.EncodeFloat(bytes, data.r);
			BinaryBundleInternal.EncodeFloat(bytes, data.g);
			BinaryBundleInternal.EncodeFloat(bytes, data.b);
			BinaryBundleInternal.EncodeFloat(bytes, data.a);
		}

		public static Color Deserialize(byte[] bytes, ref int offset) {
			bool exist = false;
			float r = 0, g = 0, b = 0, a = 0;
			if (!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref exist)) Error(offset);
			if (exist) {
				if (!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref r)) Error(offset);
				if (!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref g)) Error(offset);
				if (!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref b)) Error(offset);
				if (!BinaryBundleInternal.DecodeFloat(bytes, ref offset, ref a)) Error(offset);
			}
			return new Color(r, g, b, a);
		}

		private static void Error(int offset) {
			throw new DecodeBinaryBundleException(typeof(Color).ToString(), offset);
		}
	}
}
