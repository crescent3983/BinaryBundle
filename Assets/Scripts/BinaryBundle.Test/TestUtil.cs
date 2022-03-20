using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BinaryBundleTest {
	public static class TestUtil {

		public static bool IsEqual<T>(T t1, T t2) where T : IEquatable<T> {
			return t1.Equals(t2);
		}

		public static bool IsEqual<T>(T t1, T t2, string title) where T : IEquatable<T> {
			if (IsEqual(t1, t2)) {
				return true;
			}
			Debug.LogError($"[{title}] {t1} != {t2}");
			return false;
		}

		public static bool IsNotEqual<T>(T t1, T t2, string title) where T : IEquatable<T> {
			if (!IsEqual(t1, t2)) {
				return true;
			}
			Debug.LogError($"[{title}] {t1} == {t2}");
			return false;
		}

		public static bool IsEqual(Enum t1, Enum t2) {
			return t1.Equals(t2);
		}

		public static bool IsEqual(Enum t1, Enum t2, string title) {
			if (IsEqual(t1, t2)) {
				return true;
			}
			Debug.LogError($"[{title}] {t1} != {t2}");
			return false;
		}

		public static bool IsNotEqual(Enum t1, Enum t2, string title) {
			if (!IsEqual(t1, t2)) {
				return true;
			}
			Debug.LogError($"[{title}] {t1} == {t2}");
			return false;
		}

		public static bool IsEqual<T>(IEnumerable<T> t1, IEnumerable<T> t2) where T : IEquatable<T> {
			if (t1 == t2 ||
				(t1 == null && t2.Count() == 0) ||
				(t2 == null && t1.Count() == 0) ||
				t1.SequenceEqual(t2)) {
				return true;
			}
			return false;
		}

		public static bool IsEqual<T>(IEnumerable<T> t1, IEnumerable<T> t2, string title) where T : IEquatable<T> {
			if (IsEqual(t1, t2)) {
				return true;
			}
			Debug.LogError($"[{title}] {t1} != {t2}");
			return false;
		}

		public static bool IsNotEqual<T>(IEnumerable<T> t1, IEnumerable<T> t2, string title) where T : IEquatable<T> {
			if (!IsEqual(t1, t2)) {
				return true;
			}
			Debug.LogError($"[{title}] {t1} == {t2}");
			return false;
		}

		public static bool IsEqual<K, V>(Dictionary<K, V> t1, Dictionary<K, V> t2) where K : IEquatable<K> where V : IEquatable<V> {
			return IsEqual(t1?.Keys, t2?.Keys) && IsEqual(t1?.Values, t2?.Values);
		}

		public static bool IsEqual<K, V>(Dictionary<K, V> t1, Dictionary<K, V> t2, string title) where K : IEquatable<K> where V : IEquatable<V> {
			if (IsEqual(t1, t2)) {
				return true;
			}
			Debug.LogError($"[{title}] {t1} != {t2}");
			return false;
		}

		public static bool IsNotEqual<K,V>(Dictionary<K,V> t1, Dictionary<K, V> t2, string title) where K : IEquatable<K> where V : IEquatable<V> {
			if (!IsEqual(t1, t2)) {
				return true;
			}
			Debug.LogError($"[{title}] {t1} == {t2}");
			return false;
		}
	}
}
