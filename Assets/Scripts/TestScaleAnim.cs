using Prime31.ZestKit;
using UnityEngine;

public class TestScaleAnim : MonoBehaviour
{
	public float timeAnim;

	public EaseType typeAnim;

	private void Start()
	{
		base.transform.ZKlocalScaleTo(Vector3.one * 0.5f, timeAnim).setEaseType(typeAnim).setLoops(LoopType.RestartFromBeginning, 10)
			.start();
	}
}
