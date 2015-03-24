using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Text.RegularExpressions;

public class SignUpOption : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject LogInMenu;
	public GameObject warning;

	FilterManager filterManager;
	FilterChain filterChain;

	public void renderMainMenu(){
		InputField m =transform.Find ("mail").GetComponent<InputField>();
		InputField p =transform.Find ("password").GetComponent<InputField>();
		InputField r =transform.Find ("ReTypepassword").GetComponent<InputField>();
		InputField[] inputs = transform.GetComponentsInChildren<InputField> ();

		filterChain = new FilterChain ();
		filterChain.add (new EmptyFieldsFilter(inputs));
		filterChain.add (new EmailFilter(m.text));
		filterChain.add (new ReTypeFilter(p.text,r.text));
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
