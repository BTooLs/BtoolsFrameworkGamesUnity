using UnityEngine;
using System.Collections.Generic;

namespace Btools.util {

/// <summary>
/// Clicks or touches game objects with colliders and a compoent that has IClickable
/// TODO: ignore mouse drags too. 
/// TODO: make dbl click/tap
/// TODO: make it work with other input devices
/// </summary>
	public class ClickableTrigger : MonoBehaviour {

		public bool listenMouse = true;
		public bool listenTouch;
		public bool debug;
		/// <summary>
		/// Unity units to search for clickable objects, in the click direction, from the camera position.
		/// </summary>
		public int clickMaxDepth = 10;
		/// <summary>
		/// The next valid click cooldown Time.time
		/// </summary>
		private float nextValidClickCooldown = 0;
		/// <summary>
		/// Minimum seconds between two clicks.
		/// </summary>
		private float cooldown = 0.3f;
		/// <summary>
		/// How many objects we can trigger with one click. 0 for all
		/// </summary>
		public int maxTriggeredObjects = 1;

		public delegate void listener (Vector3 clickPosition,Vector3 clickDiretion);
		/// <summary>
		/// Attach your scripts here if you want to know a valid click was made.
		/// </summary>
		public event listener beforeClick;
		/// <summary>
		/// Called after a click and OnClick() was done.
		/// </summary>
		public event listener afterClick;

		/// <summary>
		/// Ignore the current touch.
		/// </summary>
		private bool ignoreTouch = false;
		/// <summary>
		/// For optimization, current touch.
		/// </summary>
		private Touch touch;

		/// <summary>
		/// World coordinates of the click/touch.
		/// </summary>
		private Vector3 clickPosition = Vector3.zero;
		/// <summary>
		/// Direction of the click/touch == camera forward.
		/// </summary>
		private Vector3 direction = Vector3.zero;
		/// <summary>
		/// Draw the Ray in editor mode.
		/// </summary>

		/// <summary>
		/// Ignores the current touch, works only for touches[0] and AFTER Began phase.
		/// </summary>
		public void IgnoreCurrentTouch () {
			this.ignoreTouch = true;
		}

		void Start () {
			this.debug = Application.isEditor;
			this.listenTouch = Application.isMobilePlatform;
			//TODO find a way to detect if has mouse
		}

		// Update is called once per frame
		void Update () {

			if (this.listenMouse && Input.GetMouseButtonUp (0)) {
				this.clickPosition = Input.mousePosition;
			}

			if (this.listenTouch) {
				if (Input.touchCount == 1) {
					this.touch = Input.touches [0];
				
					if (this.touch.phase == TouchPhase.Began) {
						this.ignoreTouch = false;
					}

					if (this.touch.phase == TouchPhase.Moved) {
						this.ignoreTouch = true;
					}

					if (this.touch.phase == TouchPhase.Ended && this.ignoreTouch == false) {
						this.clickPosition = this.touch.position;
					}
				} else if (this.ignoreTouch == false) {
					this.ignoreTouch = true;//cover the case when we have multi touch and ignore remains false
				}
			}

			//no click was found
			if (this.clickPosition == Vector3.zero) {
				return;
			}

			//if the click is on cooldown
			if (Time.time <= this.nextValidClickCooldown) {
				this.clickPosition = Vector3.zero;
				return;
			}

			//if the click is valid
			SetClickCooldown ();

			doClick ();
		}

		/// <summary>
		/// Ignore all the clicks/touches for the next this.cooldown seconds.
		/// </summary>
		public void SetClickCooldown () {
			this.nextValidClickCooldown = Time.time + this.cooldown;
		}

		private void doClick () {
			Ray ray = camera.ScreenPointToRay (this.clickPosition);
			this.direction = ray.direction;
		
			if (this.beforeClick != null) {
				this.beforeClick (this.clickPosition, this.direction);
			}

			if (debug) {
				Debug.DrawRay (ray.origin, ray.direction * clickMaxDepth, Color.yellow, 3f);
				Debug.Log ("Click at " + this.clickPosition + " direction " + ray.direction);
			}

			RaycastHit[] hits;
			if (this.maxTriggeredObjects == 1) {
				hits = new RaycastHit[1];
				Physics.Raycast (ray, out hits [0], this.clickMaxDepth);
			} else {
				hits = Physics.RaycastAll (ray, clickMaxDepth);
			}

			int found = 0;
			IClickable script;

			foreach (RaycastHit h in hits) {
				script = h.collider.gameObject.GetComponent (typeof(IClickable)) as IClickable;
				if (script != null) {
					found++;
					script.TriggerOnClickEvent ();
					if (found == this.maxTriggeredObjects) {
						break;
					}
				}
			}
		
			if (this.afterClick != null) {
				this.afterClick (this.clickPosition, this.direction);
			}

			this.clickPosition = Vector3.zero;
			this.direction = Vector3.zero;
		}
	}

}