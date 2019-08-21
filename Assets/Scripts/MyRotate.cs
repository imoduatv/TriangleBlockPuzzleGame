using UnityEngine;

public class MyRotate : MonoBehaviour
{
	public Vector3 direction;

	public float speed;

	private void Update()
	{
		base.transform.Rotate(direction * Time.deltaTime * speed);
	}
}
