using UnityEngine;
using UnityEngine.Events;

namespace Dta
{
	public class Button : MonoBehaviour
	{
		public UnityEvent Click;

		private void Start()
		{
		}

		private void OnMouseUp()
		{
			if (Click != null)
			{
				Click.Invoke();
			}
		}
	}
}
