namespace Prime31.ZestKit
{
	public abstract class AbstractTweenTarget<U, T> : ITweenTarget<T> where T : struct
	{
		protected U _target;

		public abstract void setTweenedValue(T value);

		public abstract T getTweenedValue();

		public AbstractTweenTarget<U, T> setTarget(U target)
		{
			_target = target;
			return this;
		}

		public bool validateTarget()
		{
			return !_target.Equals(null);
		}

		public object getTargetObject()
		{
			return _target;
		}
	}
}
