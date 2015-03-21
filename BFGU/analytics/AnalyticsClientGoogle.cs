using System;
using System.Collections.Generic;
using UnityEngine;

namespace BFGU.Analytics {
	/// <summary>
	///Wrapper for Google Analytics SDK tested with GoogleAnalyticsV3
	/// </summary>
	public class AnalyticsClientGoogle {

		/// <summary>
		///State that Setup was called.
		/// </summary>
		public static bool initialized {
			get;
			private set;
		}

		/// <summary>
		///GoogleAnalytics is working and we can send events.
		/// </summary>
		public static bool enabled {
			get;
			private set;
		}

		private static GoogleAnalyticsV3 ga;

		public static void Setup( bool enable, Dictionary<string, string> parameters ) {
			if(initialized) {
				return;
			}

			initialized = true;

			if(enable == false) {
				return;
			}

			//start the SDK
			try {
				ga = GoogleAnalyticsV3.getInstance( );
				ga.StartSession( );
			}
			catch(System.Exception e) {
				UnityEngine.Debug.LogError( "Error at setup google analytics" + e );
				return;
			};

			enabled = true;
			//attach to the main events catcher for the entire session
			Analytics.trackEventListeners += TrackEvent;
			Analytics.trackScreenListeners += TrackScreen;

			//make sure the Google Analytics Class Instance is not
			//destroyed when the scene is changed.
			MonoBehaviour.DontDestroyOnLoad( ga.gameObject );
		}


		/// <summary>
		///The wrapper, the link between Analytics main class and Google Analytics.
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="category"></param>
		/// <param name="value"></param>
		public static void TrackEvent( string eventName, string category, Dictionary<string, object> value ) {
			if(initialized == false) {
				UnityEngine.Debug.LogError( "AnalyticsClientGoogle:TrackEvent called before Setup" );
				return;
			}

			if(enabled == false) {
				return;
			}

			//GA does not support payload, so we take only the first param
			//and split it to key => value , label => value

			EventHitBuilder hit = new EventHitBuilder( );
			hit
			.SetEventCategory( category )
			.SetEventAction( eventName )
			.SetEventValue( GetBasicValueFromEvent( value ) )
				//TODO make a way to get label too
			;

			ga.LogEvent( hit );
		}

		public static void TrackScreen( string screenName ) {
			if(initialized == false) {
				UnityEngine.Debug.LogError( "AnalyticsClientGoogle:TrackScreen called before Setup" );
				return;
			}

			if(enabled == false) {
				return;
			}

			ga.LogScreen( screenName );
		}

		protected static long GetBasicValueFromEvent( Dictionary<string, object> value ) {

			if(value != null) {
				int tmp = 0;

				if(value.ContainsKey( "value" )) {
					if(Int32.TryParse( value["value"].ToString( ), out tmp )) {
						return tmp;
					}
				} else {
					//take the first key
					//note, there is no order in a dictionary, so is kinda random
					foreach(string key in value.Keys) {
						if(Int32.TryParse( value[key].ToString( ), out tmp )) {
							return tmp;
						}
					}
				}
			}

			return 0;
		}

		public static void TrackUserId( string userId ) {
			if(initialized == false) {
				UnityEngine.Debug.LogError( "AnalyticsClientGoogle:TrackUserId called before Setup" );
				return;
			}

			if(enabled == false) {
				return;
			}

			ga.SetUserIDOverride( userId );
		}

	}
}
