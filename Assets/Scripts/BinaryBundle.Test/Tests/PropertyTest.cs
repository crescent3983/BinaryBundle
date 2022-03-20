using BinaryBundle;

namespace BinaryBundleTest {
    [BinaryBundleObject]
    public partial class PropertyTest : IBinaryBundleTest {

        [BinaryBundleField(0)]
        public int fieldInt { get; private set; }

        [BinaryBundleField(1)]
        public double fieldDouble { get; set; }

        public PropertyTest() {
            fieldInt = 77;
            fieldDouble = 9.6845;
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<PropertyTest>(this);
            var inst = BinaryBundleSerializer.Decode<PropertyTest>(bytes);

            return
                TestUtil.IsEqual(fieldInt, inst.fieldInt, "fieldInt") &&
                TestUtil.IsEqual(fieldDouble, inst.fieldDouble, "fieldDouble");
        }
    }
}
