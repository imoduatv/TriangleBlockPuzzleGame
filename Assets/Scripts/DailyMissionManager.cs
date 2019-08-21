using Root;
using UnityEngine;

public class DailyMissionManager : Singleton<DailyMissionManager>
{
	[SerializeField]
	private DailyMissionConfig[] m_DailyMissionConfigs;
}
