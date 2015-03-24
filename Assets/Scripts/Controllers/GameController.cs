using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject pauseMenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			if(GameObject.Find("PauseMenu(Clone)")==null){
				MenuFactoryMethod.createPauseMenu();
			}
		}
	}
}
