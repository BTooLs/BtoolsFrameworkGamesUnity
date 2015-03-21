using UnityEngine;
using System.Collections;

namespace BFGU.util {
	public class TouchDragCamera : MonoBehaviour {

		public bool dragEnabled = true;
		public bool reverseAxis = false;

		public bool thisTouchIsDrag { get; private set; }
		//TODO set a max drag value per frame or dampening
		//TODO set a Flinging effect

		public float boundryMinX;
		public float boundryMaxX;
		public float boundryMinZ;
		public float boundryMaxZ;

		//optimization, do not create temp vars in Update
		private float addToX;
		private float addToZ;

		// Update is called once per frame
		void Update () {
			if (dragEnabled == false || Input.touchCount != 1) {
				return;
			}

			Touch t = Input.touches [0];

			if (t.phase == TouchPhase.Began) {
				thisTouchIsDrag = false;
			}

			if (t.phase == TouchPhase.Moved) {
				addToX = t.deltaPosition.x;
				addToZ = t.deltaPosition.y;

				if (reverseAxis) {
					addToX = addToX + addToZ;
					addToZ = addToZ - addToX;
					addToX = addToZ - addToX;
				}

				transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x + addToX, boundryMinX, boundryMaxX),
				transform.position.y,
				Mathf.Clamp (transform.position.z + addToZ, boundryMinZ, boundryMaxZ)
				);

				thisTouchIsDrag = true;
			}

			if (t.phase == TouchPhase.Ended) {
				thisTouchIsDrag = false;
			}
		}
	}
}
