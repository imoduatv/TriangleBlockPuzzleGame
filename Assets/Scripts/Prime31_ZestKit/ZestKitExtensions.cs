using UnityEngine;
using UnityEngine.UI;

namespace Prime31.ZestKit
{
	public static class ZestKitExtensions
	{
		public static ITween<Vector3> ZKpositionTo(this Transform self, Vector3 to, float duration = 0.3f)
		{
			TransformVector3Tween transformVector3Tween = QuickCache<TransformVector3Tween>.pop();
			transformVector3Tween.setTargetAndType(self, TransformTargetType.Position);
			transformVector3Tween.initialize(transformVector3Tween, to, duration);
			return transformVector3Tween;
		}

		public static ITween<Vector3> ZKlocalPositionTo(this Transform self, Vector3 to, float duration = 0.3f)
		{
			TransformVector3Tween transformVector3Tween = QuickCache<TransformVector3Tween>.pop();
			transformVector3Tween.setTargetAndType(self, TransformTargetType.LocalPosition);
			transformVector3Tween.initialize(transformVector3Tween, to, duration);
			return transformVector3Tween;
		}

		public static ITween<Vector3> ZKlocalScaleTo(this Transform self, Vector3 to, float duration = 0.3f)
		{
			TransformVector3Tween transformVector3Tween = QuickCache<TransformVector3Tween>.pop();
			transformVector3Tween.setTargetAndType(self, TransformTargetType.LocalScale);
			transformVector3Tween.initialize(transformVector3Tween, to, duration);
			return transformVector3Tween;
		}

		public static ITween<Vector3> ZKeulersTo(this Transform self, Vector3 to, float duration = 0.3f)
		{
			TransformVector3Tween transformVector3Tween = QuickCache<TransformVector3Tween>.pop();
			transformVector3Tween.setTargetAndType(self, TransformTargetType.EulerAngles);
			transformVector3Tween.initialize(transformVector3Tween, to, duration);
			return transformVector3Tween;
		}

		public static ITween<Vector3> ZKlocalEulersTo(this Transform self, Vector3 to, float duration = 0.3f)
		{
			TransformVector3Tween transformVector3Tween = QuickCache<TransformVector3Tween>.pop();
			transformVector3Tween.setTargetAndType(self, TransformTargetType.LocalEulerAngles);
			transformVector3Tween.initialize(transformVector3Tween, to, duration);
			return transformVector3Tween;
		}

		public static ITween<Quaternion> ZKrotationTo(this Transform self, Quaternion to, float duration = 0.3f)
		{
			TransformRotationTarget target = new TransformRotationTarget(self, TransformRotationTarget.TransformRotationType.Rotation);
			return new QuaternionTween(target, self.rotation, to, duration);
		}

		public static ITween<Quaternion> ZKlocalRotationTo(this Transform self, Quaternion to, float duration = 0.3f)
		{
			TransformRotationTarget target = new TransformRotationTarget(self, TransformRotationTarget.TransformRotationType.LocalRotation);
			return new QuaternionTween(target, self.localRotation, to, duration);
		}

		public static ITween<Color> ZKcolorTo(this SpriteRenderer self, Color to, float duration = 0.3f)
		{
			SpriteRendererColorTarget target = new SpriteRendererColorTarget(self);
			ColorTween colorTween = ColorTween.create();
			colorTween.initialize(target, to, duration);
			return colorTween;
		}

