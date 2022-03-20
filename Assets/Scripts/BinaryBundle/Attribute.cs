using System;

namespace BinaryBundle {

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public class BinaryBundleObjectAttribute : Attribute {
		public ushort Version { get; private set; }
		public ushort Minimum { get; private set; }

		public BinaryBundleObjectAttribute(ushort version = 0, ushort minimum = 0) {
			this.Version = version;
			this.Minimum = minimum;
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
	public class BinaryBundleCustomSerializerAttribute : Attribute {
		public Type Target { get; private set; }
		public Type Serializer { get; private set; }

		public BinaryBundleCustomSerializerAttribute(Type target, Type serializer) {
			this.Target = target;
			this.Serializer = serializer;
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class BinaryBundleFieldAttribute : Attribute {

		public int Order { get; private set; }
		public ushort Min { get; private set; }
		public ushort Max { get; private set; }

		public BinaryBundleFieldAttribute(int order, ushort min = 0, ushort max = UInt16.MaxValue) {
			this.Order = order;
			this.Min = min;
			this.Max = max;
		}
	}
}
