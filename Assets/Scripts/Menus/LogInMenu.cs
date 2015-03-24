using UnityEngine;
using System.Collections;

public class LogInMenu : MonoBehaviour {

	public void renderLogInOption(){
		MenuFactoryMethod.createLogInOption ();
		Destroy(this.gameObject);
	}

	public void renderSignUpOption(){
		MenuFactoryMethod.createSignUpOption ();
		Destroy(this.gameObject);
	}

}
