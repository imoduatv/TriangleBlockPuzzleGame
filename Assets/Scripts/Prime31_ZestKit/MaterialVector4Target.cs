using UnityEngine;

namespace Prime31.ZestKit
{
	public class MaterialVector4Target : AbstractMaterialTarget, ITweenTarget<Vector4>
	{
		public MaterialVector4Target(Material material, string propertyName)
		{
			prepareForUse(material, propertyName);
		}

		public void setTweenedValue(Vector4 value)
		{
			if (!ZestKit.enableBabysitter || (bool)_material)
			{
				_material.SetVector(_materialNameId, value);
			}
		}

		public Vector4 getTweenedValue()
		{
			return _material.GetVector(_materialNameId);
		}
	}
}
