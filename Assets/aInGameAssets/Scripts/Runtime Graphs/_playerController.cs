#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

public class _playerController : MonoBehaviour {	
	private CharacterController _charController;
	private Rewired.Player _rewiredPlayer;
	private float _mousePitch;
	private float _mouseYaw;
	private Transform _playerTransform;
	private Transform _camTransform;
	public float MouseSensitivity = 2F;
	private float _moveHorizontal;
	private float _moveVertical;
	private Vector3 _movementDirection;
	[SerializeField]
	private float _movementSpeed;
	private float _verticalVelocity;
	[SerializeField]
	private float _jumpHeight = 3F;
	[SerializeField]
	private float _gravity = -9.81F;
	private Vector3 _move;
	[SerializeField]
	private float sphereSize;
	public Vector3 spherePosition;
	public LayerMask ground;
	public float sphereMaxDistance;
	
	public void Awake() {
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 11, "exit");
		MaxyGames.UNode.GraphDebug.Value(_charController = MaxyGames.UNode.GraphDebug.Value(base.GetComponent<UnityEngine.CharacterController>(), this, 997231961, 13, "value", false), this, 997231961, 13, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 13, "exit");
		MaxyGames.UNode.GraphDebug.Value(_rewiredPlayer = MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(Rewired.ReInput.players, this, 997231961, 18, "-instance", false).GetPlayer(0), this, 997231961, 16, "value", false), this, 997231961, 16, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 16, "exit");
		MaxyGames.UNode.GraphDebug.Value(_playerTransform = MaxyGames.UNode.GraphDebug.Value(base.transform, this, 997231961, 33, "value", false), this, 997231961, 33, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 33, "exit");
		MaxyGames.UNode.GraphDebug.Value(_camTransform = MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(base.GetComponentInChildren<UnityEngine.Camera>(), this, 997231961, 38, "-instance", false).transform, this, 997231961, 36, "value", false), this, 997231961, 36, "target", true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 36, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 33, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 16, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 13, true);
	}
	
	private void Update() {
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 22, "exit");
		MouseLook();
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 24, "exit");
		PlayerMovement();
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 66, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 24, true);
	}
	
	public void MouseLook() {
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 20, "exit");
		MaxyGames.UNode.GraphDebug.Value(_mouseYaw = MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(_rewiredPlayer, this, 997231961, 27, "-instance", false).GetAxis(2), this, 997231961, 31, "value", false), this, 997231961, 31, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 31, "exit");
		MaxyGames.UNode.GraphDebug.Value(_playerTransform, this, 997231961, 40, "-instance", false).Rotate(MaxyGames.UNode.GraphDebug.Value(new Vector3(0F, (_mouseYaw * MouseSensitivity), 0F), this, 997231961, 40, "-0-0", false));
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 40, "exit");
		MaxyGames.UNode.GraphDebug.Value(_mousePitch += MaxyGames.UNode.GraphDebug.Value(-MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(_rewiredPlayer, this, 997231961, 26, "-instance", false).GetAxis(1), this, 997231961, 59, "7ba3e442ff2653d", false) * MaxyGames.UNode.GraphDebug.Value(MouseSensitivity, this, 997231961, 59, "7f7212c9571383ff", false)), this, 997231961, 60, "target", false), this, 997231961, 29, "value", false), this, 997231961, 29, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 29, "exit");
		MaxyGames.UNode.GraphDebug.Value(_mousePitch = MaxyGames.UNode.GraphDebug.Value(Mathf.Clamp(MaxyGames.UNode.GraphDebug.Value(_mousePitch, this, 997231961, 57, "-0-0", false), -90F, 90F), this, 997231961, 58, "value", false), this, 997231961, 58, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 58, "exit");
		MaxyGames.UNode.GraphDebug.Value(_camTransform.localRotation = MaxyGames.UNode.GraphDebug.Value(Quaternion.Euler(MaxyGames.UNode.GraphDebug.Value(_mousePitch, this, 997231961, 52, "-0-0", false), 0F, 0F), this, 997231961, 50, "value", false), this, 997231961, 50, "target", true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 50, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 58, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 29, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 40, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 31, true);
	}
	
	public void PlayerMovement() {
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 62, "exit");
		if(MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(GroundCheckBool(), this, 997231961, 107, "5130d2594b93134", false) && MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_verticalVelocity, this, 997231961, 109, "inputA", false) < 0F), this, 997231961, 107, "47ceb959432e8024", false)), this, 997231961, 105, "condition", false)) {
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 105, "onTrue");
			MaxyGames.UNode.GraphDebug.Value(_verticalVelocity = -1F, this, 997231961, 110, "target", true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 110, true);
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 105, "exit");
			ControllerMove();
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 65, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 105, true);
		} else {
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 105, "exit");
			ControllerMove();
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 65, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 105, false);
		}
	}
	
	public void ControllerMove() {
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 67, "exit");
		MaxyGames.UNode.GraphDebug.Value(_moveHorizontal = MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(_rewiredPlayer, this, 997231961, 72, "-instance", false).GetAxisRaw(0), this, 997231961, 76, "value", false), this, 997231961, 76, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 76, "exit");
		MaxyGames.UNode.GraphDebug.Value(_moveVertical = MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(_rewiredPlayer, this, 997231961, 74, "-instance", false).GetAxisRaw(3), this, 997231961, 78, "value", false), this, 997231961, 78, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 78, "exit");
		MaxyGames.UNode.GraphDebug.Value(_movementDirection = MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_moveVertical, this, 997231961, 84, "3a8bd042717851c8", false) * MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(_playerTransform, this, 997231961, 83, "-instance", false).forward, this, 997231961, 84, "498947631fd37a97", false)), this, 997231961, 85, "3f523ddc40be19e4", false) + MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_moveHorizontal, this, 997231961, 90, "3a8bd042717851c8", false) * MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(_playerTransform, this, 997231961, 89, "-instance", false).right, this, 997231961, 90, "498947631fd37a97", false)), this, 997231961, 85, "374adf9d111f7d9b", false)), this, 997231961, 86, "-instance", false).normalized, this, 997231961, 80, "value", false), this, 997231961, 80, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 80, "exit");
		if(MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(_rewiredPlayer, this, 997231961, 113, "-instance", false).GetButtonDown(4), this, 997231961, 114, "6cfe985351fb8b8c", false) && MaxyGames.UNode.GraphDebug.Value(GroundCheckBool(), this, 997231961, 114, "3c711aa3e68e329", false)), this, 997231961, 111, "condition", false)) {
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 111, "onTrue");
			MaxyGames.UNode.GraphDebug.Value(_verticalVelocity = MaxyGames.UNode.GraphDebug.Value(Mathf.Sqrt(MaxyGames.UNode.GraphDebug.Value(((MaxyGames.UNode.GraphDebug.Value(_jumpHeight, this, 997231961, 119, "4878fa0048216f91", false) * -2F) * MaxyGames.UNode.GraphDebug.Value(_gravity, this, 997231961, 119, "8188f0942e7b28f", false)), this, 997231961, 117, "-0-0", false)), this, 997231961, 116, "value", false), this, 997231961, 116, "target", true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 116, true);
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 111, "exit");
			MaxyGames.UNode.GraphDebug.Value(_verticalVelocity += MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_gravity, this, 997231961, 139, "1e51f23414aec0f7", false) * Time.deltaTime), this, 997231961, 128, "value", false), this, 997231961, 128, "target", true);
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 128, "exit");
			MaxyGames.UNode.GraphDebug.Value(_move = MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_movementSpeed, this, 997231961, 145, "274354b247f75698", false) * MaxyGames.UNode.GraphDebug.Value(_movementDirection, this, 997231961, 145, "4274bf8855e2a4d8", false)), this, 997231961, 142, "value", false), this, 997231961, 142, "target", true);
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 142, "exit");
			MaxyGames.UNode.GraphDebug.Value(_move.y = MaxyGames.UNode.GraphDebug.Value(_verticalVelocity, this, 997231961, 148, "value", false), this, 997231961, 148, "target", true);
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 148, "exit");
			MaxyGames.UNode.GraphDebug.Value(_charController, this, 997231961, 70, "-instance", false).Move(MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_move, this, 997231961, 140, "6ff31b7d58cbef8a", false) * Time.deltaTime), this, 997231961, 70, "-0-0", false));
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 70, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 148, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 142, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 128, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 111, true);
		} else {
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 111, "exit");
			MaxyGames.UNode.GraphDebug.Value(_verticalVelocity += MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_gravity, this, 997231961, 139, "1e51f23414aec0f7", false) * Time.deltaTime), this, 997231961, 128, "value", false), this, 997231961, 128, "target", true);
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 128, "exit");
			MaxyGames.UNode.GraphDebug.Value(_move = MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_movementSpeed, this, 997231961, 145, "274354b247f75698", false) * MaxyGames.UNode.GraphDebug.Value(_movementDirection, this, 997231961, 145, "4274bf8855e2a4d8", false)), this, 997231961, 142, "value", false), this, 997231961, 142, "target", true);
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 142, "exit");
			MaxyGames.UNode.GraphDebug.Value(_move.y = MaxyGames.UNode.GraphDebug.Value(_verticalVelocity, this, 997231961, 148, "value", false), this, 997231961, 148, "target", true);
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 148, "exit");
			MaxyGames.UNode.GraphDebug.Value(_charController, this, 997231961, 70, "-instance", false).Move(MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(_move, this, 997231961, 140, "6ff31b7d58cbef8a", false) * Time.deltaTime), this, 997231961, 70, "-0-0", false));
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 70, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 148, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 142, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 128, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 111, false);
		}
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 80, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 78, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 76, true);
	}
	
	public bool GroundCheckBool() {
		Collider[] _out = new Collider[0];
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 152, "exit");
		return MaxyGames.UNode.GraphDebug.Value(Physics.SphereCast(MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(this.transform, this, 997231961, 237, "-instance", false).position, this, 997231961, 236, "7ebaf4c42473f66e", false) - spherePosition), this, 997231961, 197, "-0-0", false), sphereSize, MaxyGames.UNode.GraphDebug.Value(-MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(base.transform, this, 997231961, 226, "-instance", false).up, this, 997231961, 232, "target", false), this, 997231961, 197, "-0-2", false), out _, sphereMaxDistance, ground), this, 997231961, 262, "value", false);
	}
	
	private void OnDrawGizmos() {
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 165, "exit");
		MaxyGames.UNode.GraphDebug.Value(Gizmos.color = new Color() { r = 1F, g = 0.004716992F, b = 0.004716992F, a = 1F }, this, 997231961, 168, "target", true);
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 168, "exit");
		Gizmos.DrawWireSphere(MaxyGames.UNode.GraphDebug.Value((MaxyGames.UNode.GraphDebug.Value(MaxyGames.UNode.GraphDebug.Value(this.transform, this, 997231961, 240, "-instance", false).position, this, 997231961, 239, "7ebaf4c42473f66e", false) - spherePosition), this, 997231961, 170, "-0-0", false), sphereSize);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 170, true);
		MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 168, true);
	}
	
	private void FixedUpdate() {
		MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 248, "exit");
		if(MaxyGames.UNode.GraphDebug.Value(GroundCheckBool(), this, 997231961, 251, "condition", false)) {
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 251, "onTrue");
			Debug.Log("Grounded");
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 253, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 251, true);
		} else {
			MaxyGames.UNode.GraphDebug.Flow(this, 997231961, 251, "onFalse");
			Debug.Log("Not Grounded");
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 254, true);
			MaxyGames.UNode.GraphDebug.FlowNode(this, 997231961, 251, false);
		}
	}
}

