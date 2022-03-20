using BinaryBundle;
using System;

namespace BinaryBundleTest {

    [BinaryBundleObject]
    public partial class ChildClass : IEquatable<ChildClass> {
        [BinaryBundleField(0)]
        public int fieldInt;

        [BinaryBundleField(1)]
        public string fieldString;

        public ChildClass() {
            fieldInt = 7;
            fieldString = "ChildClass";
        }

        public bool Equals(ChildClass inst) {
            return
                fieldInt.Equals(inst.fieldInt) &&
                fieldString.Equals(inst.fieldString);
        }
    }

    [BinaryBundleObject]
    public partial class NestedTest : IBinaryBundleTest {

        [BinaryBundleField(0)]
        public bool fieldBool;

        [BinaryBundleField(1)]
        public ChildClass fieldChildClass;

        public NestedTest() {
            fieldBool = false;
            fieldChildClass = new ChildClass();
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<NestedTest>(this);
            var inst = BinaryBundleSerializer.Decode<NestedTest>(bytes);

            return
                TestUtil.IsEqual(fieldBool, inst.fieldBool, "fieldBool") &&
                TestUtil.IsEqual(fieldChildClass, inst.fieldChildClass, "fieldChildClass");
        }
    }
}
