using BinaryBundle;
using System.Collections.Generic;

namespace BinaryBundleTest {

    public enum TestEnum {
        Number1,
        Number2,
        Number3,
    }

    [BinaryBundleObject]
    public partial class TypeTest : IBinaryBundleTest {

        [BinaryBundleField(0)]
        public bool fieldBool;

        [BinaryBundleField(1)]
        public byte fieldByte;

        [BinaryBundleField(2)]
        public short fieldShort;

        [BinaryBundleField(3)]
        public ushort fieldUShort;

        [BinaryBundleField(4)]
        public int fieldInt;

        [BinaryBundleField(5)]
        public uint fieldUInt;

        [BinaryBundleField(6)]
        public long fieldLong;

        [BinaryBundleField(7)]
        public ulong fieldULong;

        [BinaryBundleField(8)]
        public float fieldFloat;

        [BinaryBundleField(9)]
        public double fieldDouble;

        [BinaryBundleField(10)]
        public string fieldString;

        [BinaryBundleField(11)]
        public TestEnum fieldEnum;

        [BinaryBundleField(12)]
        public int[] fieldIntArray;

        [BinaryBundleField(13)]
        public List<int> fieldIntList;

        [BinaryBundleField(14)]
        public Dictionary<int, string> fieldDictIntString;

        public TypeTest() {
            fieldBool = true;
            fieldByte = 1;
            fieldShort = -12;
            fieldUShort = 23;
            fieldInt = -356;
            fieldUInt = 2147486986;
            fieldLong = 15256485695;
            fieldFloat = 1.2669f;
            fieldDouble = 596.56894121;
            fieldString = "test string";
            fieldEnum = TestEnum.Number2;
            fieldIntArray = new int[] { 2, 5, 6 };
            fieldIntList = new List<int>() { 8, 666, 92 };
            fieldDictIntString = new Dictionary<int, string>() {
                {1, "test1" },
                {2, "test2" },
                {3, "test3" },
            };
        }

        public bool RunTest() {
            var bytes = BinaryBundleSerializer.Encode<TypeTest>(this);
            var inst = BinaryBundleSerializer.Decode<TypeTest>(bytes);

            return
                TestUtil.IsEqual(fieldBool, inst.fieldBool, "fieldBool") &&
                TestUtil.IsEqual(fieldByte, inst.fieldByte, "fieldByte") &&
                TestUtil.IsEqual(fieldShort, inst.fieldShort, "fieldShort") &&
                TestUtil.IsEqual(fieldUShort, inst.fieldUShort, "fieldUShort") &&
                TestUtil.IsEqual(fieldInt, inst.fieldInt, "fieldInt") &&
                TestUtil.IsEqual(fieldUInt, inst.fieldUInt, "fieldUInt") &&
                TestUtil.IsEqual(fieldLong, inst.fieldLong, "fieldLong") &&
                TestUtil.IsEqual(fieldFloat, inst.fieldFloat, "fieldFloat") &&
                TestUtil.IsEqual(fieldDouble, inst.fieldDouble, "fieldDouble") &&
                TestUtil.IsEqual(fieldString, inst.fieldString, "fieldString") &&
                TestUtil.IsEqual(fieldEnum, inst.fieldEnum, "fieldEnum") &&
                TestUtil.IsEqual(fieldIntArray, inst.fieldIntArray, "fieldIntArray") &&
                TestUtil.IsEqual(fieldIntList, inst.fieldIntList, "fieldIntList") &&
                TestUtil.IsEqual(fieldDictIntString, inst.fieldDictIntString, "fieldDictIntString");
        }
    }
}