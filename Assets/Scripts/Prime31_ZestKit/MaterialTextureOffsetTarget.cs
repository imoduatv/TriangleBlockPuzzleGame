using UnityEngine;

namespace Prime31.ZestKit
{
	public class MaterialTextureOffsetTarget : AbstractMaterialTarget, ITweenTarget<Vector2>
	{
		private string _propertyName;

		public MaterialTextureOffsetTarget(Material material, string propertyName)
		{
			prepareForUse(material, propertyName);
			_propertyName = propertyName;
		}

		public void setTweenedValue(Vector2 value)
		{
			if (!ZestKit.enableBabysitter || (bool)_material)
			{
				_material.SetTextureOffset(_propertyName, value);
			}
		}

		public Vector2 getTweenedValue()
		{
			return _material.GetTextureOffset(_propertyName);
		}
	}
}
