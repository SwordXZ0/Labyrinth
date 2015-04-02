using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class SQLInjectionFilter : Filter {
	InputField[] inputs;

	public SQLInjectionFilter(InputField[] inputs){
		this.inputs = inputs;
	}

	public override bool validate(){
		if (MenuHelper.validateNoSQLInjection (inputs)) {
			message="Approved";
			return true;
			
		} else {
			message="Eres un chico malo :(";
			return false;
		}
	}

}
