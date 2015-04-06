using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;


public class Connection : MonoBehaviour {

	public int maxPlayers=2;
	//----------------------------------------------------------
	// Setup variables
	//----------------------------------------------------------
	public string serverName = "127.0.0.1";
	public int serverPort = 9933;
	public string zone = "BasicExamples";
	public LogLevel logLevel = LogLevel.DEBUG;

	// Internal / private variables
	private SmartFox smartFox;
	private string username = "";
	private string loginErrorMessage = "";
	private string serverConnectionStatusMessage = "";
	private bool isJoining = false;
	
	//----------------------------------------------------------
	// Called when program starts
	//----------------------------------------------------------
	void Start() {
		// In a webplayer (or editor in webplayer mode) we need to setup security policy negotiation with the server first
		if (Application.isWebPlayer) {
			if (!Security.PrefetchSocketPolicy(serverName, serverPort, 500)) {
				Debug.LogError("Security Exception. Policy file load failed!");
			}
		}
		
		// Lets connect
		smartFox = new SmartFox(true);
					
		// Register callback delegate
		smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		smartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);
		smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		smartFox.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		smartFox.AddEventListener(SFSEvent.LOGOUT, OnLogout);

		smartFox.AddLogListener(logLevel, OnDebugMessage);
		
		smartFox.Connect(serverName, serverPort);

		if (GameObject.Find ("StartGame")) {
			if (!LogInOption.u.Equals ("")) {
				username =LogInOption.u.text;
			}
			else if (!SignUpOption.u.Equals ("")) {
				username = SignUpOption.u.text;
			}

			GameObject.Find ("StartGame").GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => {
				Debug.Log("Sending login request");
				smartFox.Send(new LoginRequest(username, "", zone));

			});
		
		}


		
	}
	
	//----------------------------------------------------------
	// As Unity is not thread safe, we process the queued up callbacks every physics tick
	//----------------------------------------------------------
	void FixedUpdate() {
		if (smartFox != null) {
			smartFox.ProcessEvents();
		}
	}

	void Update() {
		if (smartFox != null) {
			
			if (!smartFox.IsConnected) {
				Debug.Log ("Not connected");
			} else if (isJoining) {
				Debug.Log ("Joining.....");
			}
		}
		
	}


	//----------------------------------------------------------
	// Handle connection response from server
	//----------------------------------------------------------
	public void OnConnection(BaseEvent evt) {
		bool success = (bool)evt.Params["success"];
		string error = (string)evt.Params["errorMessage"];
		
		Debug.Log("On Connection callback got: " + success + " (error : <" + error + ">)");

		if (success) {
			SmartFoxConnection.Connection = smartFox;
			
			serverConnectionStatusMessage = "Connection succesful!";
		} else {
			serverConnectionStatusMessage = "Can't connect to server!";
		}
	}


	public void OnConnectionLost(BaseEvent evt) {
		// Reset all internal states so we kick back to login screen
		Debug.Log("OnConnectionLost");
		isJoining = false;
		
		serverConnectionStatusMessage = "Connection was lost, Reason: " + (string)evt.Params["reason"];
	}

	public void OnLogin(BaseEvent evt) {
		Debug.Log("Logged in successfully");
		
		// We either create the Game Room or join it if it exists already
		if (smartFox.RoomManager.ContainsRoom("Game Room")) {
			smartFox.Send(new JoinRoomRequest("Game Room"));
			
		} else {
			RoomSettings settings = new RoomSettings("Game Room");
			settings.MaxUsers = (short)maxPlayers;
		 
			smartFox.Send(new CreateRoomRequest(settings, true));
		}
	}

	public void OnLoginError(BaseEvent evt) {
		Debug.Log("Login error: "+(string)evt.Params["errorMessage"]);
	}
	
	public void OnRoomJoin(BaseEvent evt) {
		Debug.Log("Joined room successfully");

		// Room was joined - lets load the game and remove all the listeners from this component
		smartFox.RemoveAllEventListeners();
		Application.LoadLevel(1);
	}
	
	void OnLogout(BaseEvent evt) {
		Debug.Log("OnLogout");
		isJoining = false;
	}
	
	public void OnDebugMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
//		Debug.Log("[SFS DEBUG] " + message);
	}

}