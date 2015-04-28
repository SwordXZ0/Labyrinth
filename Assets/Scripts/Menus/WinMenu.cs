using UnityEngine;
using System.Collections;

public class WinMenu : MonoBehaviour {

	public void exit(){

		if (Application.CanStreamedLevelBeLoaded (0)) {
			Application.LoadLevel (0);
		}
	}
}
