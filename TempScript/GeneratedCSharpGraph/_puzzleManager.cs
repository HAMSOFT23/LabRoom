#pragma warning disable
using UnityEngine;

/// <summary>
/// Optional manager to track overall puzzle completion
/// </summary>
public class _puzzleManager : MonoBehaviour {	
	[SerializeField]
	private PuzzleSocket[] allSockets;
	[SerializeField]
	private GameObject completionEffect;
	[SerializeField]
	private AudioClip completionSound;
	private int snappedPieces;
	public GameObject Open_Door;
	
	private void Start() {
		//Auto-find all sockets if not manually assigned
		if(((allSockets == null) || (allSockets.Length == 0))) {
			allSockets = Object.FindObjectsOfType<PuzzleSocket>();
		}
	}
	
	private void Update() {
	}
	
	public void PieceSnapped(PuzzleSocket socket) {
		snappedPieces += 1;
		//Check if puzzle is complete
		if((snappedPieces >= allSockets.Length)) {
			OnPuzzleComplete();
		}
	}
	
	private void OnPuzzleComplete() {
		Debug.Log("Puzzle Complete!");
		//Play completion effects
		if((completionEffect != null)) {
			Object.Instantiate<UnityEngine.GameObject>(completionEffect, this.transform.position, Quaternion.identity);
			Open_Door.GetComponent<openDoor>().Opening();
		}
		if((completionSound != null)) {
			AudioSource.PlayClipAtPoint(completionSound, this.transform.position);
		}
	}
}

