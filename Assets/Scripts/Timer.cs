using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
	private static float timer;
	private static Text text;
	// Use this for initialization
	void Start () {
		text= (Text)this.transform.Find("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		string minutes = Mathf.Floor(timer / 60).ToString("00");
		string seconds = Mathf.Floor(timer % 60).ToString("00");
		text.text = "Time: "+minutes + ":" + seconds;
	}
}
