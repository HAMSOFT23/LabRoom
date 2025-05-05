#pragma warning disable
using UnityEngine;
using System.Collections.Generic;
using Rewired;

public class PlayerController : MaxyGames.UNode.RuntimeBehaviour {	
	private UnityEngine.CharacterController _charController;
	private Rewired.Player _rewiredPlayer;
	private float _mousePitch;
	private float _mouseYaw;
	private UnityEngine.Transform _playerTransform;
	private UnityEngine.Transform _camTransform;
	public float MouseSensitivity = 2F;
	private float _moveHorizontal;
	private float _moveVertical;
	private UnityEngine.Vector3 _movementDirection;
	[UnityEngine.SerializeField]
	private float _movementSpeed;
	private float _verticalVelocity;
	private bool _isGrounded;
	[UnityEngine.SerializeField]
	private float _jumpHeight = 3F;
	[UnityEngine.SerializeField]
	private float _gravity = -9.81F;
	private UnityEngine.Vector3 _move;
	[UnityEngine.SerializeField]
	private float sphereSize;
	public UnityEngine.Transform spherePosition;
	public UnityEngine.LayerMask ground;
	
	public override void OnAwake() {
		_charController = base.GetComponent<UnityEngine.CharacterController>();
		_rewiredPlayer = Rewired.ReInput.players.GetPlayer(0);
		_playerTransform = base.transform;
		_camTransform = base.GetComponentInChildren<UnityEngine.Camera>().transform;
	}
	
	private void Update() {
		MouseLook();
		PlayerMovement();
	}
	
	public void MouseLook() {
		_mouseYaw = _rewiredPlayer.GetAxis(2);
		_playerTransform.Rotate(new UnityEngine.Vector3(0F, (_mouseYaw * MouseSensitivity), 0F));
		_mousePitch += -(_rewiredPlayer.GetAxis(1) * MouseSensitivity);
		_mousePitch = UnityEngine.Mathf.Clamp(_mousePitch, -90F, 90F);
		_camTransform.localRotation = UnityEngine.Quaternion.Euler(_mousePitch, 0F, 0F);
	}
	
	public void PlayerMovement() {
		_isGrounded = _charController.isGrounded;
		if((_isGrounded && (_verticalVelocity < 0F))) {
			_verticalVelocity = -1F;
		}
		ControllerMove();
	}
	
	public void ControllerMove() {
		_moveHorizontal = _rewiredPlayer.GetAxisRaw(0);
		_moveVertical = _rewiredPlayer.GetAxisRaw(3);
		_movementDirection = ((_moveVertical * _playerTransform.forward) + (_moveHorizontal * _playerTransform.right)).normalized;
		if((_rewiredPlayer.GetButtonDown(4) && _isGrounded)) {
			_verticalVelocity = UnityEngine.Mathf.Sqrt(((_jumpHeight * -2F) * _gravity));
		}
		_verticalVelocity += (_gravity * UnityEngine.Time.deltaTime);
		_move = (_movementSpeed * _movementDirection);
		_move.y = _verticalVelocity;
		_charController.Move((_move * UnityEngine.Time.deltaTime));
	}
	
	public bool GroundCheck() {
		UnityEngine.Collider[] _out = new UnityEngine.Collider[0];
		return UnityEngine.Physics.SphereCast(this.transform.position, sphereSize, ((UnityEngine.Vector3)-base.transform.up), out _, 4F, ground);
	}
	
	private void OnDrawGizmos() {
		UnityEngine.Gizmos.color = new UnityEngine.Color() { r = 1F, g = 0.004716992F, b = 0.004716992F, a = 1F };
		UnityEngine.Gizmos.DrawWireSphere(this.transform.position, sphereSize);
	}
}

