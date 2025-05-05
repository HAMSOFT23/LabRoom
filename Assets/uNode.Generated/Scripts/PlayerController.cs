#pragma warning disable
using UnityEngine;
using System.Collections.Generic;
using Rewired;

public class PlayerController : MaxyGames.UNode.RuntimeBehaviour {	
	private CharacterController _charController;
	private Player _rewiredPlayer;
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
	private bool _isGrounded;
	[SerializeField]
	private float _jumpHeight = 3F;
	[SerializeField]
	private float _gravity = -9.81F;
	private Vector3 _move;
	
	public override void OnAwake() {
		_charController = base.GetComponent<UnityEngine.CharacterController>();
		_rewiredPlayer = ReInput.players.GetPlayer(0);
		_playerTransform = base.transform;
		_camTransform = base.GetComponentInChildren<UnityEngine.Camera>().transform;
	}
	
	private void Update() {
		MouseLook();
		PlayerMovement();
	}
	
	public void MouseLook() {
		_mouseYaw = _rewiredPlayer.GetAxis(2);
		_playerTransform.Rotate(new Vector3(0F, (_mouseYaw * MouseSensitivity), 0F));
		_mousePitch += -(_rewiredPlayer.GetAxis(1) * MouseSensitivity);
		_mousePitch = Mathf.Clamp(_mousePitch, -90F, 90F);
		_camTransform.localRotation = Quaternion.Euler(_mousePitch, 0F, 0F);
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
			_verticalVelocity = Mathf.Sqrt(((_jumpHeight * -2F) * _gravity));
		}
		_verticalVelocity += (_gravity * Time.deltaTime);
		_move = (_movementSpeed * _movementDirection);
		_move.y = _verticalVelocity;
		_charController.Move((_move * Time.deltaTime));
	}
}

