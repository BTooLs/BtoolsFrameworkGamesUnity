using UnityEngine;
using System.Collections.Generic;

namespace BGU.util {

	/// <summary>
	/// Move camera and animate between more fixed locations (position and rotation).
	/// TODO Event delegates OnStart OnFinish
	/// TODO: use tween library if exists
	/// TODO queues
	/// </summary>
	public class TweenObject : MonoBehaviour {

		/// <summary>
		/// The default duration of the tween.
		/// </summary>
		public float defaultTweenDuration = 2f;
		/// <summary>
		/// Gets a value indicating whether this <see cref="TweenObject"/> is animating now.
		/// </summary>
		/// <value><c>true</c> if is animating now; otherwise, <c>false</c>.</value>
		public bool isAnimatingNow { get; private set; }
		/// <summary>
		/// The transform to animate, default is the attached game object.
		/// </summary>
		public Transform targetTransform;
		Dictionary<string, TweenObjectPosition> positionsList;
		Vector3 positionWanted;
		Quaternion rotationWanted;
		float currentDuration;
		float currentTimeAnimation;

		// Use this for initialization
		void Start () {
			if (this.targetTransform == null) {
				this.targetTransform = this.gameObject.transform;
			}

			positionsList = new Dictionary<string,TweenObjectPosition> ();
			SaveCurrentPosition ("default");
			this.isAnimatingNow = false;
		}

		// Update is called once per frame
		void Update () {
			if (isAnimatingNow == true) {

				if ((this.targetTransform.position - positionWanted).magnitude > 0.01) {
					currentTimeAnimation += Time.deltaTime / currentDuration;
					this.targetTransform.position = Vector3.Lerp (this.targetTransform.position, positionWanted, currentTimeAnimation);
				}

				if ((this.targetTransform.rotation.Equals (rotationWanted) == false)) {
					Quaternion q = Quaternion.Lerp (this.targetTransform.rotation, rotationWanted, currentTimeAnimation);
					if (float.IsNaN (q.x) == false) {
						this.targetTransform.rotation = q;
					}
				}
			}
		}

		public void SaveCurrentPosition (string positionName) {
			TweenObjectPosition pos = new TweenObjectPosition (this.targetTransform);
			positionsList.Add (positionName, pos);
		}

		public void TweenToPosition (Vector3 position, Quaternion rotation, string saveAsPositionName) {
			TweenObjectPosition pos = new TweenObjectPosition (position, rotation);

			positionsList.Add (saveAsPositionName, pos);
			TweenToPosition (position, rotation, this.defaultTweenDuration);
		}

		public void TweenToPosition (string positionName) {

			if (positionsList.ContainsKey (positionName) == false) {
				throw new UnityException ("Cannot find position name in tween camera " + positionName);
			}

			TweenObjectPosition pos = positionsList [positionName];
			TweenToPosition (pos.position, pos.rotation, this.defaultTweenDuration);
		}

		public void TweenToPosition (Vector3 position, Transform lookAt) {
			TweenToPosition (position, lookAt, this.defaultTweenDuration);
		}

		public void TweenToPosition (Vector3 position, Transform lookAt, float duration) {
			Quaternion rotation = Quaternion.LookRotation (lookAt.position - position);
			TweenToPosition (position, rotation, duration);
		}

		public void TweenToPosition (Vector3 position, Quaternion rotation, float duration) {
			positionWanted = position;
			rotationWanted = rotation;
			this.currentDuration = duration;
			currentTimeAnimation = 0;
			isAnimatingNow = true;
		}
	}

}
