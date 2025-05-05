#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

public class openDoor : MonoBehaviour {	
	public Transform trans_;
	public float displacementSpeed;
	
	private void Start() {
		trans_ = (base.GetComponent<UnityEngine.Transform>() as Transform);
	}
	
	public void Opening() {
		Debug.Log("Door Opened");
		trans_.transform.Translate(0F, (displacementSpeed * Time.deltaTime), 0F);
	}
}

