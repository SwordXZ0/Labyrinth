using UnityEngine;
using System.Collections;

public class SessionFilter : Filter {

	public override bool validate(){
		if (!PlayerPrefs.HasKey ("session")) {
			message = "No session";
			return false;
		} else {
			message="session active";
			return true;
		}
	}
}
