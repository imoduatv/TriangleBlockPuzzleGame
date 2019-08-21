using Archon.SwissArmyLib.Events;
using Archon.SwissArmyLib.Events.Loops;
using Archon.SwissArmyLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archon.SwissArmyLib.Gravity
{
	public class GravitationalSystem : IEventListener
	{
		private static readonly List<IGravitationalAffecter> Affecters;

		private static readonly List<Rigidbody> Rigidbodies;

		private static readonly List<Rigidbody2D> Rigidbodies2D;

		static GravitationalSystem()
		{
			Affecters = new List<IGravitationalAffecter>();
			Rigidbodies = new List<Rigidbody>();
			Rigidbodies2D = new List<Rigidbody2D>();
			GravitationalSystem instance = new GravitationalSystem();
			ServiceLocator.RegisterSingleton(instance);
			ServiceLocator.GlobalReset += delegate
			{
				ServiceLocator.RegisterSingleton(instance);
			};
		}

		private GravitationalSystem()
		{
			ManagedUpdate.OnFixedUpdate.AddListener(this);
		}

		~GravitationalSystem()
		{
			ManagedUpdate.OnFixedUpdate.RemoveListener(this);
		}

		public static void Register(IGravitationalAffecter affecter)
		{
			if (object.ReferenceEquals(affecter, null))
			{
				throw new ArgumentNullException("affecter");
			}
			Affecters.Add(affecter);
		}

		public static void Register(Rigidbody rigidbody)
		{
			if (object.ReferenceEquals(rigidbody, null))
			{
				throw new ArgumentNullException("rigidbody");
			}
			Rigidbodies.Add(rigidbody);
		}

		public static void Register(Rigidbody2D rigidbody)
		{
			if (object.ReferenceEquals(rigidbody, null))
			{
				throw new ArgumentNullException("rigidbody");
			}
			Rigidbodies2D.Add(rigidbody);
		}

		public static void Unregister(IGravitationalAffecter affecter)
		{
			if (object.ReferenceEquals(affecter, null))
			{
				throw new ArgumentNullException("affecter");
			}
			Affecters.Remove(affecter);
		}

		public static void Unregister(Rigidbody rigidbody)
		{
			if (object.ReferenceEquals(rigidbody, null))
			{
				throw new ArgumentNullException("rigidbody");
			}
			Rigidbodies.Remove(rigidbody);
		}

		public static void Unregister(Rigidbody2D rigidbody)
		{
			if (object.ReferenceEquals(rigidbody, null))
			{
				throw new ArgumentNullException("rigidbody");
			}
			Rigidbodies2D.Remove(rigidbody);
		}

		public static Vector3 GetGravityAtPoint(Vector3 location)
		{
			Vector3 result = default(Vector3);
			int count = Affecters.Count;
			for (int i = 0; i < count; i++)
			{
				Vector3 forceAt = Affecters[i].GetForceAt(location);
				result.x += forceAt.x;
				result.y += forceAt.y;
				result.z += forceAt.z;
			}
			return result;
		}

		void IEventListener.OnEvent(int eventId)
		{
			if (eventId != -1002)
			{
				return;
			}
			int count = Rigidbodies.Count;
			for (int i = 0; i < count; i++)
			{
				Rigidbody rigidbody = Rigidbodies[i];
				if (rigidbody.useGravity && !rigidbody.IsSleeping())
				{
					Vector3 gravityAtPoint = GetGravityAtPoint(rigidbody.position);
					if (gravityAtPoint.sqrMagnitude > 0.0001f)
					{
						rigidbody.AddForce(gravityAtPoint);
					}
				}
			}
			int count2 = Rigidbodies2D.Count;
			for (int j = 0; j < count2; j++)
			{
				Rigidbody2D rigidbody2D = Rigidbodies2D[j];
				float gravityScale = rigidbody2D.gravityScale;
				if (rigidbody2D.simulated && gravityScale > 0f && rigidbody2D.IsAwake())
				{
					Vector2 force = GetGravityAtPoint(rigidbody2D.position);
					if (!(force.sqrMagnitude < 0.0001f))
					{
						force.x *= gravityScale;
						force.y *= gravityScale;
						rigidbody2D.AddForce(force);
					}
				}
			}
		}
	}
}
