using System;
using UnityEngine;

[Serializable]
public class ThemeElement
{
	[SerializeField]
	private string m_NameUI;

	[SerializeField]
	private Color m_ColorUI;

	[SerializeField]
	private Sprite m_SpriteUI;

	public string NameUI => m_NameUI;

	public Color ColorUI => m_ColorUI;

	public Sprite SpriteUI
	{
		get
		{
			return m_SpriteUI;
		}
		set
		{
			m_SpriteUI = value;
		}
	}
}
