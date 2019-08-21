using System;
using System.Collections.Generic;
using UnityEngine.UI;

public interface IMenuUI
{
	void Init();

	void Show(Action afterShowCallback = null);

	void Hide(Action afterHideCallback = null);

	void SetThemeUI(Dictionary<string, ThemeElement> dictThemeElement);

	void SetUI(Image image, ThemeElement themeElement);

	void SetUI(Text text, ThemeElement themeElement);
}
