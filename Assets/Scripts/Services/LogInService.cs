using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogInService : BusinessService {
	private UserDTO user;
	private GameObject context;

	public LogInService(UserDTO user,GameObject context){
		this.user=user;
		this.context = context;
	}

	IEnumerator BusinessService.doProcessing(){
		string URL= "http://ancestralstudios.com/Labyrinth/log.php";
		WWWForm form = new WWWForm();
		form.AddField("user", user.name);
		form.AddField("password", user.password);
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
			PlayerPrefs.SetString("session",user.name);
			Object.Destroy(context);
		}
	}
}
