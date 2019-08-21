using UnityEngine;

public class ExchangeData : ScriptableObject
{
	[Header("Background")]
	public Sprite DiamondBG;

	public Sprite SnowFlakeBG;

	[Header("Theme")]
	public Sprite XmasTheme;

	public Sprite OceanTheme;

	public Sprite SpaceTheme;

	public Sprite FarmTheme;

	[Header("Diamond")]
	public Sprite Diamond;

	public Color ContentDiamondColor;

	[Header("SnowFlake")]
	public Sprite SnowFlake;

	public Color ContentSnowColor;
}
