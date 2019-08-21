using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class PlayersScrollController : MonoBehaviour, IEnhancedScrollerDelegate
{
	[SerializeField]
	private PlayerCellView m_PlayerCellViewPrefab;

	[SerializeField]
	private EnhancedScroller m_PlayersScroller;

	[SerializeField]
	private float m_SizeCellView = 100f;

	private List<PlayerCellData> m_Data;

	public bool IsShow;

	private void Start()
	{
	}

	public void Show(Dictionary<string, int> fbScores)
	{
		IsShow = true;
		m_Data = new List<PlayerCellData>();
		foreach (KeyValuePair<string, int> fbScore in fbScores)
		{
			m_Data.Add(new PlayerCellData(fbScore.Key, fbScore.Value));
		}
		SortListData(m_Data);
		if (m_Data.Count <= 3)
		{
			m_PlayersScroller.SetPivotCenter(0.5f, 0.5f);
		}
		else
		{
			m_PlayersScroller.SetPivotCenter(0f, 0.5f);
		}
		m_PlayersScroller.Delegate = this;
		m_PlayersScroller.ReloadData();
	}

	private void SortListData(List<PlayerCellData> data)
	{
		int count = data.Count;
		for (int i = 0; i < count; i++)
		{
			for (int j = i; j < count; j++)
			{
				if (data[i].Score < data[j].Score)
				{
					PlayerCellData value = data[i];
					data[i] = data[j];
					data[j] = value;
				}
			}
		}
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		PlayerCellView playerCellView = scroller.GetCellView(m_PlayerCellViewPrefab) as PlayerCellView;
		playerCellView.SetData(m_Data[dataIndex]);
		return playerCellView;
	}

	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return m_SizeCellView;
	}

	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return m_Data.Count;
	}
}
