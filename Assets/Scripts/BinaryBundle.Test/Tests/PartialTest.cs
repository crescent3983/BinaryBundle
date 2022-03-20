using BinaryBundle;

namespace BinaryBundleTest {
    [BinaryBundleObject]
    public partial class PartialTest : IBinaryBundleTest {

        [BinaryBundleField(0)]
        public int fieldInt;

        public long fieldLong;

        [BinaryBundleField(1)]
        public float fieldFloat;

        public PartialTest() {
            fieldInt = 5;
            fieldLong = 1024;
            fieldFloat = 5.6f;
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<PartialTest>(this);
            var inst = BinaryBundleSerializer.Decode<PartialTest>(bytes);

            return 
                TestUtil.IsEqual(fieldInt, inst.fieldInt, "fieldInt") &&
                TestUtil.IsNotEqual(fieldLong, inst.fieldLong, "fieldLong") &&
                TestUtil.IsEqual(fieldFloat, inst.fieldFloat, "fieldFloat");
        }
    }
}
