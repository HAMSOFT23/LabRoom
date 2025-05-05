#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

public interface IInteractable {	
	public void OnInteract() ;
	
	public void OnInteractExit() ;
	
	public void OnInteractStay() ;
}

