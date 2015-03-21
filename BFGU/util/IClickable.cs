using UnityEngine;

namespace BFGU.util {
	public delegate void IClickableListener ();

	/// <summary>
	/// Makes an item clickable, trough the ClickableTrigger script
	/// </summary>
	public class IClickable : MonoBehaviour {
		public event IClickableListener OnClickEvent;

		public void TriggerOnClickEvent () {
			if (this.OnClickEvent == null || this.OnClickEvent.GetInvocationList ().Length == 0) {
				return;
			}

			this.OnClickEvent ();
		}
	}

}
