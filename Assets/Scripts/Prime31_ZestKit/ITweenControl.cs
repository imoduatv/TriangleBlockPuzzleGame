using System.Collections;

namespace Prime31.ZestKit
{
	public interface ITweenControl : ITweenable
	{
		object context
		{
			get;
		}

		void jumpToElapsedTime(float elapsedTime);

		IEnumerator waitForCompletion();

		object getTargetObject();
	}
}
