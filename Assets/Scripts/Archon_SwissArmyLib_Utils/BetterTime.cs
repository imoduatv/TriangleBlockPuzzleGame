using UnityEngine;

namespace Archon.SwissArmyLib.Utils
{
	public static class BetterTime
	{
		private static float _fixedDeltaTime;

		public static float TimeScale
		{
			get
			{
				return UnityEngine.Time.timeScale;
			}
			set
			{
				UnityEngine.Time.timeScale = value;
			}
		}

		public static int FrameCount
		{
			get;
			private set;
		}

		public static float Time
		{
			get;
			private set;
		}

		public static float DeltaTime
		{
			get;
			private set;
		}

		public static float SmoothDeltaTime
		{
			get;
			private set;
		}

		public static float UnscaledDeltaTime
		{
			get;
			private set;
		}

		public static float UnscaledTime
		{
			get;
			private set;
		}

		public static float FixedDeltaTime
		{
			get
			{
				return _fixedDeltaTime;
			}
			set
			{
				_fixedDeltaTime = value;
				UnityEngine.Time.fixedDeltaTime = value;
			}
		}

		public static float FixedTime
		{
			get;
			private set;
		}

		public static float FixedUnscaledDeltaTime
		{
			get;
			private set;
		}

		public static float FixedUnscaledTime
		{
			get;
			private set;
		}

		public static bool InFixedTimeStep => UnityEngine.Time.inFixedTimeStep;

		public static float RealTimeSinceStartup => UnityEngine.Time.realtimeSinceStartup;

		public static float TimeSinceLevelLoad
		{
			get;
			private set;
		}

		public static float MaximumDeltaTime
		{
			get
			{
				return UnityEngine.Time.maximumDeltaTime;
			}
			set
			{
				UnityEngine.Time.maximumDeltaTime = value;
			}
		}

		public static float MaximumParticleDeltaTime
		{
			get
			{
				return UnityEngine.Time.maximumParticleDeltaTime;
			}
			set
			{
				UnityEngine.Time.maximumParticleDeltaTime = value;
			}
		}

		public static int CaptureFramerate
		{
			get
			{
				return UnityEngine.Time.captureFramerate;
			}
			set
			{
				UnityEngine.Time.captureFramerate = value;
			}
		}

		static BetterTime()
		{
			if (!ServiceLocator.IsRegistered<BetterTimeUpdater>())
			{
				ServiceLocator.RegisterSingleton<BetterTimeUpdater>();
			}
			ServiceLocator.GlobalReset += delegate
			{
				ServiceLocator.RegisterSingleton<BetterTimeUpdater>();
			};
		}

		internal static void Update()
		{
			FrameCount = UnityEngine.Time.frameCount;
			Time = UnityEngine.Time.time;
			TimeSinceLevelLoad = UnityEngine.Time.timeSinceLevelLoad;
			DeltaTime = UnityEngine.Time.deltaTime;
			SmoothDeltaTime = UnityEngine.Time.smoothDeltaTime;
			UnscaledTime = UnityEngine.Time.unscaledTime;
			UnscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;
			FixedTime = UnityEngine.Time.fixedTime;
			_fixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
			FixedUnscaledTime = UnityEngine.Time.fixedUnscaledTime;
			FixedUnscaledDeltaTime = UnityEngine.Time.fixedUnscaledDeltaTime;
		}
	}
}
