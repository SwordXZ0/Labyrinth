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
using Sfs2X.Entities.Variables;


public class Connection : MonoBehaviour {

//	public short maxPlayers=2;
	//----------------------------------------------------------
	// Setup variables
	//----------------------------------------------------------
//	private string serverName = "0.0.0.0";
//	private string serverName = "127.0.0.1";
	private string serverName = "63.251.106.58";
	private int serverPort = 9933;
	private string zone = "BasicExamples";
	private string genericRoomName = "GameRoom";
	
//	public string zone = "Labyrinth";
	public LogLevel logLevel = LogLevel.DEBUG;

	// Internal / private variables
	private SmartFox smartFox;
	private string username = "";
//	private string loginErrorMessage = "";
	private string serverConnectionStatusMessage = "";
	private bool isJoining = false;

	private List<Room> roomList = new List<Room>();
	private int roomId;

	//----------------------------------------------------------
	// Called when program starts
	//----------------------------------------------------------
	void Start() {
		serverConnectionStatusMessage += "";



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
		smartFox.AddEventListener(SFSEvent.ROOM_CREATION_ERROR,OnRoomCreationError);
		smartFox.AddEventListener(SFSEvent.LOGOUT, OnLogout);

		smartFox.AddLogListener(logLevel, OnDebugMessage);
		
		smartFox.Connect(serverName, serverPort);


		if (GameObject.Find ("StartGame")) {

				username =PlayerPrefs.GetString("session");

//			else if (!SignUpOption.u.Equals ("")) {
//				username = SignUpOption.u.text;
//			}

			GameObject.Find ("StartGame").GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => {
				Debug.Log("Sending login request");
				Debug.LogError("Sending login request");
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
				Debug.LogError ("Not connected");
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

			roomList = SmartFoxConnection.Connection.RoomList;
;
			Debug.LogError ("____CONNECTION  on "+ serverName);
			foreach(Room room in SmartFoxConnection.Connection.RoomList){
				Debug.LogError("____CONNECTIONroom list roomName  =  "+room.Name);
				Debug.LogError("____CONNECTIONroom list roomId  =  "+room.Id);
				
			}
			
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

		roomId = 1;
		bool roomJoined = false;
		// We either create the Game Room or join it if it exists already
//		foreach (Room room in roomList) {
//
//			Debug.Log("mi roomID = "+roomId);
////			if (SmartFoxConnection.Connection.RoomManager.ContainsRoom (room.Name) && room.UserCount != room.MaxUsers) {
//			if (smartFox.RoomManager.ContainsRoom (room.Name)) {
//				Debug.Log("room name = "+ room.Name);
//				Debug.Log("room server set id = "+ room.Id);
//				smartFox.Send (new JoinRoomRequest (room.Name));
////				SmartFoxConnection.Connection.Send (new JoinRoomRequest (room.Name));
//				roomExists = true;
//				break;
//			}
//
//			roomId++;
////			roomId = room.Id;
//			Debug.Log("despues de la suma  mi id  = "+roomId);
//
//		}
		if (smartFox.RoomManager.ContainsRoom (genericRoomName+roomId)) {

			Room room = smartFox.RoomManager.GetRoomByName(genericRoomName+roomId);
			Debug.Log("room name = "+ room.Name);
			Debug.Log("room server set id = "+ room.Id);
			if(room.UserCount<room.MaxUsers){
				smartFox.Send (new JoinRoomRequest (room.Name));
				roomJoined = true;
			}
		}
		if(!roomJoined){
//			RoomSettings settings = new RoomSettings("Game Room");
			RoomSettings settings = new RoomSettings(genericRoomName+roomId);
			settings.MaxUsers = 2;
			settings.IsGame = true;
			bool gameFinished = false;
			bool gameReady = false;
			SFSRoomVariable gameFinishedVar = new SFSRoomVariable("gameFinished",gameFinished);
			SFSRoomVariable gameReadyVar = new SFSRoomVariable("gameReady",gameReady);
			settings.Variables.Add(gameFinishedVar);
			settings.Variables.Add(gameReadyVar);
			Debug.Log("new game name = "+settings.Name);
			smartFox.Send(new CreateRoomRequest(settings, true));
//			SmartFoxConnection.Connection.Send(new CreateRoomRequest(settings, true));
		}

//		if (smartFox.RoomManager.ContainsRoom("Game Room")) {
//			smartFox.Send(new JoinRoomRequest("Game Room"));
//			
//		} else {
//			RoomSettings settings = new RoomSettings("Game Room");
//			settings.MaxUsers = 2;
//		 
//			smartFox.Send(new CreateRoomRequest(settings, true));
//		}
	}

	public void OnLoginError(BaseEvent evt) {
		short errorCode = (short)evt.Params ["errorCode"];
		Debug.Log("Login error: "+(string)evt.Params["errorMessage"]);
		Debug.Log("Login error: "+errorCode);

//		if (errorCode == 6) {
//			smartFox.Send(new LogoutRequest());
//			smartFox.Send(new LoginRequest(username, "", zone));
//		}

	}

	public void OnRoomCreationError(BaseEvent evt){
		string error = (string)evt.Params["errorMessage"];
		short errorCode = (short)evt.Params["errorCode"];
		Debug.Log("Error creating room = "+error+" CODE "+errorCode);
		if (errorCode == 12) {
			roomId++;
			RoomSettings settings = new RoomSettings(genericRoomName+roomId);
			settings.MaxUsers = 2;
			settings.IsGame = true;
			bool gameFinished = false;
			bool gameReady = false;
			SFSRoomVariable gameFinishedVar = new SFSRoomVariable("gameFinished",gameFinished);
			SFSRoomVariable gameReadyVar = new SFSRoomVariable("gameReady",gameReady);
			settings.Variables.Add(gameFinishedVar);
			settings.Variables.Add(gameReadyVar);
			Debug.Log("new game name = "+settings.Name);
			smartFox.Send(new CreateRoomRequest(settings, true));
		}
	}

	
	public void OnRoomJoin(BaseEvent evt) {
		Debug.Log("Joined room successfully");

		// Room was joined - lets load the game and remove all the listeners from this component
		smartFox.RemoveAllEventListeners();
		if (Application.CanStreamedLevelBeLoaded (1)) {
			Application.LoadLevel (1);
		}
	}
	
	void OnLogout(BaseEvent evt) {
		Debug.Log("OnLogout");
		isJoining = false;
	}
	
	public void OnDebugMessage(BaseEvent evt) {
//		string message = (string)evt.Params["message"];
//		Debug.Log("[SFS DEBUG] " + message);
	}

}