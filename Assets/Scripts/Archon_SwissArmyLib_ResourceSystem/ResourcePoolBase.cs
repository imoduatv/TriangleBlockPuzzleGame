using UnityEngine;

namespace Archon.SwissArmyLib.ResourceSystem
{
	public abstract class ResourcePoolBase : MonoBehaviour
	{
		public abstract float Current
		{
			get;
			protected set;
		}

		public abstract float Max
		{
			get;
			set;
		}

		public abstract bool EmptyTillRenewed
		{
			get;
			set;
		}

		public abstract float Percentage
		{
			get;
		}

		public abstract bool IsEmpty
		{
			get;
		}

		public abstract bool IsFull
		{
			get;
		}

		public abstract float TimeSinceEmpty
		{
			get;
		}

		public abstract float Add(float amount, bool forced = false);

		public abstract float Remove(float amount, bool forced = false);

		public abstract float Empty(bool forced = false);

		public abstract float Fill(bool forced = false);

		public abstract float Fill(float toValue, bool forced = false);

		public abstract float Renew(bool forced = false);

		public abstract float Renew(float toValue, bool forced = false);
	}
}
