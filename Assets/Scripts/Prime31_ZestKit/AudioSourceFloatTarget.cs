using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public class AudioSourceFloatTarget : AbstractTweenTarget<AudioSource, float>
	{
		public enum AudioSourceFloatType
		{
			Volume,
			Pitch,
			PanStereo
		}

		private AudioSourceFloatType _tweenType;

		public AudioSourceFloatTarget(AudioSource audioSource, AudioSourceFloatType targetType)
		{
			_target = audioSource;
			_tweenType = targetType;
		}

		public override void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || validateTarget())
			{
				switch (_tweenType)
				{
				case AudioSourceFloatType.Volume:
					_target.volume = value;
					break;
				case AudioSourceFloatType.Pitch:
					_target.pitch = value;
					break;
				case AudioSourceFloatType.PanStereo:
					_target.panStereo = value;
					break;
				}
			}
		}

		public override float getTweenedValue()
		{
			switch (_tweenType)
			{
			case AudioSourceFloatType.Volume:
				return _target.volume;
			case AudioSourceFloatType.Pitch:
				return _target.pitch;
			case AudioSourceFloatType.PanStereo:
				return _target.panStereo;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
