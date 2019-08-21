using UnityEngine;

namespace Archon.SwissArmyLib.Events.Loops
{
	public abstract class ManagedUpdateBehaviour : MonoBehaviour, IEventListener
	{
		private bool _startWasCalled;

		private bool _isListening;

		private IUpdateable _updateable;

		private ILateUpdateable _lateUpdateable;

		private IFixedUpdateable _fixedUpdateable;

		private ICustomUpdateable _customUpdateable;

		private int[] _customUpdateIds;

		protected virtual int ExecutionOrder => 0;

		protected virtual void Start()
		{
			_updateable = (this as IUpdateable);
			_lateUpdateable = (this as ILateUpdateable);
			_fixedUpdateable = (this as IFixedUpdateable);
			_customUpdateable = (this as ICustomUpdateable);
			if (_updateable == null && _lateUpdateable == null && _fixedUpdateable == null && _customUpdateable == null)
			{
				UnityEngine.Debug.LogWarning("This component doesn't implement any update interfaces.", this);
			}
			_startWasCalled = true;
			StartListening();
		}

		protected virtual void OnEnable()
		{
			if (Application.isEditor)
			{
				if (_updateable == null)
				{
					_updateable = (this as IUpdateable);
				}
				if (_lateUpdateable == null)
				{
					_lateUpdateable = (this as ILateUpdateable);
				}
				if (_fixedUpdateable == null)
				{
					_fixedUpdateable = (this as IFixedUpdateable);
				}
				if (_customUpdateable == null)
				{
					_customUpdateable = (this as ICustomUpdateable);
				}
			}
			if (_startWasCalled)
			{
				StartListening();
			}
		}

		protected virtual void OnDisable()
		{
			if (_startWasCalled)
			{
				StopListening();
			}
		}

		private void StartListening()
		{
			if (_isListening)
			{
				UnityEngine.Debug.LogError("Attempt at starting to listen for updates, while already listening. Did you forget to call base.OnDisable()?");
				return;
			}
			int executionOrder = ExecutionOrder;
			if (_updateable != null)
			{
				ManagedUpdate.OnUpdate.AddListener(this, executionOrder);
			}
			if (_lateUpdateable != null)
			{
				ManagedUpdate.OnLateUpdate.AddListener(this, executionOrder);
			}
			if (_fixedUpdateable != null)
			{
				ManagedUpdate.OnFixedUpdate.AddListener(this, executionOrder);
			}
			if (_customUpdateable != null)
			{
				if (_customUpdateIds == null)
				{
					_customUpdateIds = (_customUpdateable.GetCustomUpdateIds() ?? new int[0]);
				}
				for (int i = 0; i < _customUpdateIds.Length; i++)
				{
					ManagedUpdate.AddListener(_customUpdateIds[i], this, executionOrder);
				}
			}
			_isListening = true;
		}

		private void StopListening()
		{
			if (!_isListening)
			{
				UnityEngine.Debug.LogError("Attempted to stop listening for updates while not listening. Did you forget to call base.Start() or base.OnEnable()?");
				return;
			}
			if (_updateable != null)
			{
				ManagedUpdate.OnUpdate.RemoveListener(this);
			}
			if (_lateUpdateable != null)
			{
				ManagedUpdate.OnLateUpdate.RemoveListener(this);
			}
			if (_fixedUpdateable != null)
			{
				ManagedUpdate.OnFixedUpdate.RemoveListener(this);
			}
			if (_customUpdateable != null && _customUpdateIds != null)
			{
				for (int i = 0; i < _customUpdateIds.Length; i++)
				{
					ManagedUpdate.RemoveListener(_customUpdateIds[i], this);
				}
			}
			_isListening = false;
		}

		public virtual void OnEvent(int eventId)
		{
			switch (eventId)
			{
			case -1000:
				_updateable.OnUpdate();
				return;
			case -1001:
				_lateUpdateable.OnLateUpdate();
				return;
			case -1002:
				_fixedUpdateable.OnFixedUpdate();
				return;
			}
			if (_customUpdateIds == null)
			{
				return;
			}
			int num = 0;
			while (true)
			{
				if (num < _customUpdateIds.Length)
				{
					if (_customUpdateIds[num] == eventId)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			_customUpdateable.OnCustomUpdate(eventId);
		}
	}
}
