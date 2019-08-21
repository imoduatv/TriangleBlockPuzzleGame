using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class ActionTask : AbstractTweenable
	{
		private Action<ActionTask> _action;

		private float _unfilteredElapsedTime;

		private float _elapsedTime;

		private float _initialDelay;

		private float _repeatDelay;

		private bool _repeats;

		private bool _isTimeScaleIndependent;

		private ActionTask _continueWithTask;

		private ActionTask _waitForTask;

		public object context
		{
			get;
			private set;
		}

		public float elapsedTime => _unfilteredElapsedTime;

		public static ActionTask create(Action<ActionTask> action)
		{
			return QuickCache<ActionTask>.pop().setAction(action);
		}

		public static ActionTask create(object context, Action<ActionTask> action)
		{
			return QuickCache<ActionTask>.pop().setAction(action).setContext(context);
		}

		public static ActionTask every(float repeatDelay, object context, Action<ActionTask> action)
		{
			ActionTask actionTask = QuickCache<ActionTask>.pop().setAction(action).setRepeats(repeatDelay)
				.setContext(context);
			actionTask.start();
			return actionTask;
		}

		public static ActionTask every(float initialDelay, float repeatDelay, object context, Action<ActionTask> action)
		{
			ActionTask actionTask = QuickCache<ActionTask>.pop().setAction(action).setRepeats(repeatDelay)
				.setContext(context)
				.setDelay(initialDelay);
			actionTask.start();
			return actionTask;
		}

		public static ActionTask afterDelay(float initialDelay, object context, Action<ActionTask> action)
		{
			ActionTask actionTask = QuickCache<ActionTask>.pop().setAction(action).setDelay(initialDelay)
				.setContext(context);
			actionTask.start();
			return actionTask;
		}

		public override bool tick()
		{
			if (_waitForTask != null)
			{
				if (_waitForTask.isRunning())
				{
					return false;
				}
				_waitForTask = null;
			}
			if (_isPaused)
			{
				return false;
			}
			float num = (!_isTimeScaleIndependent) ? Time.deltaTime : Time.unscaledDeltaTime;
			if (_initialDelay > 0f)
			{
				_initialDelay -= num;
				if (_initialDelay < 0f)
				{
					_elapsedTime = 0f - _initialDelay;
					_action(this);
					if (_repeats)
					{
						return false;
					}
					if (_continueWithTask != null)
					{
						_continueWithTask.start();
					}
					if (_isCurrentlyManagedByZestKit)
					{
						_isCurrentlyManagedByZestKit = false;
						return true;
					}
					return false;
				}
				return false;
			}
			if (_repeatDelay > 0f)
			{
				if (_elapsedTime > _repeatDelay)
				{
					_elapsedTime -= _repeatDelay;
					_action(this);
				}
			}
			else
			{
				_action(this);
			}
			_unfilteredElapsedTime += num;
			_elapsedTime += num;
			return false;
		}

		public override void stop(bool runContinueWithTaskIfPresent = true)
		{
			if (runContinueWithTaskIfPresent && _continueWithTask != null)
			{
				_continueWithTask.start();
			}
			base.stop();
		}

		public override void recycleSelf()
		{
			_unfilteredElapsedTime = (_elapsedTime = (_initialDelay = (_repeatDelay = 0f)));
			_isPaused = (_isCurrentlyManagedByZestKit = (_repeats = (_isTimeScaleIndependent = false)));
			context = null;
			_action = null;
			_continueWithTask = (_waitForTask = null);
			QuickCache<ActionTask>.push(this);
		}

		public ActionTask setAction(Action<ActionTask> action)
		{
			_action = action;
			return this;
		}

		public ActionTask setDelay(float delay)
		{
			_initialDelay = delay;
			return this;
		}

		public ActionTask setRepeats(float repeatDelay = 0f)
		{
			_repeats = true;
			_repeatDelay = repeatDelay;
			return this;
		}

		public ActionTask setContext(object context)
		{
			this.context = context;
			return this;
		}

		public ActionTask setIsTimeScaleIndependent()
		{
			_isTimeScaleIndependent = true;
			return this;
		}

		public ActionTask continueWith(ActionTask actionTask)
		{
			if (actionTask.isRunning())
			{
				UnityEngine.Debug.LogError("Attempted to continueWith an ActionTask that is already running. You can only continueWith tasks that have not started yet");
			}
			else
			{
				_continueWithTask = actionTask;
			}
			return this;
		}

		public ActionTask waitFor(ActionTask actionTask)
		{
			if (!actionTask.isRunning())
			{
				UnityEngine.Debug.LogError("Attempted to waitFor an ActionTask that is not running. You can only waitFor tasks that are already running.");
			}
			else
			{
				_waitForTask = actionTask;
			}
			return this;
		}
	}
}
