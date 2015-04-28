using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Logging;


public class GameController : MonoBehaviour {

	private bool gameReady;
	public bool gameFinished;
	private GameObject waitMenu;
	//----------------------------------------------------------
	// Setup variables
	//----------------------------------------------------------
	public GameObject[] spawnPoints;
	public GameObject[] playerModels;
	public Material[] playerMaterials;


	private List<SFSUser> roomUsers;

	public LogLevel logLevel = LogLevel.DEBUG;

	// Internal / private variables
	private SmartFox smartFox;
	
	private GameObject localPlayer;
	private PlayerController localPlayerController;
	private Dictionary<SFSUser, GameObject> remotePlayers = new Dictionary<SFSUser, GameObject>();
	private Room currentRoom;
	
	//----------------------------------------------------------
	// Unity callbacks
	//----------------------------------------------------------
	void Start() {
		roomUsers = new List<SFSUser> ();
		if (!SmartFoxConnection.IsInitialized) {
			if (Application.CanStreamedLevelBeLoaded (0)) {
				Application.LoadLevel (0);
			}
			return;
		}
		smartFox = SmartFoxConnection.Connection;
		
		// Register callback delegates
		smartFox.AddEventListener (SFSEvent.OBJECT_MESSAGE, OnObjectMessage);
		smartFox.AddEventListener (SFSEvent.CONNECTION_LOST, OnConnectionLost);
		smartFox.AddEventListener (SFSEvent.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
		smartFox.AddEventListener (SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
		smartFox.AddEventListener (SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		smartFox.AddEventListener (SFSEvent.ROOM_VARIABLES_UPDATE, OnRoomVariableUpdate);
		smartFox.AddLogListener (logLevel, OnDebugMessage);
		
		// Start this clients avatar and get cracking!
		int numModel = UnityEngine.Random.Range (0, playerModels.Length);
		int numMaterial = UnityEngine.Random.Range (0, playerMaterials.Length);
		SpawnLocalPlayer (numModel, numMaterial);

		currentRoom = smartFox.LastJoinedRoom;


		Timer.initializeTimer ();
		gameReady = false;


	}
	
	void FixedUpdate() {
		if (smartFox != null) {
			smartFox.ProcessEvents();
			
			// If we spawned a local player, send position if movement is dirty
			if (localPlayer != null && localPlayerController != null && localPlayerController.MovementDirty) {
				List<UserVariable> userVariables = new List<UserVariable>();
				userVariables.Add(new SFSUserVariable("x", (double)localPlayer.transform.position.x));
				userVariables.Add(new SFSUserVariable("y", (double)localPlayer.transform.position.y));
				userVariables.Add(new SFSUserVariable("z", (double)localPlayer.transform.position.z));
				userVariables.Add(new SFSUserVariable("rot", (double)localPlayer.transform.rotation.eulerAngles.y));

				smartFox.Send(new SetUserVariablesRequest(userVariables));
				localPlayerController.MovementDirty = false;
			}
		}
	}
	public void finishGame(){
		gameFinished = true;
	}

	void Update(){
		if (gameFinished) {
			List<RoomVariable> roomVariables = new List<RoomVariable>();
			roomVariables.Add(new SFSRoomVariable("gameFinished", gameFinished));
			smartFox.Send(new SetRoomVariablesRequest(roomVariables));
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			Debug.Log("manual called");
			Debug.Log ("currentRoom is " + currentRoom.Name);
			Debug.Log("user count = "+currentRoom.UserCount);
			Debug.Log("max users = "+currentRoom.MaxUsers);
			Debug.Log("--------ALLROOMS");
			foreach(Room room in smartFox.RoomList){
				Debug.Log("____room list roomName  =  "+room.Name);
				Debug.Log("____room list roomId  =  "+room.Id);

			}
			Debug.Log("--------ALLROOMSEND");

			Debug.Log("+++++++++++++STATS");
			Debug.Log("game satrted = "+gameReady);
			Debug.Log("gameFinished"+gameFinished);
			Debug.Log("+++++++++++++STASTEND");

			Debug.Log("manual end");
		}

//		if (gameFinished) {
//			Debug.Log("game finished");
////			localPlayer.SetActive (false);
////			Debug.Log("todos los users"+currentRoom.UserCount);
////			foreach (SFSUser user in roomUsers) {
////				Debug.Log("contiene "+user.Name+"="+currentRoom.ContainsUser(user));
////				RemoveRemotePlayer (user);
////				Debug.Log("contiene "+user.Name+"="+currentRoom.ContainsUser(user));
////			}
////			Debug.Log("sin remotes"+currentRoom.UserCount);
////			RemoveLocalPlayer ();
//
//		}


		if (Input.GetKeyDown (KeyCode.P)) {
			if(GameObject.Find("PauseMenu(Clone)")==null){
				MenuFactoryMethod.createPauseMenu();
			}
		}

		if (currentRoom != null) {
			if (!gameReady) {

				if (currentRoom.UserCount < currentRoom.MaxUsers) {	

					if (GameObject.Find ("WaitMenu(Clone)") == null) {
						
						waitMenu = MenuFactoryMethod.createWaitMenu ();
						localPlayer.SetActive (false);
						
					}
				}

				gameReady = currentRoom.GetVariable ("gameReady").GetBoolValue ();
			}
			else if (gameReady) {
				if (waitMenu != null) {
					Destroy (waitMenu);
				}
				Timer.updateTimer();
				localPlayer.SetActive (true);
			}

		}

	}
	

	void OnApplicationQuit() {
		// Before leaving, lets notify the others about this client dropping out
		PlayerPrefs.DeleteKey("session");
		RemoveLocalPlayer();
	}
	
	//----------------------------------------------------------
	// SmartFox callbacks
	//----------------------------------------------------------
	
	public void OnUserExitRoom(BaseEvent evt) {
		// Someone left - lets make certain they are removed if they didnt nicely send a remove command
		SFSUser user = (SFSUser)evt.Params["user"];		
		RemoveRemotePlayer(user);
	}
	
	public void OnUserEnterRoom(BaseEvent evt) {
		// User joined - and we might be standing still (not sending position info). So lets send him our position info

		SFSUser user = (SFSUser)evt.Params["user"];
//		SFSRoom room = (SFSRoom)evt.Params ["room"];

		if (localPlayer != null) {
			List<UserVariable> userVariables = new List<UserVariable>();
			userVariables.Add(new SFSUserVariable("x", (double)localPlayer.transform.position.x));
			userVariables.Add(new SFSUserVariable("y", (double)localPlayer.transform.position.y));
			userVariables.Add(new SFSUserVariable("z", (double)localPlayer.transform.position.z));
			userVariables.Add(new SFSUserVariable("rot", (double)localPlayer.transform.rotation.eulerAngles.y));
			userVariables.Add(new SFSUserVariable("model", smartFox.MySelf.GetVariable("model").GetIntValue()));
			userVariables.Add(new SFSUserVariable("mat", smartFox.MySelf.GetVariable("mat").GetIntValue()));
			userVariables.Add(new SFSUserVariable("playerReady", true));
			smartFox.Send(new SetUserVariablesRequest(userVariables));
		}

		roomUsers.Add (user);
	}
	
	public void OnConnectionLost(BaseEvent evt) {	
		// Reset all internal states so we kick back to login screen
		smartFox.RemoveAllEventListeners();
		RemoveLocalPlayer ();
		if (Application.CanStreamedLevelBeLoaded (0)) {
			Application.LoadLevel (0);
		}

	}
	
	public void OnObjectMessage(BaseEvent evt) {
		// The only messages being send around are
		// - a remove message from someone that is dropping out
		ISFSObject dataObj = (SFSObject)evt.Params["message"];
		SFSUser sender = (SFSUser)evt.Params["sender"];
		
		if (dataObj.ContainsKey("cmd")) {
			switch (dataObj.GetUtfString("cmd")) {
			case "rm":
				Debug.Log("Removing player unit " + sender.Id);
				RemoveRemotePlayer(sender);
				break;
			}
		}
	}
	
	public void OnUserVariableUpdate(BaseEvent evt) {
		// When user variable is updated on any client, then this callback is being received
		// This is where most of the magic happens
		
		ArrayList changedVars = (ArrayList)evt.Params["changedVars"];
		SFSUser user = (SFSUser)evt.Params["user"];
		
		if (user == smartFox.MySelf) return;
		
		if (!remotePlayers.ContainsKey(user)) {
			// New client just started transmitting - lets create remote player
			Vector3 pos = new Vector3(0, 1, 0);
			if (user.ContainsVariable("x") && user.ContainsVariable("y") && user.ContainsVariable("z")) {
				pos.x = (float)user.GetVariable("x").GetDoubleValue();
				pos.y = (float)user.GetVariable("y").GetDoubleValue();
				pos.z = (float)user.GetVariable("z").GetDoubleValue();
			}
			float rotAngle = 0;
			if (user.ContainsVariable("rot")) {
				rotAngle = (float)user.GetVariable("rot").GetDoubleValue();
			}
			int numModel = 0;
			if (user.ContainsVariable("model")) {
				numModel = user.GetVariable("model").GetIntValue();
			}
			int numMaterial = 0;
			if (user.ContainsVariable("mat")) {
				numMaterial = user.GetVariable("mat").GetIntValue();
			}
			SpawnRemotePlayer(user, numModel, numMaterial, pos, Quaternion.Euler(0, rotAngle, 0));
		}
		
		// Check if the remote user changed his position or rotation
		if (changedVars.Contains("x") && changedVars.Contains("y") && changedVars.Contains("z") && changedVars.Contains("rot")) {
			// Move the character to a new position...
			remotePlayers[user].GetComponent<SimpleRemoteInterpolation>().SetTransform(
				new Vector3((float)user.GetVariable("x").GetDoubleValue(), (float)user.GetVariable("y").GetDoubleValue(), (float)user.GetVariable("z").GetDoubleValue()),
				Quaternion.Euler(0, (float)user.GetVariable("rot").GetDoubleValue(), 0),
				true);
		}
		// Remote client got new name?
		if (changedVars.Contains("name")) {
			remotePlayers[user].GetComponentInChildren<TextMesh>().text = user.Name;
		}
		// Remote client selected new model?
		if (changedVars.Contains("model")) {
			SpawnRemotePlayer(user, user.GetVariable("model").GetIntValue(), user.GetVariable("mat").GetIntValue(), remotePlayers[user].transform.position, remotePlayers[user].transform.rotation);
		}
		// Remote client selected new material?
		if (changedVars.Contains("mat")) {
			remotePlayers[user].GetComponentInChildren<Renderer>().material = playerMaterials[ user.GetVariable("mat").GetIntValue() ];
		}
		if (changedVars.Contains("playerReady")) {
			bool otherPlayerReady = true;
			foreach(SFSUser other in roomUsers){
				if(!other.Name.Equals(smartFox.MySelf.Name)){
					otherPlayerReady = other.GetVariable("playerReady").GetBoolValue();
				}
				if(!otherPlayerReady){
					break;
				}
			}
			if(otherPlayerReady){
				bool gameReadyTemp = true;
				List<RoomVariable> roomVars = new List<RoomVariable>();
				roomVars.Add(new SFSRoomVariable("gameReady", gameReadyTemp));
				smartFox.Send(new SetRoomVariablesRequest(roomVars));
			}
		}
		
	}

	public void OnRoomVariableUpdate(BaseEvent evt){

		SFSRoom room = (SFSRoom)evt.Params["room"];
		RoomVariable gameF = room.GetVariable ("gameFinished");
		if (gameF.GetBoolValue()) {
			Debug.Log ("bool=true room variable update");
//			localPlayer.SetActive(false);

			RemoveLocalPlayer();

		}

	}

	public void OnDebugMessage(BaseEvent evt) {
//		string message = (string)evt.Params["message"];
//		Debug.Log("[SFS DEBUG] " + message);
	}

	//----------------------------------------------------------
	// Private player helper methods
	//----------------------------------------------------------
	
	private void SpawnLocalPlayer(int numModel, int numMaterial) {
		Vector3 pos;
		Quaternion rot;
		
		// See if there already exists a model - if so, take its pos+rot before destroying it
		if (localPlayer != null) {
			pos = localPlayer.transform.position;
			rot = localPlayer.transform.rotation;
			Camera.main.transform.parent = null;
			Destroy(localPlayer);
		} else {

			if(spawnPoints.Length>0){
				int numposition = UnityEngine.Random.Range (0, spawnPoints.Length);
				pos= spawnPoints[numposition].transform.position;
			}
			else{
				pos = new Vector3(0, 1, 0);
			}
			rot = Quaternion.identity;
		}
		
		// Lets spawn our local player model
		localPlayer = GameObject.Instantiate(playerModels[numModel]) as GameObject;
		localPlayer.transform.position = pos;
		localPlayer.transform.rotation = rot;
		
		// Assign starting material
		localPlayer.GetComponentInChildren<Renderer>().material = playerMaterials[numMaterial];
		
		// Since this is the local player, lets add a controller and fix the camera
		localPlayer.AddComponent<PlayerController>();

		localPlayer.tag = "Player1";
		localPlayer.transform.GetChild (0).tag="Player1";

		localPlayer.AddComponent<Rigidbody>();
		localPlayer.GetComponent<Rigidbody> ().useGravity = true;
		localPlayer.GetComponent<Rigidbody> ().constraints =  RigidbodyConstraints.FreezePositionY| RigidbodyConstraints.FreezeRotation;
//		localPlayer.AddComponent<CharacterController> ();


		localPlayerController = localPlayer.GetComponent<PlayerController>();
		localPlayer.GetComponentInChildren<TextMesh>().text = smartFox.MySelf.Name;

		Camera.main.transform.position = localPlayer.transform.position;
		Camera.main.transform.parent = localPlayer.transform;
		
		// Lets set the model and material choice and tell the others about it
		List<UserVariable> userVariables = new List<UserVariable>();
		userVariables.Add(new SFSUserVariable("model", numModel));
		userVariables.Add(new SFSUserVariable("mat", numMaterial));
		smartFox.Send(new SetUserVariablesRequest(userVariables));
	}
	
	private void SpawnRemotePlayer(SFSUser user, int numModel, int numMaterial, Vector3 pos, Quaternion rot) {
		// See if there already exists a model so we can destroy it first
		if (remotePlayers.ContainsKey(user) && remotePlayers[user] != null) {
			Destroy(remotePlayers[user]);
			remotePlayers.Remove(user);
		}
		
		// Lets spawn our remote player model
		GameObject remotePlayer = GameObject.Instantiate(playerModels[numModel]) as GameObject;
		remotePlayer.AddComponent<SimpleRemoteInterpolation>();
		remotePlayer.GetComponent<SimpleRemoteInterpolation>().SetTransform(pos, rot, false);
		
		// Color and name
		remotePlayer.GetComponentInChildren<TextMesh>().text = user.Name;
		remotePlayer.GetComponentInChildren<Renderer>().material = playerMaterials[numMaterial];
		remotePlayer.tag = "Player2";
		remotePlayer.transform.GetChild (0).tag="Player2";

		remotePlayer.AddComponent<Rigidbody>();
		remotePlayer.GetComponent<Rigidbody> ().useGravity = true;
		remotePlayer.GetComponent<Rigidbody> ().constraints =  RigidbodyConstraints.FreezePositionY| RigidbodyConstraints.FreezeRotation;
		//		localPlayer.AddComponent<CharacterController> ();
		
		// Lets track the dude
		remotePlayers.Add(user, remotePlayer);
	}
	
	private void RemoveLocalPlayer() {
		// Someone dropped off the grid. Lets remove him
		SFSObject obj = new SFSObject();
		obj.PutUtfString("cmd", "rm");
		smartFox.Send(new ObjectMessageRequest(obj, smartFox.LastJoinedRoom));
		foreach(SFSUser user in smartFox.UserManager.GetUserList()){
			if(user==smartFox.MySelf){
				currentRoom.RemoveUser(user);
				smartFox.Send(new LogoutRequest());
				Debug.Log("removed local player");
				break;
			}
		}
	}
	
	private void RemoveRemotePlayer(SFSUser user) {
		if (user == smartFox.MySelf) return;
		
		if (remotePlayers.ContainsKey(user)) {
			currentRoom.RemoveUser(user);
			Destroy(remotePlayers[user]);
			remotePlayers.Remove(user);
		}
	}	
}