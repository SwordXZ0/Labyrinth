using UnityEngine;
using System.Collections;

public class ScoresMenu : MonoBehaviour {

	public void renderMainMenu(){
		MenuFactoryMethod.createMainMenu ();
		Destroy(this.gameObject);
	}
}
