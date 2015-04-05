using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	bool block=false;

	BusinessDelegate businessService;
	void Start () {
		businessService = new BusinessDelegate ();
	}

//	public void startGame(){
//		Application.LoadLevel (1);
//	}

	public void renderScores(){
		if(!block){
			block=true;
			businessService.setServiceType("scores",null,null,this.gameObject);
			StartCoroutine(businessService.doTask());
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
