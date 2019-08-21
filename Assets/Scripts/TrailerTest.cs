using System.Collections.Generic;
using UnityEngine;

public class TrailerTest : Singleton<TrailerTest>
{
	[SerializeField]
	private bool m_IsTrailer;

	[Header("Next Shape Add")]
	[SerializeField]
	private int m_ShapeId;

	[SerializeField]
	private int m_RotateTime;

	private List<MyVector2> m_ListShapeTrailer;

	public bool IsTrailer => m_IsTrailer;

	public List<MyVector2> ListShapeTrailer => m_ListShapeTrailer;

	private void Start()
	{
		if (m_IsTrailer)
		{
			m_ListShapeTrailer = new List<MyVector2>();
			m_ListShapeTrailer.Add(new MyVector2(2, 0));
			m_ListShapeTrailer.Add(new MyVector2(2, 0));
			m_ListShapeTrailer.Add(new MyVector2(2, 0));
			m_ListShapeTrailer.Add(new MyVector2(2, 0));
			m_ListShapeTrailer.Add(new MyVector2(2, 0));
			m_ListShapeTrailer.Add(new MyVector2(2, 0));
			m_ListShapeTrailer.Add(new MyVector2(1, 1));
			m_ListShapeTrailer.Add(new MyVector2(1, 2));
			m_ListShapeTrailer.Add(new MyVector2(1, 3));
			m_ListShapeTrailer.Add(new MyVector2(1, 4));
			m_ListShapeTrailer.Add(new MyVector2(1, 5));
			m_ListShapeTrailer.Add(new MyVector2(1, 6));
			m_ListShapeTrailer.Add(new MyVector2(3, 1));
			m_ListShapeTrailer.Add(new MyVector2(3, 1));
			m_ListShapeTrailer.Add(new MyVector2(3, 1));
			m_ListShapeTrailer.Add(new MyVector2(3, 2));
			m_ListShapeTrailer.Add(new MyVector2(3, 2));
			m_ListShapeTrailer.Add(new MyVector2(3, 2));
			m_ListShapeTrailer.Add(new MyVector2(2, 0));
		}
	}

	public void AddNextShape()
	{
		m_ListShapeTrailer.Insert(0, new MyVector2(m_ShapeId, m_RotateTime));
	}
}
