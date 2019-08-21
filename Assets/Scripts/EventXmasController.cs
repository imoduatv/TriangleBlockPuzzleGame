using System.Collections.Generic;
using UnityEngine;

public class EventXmasController : MonoBehaviour
{
	private bool m_IsOpenEvent;

	private List<CollectItemData> m_ListCollectDataByScore = new List<CollectItemData>
	{
		new CollectItemData(100, 1, 0.2f),
		new CollectItemData(200, 1, 0.5f),
		new CollectItemData(400, 1, 1f),
		new CollectItemData(600, 2, 0.5f),
		new CollectItemData(800, 3, 0.5f),
		new CollectItemData(1000, 3, 1f)
	};

	private List<CollectItemData> m_ListCollectDataByCombo = new List<CollectItemData>
	{
		new CollectItemData(2, 1, 0.5f),
		new CollectItemData(3, 2, 0.5f),
		new CollectItemData(4, 3, 1f)
	};

	private int m_CurrentIndex;

	private CollectItemData m_CurrentData;

	public int GetItemByScore(int score)
	{
		if (!m_IsOpenEvent)
		{
			return 0;
		}
		if (score >= m_CurrentData.ValueReach)
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			int num2 = 0;
			if (num <= m_CurrentData.Ratio)
			{
				num2 = m_CurrentData.ItemAmount;
			}
			if (m_CurrentIndex == m_ListCollectDataByScore.Count - 1)
			{
				CollectItemData collectItemData = m_CurrentData = new CollectItemData(m_CurrentData.ValueReach + 500, 3, 1f);
			}
			else
			{
				m_CurrentIndex++;
				SetCurrentData();
			}
			UnityEngine.Debug.Log("Rand:" + num);
			UnityEngine.Debug.Log("ItemAmount:" + num2);
			UnityEngine.Debug.Log("Next Data:" + m_CurrentData.ValueReach + "," + m_CurrentData.ItemAmount + "," + m_CurrentData.Ratio);
			return num2;
		}
		return 0;
	}

	public int GetItemByCombo(int combo)
	{
		if (!m_IsOpenEvent)
		{
			return 0;
		}
		if (combo > m_ListCollectDataByCombo[m_ListCollectDataByCombo.Count - 1].ValueReach)
		{
			return 3;
		}
		for (int i = 0; i < m_ListCollectDataByCombo.Count; i++)
		{
			if (combo == m_ListCollectDataByCombo[i].ValueReach)
			{
				float num = UnityEngine.Random.Range(0f, 1f);
				UnityEngine.Debug.Log("Rand:" + num);
				if (num <= m_ListCollectDataByCombo[i].Ratio)
				{
					return m_ListCollectDataByCombo[i].ItemAmount;
				}
			}
		}
		return 0;
	}

	public void SetDataFromLoadGame(int scoreLoaded)
	{
		if (scoreLoaded < m_ListCollectDataByScore[0].ValueReach)
		{
			m_CurrentData = m_ListCollectDataByScore[0];
		}
		else if (scoreLoaded >= m_ListCollectDataByScore[m_ListCollectDataByScore.Count - 1].ValueReach)
		{
			m_CurrentData = new CollectItemData(scoreLoaded / 500 * 500 + 500, 3, 1f);
		}
		else
		{
			for (int i = 0; i < m_ListCollectDataByScore.Count - 1; i++)
			{
				if (scoreLoaded >= m_ListCollectDataByScore[i].ValueReach && scoreLoaded < m_ListCollectDataByScore[i + 1].ValueReach)
				{
					m_CurrentData = m_ListCollectDataByScore[i + 1];
				}
			}
		}
		UnityEngine.Debug.Log("Next Data From Load:" + m_CurrentData.ValueReach + "," + m_CurrentData.ItemAmount + "," + m_CurrentData.Ratio);
	}

	public void ResetCurrentData()
	{
		m_CurrentIndex = 0;
		SetCurrentData();
	}

	private void SetCurrentData()
	{
		m_CurrentData = m_ListCollectDataByScore[m_CurrentIndex];
	}

	public void SetOpenEvent(bool isOpen)
	{
		m_IsOpenEvent = isOpen;
	}
}
