#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

public class openDoor : MonoBehaviour {	
	public Transform trans_;
	
	private void Start() {
		trans_ = (base.GetComponent<UnityEngine.Transform>() as Transform);
	}
	
	public void Opening() {
		Debug.Log("Function Called");
		trans_.transform.Translate(0F, -5F, 0F);
	}
}

