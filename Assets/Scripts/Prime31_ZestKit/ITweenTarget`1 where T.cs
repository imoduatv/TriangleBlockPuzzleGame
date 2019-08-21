namespace Prime31.ZestKit
{
	public interface ITweenTarget<T> where T : struct
	{
		void setTweenedValue(T value);

		T getTweenedValue();

		object getTargetObject();
	}
}
