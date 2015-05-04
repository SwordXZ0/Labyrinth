using UnityEngine;
using System.Collections;

public class TieMenu : MonoBehaviour {
	
	public void exit(){
		PlayerPrefs.DeleteKey("session");
		if (Application.CanStreamedLevelBeLoaded (0)) {
			Application.LoadLevel (0);
		}
	}
}

