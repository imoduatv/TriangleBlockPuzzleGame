using System;
using UnityEngine;

namespace Prime31.ZestKit
{
	public interface ITween<T> : ITweenControl, ITweenable where T : struct
	{
		ITween<T> setEaseType(EaseType easeType);

		ITween<T> setAnimationCurve(AnimationCurve animationCurve);

		ITween<T> setDelay(float delay);

		ITween<T> setDuration(float duration);

		ITween<T> setTimeScale(float timeScale);

		ITween<T> setIsTimeScaleIndependent();

		ITween<T> setCompletionHandler(Action<ITween<T>> completionHandler);

		ITween<T> setLoops(LoopType loopType, int loops = 1, float delayBetweenLoops = 0f);

		ITween<T> setLoopCompletionHandler(Action<ITween<T>> loopCompleteHandler);

		ITween<T> setFrom(T from);

		ITween<T> prepareForReuse(T from, T to, float duration);

		ITween<T> setRecycleTween(bool shouldRecycleTween);

		ITween<T> setIsRelative();

		ITween<T> setContext(object context);

		ITween<T> setNextTween(ITweenable nextTween);
	}
}
