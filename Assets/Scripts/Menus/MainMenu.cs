using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public void startGame(){
		Application.LoadLevel (1);
	}

	public void renderScores(){
		MenuFactoryMethod.createScroesMenu ();
		Destroy (this.gameObject);
	}

	public void renderInstructions(){
		MenuFactoryMethod.createInstructionsMenu ();
		Destroy (this.gameObject);
	}
	
	public void renderLogInMenu(){
		MenuFactoryMethod.createConfrimationMenu();
		Destroy (this.gameObject);
	}
}
