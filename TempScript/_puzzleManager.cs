#pragma warning disable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class _puzzleManager : MonoBehaviour {	
	[Header("Socket References")]
	[Tooltip("Manually assign sockets or leave empty to auto-find")]
	[SerializeField]
	private PuzzleSocket[] allSockets;
	[Header("Completion Effects")]
	[SerializeField]
	private GameObject completionEffect;
	[SerializeField]
	private AudioClip completionSound;
	[Header("Events")]
	public UnityEvent onPuzzleComplete;
	private Dictionary<PuzzleSocket, bool> socketStatus = new Dictionary<PuzzleSocket, bool>();
	private int totalSockets;
	private int snappedSockets;
	private bool puzzleCompleted;
	[SerializeField]
	private GameObject door;
	
	public static PuzzleManager Instance {
		get;
		set;
	}
	
	private void Awake() {
		//Setup singleton pattern
		if((Instance == null)) {
			Instance = null;
		}
		 else if((Instance != this)) {
			Debug.LogWarning("Multiple PuzzleManagers found. Using the first one.");
			Object.Destroy(this);
			return;
		}
	}
	
	private void Start() {
		InitializeSockets();
	}
	
	/// <summary>
	/// Finds and initializes all puzzle sockets
	/// </summary>
	private void InitializeSockets() {
		//Clear any existing data
		socketStatus.Clear();
		snappedSockets = 0;
		//Auto-find all sockets if not manually assigned
		if(((allSockets == null) || (allSockets.Length == 0))) {
			allSockets = Object.FindObjectsOfType<PuzzleSocket>();
			Debug.Log("Auto-found " + allSockets.Length + " puzzle sockets");
		}
		//Initialize tracking dictionary
		foreach(PuzzleSocket loopValue in allSockets) {
			if((loopValue != null)) {
				socketStatus[loopValue] = false;
			}
		}
		totalSockets = socketStatus.Count;
		Debug.Log("PuzzleManager initialized with " + totalSockets + " valid sockets");
	}
	
	/// <summary>
	/// Called by a socket when a piece snaps into it
	/// </summary>
	public void PieceSnapped(PuzzleSocket socket) {
		//Ignore if puzzle is already complete
		if(puzzleCompleted) {
			return;
		}
		Debug.Log("Piece snapped into socket: " + socket.name);
		//Check if this socket is already counted
		if((socketStatus.ContainsKey(socket) && !(socketStatus[socket]))) {
			socketStatus[socket] = true;
			snappedSockets += 1;
			Debug.Log("Socket progress: " + snappedSockets + "/" + totalSockets);
			//Check if puzzle is complete
			CheckCompletion();
		}
	}
	
	/// <summary>
	/// Checks if all sockets have pieces in them
	/// </summary>
	private void CheckCompletion() {
		if(((snappedSockets >= totalSockets) && !(puzzleCompleted))) {
			puzzleCompleted = true;
			OnPuzzleComplete();
		}
	}
	
	/// <summary>
	/// Called when all sockets have pieces
	/// </summary>
	private void OnPuzzleComplete() {
		Debug.Log("Puzzle Complete!");
		//Play completion effects
		if((completionEffect != null)) {
			Object.Instantiate<UnityEngine.GameObject>(completionEffect, this.transform.position, Quaternion.identity);
			door.GetComponent<openDoor>().Opening();
		}
		if((completionSound != null)) {
			AudioSource.PlayClipAtPoint(completionSound, this.transform.position);
		}
		//Invoke the Unity event
		onPuzzleComplete.Invoke();
	}
	
	/// <summary>
	/// Reset the puzzle state
	/// </summary>
	public void ResetPuzzle() {
		puzzleCompleted = false;
		snappedSockets = 0;
		//Reset all socket statuses
		foreach(PuzzleSocket loopValue1 in socketStatus.Keys) {
			var localVar = (loopValue1 as PuzzleSocket);
			socketStatus[localVar] = false;
		}
		Debug.Log("Puzzle has been reset");
	}
	
	/// <summary>
	/// Returns the current completion percentage
	/// </summary>
	public float GetCompletionPercentage() {
		if((totalSockets <= 0)) {
			return 0;
		}
		return (((float)snappedSockets) / totalSockets);
	}
}

