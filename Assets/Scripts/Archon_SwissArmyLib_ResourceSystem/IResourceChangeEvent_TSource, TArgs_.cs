namespace Archon.SwissArmyLib.ResourceSystem
{
	public interface IResourceChangeEvent<TSource, TArgs> : IResourceEvent<TSource, TArgs>
	{
		float OriginalDelta
		{
			get;
		}

		float ModifiedDelta
		{
			get;
		}

		float AppliedDelta
		{
			get;
		}
	}
}
