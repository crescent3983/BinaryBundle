using BinaryBundle;
using System.Collections.Generic;

namespace BinaryBundleTest {
    [BinaryBundleObject(3, 1)]
    public partial class VersionTest : IBinaryBundleTest {

        [BinaryBundleField(0, 1, 2)]
        public int fieldInt;

        [BinaryBundleField(1)]
        public List<string> fieldStringList;

        public VersionTest() {
            fieldInt = 5;
            fieldStringList = new List<string>() { "a1", "a2" };
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<VersionTest>(this);
            var inst = BinaryBundleSerializer.Decode<VersionTest>(bytes);

            return
                TestUtil.IsNotEqual(fieldInt, inst.fieldInt, "fieldInt") &&
                TestUtil.IsEqual(fieldStringList, inst.fieldStringList, "fieldStringList");
        }
    }
}
