using UnityEngine;

namespace BFGU.util {

	/// <summary>
	/// Pinch zoom, from Unity examples.
	/// </summary>
	public class PinchZoom : MonoBehaviour {
		public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
		public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
		public float maxSizeOrtographic = 2.5f;
		public float minSizeOrtographic = 1f;
		public float maxFieldView = 80f;
		public float minFieldView = 40f;
		public float mouseScrollSpeed = 9f;

		private float deltaMagnitudeDiff = 0;

		private Touch touchZero;
		private Touch touchOne;
		private Vector2 touchZeroPrevPos;
		private Vector2 touchOnePrevPos;

		void Update () {

			// If there are two touches on the device...
			if (Input.touchCount == 2) {
				// Store both touches.
				touchZero = Input.GetTouch (0);
				touchOne = Input.GetTouch (1);

				// Find the position in the previous frame of each touch.
				touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				// Find the magnitude of the vector (the distance) between the touches in each frame.
				// Find the difference in the distances between each frame.
				deltaMagnitudeDiff = (touchZeroPrevPos - touchOnePrevPos).magnitude -
					(touchZero.position - touchOne.position).magnitude;
			}

			if (Input.mousePresent){
				deltaMagnitudeDiff =  Input.GetAxis("Mouse ScrollWheel") * -mouseScrollSpeed;
			}

			if ( deltaMagnitudeDiff != 0 ){
				Zoom (deltaMagnitudeDiff);
				deltaMagnitudeDiff = 0;
			}

		}

		void Zoom(float deltaMagnitudeDiff){

			// If the camera is orthographic...
			if (camera.isOrthoGraphic) {
				// ... change the orthographic size based on the change in distance between the touches.
				camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

				// Make sure the orthographic size never drops below zero.
				camera.orthographicSize = Mathf.Max (camera.orthographicSize, this.minSizeOrtographic);
				camera.orthographicSize = Mathf.Min (camera.orthographicSize, this.maxSizeOrtographic);
			} else {
				// Otherwise change the field of view based on the change in distance between the touches.
				camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

				// Clamp the field of view to make sure it's between 0 and 180.
				camera.fieldOfView = Mathf.Clamp (camera.fieldOfView, minFieldView, maxFieldView);
			}
		}
	}
}
