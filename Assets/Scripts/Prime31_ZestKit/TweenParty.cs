using System.Collections.Generic;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class TweenParty : FloatTween, ITweenTarget<float>
	{
		private List<ITweenControl> _tweenList = new List<ITweenControl>();

		public int totalTweens => _tweenList.Count;

		public float currentElapsedTime
		{
			get;
			private set;
		}

		public TweenParty(float duration)
		{
			_target = this;
			_duration = duration;
			_toValue = duration;
		}

		public void setTweenedValue(float value)
		{
			currentElapsedTime = value;
			for (int i = 0; i < _tweenList.Count; i++)
			{
				_tweenList[i].jumpToElapsedTime(value);
			}
		}

		public float getTweenedValue()
		{
			return currentElapsedTime;
		}

		public new object getTargetObject()
		{
			return null;
		}

		public override void start()
		{
			if (_tweenState != TweenState.Complete)
			{
				return;
			}
			_tweenState = TweenState.Running;
			for (int i = 0; i < _tweenList.Count; i++)
			{
				if (_tweenList[i] is ITween<int>)
				{
					((ITween<int>)_tweenList[i]).setDelay(0f).setLoops(LoopType.None).setDuration(_duration);
				}
				else if (_tweenList[i] is ITween<float>)
				{
					((ITween<float>)_tweenList[i]).setDelay(0f).setLoops(LoopType.None).setDuration(_duration);
				}
				else if (_tweenList[i] is ITween<Vector2>)
				{
					((ITween<Vector2>)_tweenList[i]).setDelay(0f).setLoops(LoopType.None).setDuration(_duration);
				}
				else if (_tweenList[i] is ITween<Vector3>)
				{
					((ITween<Vector3>)_tweenList[i]).setDelay(0f).setLoops(LoopType.None).setDuration(_duration);
				}
				else if (_tweenList[i] is ITween<Vector4>)
				{
					((ITween<Vector4>)_tweenList[i]).setDelay(0f).setLoops(LoopType.None).setDuration(_duration);
				}
				else if (_tweenList[i] is ITween<Quaternion>)
				{
					((ITween<Quaternion>)_tweenList[i]).setDelay(0f).setLoops(LoopType.None).setDuration(_duration);
				}
				else if (_tweenList[i] is ITween<Color>)
				{
					((ITween<Color>)_tweenList[i]).setDelay(0f).setLoops(LoopType.None).setDuration(_duration);
				}
				else if (_tweenList[i] is ITween<Color32>)
				{
					((ITween<Color32>)_tweenList[i]).setDelay(0f).setLoops(LoopType.None).setDuration(_duration);
				}
				_tweenList[i].start();
			}
			ZestKit.instance.addTween(this);
		}

		public override void recycleSelf()
		{
			for (int i = 0; i < _tweenList.Count; i++)
			{
				_tweenList[i].recycleSelf();
			}
			_tweenList.Clear();
		}

		public TweenParty addTween(ITweenControl tween)
		{
			tween.resume();
			_tweenList.Add(tween);
			return this;
		}

		public TweenParty prepareForReuse(float duration)
		{
			for (int i = 0; i < _tweenList.Count; i++)
			{
				_tweenList[i].recycleSelf();
			}
			_tweenList.Clear();
			return (TweenParty)prepareForReuse(0f, duration, duration);
		}
	}
}
