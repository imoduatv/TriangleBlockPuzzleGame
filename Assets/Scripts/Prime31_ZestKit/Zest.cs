using UnityEngine;

namespace Prime31.ZestKit
{
	public static class Zest
	{
		public static float unclampedLerp(float from, float to, float t)
		{
			return from + (to - from) * t;
		}

		public static float lerpTowards(float from, float to, float remainingFactorPerSecond, float deltaTime)
		{
			return unclampedLerp(from, to, 1f - Mathf.Pow(remainingFactorPerSecond, deltaTime));
		}

		public static Vector2 unclampedLerp(Vector2 from, Vector2 to, float t)
		{
			return new Vector2(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t);
		}

		public static Vector2 lerpTowards(Vector2 from, Vector2 to, float remainingFactorPerSecond, float deltaTime)
		{
			return unclampedLerp(from, to, 1f - Mathf.Pow(remainingFactorPerSecond, deltaTime));
		}

		public static Vector3 unclampedLerp(Vector3 from, Vector3 to, float t)
		{
			return new Vector3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
		}

		public static Vector3 lerpTowards(Vector3 from, Vector3 to, float remainingFactorPerSecond, float deltaTime)
		{
			return unclampedLerp(from, to, 1f - Mathf.Pow(remainingFactorPerSecond, deltaTime));
		}

		public static Vector3 lerpTowards(Vector3 followerCurrentPosition, Vector3 targetPreviousPosition, Vector3 targetCurrentPosition, float smoothFactor, float deltaTime)
		{
			Vector3 a = targetCurrentPosition - targetPreviousPosition;
			Vector3 a2 = followerCurrentPosition - targetPreviousPosition + a / (smoothFactor * deltaTime);
			return targetCurrentPosition - a / (smoothFactor * deltaTime) + a2 * Mathf.Exp((0f - smoothFactor) * deltaTime);
		}

		public static Vector3 unclampedAngledLerp(Vector3 from, Vector3 to, float t)
		{
			Vector3 vector = new Vector3(Mathf.DeltaAngle(from.x, to.x), Mathf.DeltaAngle(from.y, to.y), Mathf.DeltaAngle(from.z, to.z));
			return new Vector3(from.x + vector.x * t, from.y + vector.y * t, from.z + vector.z * t);
		}

		public static Vector4 unclampedLerp(Vector4 from, Vector4 to, float t)
		{
			return new Vector4(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t, from.w + (to.w - from.w) * t);
		}

		public static Color unclampedLerp(Color from, Color to, float t)
		{
			return new Color(from.r + (to.r - from.r) * t, from.g + (to.g - from.g) * t, from.b + (to.b - from.b) * t, from.a + (to.a - from.a) * t);
		}

		public static Color32 unclampedLerp(Color32 from, Color32 to, float t)
		{
			return new Color32((byte)((float)(int)from.r + (float)(to.r - from.r) * t), (byte)((float)(int)from.g + (float)(to.g - from.g) * t), (byte)((float)(int)from.b + (float)(to.b - from.b) * t), (byte)((float)(int)from.a + (float)(to.a - from.a) * t));
		}

		public static Rect unclampedLerp(Rect from, Rect to, float t)
		{
			return new Rect(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.width + (to.width - from.width) * t, from.height + (to.height - from.height) * t);
		}

