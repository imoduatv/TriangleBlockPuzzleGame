using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public Transform camTransform;

	public float shakeDuration;

	public float shakeAmount = 0.7f;

	public float decreaseFactor = 1f;

	private Vector3 originalPos;

	private bool isShake;

	private void Awake()
	{
		if (camTransform == null)
		{
			camTransform = (GetComponent(typeof(Transform)) as Transform);
		}
	}

	public void StartShake(float duration = 1f)
	{
		shakeDuration = duration;
		isShake = true;
	}

	private void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	private void Update()
	{
		if (isShake)
		{
			if (shakeDuration > 0f)
			{
				camTransform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				shakeDuration = 0f;
				camTransform.localPosition = originalPos;
				isShake = false;
			}
		}
	}
}
