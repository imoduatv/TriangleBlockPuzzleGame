using UnityEngine;

namespace Prime31.ZestKit
{
	public static class PropertyTweens
	{
		public static ITween<int> intPropertyTo(object self, string propertyName, int to, float duration)
		{
			PropertyTarget<int> target = new PropertyTarget<int>(self, propertyName);
			IntTween intTween = (!ZestKit.cacheIntTweens) ? new IntTween() : QuickCache<IntTween>.pop();
			intTween.initialize(target, to, duration);
			return intTween;
		}

		public static ITween<float> floatPropertyTo(object self, string propertyName, float to, float duration)
		{
			PropertyTarget<float> target = new PropertyTarget<float>(self, propertyName);
			FloatTween floatTween = (!ZestKit.cacheFloatTweens) ? new FloatTween() : QuickCache<FloatTween>.pop();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<Vector2> vector2PropertyTo(object self, string propertyName, Vector2 to, float duration)
		{
			PropertyTarget<Vector2> target = new PropertyTarget<Vector2>(self, propertyName);
			Vector2Tween vector2Tween = (!ZestKit.cacheVector2Tweens) ? new Vector2Tween() : QuickCache<Vector2Tween>.pop();
			vector2Tween.initialize(target, to, duration);
			return vector2Tween;
		}

		public static ITween<Vector3> vector3PropertyTo(object self, string propertyName, Vector3 to, float duration)
		{
			PropertyTarget<Vector3> target = new PropertyTarget<Vector3>(self, propertyName);
			Vector3Tween vector3Tween = (!ZestKit.cacheVector3Tweens) ? new Vector3Tween() : QuickCache<Vector3Tween>.pop();
			vector3Tween.initialize(target, to, duration);
			return vector3Tween;
		}

		public static ITween<Vector4> vector4PropertyTo(object self, string propertyName, Vector4 to, float duration)
		{
			PropertyTarget<Vector4> target = new PropertyTarget<Vector4>(self, propertyName);
			Vector4Tween vector4Tween = (!ZestKit.cacheVector4Tweens) ? new Vector4Tween() : QuickCache<Vector4Tween>.pop();
			vector4Tween.initialize(target, to, duration);
			return vector4Tween;
		}

		public static ITween<Quaternion> quaternionPropertyTo(object self, string propertyName, Quaternion to, float duration)
		{
			PropertyTarget<Quaternion> target = new PropertyTarget<Quaternion>(self, propertyName);
			QuaternionTween quaternionTween = (!ZestKit.cacheQuaternionTweens) ? new QuaternionTween() : QuickCache<QuaternionTween>.pop();
			quaternionTween.initialize(target, to, duration);
			return quaternionTween;
		}

		public static ITween<Color> colorPropertyTo(object self, string propertyName, Color to, float duration)
		{
			PropertyTarget<Color> target = new PropertyTarget<Color>(self, propertyName);
			ColorTween colorTween = (!ZestKit.cacheColorTweens) ? new ColorTween() : QuickCache<ColorTween>.pop();
			colorTween.initialize(target, to, duration);
			return colorTween;
		}

		public static ITween<Color32> color32PropertyTo(object self, string propertyName, Color32 to, float duration)
		{
			PropertyTarget<Color32> target = new PropertyTarget<Color32>(self, propertyName);
			Color32Tween color32Tween = (!ZestKit.cacheColor32Tweens) ? new Color32Tween() : QuickCache<Color32Tween>.pop();
			color32Tween.initialize(target, to, duration);
			return color32Tween;
		}
	}
}
