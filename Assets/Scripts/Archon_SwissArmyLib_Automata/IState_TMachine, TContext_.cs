namespace Archon.SwissArmyLib.Automata
{
	public interface IState<TMachine, TContext>
	{
		TMachine Machine
		{
			get;
			set;
		}

		TContext Context
		{
			get;
			set;
		}

		void Begin();

		void Reason();

		void Act(float deltaTime);

		void End();
	}
}
