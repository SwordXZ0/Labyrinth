using UnityEngine;
using System.Collections;

public class FilterChain{
	public string filterMessage;
	ArrayList filters;

	public FilterChain(){
		filters=new ArrayList();
	}

	public void add(Filter filter){
		filters.Add (filter);
	}

	public void remove(int index){
		filters.RemoveAt (index);
	}

	public bool validate(){
		foreach(Filter f in filters){
			if(!f.validate()){
				filterMessage=f.message;
				return false;
			}
		}
		filterMessage = "approved";
		return true;
	}

}
