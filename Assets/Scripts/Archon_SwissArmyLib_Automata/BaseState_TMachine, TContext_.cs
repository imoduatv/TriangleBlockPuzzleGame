namespace Archon.SwissArmyLib.Automata
{
	public abstract class BaseState<TMachine, TContext> : IState<TMachine, TContext>
	{
		public TMachine Machine
		{
			get;
			set;
		}

		public TContext Context
		{
			get;
			set;
		}

		public float TimeInState
		{
			get;
			private set;
		}

		public virtual void Begin()
		{
			TimeInState = 0f;
		}

		public virtual void Reason()
		{
		}

		public virtual void Act(float deltaTime)
		{
			TimeInState += deltaTime;
		}

		public virtual void End()
		{
		}
	}
}
