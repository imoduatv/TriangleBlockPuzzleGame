using System;
using System.Collections;
using UnityEngine;

namespace Prime31.ZestKit
{
	public abstract class Tween<T> : ITweenable, ITween<T>, ITweenControl where T : struct
	{
		protected enum TweenState
		{
			Running,
			Paused,
			Complete
		}

		protected ITweenTarget<T> _target;

		protected bool _isFromValueOverridden;

		protected T _fromValue;

		protected T _toValue;

		protected EaseType _easeType;

		protected AnimationCurve _animationCurve;

		protected bool _shouldRecycleTween = true;

		protected bool _isRelative;

		protected Action<ITween<T>> _completionHandler;

		protected Action<ITween<T>> _loopCompleteHandler;

		protected ITweenable _nextTween;

		protected TweenState _tweenState = TweenState.Complete;

		private bool _isTimeScaleIndependent;

		protected float _delay;

		protected float _duration;

		protected float _timeScale = 1f;

		protected float _elapsedTime;

		protected LoopType _loopType;

		protected int _loops;

		protected float _delayBetweenLoops;

		private bool _isRunningInReverse;

		public object context
		{
			get;
			protected set;
		}

		public ITween<T> setEaseType(EaseType easeType)
		{
			_easeType = easeType;
			return this;
		}

		public ITween<T> setAnimationCurve(AnimationCurve animationCurve)
		{
			_animationCurve = animationCurve;
			return this;
		}

		public ITween<T> setDelay(float delay)
		{
			_delay = delay;
			_elapsedTime = 0f - _delay;
			return this;
		}

		public ITween<T> setDuration(float duration)
		{
			_duration = duration;
			return this;
		}

		public ITween<T> setTimeScale(float timeScale)
		{
			_timeScale = timeScale;
			return this;
		}

		public ITween<T> setIsTimeScaleIndependent()
		{
			_isTimeScaleIndependent = true;
			return this;
		}

		public ITween<T> setCompletionHandler(Action<ITween<T>> completionHandler)
		{
			_completionHandler = completionHandler;
			return this;
		}

		public ITween<T> setLoops(LoopType loopType, int loops = 1, float delayBetweenLoops = 0f)
		{
			_loopType = loopType;
			_delayBetweenLoops = delayBetweenLoops;
			if (loopType == LoopType.PingPong)
			{
				loops *= 2;
			}
			_loops = loops;
			return this;
		}

		public ITween<T> setLoopCompletionHandler(Action<ITween<T>> loopCompleteHandler)
		{
			_loopCompleteHandler = loopCompleteHandler;
			return this;
		}

		public ITween<T> setFrom(T from)
		{
			_isFromValueOverridden = true;
			_fromValue = from;
			return this;
		}

		public ITween<T> prepareForReuse(T from, T to, float duration)
		{
			initialize(_target, to, duration);
			return this;
		}

		public ITween<T> setRecycleTween(bool shouldRecycleTween)
		{
			_shouldRecycleTween = shouldRecycleTween;
			return this;
		}

		public abstract ITween<T> setIsRelative();

		public ITween<T> setContext(object context)
		{
			this.context = context;
			return this;
		}

		public ITween<T> setNextTween(ITweenable nextTween)
		{
			_nextTween = nextTween;
			return this;
		}

		public ITween<T> resetTweenState()
		{
			_tweenState = TweenState.Complete;
			return this;
		}

