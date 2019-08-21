using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Utils.Shake
{
	public class Shake : BaseShake<float>
	{
		private readonly List<float> _samples = new List<float>();

		public override void Start(float amplitude, int frequency, float duration)
		{
			base.Start(amplitude, frequency, duration);
			_samples.Clear();
			float num = duration * (float)frequency;
			for (int i = 0; (float)i < num; i++)
			{
				_samples.Add(Random.value * 2f - 1f);
			}
		}

		public override float GetAmplitude(float t)
		{
			int num = Mathf.FloorToInt((float)_samples.Count * t);
			float sample = GetSample(num);
			float sample2 = GetSample(num + 1);
			float t2 = (float)_samples.Count * t - (float)num;
			return Mathf.Lerp(sample, sample2, t2) * (1f - t) * base.Amplitude;
		}

		private float GetSample(int index)
		{
			if (index < 0 || index >= _samples.Count)
			{
				return 0f;
			}
			return _samples[index];
		}
	}
}
