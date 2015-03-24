using UnityEngine;
using System.Collections;

public class EmailFilter : Filter{

	string email;
	
	public EmailFilter (string email){
		this.email = email;
	}
	
	public override bool validate(){
		if (MenuHelper.validateEmailField(email)) {
			message="Approved";
			return true;
			
		} else {
			message="Wrong e-mail format";
			return false;
		}
	}
}
