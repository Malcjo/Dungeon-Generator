using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerLook : MonoBehaviour
{
	public GameObject CinemachineCameraTarget;
	private PlayerInputConfig _input;
	private GameObject _mainCamera;

	public float RotationSpeed = 1.0f;
	public float TopClamp = 90.0f;
	public float BottomClamp = -90.0f;

	private const float _threshold = 0.01f;
	private float _cinemachineTargetPitch;
	private float _rotationVelocity;
	void Start()
    {
        _input = GetComponent<PlayerInputConfig>();
    }
	private void LateUpdate()
	{
		CameraRotation();
	}

	private void CameraRotation()
	{
		// if there is an input
		if (_input.look.sqrMagnitude >= _threshold)
		{
			_cinemachineTargetPitch += _input.look.y * RotationSpeed * Time.deltaTime;
			_rotationVelocity = _input.look.x * RotationSpeed * Time.deltaTime;

			// clamp our pitch rotation
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Update Cinemachine camera target pitch
			CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

			// rotate the player left and right
			transform.Rotate(Vector3.up * _rotationVelocity);
		}
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}
}
