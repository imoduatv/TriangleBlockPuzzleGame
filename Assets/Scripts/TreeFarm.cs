using UnityEngine;

public class TreeFarm : MonoBehaviour
{
	public int indexTree;

	private Animator m_TreeAnimator;

	private void OnEnable()
	{
		m_TreeAnimator = GetComponent<Animator>();
		string trigger = "Tree" + indexTree;
		m_TreeAnimator.SetTrigger(trigger);
	}
}
