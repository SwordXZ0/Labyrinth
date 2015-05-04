using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveWinnerScore : BusinessService  {
	private ResultDTO result;
	private GameObject context;

	public SaveWinnerScore(ResultDTO result,GameObject context){
		this.result=result;
		this.context = context;
	}

	IEnumerator BusinessService.doProcessing(){
		string URL= "http://ancestralstudios.com/Labyrinth/Services/saveTime.php";
		WWWForm form = new WWWForm();
		form.AddField("user", result.username);
		form.AddField("time", result.time);
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
		} else {
			GameObject winneMenu=MenuFactoryMethod.createWinnerMenu();
			WinMenu wm = (WinMenu)winneMenu.transform.GetComponent<WinMenu>();
			Text tmp=wm.transform.GetComponentInChildren<Text>();
			tmp.text=result.time;
		}
	}
}
