namespace Prime31.ZestKit
{
	public interface ITweenable
	{
		bool tick();

		void recycleSelf();

		bool isRunning();

		void start();

		void pause();

		void resume();

		void stop(bool bringToCompletion = false);
	}
}
