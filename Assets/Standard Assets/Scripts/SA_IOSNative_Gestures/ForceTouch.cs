using SA.Common.Pattern;
using System;
using System.Threading;
using UnityEngine;

namespace SA.IOSNative.Gestures
{
	public class ForceTouch : Singleton<ForceTouch>
	{
		private static bool _IsTouchTrigerred;

		public static string AppOpenshortcutItem => string.Empty;

		public event Action OnForceTouchStarted;

		public event Action OnForceTouchFinished;

		public event Action<ForceInfo> OnForceChanged;

		public event Action<string> OnAppShortcutClick;

		public ForceTouch()
		{
			this.OnForceTouchStarted = delegate
			{
			};
			this.OnForceTouchFinished = delegate
			{
			};
			this.OnForceChanged = delegate
			{
			};
			this.OnAppShortcutClick = delegate
			{
			};
			//base._002Ector();
		}

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public void Setup(float forceTouchDelay, float baseForceTouchPressure, float triggeringForceTouchPressure)
		{
		}

		private void didStartForce(string array)
		{
			_IsTouchTrigerred = true;
			this.OnForceTouchStarted();
		}

		private void didForceChanged(string array)
		{
			if (_IsTouchTrigerred)
			{
				string[] array2 = array.Split('|');
				float force = Convert.ToSingle(array2[0]);
				float maxForce = Convert.ToSingle(array2[1]);
				ForceInfo obj = new ForceInfo(force, maxForce);
				this.OnForceChanged(obj);
			}
		}

		private void didForceEnded(string array)
		{
			_IsTouchTrigerred = false;
			this.OnForceTouchFinished();
		}

		private void performActionForShortcutItem(string action)
		{
			this.OnAppShortcutClick(action);
		}
	}
}
