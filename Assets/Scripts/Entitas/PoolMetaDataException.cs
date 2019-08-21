namespace Entitas
{
	public class PoolMetaDataException : EntitasException
	{
		public PoolMetaDataException(Pool pool, PoolMetaData poolMetaData)
			: base("Invalid PoolMetaData for '" + pool + "'!\nExpected " + pool.totalComponents + " componentName(s) but got " + poolMetaData.componentNames.Length + ":", string.Join("\n", poolMetaData.componentNames))
		{
		}
	}
}
