using System.Collections.Generic;

namespace BinaryBundle {
	public interface IBinaryBundleSerializable {
		void __BinaryBundleSerialize(List<byte> bytes);
	}

	public interface IBinaryBundleSerializationCallback {
		void OnBeforeSerialize();
		void OnAfterDeserialize();
	}
}
