using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using Mochibits.Utility;
using Newtonsoft.Json;
using UnityEngine;

namespace Mochibits.Utility
{
	public static class Utility
	{
		public static void ClearConsole()
		{
#if UNITY_EDITOR
			var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.SceneView));
			var type = assembly.GetType("UnityEditor.LogEntries");
			var method = type.GetMethod("Clear");
			method.Invoke(new object(), null);
#endif
		}

		public static string RandomString(int length)
		{
			return new string(Enumerable.Range(0, length)
							.Select(s => (char)SystemRandom.Range((int)'!', (int)'~'))
							.ToArray());
		}

		public static bool ByteArraysEqual(byte[] b1, byte[] b2)
		{
			if (b1 == b2) return true;
			if (b1 == null || b2 == null) return false;
			if (b1.Length != b2.Length) return false;
			for (int i = 0; i < b1.Length; i++)
			{
				if (b1[i] != b2[i]) return false;
			}
			return true;
		}

		public static object CreateInstance(string strFullyQualifiedName)
		{
			System.Type t = System.Type.GetType(strFullyQualifiedName);
			return System.Activator.CreateInstance(t);
		}

		// wrap any item into IEnumerable
		public static IEnumerable<T> Yield<T>(this T item)
		{
			yield return item;
		}

		public static IEnumerable<T> EnumEnumerable<T>()
		{
			if (!typeof(T).IsEnum)
			{
				return null;
			}

			return System.Enum.GetValues(typeof(T)).Cast<T>();
		}

		public static T ToggleEnum<T>(this T current)
		{
			if (!typeof(T).IsEnum)
			{
				return current;
			}

			var l = EnumEnumerable<T>().ToList();

			int index = l.IndexOf(current);

			// not found
			if (index == -1)
			{
				return current;
			}

			// return the next index
			return l[++index % l.Count];
		}

		public static IEnumerable<T> EnumEnumerable<T>(T except)
		{
			if (!typeof(T).IsEnum)
			{
				return null;
			}

			return System.Enum.GetValues(typeof(T)).Cast<T>().Except(new T[] { except });
		}

		public static T RandomEnum<T>()
		{
			var l = EnumEnumerable<T>();
			if (l == null)
			{
				return default(T);
			}
			return l.RandomOne<T>();
		}

		public static T RandomEnum<T>(T except)
		{
			var l = EnumEnumerable<T>(except);
			if (l == null)
			{
				return default(T);
			}
			return l.RandomOne<T>();
		}

		public static int EnumCount<T>()
		{
			if (!typeof(T).IsEnum)
			{
				return 0;
			}
			return System.Enum.GetNames(typeof(T)).Length;
		}

		// enumValue should be in Enum type
		// See ToEnum.Extensions for more Enum functions
		public static T EnumToEnum<T>(object enumValue)
		{
			return (T)System.Enum.Parse(typeof(T), enumValue.ToString());
		}

		//
		// MD5
		public static string MD5SumFlurry(string strToEncrypt)
		{
			//System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
			byte[] bytes = System.Text.Encoding.UTF32.GetBytes(strToEncrypt);

			// encrypt bytes
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] hashBytes = md5.ComputeHash(bytes);

			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";

			for (int i = 0; i < hashBytes.Length; i++)
			{
				hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
			}

			return hashString.PadLeft(32, '0');

			// byte[] asciiBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(strToEncrypt);
			// byte[] hashedBytes = System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(asciiBytes);
			// return System.BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
		}

		public static string MD5(string stringToHash)
		{
			System.Security.Cryptography.MD5 alg = System.Security.Cryptography.MD5.Create();
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			byte[] hash = alg.ComputeHash(enc.GetBytes(stringToHash));
			return System.Convert.ToBase64String(hash);
		}


	}

	public static class MethodExtensionForMonoBehaviourTransform
	{
		/// <summary>
		/// Gets or add a component. Usage example:
		/// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
		/// </summary>
		static public T GetOrAddComponent<T>(this UnityEngine.Component child) where T : UnityEngine.Component
		{
			T result = child.GetComponent<T>();
			if (result == null)
			{
				result = child.gameObject.AddComponent<T>();
			}
			return result;
		}
	}

	public static class NumberExtension
	{
		// digits from the right -> left
		// e.g: 1234 -> 4, 3, 2, 1
		public static IEnumerable<int> Digits(this int n)
		{
			n = System.Math.Abs(n);
			while (n > 9)
			{
				yield return n % 10;
				n /= 10;
			}
			yield return n;
		}

		public static int FirstDigit(this int n)
		{
			n = System.Math.Abs(n);
			while (n > 9)
			{
				n /= 10;
			}
			return n;
		}

		public static int NumOfDigits(this int n)
		{
			n = System.Math.Abs(n);
			return (int)System.Math.Floor(System.Math.Log10((double)n) + 1);
		}

		public static int ToInt(this int[] source)
		{
			int result = 0;
			for (int i = 0; i < source.Length; i++)
			{
				result += (int)System.Math.Pow(10, (double)i) * source[i];
			}
			return result;
		}

		public static int LvRRound(this int value, int roundingDigitIndex = 4)
		{
			int[] digits = value.Digits().ToArray();

			for (int i = 0; i < digits.Length && i <= roundingDigitIndex; i++)
			{
				digits[i]++;
				for (int r = i - 1; r >= 0; r--)
				{
					digits[r] = 0;
				}
			}
			return digits.ToInt();
		}

		public static string Humanize(this double num)
		{
			return ((long)num).Humanize();
		}
		public static string Humanize(this float num)
		{
			return ((long)num).Humanize();
		}
		public static string Humanize(this int num)
		{
			return ((long)num).Humanize();
		}
		public static string Humanize(this long num)
		{
			if (num >= 100000000000000) { return (num / 1000000000000D).ToString("0.#T"); }
			if (num >= 1000000000000) { return (num / 1000000000000D).ToString("0.##T"); }
			if (num >= 100000000000) { return (num / 1000000000D).ToString("0.#B"); }
			if (num >= 1000000000) { return (num / 1000000000D).ToString("0.##B"); }
			if (num >= 100000000) { return (num / 1000000D).ToString("0.#M"); }
			if (num >= 1000000) { return (num / 1000000D).ToString("0.##M"); }
			if (num >= 100000) { return (num / 1000D).ToString("0.#k"); }
			if (num >= 10000) { return (num / 1000D).ToString("0.##k"); }

			return num.ToString("0");
		}
	}

	public static class CameraExtension
	{
		// ViewportToWorld
		public static Vector3 ViewportToWorld(this Camera camera, Vector3 top, Vector3 bottom = new Vector3())
		{
			return camera.ViewportToWorldPoint(top) - camera.ViewportToWorldPoint(bottom);
		}
		public static float ViewportToWorldHeight(this Camera camera, Vector3 top, Vector3 bottom = new Vector3())
		{
			return ViewportToWorld(camera, top, bottom).y;
		}
		public static float ViewportToWorldWidth(this Camera camera, Vector3 top, Vector3 bottom = new Vector3())
		{
			return ViewportToWorld(camera, top, bottom).x;
		}

		// ScreenToWorld
		public static Vector3 ScreenToWorld(this Camera camera, Vector3 top, Vector3 bottom = new Vector3())
		{
			return camera.ScreenToWorldPoint(top) - camera.ScreenToWorldPoint(bottom);
		}
		public static float ScreenToWorldHeight(this Camera camera, Vector3 top, Vector3 bottom = new Vector3())
		{
			return ScreenToWorld(camera, top, bottom).y;
		}
		public static float ScreenToWorldWidth(this Camera camera, Vector3 top, Vector3 bottom = new Vector3())
		{
			return ScreenToWorld(camera, top, bottom).x;
		}
	}

	public static class ObjectToDictionaryHelper
	{
		public static readonly JsonSerializerSettings prettyDateFormatSettings = new JsonSerializerSettings()
		{
			//Converters = new List<JsonConverter>() { new Mochibits.JsonConverters.PrettyDateTimeConverter() }
		};

		public static bool IsDictionary(this object o)
		{
			if (o == null) return false;
			return o is IDictionary &&
				   o.GetType().IsGenericType &&
				   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
		}

		public static string ToPrettyString(this object source, int maxStringLength = 50)
		{
			if (source == null) { return null; }

			// no need to convert to Dictionary if it is of type
			if (source.IsDictionary())
			{
				//CustomDebug.Log("Dictionary type detected");
				return JsonConvert.SerializeObject(source, Formatting.Indented, prettyDateFormatSettings);
			}

			//CustomDebug.Log("object detected");
			ObjectToDictionaryHelper.maxStringLength = maxStringLength;
			IDictionary<string, object> dictObject = source.ToDictionary();
			return JsonConvert.SerializeObject(dictObject, Formatting.Indented, prettyDateFormatSettings);
		}

		public static IDictionary<string, object> ToDictionary(this object source)
		{
			return source.ToDictionary<object>();
		}

		public static IDictionary<string, T> ToDictionary<T>(this object source)
		{
			if (source == null)
				ThrowExceptionWhenSourceArgumentIsNull();

			var dictionary = new Dictionary<string, T>();

			System.Type type = source.GetType();
			const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
			MemberInfo[] members = type.GetFields(bindingFlags).Cast<MemberInfo>().Concat(type.GetProperties(bindingFlags)).ToArray();

			foreach (MemberInfo memberInfo in members)
				AddPropertyToDictionary<T>(memberInfo, source, dictionary);

			return dictionary;
		}

		public static object GetValue(this MemberInfo memberInfo, object source)
		{
			switch (memberInfo.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)memberInfo).GetValue(source);
				case MemberTypes.Property:
					return ((PropertyInfo)memberInfo).GetValue(source);
				//try { return ((PropertyInfo)member).GetValue(source, null); }
				//catch { return null; }
				default:
					//throw new System.ArgumentException("MemberInfo is not of type FieldInfo or PropertyInfo", "member");
					CustomDebug.LogError("MemberInfo is not of type FieldInfo or PropertyInfo - returning null");
					return null;
			}
		}

		public static void SetValue(this MemberInfo memberInfo, object obj, object value)
		{
			switch (memberInfo.MemberType)
			{
				case MemberTypes.Field:
					obj.GetType().GetField(memberInfo.Name).SetValue(obj, value);
					break;
				case MemberTypes.Property:
					obj.GetType().GetProperty(memberInfo.Name).SetValue(obj, value);
					break;
				default:
					break;
			}
		}


		private static IEnumerable<IDictionary<string, object>> ToDictionary(this IEnumerable source)
		{
			if (source == null)
			{
				yield return null;
			}
			else
			{
				foreach (var item in source)
				{
					yield return item.ToDictionary();
				}
			}
		}

		private static Dictionary<TKey, IDictionary<string, object>> ToDictionary<TKey, T>(this Dictionary<TKey, T> source)
		{
			if (source == null)
			{
				return null;
			}
			return source.ToDictionary(k => k.Key, v => v.ToDictionary());
		}

		private static IEnumerable<DictionaryEntry> CastDict(IDictionary dictionary)
		{
			foreach (DictionaryEntry entry in dictionary)
			{
				yield return entry;
			}
		}

		public static int maxStringLength = 50;

		private static void AddPropertyToDictionary<T>(MemberInfo memberInfo, object source, Dictionary<string, T> dictionary)
		{
			// get the value
			object value = memberInfo.GetValue(source);

			if (value != null)
			{
				// get the type
				System.Type type = value.GetType();

				// determine what type
				if (type.Namespace.IsNullOrEmpty())
				{
					if (type.IsEnum)
					{
						//CustomDebug.Log(memberInfo.Name + " - custom enum type:" + type);
						value = type + "." + value.ToString();
					}
					else if (type.IsArray)
					{
						if (!type.GetElementType().IsEnum)
						{
							// custom array
							//CustomDebug.Log(memberInfo.Name + " - custom array type:" + type);
							value = (value as IEnumerable).ToDictionary();
						}
					}
					else
					{
						// custom Dictionary
						//CustomDebug.Log(memberInfo.Name + " - custom type:" + type);
						value = value.ToDictionary();
					}
				}
				else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
				{
					// custom Dictionary
					//CustomDebug.Log(memberInfo.Name + " - custom dictionary type:" + type);
					value = CastDict(value as IDictionary).ToDictionary(k => k.Key, v => v.Value).ToDictionary();
				}
				else if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
				{
					// custom array of dictionary
					//CustomDebug.Log(memberInfo.Name + " - custom Enumerable type:" + type);
					value = (value as IEnumerable).ToDictionary();
				}
				else if (maxStringLength > 0 && type == typeof(string))
				{
					string stringValue = value as string;
					if (stringValue.Length > maxStringLength)
					{
						value = stringValue.Substring(0, maxStringLength) + "...";
					}
				}
			}

			if (IsOfType<T>(value))
			{
				dictionary.Add(memberInfo.Name, (T)value);
			}
		}

		private static bool IsOfType<T>(object value)
		{
			return value is T;
		}

		private static void ThrowExceptionWhenSourceArgumentIsNull()
		{
			throw new System.ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
		}
	}

	public static class DictionaryMethods
	{
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> list)
		{
			return list.ToDictionary(x => x.Key, x => x.Value);
		}

		public static TValue ValueAtOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue value;
			return dictionary.TryGetValue(key, out value) ? value : default(TValue);
		}

		public static TValue ValueAtOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			TValue value;
			return dictionary.TryGetValue(key, out value) ? value : defaultValue;
		}

		public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> collection)
		{
			if (collection == null)
			{
				CustomDebug.LogWarning("AddRange collection is null");
				return;
			}
			foreach (var item in collection)
			{
				// check for duplicate key
				if (source.ContainsKey(item.Key))
				{
					CustomDebug.LogWarning("collection contains duplicate KeyValuePair: " + item);
					continue;
				}

				// add to source
				source.Add(item.Key, item.Value);
			}
		}

		public static IEnumerable<TValue> SelectValues<TKey, TValue>(this Dictionary<TKey, TValue> source)
		{
			return source.Select(v => v.Value);
		}

		// merge two dictionaries takes the bigger TValue
		public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> other)
		{
			return source.Merge(other, g => g.Max());
		}

		// merge two dictionaries with custom compare func
		public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
						this Dictionary<TKey, TValue> source,
						Dictionary<TKey, TValue> other,
						System.Func<IGrouping<TKey, TValue>, TValue> compareFunc)
		{
			if (source.IsNullOrEmpty())
			{
				return other;
			}

			if (other.IsNullOrEmpty())
			{
				return source;
			}

			IEnumerable<Dictionary<TKey, TValue>> dictionaries = new Dictionary<TKey, TValue>[] { source, other };
			return dictionaries.SelectMany(dict => dict)
							.ToLookup(pair => pair.Key, pair => pair.Value)
							.ToDictionary(group => group.Key, compareFunc);
		}

		public static bool Equals<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> other)
		{
			TKey[] sourceKeys = source.Keys.ToArray();
			TKey[] otherKeys = other.Keys.ToArray();

			// prelimenary test
			if (sourceKeys.Length != otherKeys.Length || sourceKeys.Except(otherKeys).Any() || otherKeys.Except(sourceKeys).Any())
			{
				return false;
			}

			// Value Equals test
			foreach (TKey key in sourceKeys)
			{
				if (!source[key].Equals(other[key]))
				{
					return false;
				}
			}

			return true;
		}
	}

	public static class ExtensionMethods
	{
		// Deep clone
		public static T DeepClone<T>(this T a)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, a);
				stream.Position = 0;
				return (T)formatter.Deserialize(stream);
			}
		}

		// usage: CustomDebug.Log(source.ToString<T>());
		public static string ToString<T>(this IEnumerable<T> source, string delimiter = ",")
		{
			if (source.IsNullOrEmpty()) { return null; }
			return string.Join(delimiter, source.Select(s => s.ToString()).ToArray());
		}

		public static IEnumerable<Vector2> ToVector2(this IEnumerable<int> source)
		{
			// return ToVector2(source.Select(v => (float)v));
			if (!source.IsNullOrEmpty())
			{
				float i = 1f;
				foreach (var item in source)
				{
					yield return new Vector2(i++, (float)item);
				}
			}
		}

		public static IEnumerable<Vector2> ToVector2(this IEnumerable<Vector3> source)
		{
			foreach (var vector in source)
			{
				yield return new Vector2(vector.x, vector.y);
			}
		}

		public static IEnumerable<T> ToEnumerable<T>(this T item)
		{
			yield return item;
		}

		public static IEnumerable<Vector2> ToVector2(this IEnumerable<float> source)
		{
			if (!source.IsNullOrEmpty())
			{
				float i = 1f;
				foreach (var item in source)
				{
					yield return new Vector2(i++, item);
				}
			}
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || !source.Any();
		}

		public static bool IsNullOrEmpty<T>(this T[] source)
		{
			return source == null || source.Length == 0;
		}

		public static T RandomOne<T>(this T[] source)
		{
			if (source.IsNullOrEmpty())
			{
				return default;
			}
			return source[SystemRandom.Range(source.Length)];
		}

		public static T RandomOne<T>(this IEnumerable<T> source)
		{
			return source.Skip(SystemRandom.Range(source.Count())).FirstOrDefault();
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			if (source.IsNullOrEmpty()) return Enumerable.Empty<T>();
			return source.ShuffleIterator();
		}

		private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source)
		{
			var buffer = source.ToList();

			for (int i = 0; i < buffer.Count; i++)
			{
				int j = SystemRandom.Range(i, buffer.Count);
				yield return buffer[j];
				buffer[j] = buffer[i];
			}
		}

		public static bool IndexCheck<T>(this T[] array, int index)
		{
			if (array.IsNullOrEmpty())
			{
				return false;
			}

			if (index < 0 || index >= array.Length)
			{
				return false;
			}

			return true;
		}

		public static bool IndexCheck<T>(this T[] array, int index, out T element)
		{
			element = default;

			if (array.IsNullOrEmpty())
			{
				return false;
			}

			if (index < 0 || index >= array.Length)
			{
				return false;
			}

			element = array[index];
			return true;
		}

		public static int RandomIndex<T>(this T[] array)
		{
			return SystemRandom.Range(array.Length);
		}

		public static void Reverse<T>(this T[] array)
		{
			for (int n = 0; n < array.Length / 2; n++)
			{
				int i = array.Length - 1 - n;
				T temp = array[i];
				array[i] = array[n];
				array[n] = temp;
			}
		}

		public static void Shuffle<T>(this T[] array)
		{
			for (int n = 0; n < array.Length; n++)
			{
				int i = Random.Range(n, array.Length);
				T temp = array[i];
				array[i] = array[n];
				array[n] = temp;
			}
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			for (int n = 0; n < list.Count; n++)
			{
				int i = Random.Range(n, list.Count);
				T temp = list[i];
				list[i] = list[n];
				list[n] = temp;
			}
		}

		// randomly remove items off the list
		public static int RandomRemove<T>(this IList<T> list)
		{
			// randomly remove items off an list
			int takes = SystemRandom.Range(list.Count - 1);
			for (int t = 0; t < takes; t++)
			{
				list.RemoveAt(SystemRandom.Range(list.Count));
			}
			return takes;
		}

		// randomly remove items off the list
		public static int RandomRemove<T>(this IList<T> list, System.Action<int, T> removeItemAction)
		{
			// randomly remove items off an list
			int takes = SystemRandom.Range(list.Count - 1);
			for (int t = 0; t < takes; t++)
			{
				int removeIndex = SystemRandom.Range(list.Count);

				removeItemAction(removeIndex, list[removeIndex]);

				list.RemoveAt(removeIndex);
			}
			return takes;
		}

		public static IEnumerator SyncList<T>(this List<T> list, List<T> masterList, float wait = 0.05f)
		{
			if (list == null) { list = new List<T>(); }
			if (masterList.IsNullOrEmpty()) { list.Clear(); yield return null; }

			int index = 0;
			for (; index < list.Count && index < masterList.Count; index++)
			{
				list[index] = masterList[index];
				yield return new WaitForSeconds(wait);
			}
			// list is longer than masterList
			if (index < list.Count)
			{
				// remove index -> n
				list.RemoveRange(index, list.Count - index);
			}
			// masterList is longer than list
			else if (index < masterList.Count)
			{
				// add
				for (; index < masterList.Count; index++)
				{
					list.Add(masterList[index]);
					yield return new WaitForSeconds(wait);
				}
			}
		}

		// ValidateCount
		// list
		public static bool ValidateCount<T>(this IList<T> source, int count)
		{
			return source != null && source.Count == count;
		}
		// array
		public static bool ValidateCount<T>(this T[] source, int count)
		{
			return source != null && source.Length == count;
		}
		// IEnumerable
		public static bool ValidateCount<T>(this IEnumerable<T> source, int count)
		{
			return source != null && source.Count() == count;
		}

		public static void ForEach<T>(this IEnumerable<T> source, System.Action<T> action)
		{
			if (source == null) return;
			if (action == null) return;
			foreach (T item in source)
			{
				action(item);
			}
		}

		public static bool TypeCheck<T>(this T a, object obj, System.Func<T, bool> func = null) where T : class
		{
			if (obj == null || a.GetType() != obj.GetType())
			{
				return false;
			}
			T objT = obj as T;
			if ((System.Object)objT == null)
			{
				return false;
			}
			if (func == null) { return true; }
			return func(objT);
		}
	}

	public static class BoundsExtension
	{
		public static float RandomX(this UnityEngine.Bounds bounds, float radius = 0f)
		{
			return SystemRandom.Range(bounds.min.x + radius, bounds.max.x - radius);
		}
		public static float RandomY(this UnityEngine.Bounds bounds, float radius = 0f)
		{
			return SystemRandom.Range(bounds.min.y + radius, bounds.max.y - radius);
		}
		public static float RandomZ(this UnityEngine.Bounds bounds, float radius = 0f)
		{
			return SystemRandom.Range(bounds.min.z + radius, bounds.max.z - radius);
		}
		// generate 2D point
		public static Vector3 RandomPoint(this UnityEngine.Bounds bounds, float radius = 0f)
		{
			return new Vector3(bounds.RandomX(radius), bounds.RandomY(radius));
		}
		// generate 3D position
		public static Vector3 RandomPosition(this UnityEngine.Bounds bounds, float radius = 0f)
		{
			return new Vector3(bounds.RandomX(radius), bounds.RandomY(radius), bounds.RandomZ(radius));
		}
	}

	public static class FloatingEqualsExtension
	{
		public static bool AlmostEquals(this double value1, double value2, double precision = 0.00000001)
		{
			return (System.Math.Abs(value1 - value2) <= precision);
		}
	}

	public static class StringExtension
	{
		public static string FirstCharToUpper(this string input)
		{
			if (!input.IsNullOrEmpty())
			{
				return input.First().ToString().ToUpper() + input.Substring(1);
			}
			else
			{
				return input;
			}
		}

		public static string Uncamel(this string str)
		{
			return Regex.Replace(str, "[A-Z]", " $0").Trim();
		}

		public static readonly string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
		public static string RemoveControlCharacters(this string str)
		{
			if (str == null) return null;

			// remove byte order marker
			if (str.StartsWith(_byteOrderMarkUtf8))
			{
				str = str.Remove(0, _byteOrderMarkUtf8.Length);
			}

			StringBuilder newString = new StringBuilder();
			char ch;
			for (int i = 0; i < str.Length; i++)
			{
				ch = str[i];
				if (!char.IsControl(ch))
				{
					newString.Append(ch);
				}
			}

			return newString.ToString();
		}

		private static string DomainMapper(Match match)
		{
			// IdnMapping class with default property values.
			IdnMapping idn = new IdnMapping();
			string domainName = match.Groups[2].Value;
			try
			{
				domainName = idn.GetAscii(domainName);
			}
			catch
			{
				isEmailInvalidFlag = true;
			}
			return match.Groups[1].Value + domainName;
		}

		private static bool isEmailInvalidFlag = false;
		public static bool IsEmailValid(this string emailString)
		{
			isEmailInvalidFlag = false;
			if (emailString.IsNullOrEmpty())
				return false;

			// Use IdnMapping class to convert Unicode domain names.
			emailString = Regex.Replace(emailString, @"(@)(.+)$", DomainMapper);
			if (isEmailInvalidFlag)
				return false;

			// Return true if strIn is in valid e-mail format.
			return Regex.IsMatch(emailString,
							@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
							@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
							RegexOptions.IgnoreCase);
		}

		public static bool IsNullOrEmpty(this string str)
		{
			return System.String.IsNullOrEmpty(str);
		}

		public static bool IsNullOrWhiteSpace(this string str)
		{
			return System.String.IsNullOrEmpty(str) || str.Trim().Length == 0;
		}

		public static T DeserializeObject<T>(this string str, params JsonConverter[] converters)
		{
			try
			{
				if (converters.IsNullOrEmpty()) { return JsonConvert.DeserializeObject<T>(str); }
				return JsonConvert.DeserializeObject<T>(str, converters);
			}
			catch (System.Exception e)
			{
				CustomDebug.LogWarning(string.Format("Exception:{0} in deserializing:{1}", e, str));
				return default(T);
			}
		}

		public static bool TryPopulateObject(this string str, object obj)
		{
			if (str.IsNullOrEmpty())
			{
				return false;
			}

			try
			{
				JsonConvert.PopulateObject(str, obj);
				return true;
			}
			catch (System.Exception e)
			{
				CustomDebug.LogWarning(string.Format("Exception:{0} in deserializing:{1}", e, str));
				return false;
			}
		}
	}
}