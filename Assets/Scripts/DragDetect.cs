using UnityEngine;

public class DragDetect : MonoBehaviour
{
	public Drag drag;

	private void OnMouseDown()
	{
		drag.SetMouseDown();
	}

	private void OnMouseUp()
	{
		drag.SetMouseUp();
	}
}
