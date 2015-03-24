using UnityEngine;
using System.Collections;

public class CredentialsFilter : Filter {
	
	string user,password;
	
	public CredentialsFilter (string user,string password){
		this.user = user;
		this.password = password;
	}
	
	public override bool validate(){
		if (MenuHelper.validateCredentials(user,password)) {
			message="Approved";
			return true;
			
		} else {
			message="Incorrect username or password";
			return false;
		}
	}
}
