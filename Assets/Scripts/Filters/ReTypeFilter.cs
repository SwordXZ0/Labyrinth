using UnityEngine;
using System.Collections;

public class ReTypeFilter : Filter {

	string pass1,pass2;
	
	public ReTypeFilter (string pass1,string pass2){
		this.pass1 = pass1;
		this.pass2 = pass2;
	}
	
	public override bool validate(){
		if (MenuHelper.validateMatchingPasswords(pass1,pass2)) {
			message="Approved";
			return true;
			
		} else {
			message="The passwords do not match";
			return false;
		}
	}
}