		public static ITween<float> ZKalphaTo(this SpriteRenderer self, float to, float duration = 0.3f)
		{
			SpriteRendererAlphaTarget target = new SpriteRendererAlphaTarget(self);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<Color> ZKcolorTo(this Material self, Color to, float duration = 0.3f, string propertyName = "_Color")
		{
			MaterialColorTarget target = new MaterialColorTarget(self, propertyName);
			ColorTween colorTween = ColorTween.create();
			colorTween.initialize(target, to, duration);
			return colorTween;
		}

		public static ITween<float> ZKalphaTo(this Material self, float to, float duration = 0.3f, string propertyName = "_Color")
		{
			MaterialAlphaTarget target = new MaterialAlphaTarget(self, propertyName);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKfloatTo(this Material self, float to, float duration = 0.3f, string propertyName = "_Color")
		{
			MaterialFloatTarget target = new MaterialFloatTarget(self, propertyName);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<Vector4> ZKVector4To(this Material self, Vector4 to, float duration, string propertyName)
		{
			MaterialVector4Target target = new MaterialVector4Target(self, propertyName);
			Vector4Tween vector4Tween = Vector4Tween.create();
			vector4Tween.initialize(target, to, duration);
			return vector4Tween;
		}

		public static ITween<Vector2> ZKtextureOffsetTo(this Material self, Vector2 to, float duration, string propertyName = "_MainTex")
		{
			MaterialTextureOffsetTarget target = new MaterialTextureOffsetTarget(self, propertyName);
			Vector2Tween vector2Tween = Vector2Tween.create();
			vector2Tween.initialize(target, to, duration);
			return vector2Tween;
		}

		public static ITween<Vector2> ZKtextureScaleTo(this Material self, Vector2 to, float duration, string propertyName = "_MainTex")
		{
			MaterialTextureScaleTarget target = new MaterialTextureScaleTarget(self, propertyName);
			Vector2Tween vector2Tween = Vector2Tween.create();
			vector2Tween.initialize(target, to, duration);
			return vector2Tween;
		}

		public static ITween<float> ZKvolumeTo(this AudioSource self, float to, float duration = 0.3f)
		{
			AudioSourceFloatTarget target = new AudioSourceFloatTarget(self, AudioSourceFloatTarget.AudioSourceFloatType.Volume);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKpitchTo(this AudioSource self, float to, float duration = 0.3f)
		{
			AudioSourceFloatTarget target = new AudioSourceFloatTarget(self, AudioSourceFloatTarget.AudioSourceFloatType.Pitch);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKpanStereoTo(this AudioSource self, float to, float duration = 0.3f)
		{
			AudioSourceFloatTarget target = new AudioSourceFloatTarget(self, AudioSourceFloatTarget.AudioSourceFloatType.PanStereo);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKfieldOfViewTo(this Camera self, float to, float duration = 0.3f)
		{
			CameraFloatTarget target = new CameraFloatTarget(self, CameraFloatTarget.CameraTargetType.FieldOfView);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKorthographicSizeTo(this Camera self, float to, float duration = 0.3f)
		{
			CameraFloatTarget target = new CameraFloatTarget(self);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<Color> ZKbackgroundColorTo(this Camera self, Color to, float duration = 0.3f)
		{
			CameraBackgroundColorTarget target = new CameraBackgroundColorTarget(self);
			ColorTween colorTween = ColorTween.create();
			colorTween.initialize(target, to, duration);
			return colorTween;
		}

		public static ITween<Rect> ZKrectTo(this Camera self, Rect to, float duration = 0.3f)
		{
			CameraRectTarget target = new CameraRectTarget(self);
			RectTween rectTween = RectTween.create();
			rectTween.initialize(target, to, duration);
			return rectTween;
		}

		public static ITween<Rect> ZKpixelRectTo(this Camera self, Rect to, float duration = 0.3f)
		{
			CameraRectTarget target = new CameraRectTarget(self, CameraRectTarget.CameraTargetType.PixelRect);
			RectTween rectTween = RectTween.create();
			rectTween.initialize(target, to, duration);
			return rectTween;
		}

		public static ITween<float> ZKalphaTo(this CanvasGroup self, float to, float duration = 0.3f)
		{
			CanvasGroupAlphaTarget target = new CanvasGroupAlphaTarget(self);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKalphaTo(this Image self, float to, float duration = 0.3f)
		{
			ImageFloatTarget target = new ImageFloatTarget(self);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKfillAmountTo(this Image self, float to, float duration = 0.3f)
		{
			ImageFloatTarget target = new ImageFloatTarget(self, ImageFloatTarget.ImageTargetType.FillAmount);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<Color> ZKcolorTo(this Image self, Color to, float duration = 0.3f)
		{
			ImageColorTarget target = new ImageColorTarget(self);
			ColorTween colorTween = ColorTween.create();
			colorTween.initialize(target, to, duration);
			return colorTween;
		}

		public static ITween<Vector2> ZKanchoredPositionTo(this RectTransform self, Vector2 to, float duration = 0.3f)
		{
			RectTransformAnchoredPositionTarget target = new RectTransformAnchoredPositionTarget(self);
			Vector2Tween vector2Tween = Vector2Tween.create();
			vector2Tween.initialize(target, to, duration);
			return vector2Tween;
		}

		public static ITween<Vector3> ZKanchoredPosition3DTo(this RectTransform self, Vector3 to, float duration = 0.3f)
		{
			RectTransformAnchoredPosition3DTarget target = new RectTransformAnchoredPosition3DTarget(self);
			Vector3Tween vector3Tween = Vector3Tween.create();
			vector3Tween.initialize(target, to, duration);
			return vector3Tween;
		}

		public static ITween<Vector2> ZKsizeDeltaTo(this RectTransform self, Vector2 to, float duration = 0.3f)
		{
			RectTransformSizeDeltaTarget target = new RectTransformSizeDeltaTarget(self);
			Vector2Tween vector2Tween = Vector2Tween.create();
			vector2Tween.initialize(target, to, duration);
			return vector2Tween;
		}

		public static ITween<Vector2> ZKnormalizedPositionTo(this ScrollRect self, Vector2 to, float duration = 0.3f)
		{
			ScrollRectNormalizedPositionTarget target = new ScrollRectNormalizedPositionTarget(self);
			Vector2Tween vector2Tween = Vector2Tween.create();
			vector2Tween.initialize(target, to, duration);
			return vector2Tween;
		}

		public static ITween<float> ZKintensityTo(this Light self, float to, float duration = 0.3f)
		{
			LightFloatTarget target = new LightFloatTarget(self);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKrangeTo(this Light self, float to, float duration = 0.3f)
		{
			LightFloatTarget target = new LightFloatTarget(self, LightFloatTarget.LightTargetType.Range);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<float> ZKspotAngleTo(this Light self, float to, float duration = 0.3f)
		{
			LightFloatTarget target = new LightFloatTarget(self, LightFloatTarget.LightTargetType.SpotAngle);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}

		public static ITween<Color> ZKcolorTo(this Light self, Color to, float duration = 0.3f)
		{
			LightColorTarget target = new LightColorTarget(self);
			ColorTween colorTween = ColorTween.create();
			colorTween.initialize(target, to, duration);
			return colorTween;
		}

		public static ITween<Color> ZKcolorTo(this Text self, Color to, float duration = 0.3f)
		{
			TextColorTarget target = new TextColorTarget(self);
			ColorTween colorTween = ColorTween.create();
			colorTween.initialize(target, to, duration);
			return colorTween;
		}

		public static ITween<float> ZKalphaTo(this Text self, float to, float duration = 0.3f)
		{
			TextAlphaTarget target = new TextAlphaTarget(self);
			FloatTween floatTween = FloatTween.create();
			floatTween.initialize(target, to, duration);
			return floatTween;
		}
	}
}
