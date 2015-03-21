using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BFGU.util {
	/// <summary>
	/// Easy way to version your builds automatically. While in Editor mode saves the date
	/// as text. This way you will know what build QA/Players have.
	/// The version is visible only 3 seconds then it dissapear.
	/// Attach this script to an object that has UI.Button and UI.Text components.
	/// </summary>
	[ExecuteInEditMode]
	public class VersionTime : MonoBehaviour {

		private Text textui;
		public float secVisible = 3f;

		public void Awake( ) {
			textui = GetComponent<Text>( );

			//http://www.ezineasp.net/post/C-DateTime-Format-String.aspx
			if(Application.isEditor && Application.isPlaying == false) {
				textui.text = DateTime.UtcNow.ToString( "v.ddMMyy.hhmm" );
			}

			OnClick( );
		}

		public void OnClick( ) {
			StopCoroutine( ShowAndAutoHide( ) );
			StartCoroutine( ShowAndAutoHide( ) );
		}

		private IEnumerator ShowAndAutoHide( ) {
			Color newColor = textui.color;
			newColor.a = 1;
			textui.color = newColor;

			yield return new WaitForSeconds( secVisible );

			newColor.a = 0;
			textui.color = newColor;
		}
	}
}
