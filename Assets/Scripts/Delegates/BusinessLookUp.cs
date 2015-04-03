using UnityEngine;
using System.Collections;

public class BusinessLookUp {

	public BusinessService getBusinessService(string serviceType, GameObject context, UserDTO user,ResultDTO result){
		
		if (serviceType.Equals ("login")) {
			return new LogInService (user, context);
		} else if (serviceType.Equals ("signup")) {
			return new SignUpService (user, context);
		} else if (serviceType.Equals ("scores")) {
			return new ViewScoresService (context);
		} else if (serviceType.Equals ("saveScore")) {
			return new SaveWinnerScore (result, context);
		} else {
			return null;
		}
	}
}
