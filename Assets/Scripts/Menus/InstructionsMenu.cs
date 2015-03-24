using UnityEngine;
using System.Collections;

public class InstructionsMenu : MonoBehaviour {

	public GameObject mainMenu;
	
	public void renderMainMenu(){
		Instantiate (mainMenu);
		Destroy(this.gameObject);
	}
}
