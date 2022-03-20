using BinaryBundle;

namespace BinaryBundleTest {
    [BinaryBundleObject]
    public partial class PrivateTest : IBinaryBundleTest {

        [BinaryBundleField(0)]
        public int fieldInt;

        [BinaryBundleField(1)]
        private double fieldDouble;

        public PrivateTest() {
            fieldInt = 77;
            fieldDouble = 9.6845;
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<PrivateTest>(this);
            var inst = BinaryBundleSerializer.Decode<PrivateTest>(bytes);

            return
                TestUtil.IsEqual(fieldInt, inst.fieldInt, "fieldInt") &&
                TestUtil.IsEqual(fieldDouble, inst.fieldDouble, "fieldDouble");
        }
    }
}
