#pragma warning disable
using UnityEngine;

public class PickupSystem : MonoBehaviour {	
	[Header("Pickup Settings")]
	[SerializeField]
	private Transform holdArea;
	private GameObject heldObj;
	private Rigidbody heldObjRB;
	[Header("Pickup Parameters")]
	[SerializeField]
	private float pickupForce = 150F;
	[SerializeField]
	private float pickupRange = 5F;
	
	private void Update() {
		if(Input.GetMouseButtonDown(0)) {
			Debug.Log("Mouse Down");
			if((heldObj == null)) {
				var hit = new RaycastHit();
				if(Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit, pickupRange)) {
					PickupObject(hit.transform.gameObject);
				}
			}
			 else {
				DropObject();
			}
		}
		if((heldObj != null)) {
			MoveObject();
		}
	}
	
	private void MoveObject() {
		if((Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1F)) {
			var moveDirection = (holdArea.position - heldObj.transform.position);
			heldObjRB.AddForce((moveDirection * pickupForce));
		}
	}
	
	private void PickupObject(GameObject pickObj) {
		if(pickObj.GetComponent<UnityEngine.Rigidbody>()) {
			heldObjRB = pickObj.GetComponent<UnityEngine.Rigidbody>();
			heldObjRB.useGravity = false;
			heldObjRB.linearDamping = 10;
			heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
			heldObjRB.transform.parent = holdArea;
			heldObj = pickObj;
		}
	}
	
	private void DropObject() {
		heldObjRB.useGravity = true;
		heldObjRB.linearDamping = 1;
		heldObjRB.constraints = RigidbodyConstraints.None;
		heldObj.transform.parent = null;
		heldObj = null;
	}
}

