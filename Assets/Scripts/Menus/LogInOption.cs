using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogInOption : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject LogInMenu;
	public GameObject warning;

	FilterManager filterManager;
	FilterChain filterChain;


	public void renderMainMenu(){
		InputField u =transform.Find ("User").GetComponent<InputField>();
		InputField p =transform.Find ("password").GetComponent<InputField>();
		InputField[] inputs = transform.GetComponentsInChildren<InputField> ();

		filterChain = new FilterChain ();
		filterChain.add (new EmptyFieldsFilter(inputs));
		filterChain.add (new CredentialsFilter(u.text, p.text));
		filterManager = new FilterManager (filterChain);

		if (filterManager.validate ()) {
			Instantiate(mainMenu);
			Destroy(this.gameObject);
		} else {
			GameObject warningScreen= Instantiate(warning);
			warningScreen.transform.Find("Text").GetComponent<Text>().text=filterManager.operationMessage;
			WarningMenu w=(WarningMenu)warningScreen.transform.GetComponent<WarningMenu>();
			w.Login=this.gameObject;
			this.gameObject.SetActive(false);
		}
	}
	
	public void renderLogInMenu(){
		Instantiate(LogInMenu);
		Destroy(this.gameObject);
	}

}
