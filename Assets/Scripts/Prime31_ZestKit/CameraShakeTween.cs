using UnityEngine;

namespace Prime31.ZestKit
{
	public class CameraShakeTween : AbstractTweenable
	{
		private Transform _cameraTransform;

		private Vector3 _shakeDirection = Vector3.zero;

		private Vector3 _shakeOffset = Vector3.zero;

		private float _shakeIntensity = 0.3f;

		private float _shakeDegredation = 0.95f;

		public CameraShakeTween(Camera camera, float shakeIntensity = 0.3f, float shakeDegredation = 0.95f, Vector3 shakeDirection = default(Vector3))
		{
			_cameraTransform = camera.transform;
			_shakeIntensity = shakeIntensity;
			_shakeDegredation = shakeDegredation;
			_shakeDirection = shakeDirection.normalized;
		}

		public void shake(float shakeIntensity = 0.3f, float shakeDegredation = 0.95f, Vector3 shakeDirection = default(Vector3))
		{
			if (!_isCurrentlyManagedByZestKit || _shakeIntensity < shakeIntensity)
			{
				_shakeDirection = shakeDirection.normalized;
				_shakeIntensity = shakeIntensity;
				if (shakeDegredation < 0f || shakeDegredation >= 1f)
				{
					shakeDegredation = 0.98f;
				}
				_shakeDegredation = shakeDegredation;
			}
			if (!_isCurrentlyManagedByZestKit)
			{
				start();
			}
		}

		public override bool tick()
		{
			if (_isPaused)
			{
				return false;
			}
			if (Mathf.Abs(_shakeIntensity) > 0f)
			{
				_shakeOffset = _shakeDirection;
				if (_shakeOffset != Vector3.zero)
				{
					_shakeOffset.Normalize();
				}
				else
				{
					_shakeOffset.x += Random.Range(0f, 1f) - 0.5f;
					_shakeOffset.y += Random.Range(0f, 1f) - 0.5f;
				}
				_shakeOffset *= _shakeIntensity;
				_shakeIntensity *= 0f - _shakeDegredation;
				if (Mathf.Abs(_shakeIntensity) <= 0.01f)
				{
					_shakeIntensity = 0f;
				}
				_cameraTransform.position += _shakeOffset;
				return false;
			}
			_isCurrentlyManagedByZestKit = false;
			return true;
		}
	}
}
