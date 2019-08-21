using JetBrains.Annotations;
using UnityEngine;

namespace Archon.SwissArmyLib.Gravity
{
	[AddComponentMenu("Archon/Gravity/SphericalGravitationalPoint")]
	public class SphericalGravitationalPoint : MonoBehaviour, IGravitationalAffecter
	{
		[SerializeField]
		private float _strength = 9.82f;

		[SerializeField]
		private float _radius = 1f;

		[SerializeField]
		private AnimationCurve _dropoffCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		[SerializeField]
		private bool _isGlobal;

		private float _radiusSqr;

		private Transform _transform;

		public float Strength
		{
			get
			{
				return _strength;
			}
			set
			{
				_strength = value;
			}
		}

		public float Radius
		{
			get
			{
				return _radius;
			}
			set
			{
				_radius = value;
				_radiusSqr = value * value;
			}
		}

		public AnimationCurve DropoffCurve
		{
			get
			{
				return _dropoffCurve;
			}
			set
			{
				_dropoffCurve = value;
			}
		}

		public bool IsGlobal
		{
			get
			{
				return _isGlobal;
			}
			set
			{
				_isGlobal = value;
			}
		}

		[UsedImplicitly]
		private void Awake()
		{
			_transform = base.transform;
			_radiusSqr = _radius * _radius;
		}

		[UsedImplicitly]
		private void OnEnable()
		{
			GravitationalSystem.Register(this);
		}

		[UsedImplicitly]
		private void OnDisable()
		{
			GravitationalSystem.Unregister(this);
		}

		[UsedImplicitly]
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(_transform.position, _radius);
		}

		public Vector3 GetForceAt(Vector3 location)
		{
			Vector3 vector = _transform.position - location;
			float sqrMagnitude = vector.sqrMagnitude;
			if (_isGlobal || sqrMagnitude < _radiusSqr)
			{
				float num = _dropoffCurve.Evaluate(sqrMagnitude / _radiusSqr) * _strength;
				Vector3 normalized = vector.normalized;
				normalized.x *= num;
				normalized.y *= num;
				normalized.z *= num;
				return normalized;
			}
			return default(Vector3);
		}
	}
}
