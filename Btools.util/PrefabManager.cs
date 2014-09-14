using UnityEngine;
using System.Collections.Generic;

namespace Btools.util {
/// <summary>
/// Used to fetch prefabs from the Resource folder.
/// TODO: test with specific Types of assets.
/// TODO: check for memory leaks or problems.
/// </summary>
	public class PrefabManager {
		private static Dictionary<string, GameObject> cache = new Dictionary<string,GameObject> ();
		public static string prefixPath = "";

		public static GameObject GetPrefab (string path) {
			return GetPrefab (path, true);
		}

		public static GameObject GetPrefab (string path, bool useCache) {

			path = prefixPath + path;

			if (useCache == false) {
				return Resources.Load (path) as GameObject;
			}

			if (cache.ContainsKey (path) == false) {
				cache [path] = Resources.Load (path) as GameObject;
			}
		
			return cache [path];
		}

		/// <summary>
		/// Remove an item cache.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void ClearCache (string path) {
			cache.Remove (prefixPath + path);
		}

		/// <summary>
		/// Remove all objects from the cache.
		/// </summary>
		public static void ClearAllCache () {
			cache.Clear ();
		}

		//public static System.Type getResource(string path, System.Type type){
		//	return Resources.Load(path, type);
		//}
	}
}