		public static float ease(EaseType easeType, float from, float to, float t, float duration)
		{
			return unclampedLerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static float ease(AnimationCurve curve, float from, float to, float t, float duration)
		{
			return unclampedLerp(from, to, curve.Evaluate(t / duration));
		}

		public static Vector2 ease(EaseType easeType, Vector2 from, Vector2 to, float t, float duration)
		{
			return unclampedLerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static Vector2 ease(AnimationCurve curve, Vector2 from, Vector2 to, float t, float duration)
		{
			return unclampedLerp(from, to, curve.Evaluate(t / duration));
		}

		public static Vector3 ease(EaseType easeType, Vector3 from, Vector3 to, float t, float duration)
		{
			return unclampedLerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static Vector3 ease(AnimationCurve curve, Vector3 from, Vector3 to, float t, float duration)
		{
			return unclampedLerp(from, to, curve.Evaluate(t / duration));
		}

		public static Vector3 easeAngle(EaseType easeType, Vector3 from, Vector3 to, float t, float duration)
		{
			return unclampedAngledLerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static Vector3 easeAngle(AnimationCurve curve, Vector3 from, Vector3 to, float t, float duration)
		{
			return unclampedAngledLerp(from, to, curve.Evaluate(t / duration));
		}

		public static Vector4 ease(EaseType easeType, Vector4 from, Vector4 to, float t, float duration)
		{
			return unclampedLerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static Vector4 ease(AnimationCurve curve, Vector4 from, Vector4 to, float t, float duration)
		{
			return unclampedLerp(from, to, curve.Evaluate(t / duration));
		}

		public static Quaternion ease(EaseType easeType, Quaternion from, Quaternion to, float t, float duration)
		{
			return Quaternion.Lerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static Quaternion ease(AnimationCurve curve, Quaternion from, Quaternion to, float t, float duration)
		{
			return Quaternion.Lerp(from, to, curve.Evaluate(t / duration));
		}

		public static Color ease(EaseType easeType, Color from, Color to, float t, float duration)
		{
			return unclampedLerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static Color ease(AnimationCurve curve, Color from, Color to, float t, float duration)
		{
			return unclampedLerp(from, to, curve.Evaluate(t / duration));
		}

		public static Color32 ease(EaseType easeType, Color32 from, Color32 to, float t, float duration)
		{
			return unclampedLerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static Color32 ease(AnimationCurve curve, Color32 from, Color32 to, float t, float duration)
		{
			return unclampedLerp(from, to, curve.Evaluate(t / duration));
		}

		public static Rect ease(EaseType easeType, Rect from, Rect to, float t, float duration)
		{
			return unclampedLerp(from, to, EaseHelper.ease(easeType, t, duration));
		}

		public static Rect ease(AnimationCurve curve, Rect from, Rect to, float t, float duration)
		{
			return unclampedLerp(from, to, curve.Evaluate(t / duration));
		}

		public static float fastSpring(float currentValue, float targetValue, ref float velocity, float dampingRatio, float angularFrequency)
		{
			velocity += -2f * Time.deltaTime * dampingRatio * angularFrequency * velocity + Time.deltaTime * angularFrequency * angularFrequency * (targetValue - currentValue);
			currentValue += Time.deltaTime * velocity;
			return currentValue;
		}

		public static float stableSpring(float currentValue, float targetValue, ref float velocity, float dampingRatio, float angularFrequency)
		{
			float num = 1f + 2f * Time.deltaTime * dampingRatio * angularFrequency;
			float num2 = angularFrequency * angularFrequency;
			float num3 = Time.deltaTime * num2;
			float num4 = Time.deltaTime * num3;
			float num5 = 1f / (num + num4);
			float num6 = num * currentValue + Time.deltaTime * velocity + num4 * targetValue;
			float num7 = velocity + num3 * (targetValue - currentValue);
			currentValue = num6 * num5;
			velocity = num7 * num5;
			return currentValue;
		}

		public static Vector3 fastSpring(Vector3 currentValue, Vector3 targetValue, ref Vector3 velocity, float dampingRatio, float angularFrequency)
		{
			velocity += -2f * Time.deltaTime * dampingRatio * angularFrequency * velocity + Time.deltaTime * angularFrequency * angularFrequency * (targetValue - currentValue);
			currentValue += Time.deltaTime * velocity;
			return currentValue;
		}

		public static Vector3 stableSpring(Vector3 currentValue, Vector3 targetValue, ref Vector3 velocity, float dampingRatio, float angularFrequency)
		{
			float num = 1f + 2f * Time.deltaTime * dampingRatio * angularFrequency;
			float num2 = angularFrequency * angularFrequency;
			float num3 = Time.deltaTime * num2;
			float num4 = Time.deltaTime * num3;
			float d = 1f / (num + num4);
			Vector3 a = num * currentValue + Time.deltaTime * velocity + num4 * targetValue;
			Vector3 a2 = velocity + num3 * (targetValue - currentValue);
			currentValue = a * d;
			velocity = a2 * d;
			return currentValue;
		}
	}
}
