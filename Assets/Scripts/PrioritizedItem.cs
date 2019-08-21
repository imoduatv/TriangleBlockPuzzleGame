using System;
using System.Collections.Generic;

namespace Archon.SwissArmyLib.Collections
{
	public struct PrioritizedItem<T> : IEquatable<PrioritizedItem<T>>
	{
		public readonly T Item;

		public readonly int Priority;

		public PrioritizedItem(T item, int priority)
		{
			Item = item;
			Priority = priority;
		}

		public bool Equals(PrioritizedItem<T> other)
		{
			if (default(T) == null)
			{
				return object.ReferenceEquals(Item, other.Item);
			}
			return EqualityComparer<T>.Default.Equals(Item, other.Item);
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj is PrioritizedItem<T> && Equals((PrioritizedItem<T>)obj);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<T>.Default.GetHashCode(Item);
		}

		public static bool operator ==(PrioritizedItem<T> left, PrioritizedItem<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(PrioritizedItem<T> left, PrioritizedItem<T> right)
		{
			return !left.Equals(right);
		}
	}
}