		public bool tick()
		{
			if (_tweenState == TweenState.Paused)
			{
				return false;
			}
			float elapsedTimeExcess = 0f;
			if (!_isRunningInReverse && _elapsedTime >= _duration)
			{
				elapsedTimeExcess = _elapsedTime - _duration;
				_elapsedTime = _duration;
				_tweenState = TweenState.Complete;
			}
			else if (_isRunningInReverse && _elapsedTime <= 0f)
			{
				elapsedTimeExcess = 0f - _elapsedTime;
				_elapsedTime = 0f;
				_tweenState = TweenState.Complete;
			}
			if (_elapsedTime >= 0f && _elapsedTime <= _duration)
			{
				updateValue();
			}
			if (_loopType != 0 && _tweenState == TweenState.Complete && _loops > 0)
			{
				handleLooping(elapsedTimeExcess);
			}
			float num = (!_isTimeScaleIndependent) ? Time.deltaTime : Time.unscaledDeltaTime;
			num *= _timeScale;
			if (_isRunningInReverse)
			{
				_elapsedTime -= num;
			}
			else
			{
				_elapsedTime += num;
			}
			if (_tweenState == TweenState.Complete)
			{
				if (_completionHandler != null)
				{
					_completionHandler(this);
				}
				if (_nextTween != null)
				{
					_nextTween.start();
					_nextTween = null;
				}
				return true;
			}
			return false;
		}

		public virtual void recycleSelf()
		{
			if (_shouldRecycleTween)
			{
				_target = null;
				_nextTween = null;
			}
		}

		public bool isRunning()
		{
			return _tweenState == TweenState.Running;
		}

		public virtual void start()
		{
			if (!_isFromValueOverridden)
			{
				_fromValue = _target.getTweenedValue();
			}
			if (_tweenState == TweenState.Complete)
			{
				_tweenState = TweenState.Running;
				ZestKit.instance.addTween(this);
			}
		}

		public void pause()
		{
			_tweenState = TweenState.Paused;
		}

		public void resume()
		{
			_tweenState = TweenState.Running;
		}

		public void stop(bool bringToCompletion = false)
		{
			_tweenState = TweenState.Complete;
			if (bringToCompletion)
			{
				_elapsedTime = ((!_isRunningInReverse) ? _duration : 0f);
				_loopType = LoopType.None;
				_loops = 0;
			}
			else
			{
				ZestKit.instance.removeTween(this);
			}
		}

		public void jumpToElapsedTime(float elapsedTime)
		{
			_elapsedTime = Mathf.Clamp(elapsedTime, 0f, _duration);
			updateValue();
		}

		public void reverseTween()
		{
			_isRunningInReverse = !_isRunningInReverse;
		}

		public IEnumerator waitForCompletion()
		{
			while (_tweenState != TweenState.Complete)
			{
				yield return null;
			}
		}

		public object getTargetObject()
		{
			return _target.getTargetObject();
		}

		private void resetState()
		{
			context = null;
			_completionHandler = (_loopCompleteHandler = null);
			_isFromValueOverridden = false;
			_isTimeScaleIndependent = false;
			_tweenState = TweenState.Complete;
			_isRelative = false;
			_easeType = ZestKit.defaultEaseType;
			_animationCurve = null;
			if (_nextTween != null)
			{
				_nextTween.recycleSelf();
				_nextTween = null;
			}
			_delay = 0f;
			_duration = 0f;
			_timeScale = 1f;
			_elapsedTime = 0f;
			_loopType = LoopType.None;
			_delayBetweenLoops = 0f;
			_loops = 0;
			_isRunningInReverse = false;
		}

		public void initialize(ITweenTarget<T> target, T to, float duration)
		{
			resetState();
			_target = target;
			_toValue = to;
			_duration = duration;
		}

		private void handleLooping(float elapsedTimeExcess)
		{
			_loops--;
			if (_loopType == LoopType.PingPong)
			{
				reverseTween();
			}
			if ((_loopType == LoopType.RestartFromBeginning || _loops % 2 == 0) && _loopCompleteHandler != null)
			{
				_loopCompleteHandler(this);
			}
			if (_loops > 0)
			{
				_tweenState = TweenState.Running;
				if (_loopType == LoopType.RestartFromBeginning)
				{
					_elapsedTime = elapsedTimeExcess - _delayBetweenLoops;
				}
				else if (_isRunningInReverse)
				{
					_elapsedTime += _delayBetweenLoops - elapsedTimeExcess;
				}
				else
				{
					_elapsedTime = elapsedTimeExcess - _delayBetweenLoops;
				}
				if (_delayBetweenLoops == 0f && elapsedTimeExcess > 0f)
				{
					updateValue();
				}
			}
		}

		protected abstract void updateValue();
	}
}
