using Entitas;
using UnityEngine;

namespace Components
{
	public class Board : IComponent
	{
		public int rows;

		public int cols;

		public float x;

		public float y;

		public float width;

		public float height;

		public UnityEngine.Transform tranform;
	}
}
