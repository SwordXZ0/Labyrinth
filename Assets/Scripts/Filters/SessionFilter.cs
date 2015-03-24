using UnityEngine;
using System.Collections;

public class SessionFilter : Filter {

	public override bool validate(){
		message="No session";
		return false;
	}
}
