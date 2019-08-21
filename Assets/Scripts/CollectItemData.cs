using System;

[Serializable]
public class CollectItemData
{
	public float Ratio;

	public int ValueReach;

	public int ItemAmount;

	public CollectItemData(int valueReach, int itemAmount, float ratio)
	{
		Ratio = ratio;
		ValueReach = valueReach;
		ItemAmount = itemAmount;
	}
}
