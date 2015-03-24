using UnityEngine;
using System.Collections;

public class LogInMenu : MonoBehaviour {
	public GameObject LogInOption;
	public GameObject SignUpOption;

	public void renderLogInOption(){
		Instantiate (LogInOption);
		Destroy(this.gameObject);
	}

	public void renderSignUpOption(){
		Instantiate (SignUpOption);
		Destroy(this.gameObject);
	}

}
