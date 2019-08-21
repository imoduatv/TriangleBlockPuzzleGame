using UnityEngine;

namespace Prime31.ZestKit
{
	public class MaterialColorTarget : AbstractMaterialTarget, ITweenTarget<Color>
	{
		public MaterialColorTarget(Material material, string propertyName)
		{
			prepareForUse(material, propertyName);
		}

		public void setTweenedValue(Color value)
		{
			if (!ZestKit.enableBabysitter || (bool)_material)
			{
				_material.SetColor(_materialNameId, value);
			}
		}

		public Color getTweenedValue()
		{
			return _material.GetColor(_materialNameId);
		}
	}
}
