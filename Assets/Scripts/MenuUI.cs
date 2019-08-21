using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour, IMenuUI
{
	protected bool m_IsInit;

	protected bool m_IsAnimatingReward;

	public virtual void Init()
	{
	}

	public virtual void Show(Action afterShowCallback = null)
	{
	}

	public virtual void Hide(Action afterHideCallback = null)
	{
	}

	public virtual void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement)
	{
	}

	public void SetUI(Image image, ThemeElement themeElement)
	{
		if (themeElement.SpriteUI != null)
		{
			image.sprite = themeElement.SpriteUI;
		}
		image.color = themeElement.ColorUI;
	}

	public void SetUI(Text text, ThemeElement themeElement)
	{
		text.color = themeElement.ColorUI;
	}
}
