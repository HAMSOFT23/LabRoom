#pragma warning disable
using UnityEngine;

public class SlidingDoor : MonoBehaviour {	
	[Header("Door Settings")]
	[Tooltip("How far the door should slide when opened")]
	public float doorOpenDistance = 3F;
	[Tooltip("Maximum speed the door can move")]
	public float maxMovementSpeed = 2F;
	[Tooltip("How quickly the door reaches maximum speed")]
	public float acceleration = 4F;
	[Tooltip("How quickly the door slows down before reaching target")]
	public float deceleration = 6F;
	[Tooltip("Direction the door slides (default is along X axis)")]
	public Vector3 slideDirection = Vector3.right;
	private Vector3 initialPosition;
	private Vector3 targetPosition;
	private float currentMovementSpeed;
	private bool isOpening;
	private bool isClosing;
	[SerializeField]
	private AudioSource audSource;
	[SerializeField]
	private AudioClip doorSlide;
	
	private void Start() {
		//Store the initial position and calculate target position
		initialPosition = this.transform.position;
		slideDirection = slideDirection.normalized;
		targetPosition = (initialPosition + (slideDirection * doorOpenDistance));
	}
	
	public void Opening() {
		Debug.Log("Door Opened");
		isOpening = true;
		isClosing = false;
		audSource.PlayOneShot(doorSlide);
	}
	
	public void Closing() {
		Debug.Log("Door Closed");
		isOpening = false;
		isClosing = true;
	}
	
	public void ToggleDoor() {
		if((isOpening || (!(isClosing) && (Vector3.Distance(this.transform.position, targetPosition) < 0.01F)))) {
			Closing();
		}
		 else {
			Opening();
		}
	}
	
	private void Update() {
		if((isOpening || isClosing)) {
			//Determine target based on door state
			var target = (isOpening ? targetPosition : initialPosition);
			//Calculate direction and remaining distance
			var directionToTarget = (target - this.transform.position).normalized;
			var distanceToTarget = Vector3.Distance(this.transform.position, target);
			//Adjust speed based on distance to target (acceleration and deceleration)
			if((distanceToTarget < (((deceleration * currentMovementSpeed) * currentMovementSpeed) / 2))) {
				//Deceleration phase - slow down as we approach target
				currentMovementSpeed = Mathf.Max(0.1F, (currentMovementSpeed - (deceleration * Time.deltaTime)));
			}
			 else if((currentMovementSpeed < maxMovementSpeed)) {
				//Acceleration phase - speed up from standstill
				currentMovementSpeed = Mathf.Min(maxMovementSpeed, (currentMovementSpeed + (acceleration * Time.deltaTime)));
			}
			//Calculate movement this frame
			var movementThisFrame = (currentMovementSpeed * Time.deltaTime);
			//Prevent overshooting the target
			if((movementThisFrame > distanceToTarget)) {
				this.transform.position = target;
				//Door reached its destination
				isOpening = false;
				isClosing = false;
				currentMovementSpeed = 0;
			}
			 else {
				//Apply smooth movement
				this.transform.position += (directionToTarget * movementThisFrame);
			}
		}
	}
}

