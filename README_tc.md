# BinaryBundle

Unity C#數據二進制打包，無引進任何新的格式定義，單純採用預生成序列化和解序列化程式碼，

減少人工撰寫時間並提升執行速度。可用來作為網路傳輸與本地儲存格式。

## 相容性

目前僅測試過以下版本，但也可能兼容其他版本

| 版本
|------
| Unity 2018.4.x
| Unity 2020.3.x

## 使用方法

使用本專案或複製`Assets/Scripts/BinaryBundle`到你的專案

## 數據定義

```csharp
using BinaryBundle;

// 要序列化的類型加上BinaryBundleObject
[BinaryBundleObject]
// 需使用partial關鍵字
public partial class TestClass {
    // 需要序列化的對象加上BinaryBundleField，後面的數字代表順序
    [BinaryBundleField(0)]
    public int fieldInt;

    [BinaryBundleField(1)]
    public string fieldString;

    [BinaryBundleField(2)]
    public float fieldFloat { get; set; }

    // 添加預設建構子
    public TestClass() {}
}
```

## 支援類型

目前僅支援最基本的類型和自定義結構、類別

| Type           |                         |
|----------------|-------------------------|
| bool           |                         |
| byte           |                         |
| sbyte          |                         |
| short          |                         |
| ushort         |                         |
| int            |                         |
| uint           |                         |
| long           |                         |
| ulong          |                         |
| float          |                         |
| double         |                         |
| string         |                         |
| Enum           |  看UnderlyingType       |
| T[]            |  T需為支援類型           |
| List\<T\>      |  T需為支援類型           |
| Dictionary<K,V>|  K, V需為支援類型        |
| Struct         |  需為BinaryBundleObject   |
| Class          |  需為BinaryBundleObject   |

## 產生程式碼

為了避免任何反射調用，所以採用程式碼預產生的方式，但相對也有一些限制，

定義完類型後，使用以下方法生成程式碼(詳細參考`BinaryBundle.Test/TestRunner.cs`的範例)。

```csharp
void BinaryBundleSerializer.GenerateCode(string outputpath, Type[] types)
```

如果有編譯錯誤，請注意以下限制：

+ 類別需有`partial`關鍵字

    由於使用附加程式碼的關係，需利用`partial`來擴充

+ 不支援泛型

    內部型態解析上會有問題

+ 視情況添加預設建構子

    由於程式碼會產生特別的建構子，如果是依賴編譯器預產生的建構子，需改成自行定義

    ```csharp
    public YourClass() {}
    ```

產生出來的程式碼可以參考`BinaryBundle.Test\Generated\Generated.cs`

```csharp
#define USE_TEST_CLASS

namespace BinaryBundleTest {
    partial class TestClass : IBinaryBundleSerializable {

        // 反序列化專用建構子
        public TestClass(byte[] bytes, ref int offset)  {
#if USE_TEST_CLASS
            var __Int32_1 = default(Int32);
			var __String_1 = default(String);

			// fieldInt
			if(!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref __Int32_1)) throw new DecodeBinaryBundleException("fieldInt", offset);
			fieldInt = __Int32_1;
			// fieldString
			if(!BinaryBundleInternal.DecodeString(bytes, ref offset, ref __String_1)) throw new DecodeBinaryBundleException("fieldString", offset);
			fieldString = __String_1;
#endif
        }

        // 序列化內部調用方法
        public virtual void __BinaryBundleSerialize(List<byte> bytes) {
#if USE_TEST_CLASS
            // fieldInt
			BinaryBundleInternal.EncodeInt(bytes, fieldInt);
			// fieldString
			BinaryBundleInternal.EncodeString(bytes, fieldString);
#endif
        }
    }
}

```

※ Tips: 如果變更結構後想重產但有編譯錯誤，可以先註解掉上方對應的定義(ex: USE_TEST_CLASS)

## 使用方法

```csharp
// 序列化
byte[] BinaryBundleSerializer.Encode<T>(T target);
// 解序列化
T BinaryBundleSerializer.Decode<T>(byte[] bytes);
```

```csharp
using BinaryBundle;

TestClass inst = new TestClass();

// 序列化
byte[] bytes = BinaryBundleSerializer.Encode<TestClass>(this);

// 解序列化
TestClass reversed;
try {
    reversed = BinaryBundleSerializer.Decode<TestClass>(bytes);
}
catch(DecodeBinaryBundleException e) {
    UnityEngine.Debug.LogError("Deserialize failed " + e.Message);
    return;
}
```

## 回調

實作`IBinaryBundleSerializationCallback`介面(需重新生成程式碼)

```csharp
[BinaryPackObject]
public class TestClass : IBinaryPackSerializationCallback {

    public void OnBeforeSerialize() {}
        
    public void OnAfterDeserialize() {}
}
```

※ 如果繼承類別本身也有回調，會先呼叫到繼承類別

## 自定義解析

為了可以處理一些我們無法添加BinaryBundleObject的類型，新增可自行實作序列化的方法。

