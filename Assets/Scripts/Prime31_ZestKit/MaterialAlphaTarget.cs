using UnityEngine;

namespace Prime31.ZestKit
{
	public class MaterialAlphaTarget : AbstractMaterialTarget, ITweenTarget<float>
	{
		public MaterialAlphaTarget(Material material, string propertyName)
		{
			prepareForUse(material, propertyName);
		}

		public void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || (bool)_material)
			{
				Color color = _material.GetColor(_materialNameId);
				color.a = value;
				_material.SetColor(_materialNameId, color);
			}
		}

		public float getTweenedValue()
		{
			Color color = _material.GetColor(_materialNameId);
			return color.a;
		}
	}
}
