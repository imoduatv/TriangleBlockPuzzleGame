namespace Archon.SwissArmyLib.ResourceSystem
{
	public interface IResourcePreChangeEvent<TSource, TArgs> : IResourceEvent<TSource, TArgs>
	{
		float OriginalDelta
		{
			get;
		}

		float ModifiedDelta
		{
			get;
			set;
		}
	}
}
