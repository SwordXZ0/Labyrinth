using UnityEngine;
using System.Collections;

public class FilterManager{

	FilterChain filterChain;
	public string operationMessage;
	public FilterManager(FilterChain filterChain){
		this.filterChain = filterChain;
	}

	public bool validate(){
		bool result=filterChain.validate ();
		operationMessage = filterChain.filterMessage;
		return result;
	}
}
