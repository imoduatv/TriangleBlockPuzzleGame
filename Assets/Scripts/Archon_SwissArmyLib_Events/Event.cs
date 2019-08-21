using Archon.SwissArmyLib.Collections;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Archon.SwissArmyLib.Events
{
	public class Event
	{
		public struct Listener : IEquatable<Listener>
		{
			public readonly Action DelegateListener;

			public readonly IEventListener InterfaceListener;

			internal Listener(Action listener)
			{
				DelegateListener = listener;
				InterfaceListener = null;
			}

			internal Listener(IEventListener listener)
			{
				InterfaceListener = listener;
				DelegateListener = null;
			}

			public bool Equals(Listener other)
			{
				return object.ReferenceEquals(DelegateListener, other.DelegateListener) && object.ReferenceEquals(InterfaceListener, other.InterfaceListener);
			}

			public override bool Equals(object obj)
			{
				if (object.ReferenceEquals(null, obj))
				{
					return false;
				}
				return obj is Listener && Equals((Listener)obj);
			}

			public override int GetHashCode()
			{
				return (((DelegateListener != null) ? DelegateListener.GetHashCode() : 0) * 397) ^ ((InterfaceListener != null) ? InterfaceListener.GetHashCode() : 0);
			}

			public static bool operator ==(Listener left, Listener right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(Listener left, Listener right)
			{
				return !left.Equals(right);
			}
		}

		private readonly DelayedList<PrioritizedItem<Listener>> _listeners;

		private bool _isIterating;

		private readonly int _id;

		public int Id => _id;

		public bool SuppressExceptions
		{
			get;
			set;
		}

		public ReadOnlyCollection<PrioritizedItem<Listener>> Listeners
		{
			get
			{
				if (!_isIterating)
				{
					_listeners.ProcessPending();
				}
				return _listeners.BackingList;
			}
		}

		public Event(int id)
		{
			_id = id;
			_listeners = new DelayedList<PrioritizedItem<Listener>>(new PrioritizedList<Listener>());
		}

		public Event(int id, int initialListenerCapacity)
		{
			_id = id;
			_listeners = new DelayedList<PrioritizedItem<Listener>>(new PrioritizedList<Listener>(initialListenerCapacity));
		}

		public void AddListener(IEventListener listener, int priority = 0)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Add(new PrioritizedItem<Listener>(new Listener(listener), priority));
		}

		public void AddListener(Action listener, int priority = 0)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Add(new PrioritizedItem<Listener>(new Listener(listener), priority));
		}

		public void RemoveListener(IEventListener listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Remove(new PrioritizedItem<Listener>(new Listener(listener), 0));
		}

		public void RemoveListener(Action listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Remove(new PrioritizedItem<Listener>(new Listener(listener), 0));
		}

		public bool HasListener(IEventListener listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			return HasListenerInternal(listener);
		}

		public bool HasListener(Action listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			return HasListenerInternal(listener);
		}

		private bool HasListenerInternal(object listener)
		{
			if (!_isIterating)
			{
				_listeners.ProcessPending();
			}
			int count = _listeners.Count;
			int num = 0;
			while (num < count)
			{
				PrioritizedItem<Listener> prioritizedItem = _listeners[num];
				Listener item = prioritizedItem.Item;
				if (!object.ReferenceEquals(item.InterfaceListener, listener))
				{
					Listener item2 = prioritizedItem.Item;
					if (!object.ReferenceEquals(item2.DelegateListener, listener))
					{
						num++;
						continue;
					}
				}
				return true;
			}
			return false;
		}

		public void Invoke()
		{
			_listeners.ProcessPending();
			_isIterating = true;
			int count = _listeners.Count;
			for (int i = 0; i < count; i++)
			{
				PrioritizedItem<Listener> prioritizedItem = _listeners[i];
				Listener item = prioritizedItem.Item;
				try
				{
					if (item.InterfaceListener != null)
					{
						item.InterfaceListener.OnEvent(_id);
					}
					else
					{
						item.DelegateListener();
					}
				}
				catch (Exception exception)
				{
					if (!SuppressExceptions)
					{
						UnityEngine.Object @object = item.InterfaceListener as UnityEngine.Object;
						if (@object != null)
						{
							UnityEngine.Debug.LogException(exception, @object);
						}
						else
						{
							UnityEngine.Debug.LogException(exception);
						}
					}
				}
			}
			_isIterating = false;
		}

		public void Clear()
		{
			_listeners.Clear();
		}
	}
	public class Event<T>
	{
		public struct Listener : IEquatable<Listener>
		{
			public readonly Action<T> DelegateListener;

			public readonly IEventListener<T> InterfaceListener;

			internal Listener(Action<T> listener)
			{
				DelegateListener = listener;
				InterfaceListener = null;
			}

			internal Listener(IEventListener<T> listener)
			{
				InterfaceListener = listener;
				DelegateListener = null;
			}

			public bool Equals(Listener other)
			{
				return object.ReferenceEquals(DelegateListener, other.DelegateListener) && object.ReferenceEquals(InterfaceListener, other.InterfaceListener);
			}

			public override bool Equals(object obj)
			{
				if (object.ReferenceEquals(null, obj))
				{
					return false;
				}
				return obj is Listener && Equals((Listener)obj);
			}

			public override int GetHashCode()
			{
				return (((DelegateListener != null) ? DelegateListener.GetHashCode() : 0) * 397) ^ ((InterfaceListener != null) ? InterfaceListener.GetHashCode() : 0);
			}

			public static bool operator ==(Listener left, Listener right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(Listener left, Listener right)
			{
				return !left.Equals(right);
			}
		}

		private readonly DelayedList<PrioritizedItem<Listener>> _listeners;

		private bool _isIterating;

		private readonly int _id;

		public int Id => _id;

		public bool SuppressExceptions
		{
			get;
			set;
		}

		public ReadOnlyCollection<PrioritizedItem<Listener>> Listeners
		{
			get
			{
				if (!_isIterating)
				{
					_listeners.ProcessPending();
				}
				return _listeners.BackingList;
			}
		}

		public Event(int id)
		{
			_id = id;
			_listeners = new DelayedList<PrioritizedItem<Listener>>(new PrioritizedList<Listener>());
		}

		public Event(int id, int initialListenerCapacity)
		{
			_id = id;
			_listeners = new DelayedList<PrioritizedItem<Listener>>(new PrioritizedList<Listener>(initialListenerCapacity));
		}

		public void AddListener(IEventListener<T> listener, int priority = 0)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Add(new PrioritizedItem<Listener>(new Listener(listener), priority));
		}

		public void AddListener(Action<T> listener, int priority = 0)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Add(new PrioritizedItem<Listener>(new Listener(listener), priority));
		}

		public void RemoveListener(IEventListener<T> listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Remove(new PrioritizedItem<Listener>(new Listener(listener), 0));
		}

		public void RemoveListener(Action<T> listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			_listeners.Remove(new PrioritizedItem<Listener>(new Listener(listener), 0));
		}

		public bool HasListener(IEventListener<T> listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			return HasListenerInternal(listener);
		}

		public bool HasListener(Action<T> listener)
		{
			if (object.ReferenceEquals(listener, null))
			{
				throw new ArgumentNullException("listener");
			}
			return HasListenerInternal(listener);
		}

		private bool HasListenerInternal(object listener)
		{
			if (!_isIterating)
			{
				_listeners.ProcessPending();
			}
			int count = _listeners.Count;
			int num = 0;
			while (num < count)
			{
				PrioritizedItem<Listener> prioritizedItem = _listeners[num];
				Listener item = prioritizedItem.Item;
				if (!object.ReferenceEquals(item.InterfaceListener, listener))
				{
					Listener item2 = prioritizedItem.Item;
					if (!object.ReferenceEquals(item2.DelegateListener, listener))
					{
						num++;
						continue;
					}
				}
				return true;
			}
			return false;
		}

		public void Invoke(T args)
		{
			_listeners.ProcessPending();
			_isIterating = true;
			int count = _listeners.Count;
			for (int i = 0; i < count; i++)
			{
				try
				{
					PrioritizedItem<Listener> prioritizedItem = _listeners[i];
					Listener item = prioritizedItem.Item;
					if (item.InterfaceListener != null)
					{
						item.InterfaceListener.OnEvent(_id, args);
					}
					else
					{
						item.DelegateListener(args);
					}
				}
				catch (Exception message)
				{
					if (!SuppressExceptions)
					{
						UnityEngine.Debug.LogError(message);
					}
				}
			}
			_isIterating = false;
		}

		public void Clear()
		{
			_listeners.Clear();
		}
	}
}
