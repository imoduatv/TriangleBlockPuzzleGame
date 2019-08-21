using UnityEngine;

public class ExchangeItem : ScriptableObject
{
	[Header("Price")]
	public ExchangeItemType PriceType;

	public int PriceValue;

	[Header("Item")]
	public ExchangeItemType ItemType;

	public int DiamondValue;

	public int SnowFlake;

	public ThemeName Theme;
}
