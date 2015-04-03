using UnityEngine;
using System.Collections;

using UnityEngine.UI;
public class LengthFilter : Filter {
	InputField[] inputs;

	public LengthFilter(InputField[] inputs){
		this.inputs = inputs;
	}
	
	public override bool validate(){
		if (MenuHelper. validateLength (inputs)) {
			message="Approved";
			return true;
			
		} else {
			message="Please just 20 characters per field";
			return false;
		}
	}
}
