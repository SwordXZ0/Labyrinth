using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogInOption : MonoBehaviour {
	
	FilterManager filterManager;
	FilterChain filterChain;
	public bool block=false;

	public static InputField u;
	BusinessDelegate businessService;
	void Start () {
		businessService = new BusinessDelegate ();
	}


	public void renderMainMenu(){
		u =transform.Find ("User").GetComponent<InputField>();
		InputField p =transform.Find ("password").GetComponent<InputField>();
		InputField[] inputs = transform.GetComponentsInChildren<InputField> ();

		filterChain = new FilterChain ();
		filterChain.add (new EmptyFieldsFilter(inputs));
		filterChain.add (new LengthFilter(inputs));
		filterChain.add (new SQLInjectionFilter(inputs));
		filterManager = new FilterManager (filterChain);

		if (filterManager.validate ()) {
			if(!block){
				block=true;
				UserDTO user = new UserDTO (u.text,p.text);
				businessService.setServiceType("login", user,null,this.gameObject);
				StartCoroutine(businessService.doTask());
			}
		} else {
			GameObject warningScreen= MenuFactoryMethod.createWarningMenu();
			warningScreen.transform.Find("Text").GetComponent<Text>().text=filterManager.operationMessage;
			WarningMenu w=(WarningMenu)warningScreen.transform.GetComponent<WarningMenu>();
			w.Login=this.gameObject;
			this.gameObject.SetActive(false);
		}
	}
	
	public void renderLogInMenu(){
		MenuFactoryMethod.createLogInMenu ();
		Destroy(this.gameObject);
	}


}
