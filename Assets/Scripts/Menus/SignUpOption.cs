﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Text.RegularExpressions;

public class SignUpOption : MonoBehaviour {

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
		InputField m =transform.Find ("mail").GetComponent<InputField>();
		InputField p =transform.Find ("password").GetComponent<InputField>();
		InputField r =transform.Find ("ReTypepassword").GetComponent<InputField>();
		InputField[] inputs = transform.GetComponentsInChildren<InputField> ();

		filterChain = new FilterChain ();
		filterChain.add (new EmptyFieldsFilter(inputs));
		filterChain.add (new LengthFilter(inputs));
		filterChain.add (new EmailFilter(m.text));
		filterChain.add (new ReTypeFilter(p.text,r.text));
		filterChain.add (new SQLInjectionFilter(inputs));
		filterManager = new FilterManager (filterChain);
		
		if (filterManager.validate ()) {
			if(!block){
				block=true;
				UserDTO user = new UserDTO(u.text,p.text,m.text);
				businessService.setServiceType("signup",user,null, this.gameObject);
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
