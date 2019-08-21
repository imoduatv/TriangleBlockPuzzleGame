using UnityEngine;

public class CameraSeasonColor : MonoBehaviour
{
	public Color[] colors;

	public void SetColor(int index)
	{
		if (index < colors.Length)
		{
			Camera.main.backgroundColor = colors[index];
		}
	}
}
