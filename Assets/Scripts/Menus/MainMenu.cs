using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	bool block=false;
	public void startGame(){
		Application.LoadLevel (1);
	}

	public void renderScores(){
		if(!block){
			block=true;
			StartCoroutine(BusinessDelegate.viewScoresService(this.gameObject));
		}
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
