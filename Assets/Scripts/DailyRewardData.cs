using UnityEngine;

[CreateAssetMenu(fileName = "DailyRewardData", menuName = "GameData/DailyRewardData")]
public class DailyRewardData : ScriptableObject
{
	[SerializeField]
	private int[] m_MoneyRewards;

	public int[] MoneyRewards => m_MoneyRewards;
}
