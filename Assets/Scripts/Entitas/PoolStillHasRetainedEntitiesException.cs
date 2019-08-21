namespace Entitas
{
	public class PoolStillHasRetainedEntitiesException : EntitasException
	{
		public PoolStillHasRetainedEntitiesException(Pool pool)
			: base("'" + pool + "' detected retained entities although all entities got destroyed!", "Did you release all entities? Try calling pool.ClearGroups() and systems.ClearReactiveSystems() before calling pool.DestroyAllEntities() to avoid memory leaks.")
		{
		}
	}
}
