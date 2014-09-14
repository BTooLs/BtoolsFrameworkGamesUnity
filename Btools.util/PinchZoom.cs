using UnityEngine;

namespace Btools.util {

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
	
		void Update () {
			// If there are two touches on the device...
			if (Input.touchCount != 2) { 
				return;
			}

			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);
		
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
		
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
		
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
		
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