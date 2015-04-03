using UnityEngine;
using System.Collections;

public class UserDTO {
	public string name;
	public string password;
	public string mail;

	public UserDTO(string name,string password,string mail){
		this.name = name;
		this.password = password;
		this.mail = mail;
	}

	public UserDTO (string name, string password):this(name,password,null){
	}

}
