using UnityEngine;

namespace Archon.SwissArmyLib.Gravity
{
	public interface IGravitationalAffecter
	{
		Vector3 GetForceAt(Vector3 location);
	}
}
