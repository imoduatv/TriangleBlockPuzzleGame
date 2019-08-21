using UnityEngine;

namespace Prime31.ZestKit
{
	public class MaterialFloatTarget : AbstractMaterialTarget, ITweenTarget<float>
	{
		public MaterialFloatTarget(Material material, string propertyName)
		{
			prepareForUse(material, propertyName);
		}

		public void setTweenedValue(float value)
		{
			if (!ZestKit.enableBabysitter || (bool)_material)
			{
				_material.SetFloat(_materialNameId, value);
			}
		}

		public float getTweenedValue()
		{
			return _material.GetFloat(_materialNameId);
		}
	}
}
