namespace Prime31.ZestKit
{
	public abstract class AbstractTweenable : ITweenable
	{
		protected bool _isPaused;

		protected bool _isCurrentlyManagedByZestKit;

		public abstract bool tick();

		public virtual void recycleSelf()
		{
		}

		public bool isRunning()
		{
			return _isCurrentlyManagedByZestKit && !_isPaused;
		}

		public virtual void start()
		{
			if (_isCurrentlyManagedByZestKit)
			{
				_isPaused = false;
				return;
			}
			ZestKit.instance.addTween(this);
			_isCurrentlyManagedByZestKit = true;
			_isPaused = false;
		}

		public void pause()
		{
			_isPaused = true;
		}

		public void resume()
		{
			_isPaused = false;
		}

		public virtual void stop(bool bringToCompletion = false)
		{
			ZestKit.instance.removeTween(this);
			_isCurrentlyManagedByZestKit = false;
			_isPaused = true;
		}
	}
}
