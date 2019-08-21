namespace Archon.SwissArmyLib.ResourceSystem
{
	public interface IResourceEvent<TSource, TArgs>
	{
		TSource Source
		{
			get;
			set;
		}

		TArgs Args
		{
			get;
			set;
		}
	}
}