需要新增序列化解析類，並實作兩個靜態方法`Serialize`和`Deserialize`。

```csharp
public static class Vector2IntSerializer {
    // 序列化
    public static void Serialize(Vector2Int data, List<byte> bytes) {
        BinaryBundleInternal.EncodeBool(bytes, true);
        BinaryBundleInternal.EncodeInt(bytes, data.x);
        BinaryBundleInternal.EncodeInt(bytes, data.y);
    }
    // 反序列化
    public static Vector2Int Deserialize(byte[] bytes, ref int offset) {
        bool exist = false;
        int x = 0, y = 0;
        if (!BinaryBundleInternal.DecodeBool(bytes, ref offset, ref exist)) Error(offset);
        if (exist) {
            if (!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref x)) Error(offset);
            if (!BinaryBundleInternal.DecodeInt(bytes, ref offset, ref y)) Error(offset);
        }
        return new Vector2Int(x, y);
    }

    private static void Error(int offset) {
        throw new DecodeBinaryBundleException(typeof(Vector2Int).ToString(), offset);
    }
}
```

在需要解析的類型上添加屬性`BinaryBundleCustomSerializer`，並指定要解析的類型和解析方法的類。

```csharp
[BinaryBundleObject]
[BinaryBundleCustomSerializer(typeof(Vector2Int), typeof(Vector2IntSerializer))]
public partial class TestClass {
    [BinaryBundleField(0)]
    public Vector2Int a1;
    [BinaryBundleField(0)]
    public List<Vector2Int> a2;

    public TestClass() {}
}
```

## 版號機制

有限的支援版號功能，能根據不同版本來決定是否序列化。

```csharp
// BinaryBundleObject(目前版號, 最低版號)
[BinaryBundleObject(3, 1)]
public sealed partial class Test {
    // BinaryBundleField(順序, 最低版號, 最高版號)
    [BinaryBundleField(0, 1, 2)]
    public int a1;

    // 不帶參數預設為所有版號
    [BinaryBundleField(0)]
    public List<string> a2;

    public Test() {}
}
```

## 底層API

如果有特別需求想自行處理bytes，可以利用內部定義的方法。

```csharp
using BinaryBundle.Internal;

BinaryBundleInternal.EncodeBool(List<byte> bytes, bool data);
BinaryBundleInternal.EncodeByte(List<byte> bytes, byte data);
BinaryBundleInternal.EncodeSByte(List<byte> bytes, sbyte data);
BinaryBundleInternal.EncodeShort(List<byte> bytes, short data);
BinaryBundleInternal.EncodeUShort(List<byte> bytes, ushort data);
BinaryBundleInternal.EncodeInt(List<byte> bytes, int data);
BinaryBundleInternal.EncodeUInt(List<byte> bytes, uint data);
BinaryBundleInternal.EncodeLong(List<byte> bytes, long data);
BinaryBundleInternal.EncodeULong(List<byte> bytes, ulong data);
BinaryBundleInternal.EncodeFloat(List<byte> bytes, float data);
BinaryBundleInternal.EncodeDouble(List<byte> bytes, double data);
BinaryBundleInternal.EncodeString(List<byte> bytes, string data, Encoding encoding = null);

BinaryBundleInternal.DecodeBool(byte[] bytes, ref int offset, ref bool data);
BinaryBundleInternal.DecodeByte(byte[] bytes, ref int offset, ref byte data);
BinaryBundleInternal.DecodeSByte(byte[] bytes, ref int offset, ref sbyte data);
BinaryBundleInternal.DecodeShort(byte[] bytes, ref int offset, ref short data);
BinaryBundleInternal.DecodeUShort(byte[] bytes, ref int offset, ref ushort data);
BinaryBundleInternal.DecodeInt(byte[] bytes, ref int offset, ref int data);
BinaryBundleInternal.DecodeUInt(byte[] bytes, ref int offset, ref uint data);
BinaryBundleInternal.DecodeLong(byte[] bytes, ref int offset, ref long data);
BinaryBundleInternal.DecodeULong(byte[] bytes, ref int offset, ref ulong data);
BinaryBundleInternal.DecodeFloat(byte[] bytes, ref int offset, ref float data);
BinaryBundleInternal.DecodeDouble(byte[] bytes, ref int offset, ref double data);
BinaryBundleInternal.DecodeString(byte[] bytes, ref int offset, ref string data, Encoding encoding = null);
```

## 除錯

想知道詳細的處理過程可以添加編譯定義`BINARY_BUNDLE_DEBUG`

![Debug Log](./readme_assets/debug_log.png)

## 多線程

想在多線程環境中使用請添加編譯定義`BINARY_BUNDLE_THREAD_SAFE`，避免靜態變數存取衝突

## 測試

測試專案在`BinaryBundle.Test`

+ 選單`Window/BinaryBundle/Test/Generate Code` 生成程式碼

+ 選單`Window/BinaryBundle/Test/Run Test` 執行測試範例