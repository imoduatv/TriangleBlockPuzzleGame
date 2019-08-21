using UnityEngine;

namespace Archon.SwissArmyLib.Utils.Shake
{
	public abstract class BaseShake<T>
	{
		public bool UnscaledTime
		{
			get;
			set;
		}

		public float Duration
		{
			get;
			private set;
		}

		public int Frequency
		{
			get;
			private set;
		}

		public float Amplitude
		{
			get;
			private set;
		}

		public float NormalizedTime => Mathf.InverseLerp(StartTime, StartTime + Duration, CurrentTime);

		public bool IsDone => NormalizedTime >= 1f;

		protected float StartTime
		{
			get;
			set;
		}

		protected float CurrentTime => (!UnscaledTime) ? BetterTime.Time : BetterTime.UnscaledTime;

		public virtual void Start(float amplitude, int frequency, float duration)
		{
			Amplitude = amplitude;
			Duration = duration;
			Frequency = frequency;
			StartTime = CurrentTime;
		}

		public T GetAmplitude()
		{
			return GetAmplitude(NormalizedTime);
		}

		public abstract T GetAmplitude(float t);
	}
}
