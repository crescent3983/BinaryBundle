using System;

namespace BinaryBundle {
	public class EncodeBinaryBundleException : Exception {
		public EncodeBinaryBundleException(string message) : base(message) {

		}
	}

	public class DecodeBinaryBundleException : Exception {
		public DecodeBinaryBundleException(string message) : base(message) {

		}

		public DecodeBinaryBundleException(string name, int offset) : base($"Decode {name} failed at offset {offset}") {

		}
	}

	public class VersionNotMatchException : Exception {
		public VersionNotMatchException(uint version, uint min, uint max) : base($"Version {version} is not in supported range ({min}, {max})") {

		}
	}
}
