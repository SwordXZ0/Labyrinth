using UnityEngine;
using System.Collections;

public class LostMenu : MonoBehaviour {

	public void exit(){
		if (Application.CanStreamedLevelBeLoaded (0)) {
			Application.LoadLevel (0);
		}
	}
}
