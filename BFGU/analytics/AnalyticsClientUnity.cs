using System;
using System.Collections.Generic;

namespace BFGU.Analytics {
	/// <summary>
	///Wrapper for Unity Analytics SDK tested with v1.8.1.
	/// </summary>
	public class AnalyticsClientUnity {

		/// <summary>
		///State that Setup was called.
		/// </summary>
		public static bool initialized {
			get;
			private set;
		}

		/// <summary>
		///UnityAnalytics is working and we can send events.
		/// </summary>
		public static bool enabled {
			get;
			private set;
		}

		public static void Setup( bool enable, Dictionary<string, string> parameters ) {
			if (initialized){
				return;
			}

			initialized = true;

			if (enable == false){
				return;
			}

			if (parameters.ContainsKey("project") == false){
				throw new Exception("AnalyticsClientUnity:Setup:params must contain the 'project' key.");
			}

			//start the SDK
			try {
				UnityEngine.Cloud.Analytics.UnityAnalytics.StartSDK( parameters["project"] );
			}
			catch(System.Exception e) {
				UnityEngine.Debug.LogError( "Error at setup unity analytics" + e );
				return;
			};

			enabled = true;
			//attach to the main events catcher for the entire session
			Analytics.trackEventListeners += TrackEvent;
			Analytics.trackScreenListeners += TrackScreen;

		}


		/// <summary>
		///The wrapper, the link between Analytics main class and Unity Analytics.
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="category"></param>
		/// <param name="value"></param>
		public static void TrackEvent( string eventName, string category, Dictionary<string, object> value ) {
			if (initialized == false){
				UnityEngine.Debug.LogError( "AnalyticsClientUnity:TrackEvent called before Setup" );
				return;
			}

			if (enabled == false){
				return;
			}

			UnityEngine.Cloud.Analytics.UnityAnalytics.CustomEvent( eventName, value );
		}

		public static void TrackScreen( string screenName ) {
			if(initialized == false) {
				UnityEngine.Debug.LogError( "AnalyticsClientUnity:TrackScreen called before Setup" );
				return;
			}

			if(enabled == false) {
				return;
			}

			//Unity Analytics does not know to track views/screens/pages
			//so we use "screenView.screen_name", similar with "unity.sceneLoad.level_name"
			UnityEngine.Cloud.Analytics.UnityAnalytics.CustomEvent(
				"screenView",
				new Dictionary<string, object>{
					{"screen_name", screenName}
			} );
		}
	}
}
