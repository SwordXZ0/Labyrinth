using UnityEngine;
using System.Collections;

public class ConfirmationMenu : MonoBehaviour {

	public GameObject yesMenu;
	public GameObject noMenu;
	
	public void yes(){
		Instantiate(yesMenu);
		PlayerPrefs.DeleteKey("session");
		Destroy(this.gameObject);
	}

	public void no(){
		Instantiate(noMenu);
		Destroy(this.gameObject);
	}

	public void exit(){
		if (Application.CanStreamedLevelBeLoaded (0)) {
			Application.LoadLevel (0);
		}

	}
}
