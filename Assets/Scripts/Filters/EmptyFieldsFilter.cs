using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class EmptyFieldsFilter : Filter{
	InputField[] inputs;

	public EmptyFieldsFilter(InputField[] inputs){
		this.inputs = inputs;
	}

	public override bool validate(){
		if (MenuHelper.validateNotEmptyFields (inputs)) {
			message="Approved";
			return true;

		} else {
			message="Please fill all the fields";
			return false;
		}
	}
	
}
