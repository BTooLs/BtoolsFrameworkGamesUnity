using UnityEngine;

namespace Btools.util {
	public class TweenObjectPosition {
		public Vector3 position { get; private set; }

		public Quaternion rotation { get; private set; }
	
		public TweenObjectPosition (Transform transform) {
			this.position = transform.position;
			this.rotation = transform.rotation;
		}
	
		public TweenObjectPosition (Vector3 position, Quaternion rotation) {
			this.rotation = rotation;
			this.position = position;
		}
	}
}