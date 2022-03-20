using BinaryBundle;

namespace BinaryBundleTest {
    [BinaryBundleObject]
    public partial struct StructTest : IBinaryBundleTest {

        [BinaryBundleField(0)]
        public bool fieldBool;

        [BinaryBundleField(1)]
        public float fieldFloat;

        public float fieldInt;

        public bool RunTest() {
            fieldBool = false;
            fieldFloat = 7.7f;
            fieldInt = 10;

            var bytes = BinaryBundleSerializer.Encode<StructTest>(this);
            var inst = BinaryBundleSerializer.Decode<StructTest>(bytes);

            return
                TestUtil.IsEqual(fieldBool, inst.fieldBool, "fieldBool") &&
                TestUtil.IsEqual(fieldFloat, inst.fieldFloat, "fieldFloat") &&
                TestUtil.IsNotEqual(fieldInt, inst.fieldInt, "fieldInt");
        }
    }
}
