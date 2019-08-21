using UnityEngine;

namespace Prime31.ZestKit
{
	public abstract class AbstractMaterialTarget
	{
		protected Material _material;

		protected int _materialNameId;

		public void prepareForUse(Material material, string propertyName)
		{
			_material = material;
			_materialNameId = Shader.PropertyToID(propertyName);
		}

		public object getTargetObject()
		{
			return _material;
		}
	}
}
