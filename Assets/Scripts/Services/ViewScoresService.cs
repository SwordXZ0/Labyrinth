using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MiniJSON;

public class ViewScoresService: BusinessService {
	private GameObject context;

	public ViewScoresService(GameObject context){
		this.context = context;
	}

	IEnumerator BusinessService.doProcessing(){
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
