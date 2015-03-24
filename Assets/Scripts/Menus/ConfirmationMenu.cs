using UnityEngine;
using System.Collections;

public class ConfirmationMenu : MonoBehaviour {

	public GameObject yesMenu;
	public GameObject noMenu;
	
	public void yes(){
		Instantiate(yesMenu);
		Destroy(this.gameObject);
	}

	public void no(){
		Instantiate(noMenu);
		Destroy(this.gameObject);
	}

	public void exit(){
		Application.LoadLevel (0);
	}
}
