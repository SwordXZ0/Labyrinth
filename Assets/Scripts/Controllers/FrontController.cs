using UnityEngine;
using System.Collections;

public class FrontController : MonoBehaviour {

	private FilterManager filters;
	private FilterChain filterChain;

	void Start () {
		filterChain = new FilterChain ();
		filterChain.add (new SessionFilter());
		filters = new FilterManager (filterChain);
		if (filters.validate()) {
			MenuFactoryMethod.createMainMenu();
		}else{
			MenuFactoryMethod.createLogInMenu();
		}
	}

	void Update () {

	}

	void OnApplicationQuit() {
		PlayerPrefs.DeleteKey("session");
	}
}
