using UnityEngine;

public class PickupSystem : MonoBehaviour 
{ 
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    
    [Header("Pickup Parameters")]
    [SerializeField] private float pickupForce = 150F;
    [SerializeField] private float pickupRange = 5F;
    [SerializeField] private float maxVelocityMagnitude = 3f; // Add velocity clamping
    private void Update() {
        // Keep input detection in Update
        if(Input.GetMouseButtonDown(0)) {
            Debug.Log("Mouse Down");
            if(heldObj == null) {
                var hit = new RaycastHit();
                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject);
                }
            } else {
                DropObject();
            }
        }
    }
    
    // Move physics handling to FixedUpdate
    private void FixedUpdate() {
        if(heldObj != null) {
            MoveObject();
        }
    }
    
    private void MoveObject() {
        if(Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1F) {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
            
            // Clamp velocity to prevent excessive speed
            if(heldObjRB.linearVelocity.magnitude > maxVelocityMagnitude) {
                heldObjRB.linearVelocity = heldObjRB.linearVelocity.normalized * maxVelocityMagnitude;
            }
        }
    }
    
    private void PickupObject(GameObject pickObj)
    {
        if(pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.linearDamping = 10; // Use drag instead of linearDamping
            heldObjRB.angularDamping = 5; // Add angular drag
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            heldObjRB.interpolation = RigidbodyInterpolation.Interpolate; // Add interpolation
            
            heldObjRB.includeLayers = ~0;
            heldObjRB.excludeLayers = LayerMask.GetMask("Player");
            heldObj = pickObj;
            // Don't parent the object - can cause physics issues
        }
    }
    
    private void DropObject() {
        heldObjRB.useGravity = true;
        heldObjRB.linearDamping = 0.05f; // More realistic than 1
        heldObjRB.angularDamping = 0.05f;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.interpolation = RigidbodyInterpolation.None;
        
        heldObjRB.excludeLayers = LayerMask.GetMask("Nothing");

        heldObj = null;
    }
}