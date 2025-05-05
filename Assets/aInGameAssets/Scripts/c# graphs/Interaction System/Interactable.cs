#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

public abstract class Interactable : MonoBehaviour, IInteractable {	
	public virtual void OnInteract() {
		Debug.Log("Interacted");
	}
	
	public virtual void OnInteractStay() {
		Debug.Log("Interactive stay");
	}
	
	public virtual void OnInteractExit() {
		Debug.Log("Interactive Left");
	}
}

