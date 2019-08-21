using Archon.SwissArmyLib.Pooling;
using System;
using System.Collections.Generic;

namespace Archon.SwissArmyLib.Automata
{
	public class PushdownAutomaton<T>
	{
		private readonly Stack<IPdaState<T>> _stateStack = new Stack<IPdaState<T>>();

		private readonly Dictionary<Type, Pool<IPdaState<T>>> _statePools = new Dictionary<Type, Pool<IPdaState<T>>>();

		public T Context
		{
			get;
			private set;
		}

		public IPdaState<T> CurrentState => (_stateStack.Count <= 0) ? null : _stateStack.Peek();

		public PushdownAutomaton(T context)
		{
			Context = context;
		}

		public void Update(float deltaTime)
		{
			IPdaState<T> currentState = CurrentState;
			if (currentState != null)
			{
				currentState.Reason();
				if (currentState == CurrentState)
				{
					currentState.Act(deltaTime);
				}
			}
		}

		public TState ChangeState<TState>() where TState : IPdaState<T>
		{
			PopStateSilently();
			return PushStateSilently<TState>();
		}

		public TState ChangeStateAuto<TState>() where TState : IPdaState<T>, new()
		{
			if (!IsRegistered<TState>())
			{
				RegisterStateType<TState>();
			}
			return ChangeState<TState>();
		}

		public void PopState()
		{
			PopStateSilently();
			if (CurrentState != null)
			{
				CurrentState.Resume();
			}
		}

		private void PopStateSilently()
		{
			IPdaState<T> pdaState = _stateStack.Pop();
			pdaState.End();
			FreeState(pdaState);
		}

		public void PopAll(bool excludingRoot = false)
		{
			int num = excludingRoot ? 1 : 0;
			while (_stateStack.Count > num)
			{
				PopState();
			}
		}

		public TState PushState<TState>() where TState : IPdaState<T>
		{
			if (CurrentState != null)
			{
				CurrentState.Pause();
			}
			return PushStateSilently<TState>();
		}

		public TState PushStateSilently<TState>() where TState : IPdaState<T>
		{
			TState val = ObtainState<TState>();
			val.Machine = this;
			val.Context = Context;
			_stateStack.Push(val);
			val.Begin();
			return val;
		}

		public TState PushStateAuto<TState>() where TState : IPdaState<T>, new()
		{
			if (CurrentState != null)
			{
				CurrentState.Pause();
			}
			return PushStateSilentlyAuto<TState>();
		}

		public TState PushStateSilentlyAuto<TState>() where TState : IPdaState<T>, new()
		{
			if (!IsRegistered<TState>())
			{
				RegisterStateType<TState>();
			}
			return PushStateSilently<TState>();
		}

		private TState ObtainState<TState>() where TState : IPdaState<T>
		{
			Pool<IPdaState<T>> pool = GetPool(typeof(TState));
			return (TState)pool.Spawn();
		}

		private void FreeState(IPdaState<T> state)
		{
			Type type = state.GetType();
			Pool<IPdaState<T>> pool = GetPool(type);
			pool.Despawn(state);
		}

		private Pool<IPdaState<T>> GetPool(Type stateType)
		{
			_statePools.TryGetValue(stateType, out Pool<IPdaState<T>> value);
			return value;
		}

		public void RegisterStateType<TState>() where TState : IPdaState<T>, new()
		{
			Type typeFromHandle = typeof(TState);
			RegisterStateType(typeFromHandle, () => new TState());
		}

		public void RegisterStateType(Type type, Func<IPdaState<T>> creationMethod)
		{
			Pool<IPdaState<T>> value = new Pool<IPdaState<T>>(creationMethod);
			_statePools[type] = value;
		}

		public bool IsRegistered<TState>()
		{
			return _statePools.ContainsKey(typeof(TState));
		}
	}
}
