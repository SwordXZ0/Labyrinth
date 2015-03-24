using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public GameObject scores;
	public GameObject instructions;
	public GameObject confirmationMenu;

	public void startGame(){
		Application.LoadLevel (1);
	}

	public void renderScores(){
		Instantiate (scores);
		Destroy (this.gameObject);
	}

	public void renderInstructions(){
		Instantiate (instructions);
		Destroy (this.gameObject);
	}
	
	public void renderLogInMenu(){
		Instantiate (confirmationMenu);
		Destroy (this.gameObject);
	}
}
