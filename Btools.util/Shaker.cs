using UnityEngine;

namespace Btools.util {
/// <summary>
/// Move an object on all axis in bursts for a short period of time === shake.
/// Do NOT move the object while is shaking (will be reset) (including other forces like gravity).
/// The algorithm alters the position, is not using any force.
/// </summary>
	public class Shaker : MonoBehaviour {

		/// <summary>
		/// How big the moves are.
		/// </summary>
		public float shakeAmount = 0.2f;
		/// <summary>
		/// How long the shake last.
		/// </summary>
		public float decreaseFactor = 0.7f;

		/// <summary>
		/// The transform to shake, default is the attached object one.
		/// </summary>
		public Transform transformToShake;
		private Vector3 shakeOriginalPosition;
		private Vector3 shake = Vector3.zero;

		// Use this for initialization
		void Start () {
			if (transformToShake == null) {
				transformToShake = this.transform;
			}
		}
	
		// Update is called once per frame
		void Update () {
			if (this.shake.magnitude > 0.01) {

				this.transformToShake.localPosition = new Vector3 (
				this.shakeOriginalPosition.x + (this.shake.x > 0 ? Random.insideUnitSphere.x * this.shakeAmount : 0),
				this.shakeOriginalPosition.y + (this.shake.y > 0 ? Random.insideUnitSphere.z * this.shakeAmount : 0),
				this.shakeOriginalPosition.z + (this.shake.z > 0 ? Random.insideUnitSphere.z * this.shakeAmount : 0)
				);

				//TODO refactor this mess
				this.shake.x -= Time.deltaTime * this.decreaseFactor;
				this.shake.y -= Time.deltaTime * this.decreaseFactor;
				this.shake.z -= Time.deltaTime * this.decreaseFactor;

			} else if (this.shake != Vector3.zero) {
				//TODO optimize this else
				this.shake = Vector3.zero;
				this.transformToShake.localPosition = this.shakeOriginalPosition;
			}
		}

		/// <summary>
		/// Shake the specified shakeForce on ALL axis
		/// </summary>
		/// <param name="shakeForce">Shake force, bigger then 0.01</param>
		public void Shake (float shakeForce) {
			Shake (new Vector3 (shakeForce, shakeForce, shakeForce));
		}

		/// <summary>
		/// Shake the specified shakeForce on specific axis, each ax with a different force.
		/// </summary>
		/// <param name="shakeForce">Shake force.</param>
		public void Shake (Vector3 shakeForce) {

			//save the current position as original only if is not shaking now !
			if (this.shake == Vector3.zero) {
				this.shakeOriginalPosition = this.transformToShake.localPosition;
			}

			this.shake = shakeForce;
		}
	}
}