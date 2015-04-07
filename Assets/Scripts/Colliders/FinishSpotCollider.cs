using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishSpotCollider : MonoBehaviour {
	BusinessDelegate businessService;

	void Start () {
		businessService = new BusinessDelegate ();
	}

	void OnTriggerEnter(Collider other){
//		if(other.gameObject.tag.Equals("Player1")){
			Debug.Log("collided:)");
			GameObject timer = GameObject.FindGameObjectWithTag ("Timer");
//			Timer time = (Timer)timer.transform.GetComponent<Timer> ();
//			Text elapsed = time.transform.GetComponentInChildren<Text>();
			Text elapsed = Timer.text;
			ResultDTO result= new ResultDTO(PlayerPrefs.GetString("session","unknow"),elapsed.text);
			businessService.setServiceType ("saveScore", null, result, this.gameObject);
			StartCoroutine(businessService.doTask());

//		}
//		else if(other.gameObject.tag.Equals("Player2")){
//
//		}
	}

}
