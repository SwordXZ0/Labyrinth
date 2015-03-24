using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

using UnityEngine.UI;

public static class MenuHelper {
	public static bool validateNotEmptyFields(InputField[] fields){
		foreach(InputField i in fields){
			if(i.text.Equals("")){
				return false;
			}
		}
		return true;
	}


	public static bool validateCredentials(string user, string password){
		if(user.Equals("das")&& user.Equals("das")){
			return true;
		}
		return false;
	}

	public static bool validateMatchingPasswords(string password1, string password2){
		if(password1.Equals(password2)){
			return true;
		}
		return false;
	}

	public  static bool validateEmailField(string emailAddress){
		string EmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
			+ "@"
				+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
		return Regex.IsMatch (emailAddress, EmailPattern);
	}

}
