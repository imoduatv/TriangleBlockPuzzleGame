using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class AssetManagers : Singleton<AssetManagers>
{
	public bool isUseFourSeasons = true;

	public Sprite normalGrid;

	public Sprite hexGrid;

	public Sprite triGrid;

	public Color tileEmpty;

	public Color tileEmptyLock;

	public Sprite spriteEmpty;

	public Color colorBG;

	public List<Color> colors;

	public List<Sprite> sprites;

	private int currentColor;

	private int currentSprite;

	private ThemeName m_ThemeName;

	private void Start()
	{
	}

	public void InitColor()
	{
		colors = new List<Color>();
		sprites = new List<Sprite>();
		SetSkinByIndex();
	}

	private void SetSkinByIndex()
	{
		UpdateSeasonSkin();
	}

	public void ChangeTheme(ThemeName themeName)
	{
		m_ThemeName = themeName;
		GameData.Instance().SetCurrentTheme(themeName);
		SetSkinByIndex();
	}

	public int GetColorID(Color color)
	{
		return colors.FindIndex((Color x) => x.a == color.a && x.r == color.r && x.g == color.g && x.b == color.b);
	}

	public int GetRandomColorIndex()
	{
		int result = currentColor;
		currentColor++;
		if (currentColor >= 20)
		{
			currentColor = 0;
		}
		return result;
	}

	public Sprite GetRandomSpriteIndex()
	{
		int index = currentSprite;
		currentSprite++;
		if (currentSprite >= sprites.Count)
		{
			currentSprite = 0;
		}
		return sprites[index];
	}

	public Color GetColor(int index)
	{
		int count = colors.Count;
		if (index >= count)
		{
			index %= count;
		}
		return colors[index];
	}

	public Sprite GetSprite(int colorIndex)
	{
		int count = sprites.Count;
		if (colorIndex >= count)
		{
			colorIndex %= count;
		}
		return sprites[colorIndex];
	}

	private void UpdateSeasonSkin()
	{
		SetColorTheme(m_ThemeName);
		Singleton<GameManager>.Instance.UpdateAllGridColor();
	}

	private void SetColorTheme(ThemeName name)
	{
		ThemeData themeData = Singleton<ThemeManager>.instance.GetThemeData(name);
		tileEmpty = themeData.BoardColor;
		tileEmptyLock = themeData.BoardColorLock;
		spriteEmpty = themeData.BoardSprite;
		triGrid = spriteEmpty;
		colors.Clear();
		sprites.Clear();
		Color[] cellColors = themeData.CellColors;
		Sprite[] cellSprites = themeData.CellSprites;
		sprites.AddRange(cellSprites);
		colors.AddRange(cellColors);
		currentColor = Random.Range(0, colors.Count);
		currentSprite = Random.Range(0, sprites.Count);
	}

	public Color GetRandomShapeColor()
	{
		int index = currentColor;
		currentColor++;
		if (currentColor >= colors.Count)
		{
			currentColor = 0;
		}
		return colors[index];
	}

	private string ColorToHex(Color32 color)
	{
		return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
	}

	private Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		return new Color32(r, g, b, byte.MaxValue);
	}
}
