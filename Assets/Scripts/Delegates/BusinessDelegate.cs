using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MiniJSON;

public static class BusinessDelegate{

	public  static IEnumerator startService(string user, string password, GameObject context){
		string URL= "http://ancestralstudios.com/Labyrinth/log.php";
		WWWForm form = new WWWForm();
		form.AddField("user", user);
		form.AddField("password", password);
		WWW response = new WWW(URL, form);
		yield return response;
		
		if (!string.IsNullOrEmpty (response.error)) {
			GameObject warningScreen= MenuFactoryMethod.createWarningMenu();
			warningScreen.transform.Find("Text").GetComponent<Text>().text="Sorry, something went wrong :(";
			WarningMenu w=(WarningMenu)warningScreen.transform.GetComponent<WarningMenu>();
			w.Login=context;
			LogInOption tmp = (LogInOption)context.transform.GetComponent<LogInOption> ();
			tmp.block = false;
			context.SetActive(false);
		} else if (response.text.Equals ("false")) {
			GameObject warningScreen= MenuFactoryMethod.createWarningMenu();
			warningScreen.transform.Find("Text").GetComponent<Text>().text="Incorrect username or password";
			WarningMenu w=(WarningMenu)warningScreen.transform.GetComponent<WarningMenu>();
			w.Login=context;
			LogInOption tmp = (LogInOption)context.transform.GetComponent<LogInOption> ();
			tmp.block = false;
			context.SetActive(false);
		} else {
			MenuFactoryMethod.createMainMenu();
			PlayerPrefs.SetString("session",user);
			Object.Destroy(context);
		}
	}

	public  static IEnumerator signUpService(string user, string password, string email,GameObject context){
		string URL= "http://ancestralstudios.com/Labyrinth/signup.php";
		WWWForm form = new WWWForm();
		form.AddField("user", user);
		form.AddField("password", password);
		form.AddField("email", email);
		WWW response = new WWW(URL, form);
		yield return response;
		
		if (!string.IsNullOrEmpty (response.error)) {
			GameObject warningScreen= MenuFactoryMethod.createWarningMenu();
			warningScreen.transform.Find("Text").GetComponent<Text>().text="Sorry, something went wrong :(";
			WarningMenu w=(WarningMenu)warningScreen.transform.GetComponent<WarningMenu>();
			w.Login=context;
			SignUpOption tmp = (SignUpOption)context.transform.GetComponent<SignUpOption> ();
			tmp.block = false;
			context.SetActive(false);
		} else if (response.text.Equals ("repeated")) {
			GameObject warningScreen= MenuFactoryMethod.createWarningMenu();
			warningScreen.transform.Find("Text").GetComponent<Text>().text="Sorry, that username allready exists :(";
			WarningMenu w=(WarningMenu)warningScreen.transform.GetComponent<WarningMenu>();
			w.Login=context;
			SignUpOption tmp = (SignUpOption)context.transform.GetComponent<SignUpOption> ();
			tmp.block = false;
			context.SetActive(false);
		} else {
			MenuFactoryMethod.createMainMenu();
			PlayerPrefs.SetString("session",user);
			Object.Destroy(context);
		}
	}

	public  static IEnumerator viewScoresService(GameObject context){
		WWW www = new WWW("http://ancestralstudios.com/Labyrinth/retrieveTimes.php");
		yield return www;

		if (!string.IsNullOrEmpty (www.error)) {
			GameObject warningScreen = MenuFactoryMethod.createWarningMenu ();
			warningScreen.transform.Find ("Text").GetComponent<Text> ().text = "Sorry, something went wrong :(";
			WarningMenu w = (WarningMenu)warningScreen.transform.GetComponent<WarningMenu> ();
			w.Login = context;
			SignUpOption tmp = (SignUpOption)context.transform.GetComponent<SignUpOption> ();
			tmp.block = false;
			context.SetActive (false);
		} else {
			string response = www.text;
			IList times = (IList) Json.Deserialize(response);
			GameObject Smenu=MenuFactoryMethod.createScroesMenu ();
			ScoresMenu sms=Smenu.transform.GetComponent<ScoresMenu> ();
			Text[] texts = sms.transform.GetComponentsInChildren<Text> ();
			int player=1;
			foreach (IDictionary time in times) {
				texts[player].text=string.Format("{0}: {1}  {2}", player, time["username"], time["time"]);
				player++;
			}
			Object.Destroy (context);
		}
	}

}
