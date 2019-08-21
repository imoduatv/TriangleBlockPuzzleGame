namespace Archon.SwissArmyLib.Events.Loops
{
	public interface ICustomUpdateable
	{
		int[] GetCustomUpdateIds();

		void OnCustomUpdate(int eventId);
	}
}
