using BinaryBundle;
using System.Collections.Generic;

namespace BinaryBundleTest {
    [BinaryBundleObject]
    public partial class EmptyTest : IBinaryBundleTest {

        [BinaryBundleField(0)]
        public int[] fieldIntArray;

        [BinaryBundleField(1)]
        public List<int> fieldIntList;

        public EmptyTest() {
            fieldIntArray = null;
            fieldIntList = new List<int>();
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<EmptyTest>(this);
            var inst = BinaryBundleSerializer.Decode<EmptyTest>(bytes);

            return
                TestUtil.IsEqual(fieldIntArray, inst.fieldIntArray, "fieldIntArray") &&
                TestUtil.IsEqual(fieldIntList, inst.fieldIntList, "fieldIntList");
        }
    }
}
