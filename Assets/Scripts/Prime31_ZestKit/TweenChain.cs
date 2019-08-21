using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class TweenChain : AbstractTweenable
	{
		private List<ITweenable> _tweenList = new List<ITweenable>();

		private int _currentTween;

		private Action<TweenChain> _completionHandler;

		public int totalTweens => _tweenList.Count;

		public override void start()
		{
			if (_tweenList.Count > 0)
			{
				_tweenList[0].start();
			}
			base.start();
		}

		public override bool tick()
		{
			if (_isPaused)
			{
				return false;
			}
			if (_currentTween >= _tweenList.Count)
			{
				return true;
			}
			ITweenable tweenable = _tweenList[_currentTween];
			if (tweenable.tick())
			{
				_currentTween++;
				if (_currentTween == _tweenList.Count)
				{
					if (_completionHandler != null)
					{
						_completionHandler(this);
					}
					_isCurrentlyManagedByZestKit = false;
					return true;
				}
				_tweenList[_currentTween].start();
			}
			return false;
		}

		public override void recycleSelf()
		{
			for (int i = 0; i < _tweenList.Count; i++)
			{
				_tweenList[i].recycleSelf();
			}
			_tweenList.Clear();
		}

		public override void stop(bool bringToCompletion = false)
		{
			_currentTween = _tweenList.Count;
		}

		public IEnumerator waitForCompletion()
		{
			while (_currentTween < _tweenList.Count)
			{
				yield return null;
			}
		}

		public TweenChain appendTween(ITweenable tween)
		{
			if (tween != null)
			{
				tween.resume();
				_tweenList.Add(tween);
			}
			else
			{
				UnityEngine.Debug.LogError("attempted to add a tween that does not implement ITweenable to a TweenChain!");
			}
			return this;
		}

		public TweenChain setCompletionHandler(Action<TweenChain> completionHandler)
		{
			_completionHandler = completionHandler;
			return this;
		}
	}
}
