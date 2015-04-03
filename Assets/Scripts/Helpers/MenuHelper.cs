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

	public static bool validateLength(InputField[] fields){
		foreach(InputField i in fields){
			if(i.text.Length>20){
				return false;
			}
		}
		return true;
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

	public  static bool validateNoSQLInjection(InputField[] fields){
		bool clean = true;
		ArrayList test = new ArrayList ();
		test.Add (@"/(\%27)|(\')|(\-\-)|(\%23)|(#)/ix");//SQLmetaCharsPattern1
		test.Add (@"/((\%3D)|(=))[^\n]*((\%27)|(\')|(\-\-)|(\%3B)|(;))/i");//SQLmetaCharsPattern2 
		test.Add (@"/\w*((\%27)|(\'))((\%6F)|o|(\%4F))((\%72)|r|(\%52))/ix");//TypicalSQLinjectionPattern
		test.Add (@"/((\%27)|(\'))union/ix");//UnionPatternWord
		test.Add (@"/((\%27)|(\'))select/ix");//SeletPatternWord
		test.Add (@"/((\%27)|(\'))insert/ix");//InsertPatternWord
		test.Add (@"/((\%27)|(\'))update/ix");//UpdatePatternWord
		test.Add (@"/((\%27)|(\'))delete/ix");//DeletePatternWord
		test.Add (@"/((\%27)|(\'))drop/ix");//DropPatternWord
		test.Add (@"/exec(\s|\+)+(s|x)p\w+/ix");//MSQLinjectionPattern

		foreach(InputField field in fields){
			foreach(string pattern in test){
				if(Regex.IsMatch (field.text, pattern)){
					clean=false;
					break;
				}
			}
		}
		return clean;
	}

}
