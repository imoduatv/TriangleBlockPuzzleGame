namespace Archon.SwissArmyLib.ResourceSystem
{
	internal class ResourceEvent<TSource, TArgs> : IResourceChangeEvent<TSource, TArgs>, IResourcePreChangeEvent<TSource, TArgs>, IResourceEvent<TSource, TArgs>
	{
		public float OriginalDelta
		{
			get;
			set;
		}

		public float ModifiedDelta
		{
			get;
			set;
		}

		public float AppliedDelta
		{
			get;
			set;
		}

		public TSource Source
		{
			get;
			set;
		}

		public TArgs Args
		{
			get;
			set;
		}
	}
}
