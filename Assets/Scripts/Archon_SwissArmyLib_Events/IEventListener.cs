namespace Archon.SwissArmyLib.Events
{
	public interface IEventListener
	{
		void OnEvent(int eventId);
	}
	public interface IEventListener<in TArgs>
	{
		void OnEvent(int eventId, TArgs args);
	}
}
