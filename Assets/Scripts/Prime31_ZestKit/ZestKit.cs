using System.Collections.Generic;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class ZestKit : MonoBehaviour
	{
		public static EaseType defaultEaseType = EaseType.QuartIn;

		public static bool enableBabysitter;

		public static bool removeAllTweensOnLevelLoad;

		public static bool cacheIntTweens;

		public static bool cacheFloatTweens;

		public static bool cacheVector2Tweens;

		public static bool cacheVector3Tweens;

		public static bool cacheVector4Tweens;

		public static bool cacheQuaternionTweens;

		public static bool cacheColorTweens;

		public static bool cacheColor32Tweens;

		public static bool cacheRectTweens;

		private List<ITweenable> _activeTweens = new List<ITweenable>();

		private List<ITweenable> _tempTweens = new List<ITweenable>();

		private static bool _applicationIsQuitting;

		private bool _isUpdating;

		private static ZestKit _instance;

		public static ZestKit instance
		{
			get
			{
				if (!_instance && !_applicationIsQuitting)
				{
					_instance = (UnityEngine.Object.FindObjectOfType(typeof(ZestKit)) as ZestKit);
					if (!_instance)
					{
						GameObject gameObject = new GameObject("ZestKit");
						_instance = gameObject.AddComponent<ZestKit>();
						Object.DontDestroyOnLoad(gameObject);
					}
				}
				return _instance;
			}
		}

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
			}
		}

		private void OnApplicationQuit()
		{
			_instance = null;
			UnityEngine.Object.Destroy(base.gameObject);
			_applicationIsQuitting = true;
		}

		private void OnLevelWasLoaded(int level)
		{
			if (removeAllTweensOnLevelLoad)
			{
				_activeTweens.Clear();
			}
		}

		private void Update()
		{
			_isUpdating = true;
			for (int i = 0; i < _activeTweens.Count; i++)
			{
				ITweenable tweenable = _activeTweens[i];
				if (tweenable.tick())
				{
					_tempTweens.Add(tweenable);
				}
			}
			_isUpdating = false;
			for (int j = 0; j < _tempTweens.Count; j++)
			{
				_tempTweens[j].recycleSelf();
				_activeTweens.Remove(_tempTweens[j]);
			}
			_tempTweens.Clear();
		}

		public void addTween(ITweenable tween)
		{
			_activeTweens.Add(tween);
		}

		public void removeTween(ITweenable tween)
		{
			if (_isUpdating)
			{
				_tempTweens.Add(tween);
				return;
			}
			tween.recycleSelf();
			_activeTweens.Remove(tween);
		}

		public void stopAllTweens(bool bringToCompletion = false)
		{
			for (int num = _activeTweens.Count - 1; num >= 0; num--)
			{
				_activeTweens[num].stop(bringToCompletion);
			}
		}

		public List<ITweenable> allTweensWithContext(object context)
		{
			List<ITweenable> list = new List<ITweenable>();
			for (int i = 0; i < _activeTweens.Count; i++)
			{
				if (_activeTweens[i] != null && (_activeTweens[i] as ITweenControl).context == context)
				{
					list.Add(_activeTweens[i]);
				}
			}
			return list;
		}

		public void stopAllTweensWithContext(object context, bool bringToCompletion = false)
		{
			for (int num = _activeTweens.Count - 1; num >= 0; num--)
			{
				if (_activeTweens[num] != null && (_activeTweens[num] as ITweenControl).context == context)
				{
					_activeTweens[num].stop(bringToCompletion);
				}
			}
		}

		public List<ITweenable> allTweensWithTarget(object target)
		{
			List<ITweenable> list = new List<ITweenable>();
			for (int i = 0; i < _activeTweens.Count; i++)
			{
				if (_activeTweens[i] is ITweenControl)
				{
					ITweenControl tweenControl = _activeTweens[i] as ITweenControl;
					if (tweenControl.getTargetObject() == target)
					{
						list.Add(_activeTweens[i]);
					}
				}
			}
			return list;
		}

		public void stopAllTweensWithTarget(object target, bool bringToCompletion = false)
		{
			for (int num = _activeTweens.Count - 1; num >= 0; num--)
			{
				if (_activeTweens[num] is ITweenControl)
				{
					ITweenControl tweenControl = _activeTweens[num] as ITweenControl;
					if (tweenControl.getTargetObject() == target)
					{
						tweenControl.stop(bringToCompletion);
					}
				}
			}
		}
	}
}
