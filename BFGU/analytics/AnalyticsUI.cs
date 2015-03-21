using System.Collections;
using UnityEngine;

namespace BFGU.Analytics {
	public class AnalyticsUI : MonoBehaviour {

		/// <summary>
		///Quick reference, best used with unity UI callback system.
		/// </summary>
		/// <param name="eventName"></param>
		public void TrackUIEvent( string eventName ) {
			Analytics.Track( eventName, "UI" );
		}
	}
}
