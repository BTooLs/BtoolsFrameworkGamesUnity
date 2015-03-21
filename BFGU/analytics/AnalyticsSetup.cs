using System.Collections.Generic;
using UnityEngine;

namespace BFGU.Analytics {
	/// <summary>
	///Put this on any object in your first scene (loading, menu etc).
	///The object will persist on change scene along with the GAv3 object.
	/// </summary>
	public class AnalyticsSetup : MonoBehaviour {

		[Tooltip( "Track each scene load as a screen view." )]
		public bool trackSceneLoad = true;

		[Header( "Unity Analytics" )]
		public bool UAEnabled = true;
		[Tooltip( "The tracking code to be used. Example value: XXXXX-XXXXX-XXXXX-XXXXX-XXXXX." )]
		public string UAProjectId;
		[Range( 1, 100 )]
		[Tooltip( "Send only x% of sessions to Unity Analytics." )]
		public int UASessionSampling = 100;
		[Tooltip( "Send a test event at load." )]
		public bool UATest = true;

		[Header( "Google Analytics" )]
		[Tooltip( "The object where GoogleAnalyticsV3 script is attached, leave empty if you attach it on this object" )]
		public bool GAEnabled = true;
		[Range( 1, 100 )]
		[Tooltip( "Send only x% of sessions to Google Analytics." )]
		public int GASessionSampling = 100;
		[Tooltip( "Send a test event at load." )]
		public bool GATest = true;

		public void Start( ) {
			SetupUnityAnalytics( );
			SetupGoogleAnalytics( );
			DontDestroyOnLoad( transform.gameObject );
		}

		private void SetupUnityAnalytics( ) {
			bool toEnable = UAEnabled && SamplingOk( UASessionSampling );
			var parameters = new Dictionary<string, string>{
				{"project", UAProjectId}
			};
			AnalyticsClientUnity.Setup( toEnable, parameters );

			if(UATest) {
				AnalyticsClientUnity.TrackEvent( "testEvent", "test", new Dictionary<string, object>( ) );
			}
		}

		private void SetupGoogleAnalytics( ) {
			AnalyticsClientGoogle.Setup( GAEnabled && SamplingOk( GASessionSampling )
			, new Dictionary<string, string>( ));

			if (GATest){
				AnalyticsClientGoogle.TrackEvent("testEvent", "test", new Dictionary<string, object>{
					{"value", 1}
				});
			}
		}

		/// <summary>
		/// Helper for random between 1 and 100.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool SamplingOk( int value ) {
			if(value == 100) {
				return true;
			}

			return UnityEngine.Random.Range( 1, 100 ) <= value;
		}

		/// <summary>
		///Automatically track the scene as a screen/view.
		/// </summary>
		System.Collections.IEnumerator OnLevelWasLoaded( int level ) {

			if(trackSceneLoad) {
				//wait a bit to load scene, awake scripts and start Analytics SDKs
				yield return new WaitForSeconds( 0.3f );
				try {
					Analytics.Screen( "scene." + Application.loadedLevelName );
				}
				catch(System.Exception e) {
					Debug.LogError( "Error at OnLevelWasLoaded " + e );
				}
			}
		}
	}
}
