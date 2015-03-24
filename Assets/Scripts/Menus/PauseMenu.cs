using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
	public GameObject Instruction;
	public GameObject confirmation;
	public GameObject pause;


	public void Controls(){
		Instantiate(Instruction);
		Destroy(this.gameObject);
	}

	public void Continue(){
		Destroy (this.gameObject);
	}

	public void surrender(){
		Instantiate(confirmation);
		Destroy (this.gameObject);
	}
}
