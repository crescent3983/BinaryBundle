using BinaryBundle;
using System;

namespace BinaryBundleTest {

    [BinaryBundleObject]
    public partial class InnerTest : IBinaryBundleTest {

        [BinaryBundleObject]
        public partial class InnerClass : IEquatable<InnerClass> {
            [BinaryBundleField(0)]
            public int fieldInt;

            [BinaryBundleField(1)]
            public string fieldString;

            public InnerClass() {
                fieldInt = 7;
                fieldString = "InnerClass";
            }

            public bool Equals(InnerClass inst) {
                return
                    fieldInt.Equals(inst.fieldInt) &&
                    fieldString.Equals(inst.fieldString);
            }
        }

        [BinaryBundleField(0)]
        public bool fieldBool;

        [BinaryBundleField(1)]
        public InnerClass fieldInnerClass;

        public InnerTest() {
            fieldBool = false;
            fieldInnerClass = new InnerClass();
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<InnerTest>(this);
            var inst = BinaryBundleSerializer.Decode<InnerTest>(bytes);

            return
                TestUtil.IsEqual(fieldBool, inst.fieldBool, "fieldBool") &&
                TestUtil.IsEqual(fieldInnerClass, inst.fieldInnerClass, "fieldInnerClass");
        }
    }
}
