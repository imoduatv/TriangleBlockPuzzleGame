using UnityEngine;

public class SetAnimateThemeFarm : MonoBehaviour
{
	[SerializeField]
	private bool m_IsActiveAnimation;

	[Header("Animate Object")]
	[SerializeField]
	private Animator[] m_ChickenAnim;

	[SerializeField]
	private Animator[] m_TreeAnim;

	[SerializeField]
	private CloudMove[] m_CloudAnim;

	private void Awake()
	{
		for (int i = 0; i < m_ChickenAnim.Length; i++)
		{
			m_ChickenAnim[i].enabled = m_IsActiveAnimation;
		}
		for (int j = 0; j < m_TreeAnim.Length; j++)
		{
			m_TreeAnim[j].enabled = m_IsActiveAnimation;
		}
		for (int k = 0; k < m_CloudAnim.Length; k++)
		{
			m_CloudAnim[k].enabled = m_IsActiveAnimation;
		}
	}
}
