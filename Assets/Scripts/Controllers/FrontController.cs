using UnityEngine;
using System.Collections;

public class FrontController : MonoBehaviour {
	public GameObject LogInMenu;
	public GameObject MainMenu;

	private FilterManager filters;
	private FilterChain filterChain;

	void Start () {
		filterChain = new FilterChain ();
		filterChain.add (new SessionFilter());
		filters = new FilterManager (filterChain);
		if (filters.validate()) {
			Instantiate(MainMenu);
		}else{
			Instantiate(LogInMenu);
		}
	}

	void Update () {

	}
}
