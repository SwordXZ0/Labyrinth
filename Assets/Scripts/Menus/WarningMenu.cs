using UnityEngine;
using System.Collections;

public class WarningMenu : MonoBehaviour {
	
	public GameObject Login;
	
	public void renderLogInOption(){
		Login.SetActive(true);
		Destroy (this.gameObject);
	}
}
