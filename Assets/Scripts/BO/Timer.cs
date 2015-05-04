using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class Timer{
	private static float timer;
	public static Text text;
	// Use this for initialization
	public static void initializeTimer () {
		timer = 0;
		text = GameObject.FindGameObjectWithTag ("Timer").GetComponentInChildren<Text>();

//		text= (Text)this.transform.Find("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	public static void updateTimer () {
		timer += Time.deltaTime;
		string minutes = Mathf.Floor(timer / 60).ToString("00");
		string seconds = Mathf.Floor(timer % 60).ToString("00");
		text.text = "Time: "+minutes + ":" + seconds;

	}

	public static void stopTimer(){
		float finalTime = timer;
		timer = finalTime;
	}
}
