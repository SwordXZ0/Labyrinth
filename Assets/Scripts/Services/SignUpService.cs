using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class SignUpService : BusinessService {

	private UserDTO user;
	private GameObject context;
	
	public SignUpService(UserDTO user,GameObject context){
		this.user=user;
		this.context = context;
	}

	IEnumerator BusinessService.doProcessing(){
		string URL= "http://ancestralstudios.com/Labyrinth/signup.php";
		WWWForm form = new WWWForm();
		form.AddField("user", user.name);
		form.AddField("password", user.password);
		form.AddField("email", user.mail);
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
			PlayerPrefs.SetString("session",user.name);
			Object.Destroy(context);
		}
	}
}
