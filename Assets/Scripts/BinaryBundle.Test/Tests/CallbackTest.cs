using BinaryBundle;

namespace BinaryBundleTest {
    [BinaryBundleObject]
    public partial class CallbackTest : IBinaryBundleTest, IBinaryBundleSerializationCallback {

        [BinaryBundleField(0)]
        public int fieldInt;

        [BinaryBundleField(1)]
        public float fieldFloat;

        public string fieldString;

        public CallbackTest() {
            fieldInt = 1;
            fieldFloat = 5.6f;
        }

        public void OnBeforeSerialize() {
            fieldString = "test";
        }

        public void OnAfterDeserialize() {
            fieldString = "test";
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<CallbackTest>(this);
            var inst = BinaryBundleSerializer.Decode<CallbackTest>(bytes);

            return 
                TestUtil.IsEqual(fieldInt, inst.fieldInt, "fieldInt") &&
                TestUtil.IsEqual(fieldFloat, inst.fieldFloat, "fieldFloat") &&
                TestUtil.IsEqual(fieldString, inst.fieldString, "fieldString");
        }
    }
}
