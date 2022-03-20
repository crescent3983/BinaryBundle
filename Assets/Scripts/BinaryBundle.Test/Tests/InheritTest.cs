using BinaryBundle;

namespace BinaryBundleTest {

    [BinaryBundleObject]
    public abstract partial class BaseClass {

        [BinaryBundleField(0)]
        public bool fieldBool;

        [BinaryBundleField(1)]
        public string fieldString;

        public BaseClass() {
            fieldBool = true;
            fieldString = "BaseClass";
        }
    }

    [BinaryBundleObject]
    public partial class MiddleClass : BaseClass {

        [BinaryBundleField(0)]
        public short fieldShort;

        [BinaryBundleField(1)]
        public byte fieldByte;

        public MiddleClass() : base() {
            fieldShort = 99;
            fieldByte = 1;
        }
    }

    [BinaryBundleObject]
    public partial class InheritTest : MiddleClass, IBinaryBundleTest {

        [BinaryBundleField(0)]
        public int fieldInt;

        [BinaryBundleField(1)]
        public float fieldFloat;

        public InheritTest() : base() {
            fieldInt = 3;
            fieldFloat = 6.5f;
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<InheritTest>(this);
            var inst = BinaryBundleSerializer.Decode<InheritTest>(bytes);

            return
                TestUtil.IsEqual(fieldBool, inst.fieldBool, "fieldBool") &&
                TestUtil.IsEqual(fieldString, inst.fieldString, "fieldString") &&
                TestUtil.IsEqual(fieldShort, inst.fieldShort, "fieldShort") &&
                TestUtil.IsEqual(fieldByte, inst.fieldByte, "fieldString") &&
                TestUtil.IsEqual(fieldInt, inst.fieldInt, "fieldInt") &&
                TestUtil.IsEqual(fieldFloat, inst.fieldFloat, "fieldFloat");
        }
    }
}
