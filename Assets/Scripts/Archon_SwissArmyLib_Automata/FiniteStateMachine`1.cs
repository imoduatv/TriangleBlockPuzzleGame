using System;
using System.Collections.Generic;

namespace Archon.SwissArmyLib.Automata
{
	public class FiniteStateMachine<T>
	{
		private readonly Dictionary<Type, IFsmState<T>> _states = new Dictionary<Type, IFsmState<T>>();

		public T Context
		{
			get;
			private set;
		}

		public IFsmState<T> CurrentState
		{
			get;
			private set;
		}

		public IFsmState<T> PreviousState
		{
			get;
			private set;
		}

		public FiniteStateMachine(T context)
		{
			Context = context;
		}

		public FiniteStateMachine(T context, IFsmState<T> startState)
			: this(context)
		{
			RegisterState(startState);
			ChangeState(startState);
		}

		public void Update(float deltaTime)
		{
			IFsmState<T> currentState = CurrentState;
			if (currentState != null)
			{
				currentState.Reason();
				if (currentState == CurrentState)
				{
					CurrentState.Act(deltaTime);
				}
			}
		}

		public void RegisterState(IFsmState<T> state)
		{
			_states[state.GetType()] = state;
		}

		public bool IsStateRegistered(Type stateType)
		{
			return _states.ContainsKey(stateType);
		}

		public bool IsStateRegistered<TState>() where TState : IFsmState<T>
		{
			return _states.ContainsKey(typeof(TState));
		}

		public TState ChangeStateAuto<TState>() where TState : IFsmState<T>, new()
		{
			Type typeFromHandle = typeof(TState);
			if (!_states.TryGetValue(typeFromHandle, out IFsmState<T> value))
			{
				value = (_states[typeFromHandle] = new TState());
			}
			return ChangeState((TState)value);
		}

		public TState ChangeState<TState>() where TState : IFsmState<T>
		{
			Type typeFromHandle = typeof(TState);
			if (!_states.TryGetValue(typeFromHandle, out IFsmState<T> value))
			{
				throw new InvalidOperationException($"A state of type '{typeFromHandle}' is not registered, did you mean to use ChangeStateAuto?");
			}
			return ChangeState((TState)value);
		}

		public TState ChangeState<TState>(TState state) where TState : IFsmState<T>
		{
			if (CurrentState != null)
			{
				CurrentState.End();
			}
			PreviousState = CurrentState;
			CurrentState = state;
			if (CurrentState != null)
			{
				RegisterState(state);
				CurrentState.Machine = this;
				CurrentState.Context = Context;
				CurrentState.Begin();
			}
			return state;
		}
	}
}
