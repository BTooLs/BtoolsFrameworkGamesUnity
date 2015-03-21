using System;
using System.Collections.Generic;

namespace BFGU.Analytics {
	/// <summary>
	///Only class the project should access regarding events, IAP and user parameters.
	///This way you decouple your proejcts by 3rdparty libraries.
	/// </summary>
	public class Analytics {

		public delegate void eventTrackEvent( string eventName, string category, Dictionary<string, object> value );
		public static event eventTrackEvent trackEventListeners;

		public delegate void eventTrackScreen( string screenName );
		public static event eventTrackScreen trackScreenListeners;

		public delegate void eventTrackUserId( string userId );
		public static event eventTrackUserId trackUserIdListeners;

		//no value or swrve style
		public static void Track( string eventName ) {
			TriggerCustomEvent( eventName, null, null );
		}

		public static void Track( string eventName , string category) {
			TriggerCustomEvent( eventName, category, null );
		}

		//lazy style
		public static void Track( string eventName, int value ) {
			var payload = new Dictionary<string, object>( );
			payload["value"] = value;
			TriggerCustomEvent( eventName, null, payload );
		}

		public static void Track( string eventName, int value, string category ) {
			var payload = new Dictionary<string, object>( );
			payload["value"] = value;
			TriggerCustomEvent( eventName, category, payload );
		}

		//UnityAnalytics style
		public static void Track( string eventName, Dictionary<string, object> value ) {
			TriggerCustomEvent( eventName, null, value );
		}

		//Swrve style
		public static void Track( string eventName, Dictionary<string, string> value, string category ) {
			//transform from strings to object
			var payload = new Dictionary<string, object>( );
			foreach(string key in value.Keys) {
				payload.Add( key, value[key] );
			}
			TriggerCustomEvent( eventName, category, payload );
		}

		//Google Analytics style
//		public static void Track( string eventName, int value, string category, string label ) {
//			var payload = new Dictionary<string, object>( );
//			payload[label] = value.ToString( );
//			TriggerCustomEvent( eventName, category, payload );
//		}

		//Normalization of all event tracking libraries, we de-couple data from Object
		private static void TriggerCustomEvent( string eventName, string category, Dictionary<string, object> value ) {
			if(trackEventListeners != null) {
				trackEventListeners( eventName, category, value );
			}
		}

		/// <summary>
		///Track a view/screen name.
		/// </summary>
		/// <param name="screenName"></param>
		public static void Screen( string screenName ) {
			if(trackScreenListeners != null) {
				trackScreenListeners( screenName );
			}
		}

		public static void SetUniqueIdentifier(string userId){
			if (trackUserIdListeners != null){
				trackUserIdListeners( userId );
			}
		}
	}
}
