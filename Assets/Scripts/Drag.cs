using Entitas;
using Prime31.ZestKit;
using System.Collections;
using UnityEngine;

public class Drag : MonoBehaviour
{
	public delegate void DragCallback();

	private bool isEnable = true;

	private bool isDrag;

	public DragCallback startDragCallback;

	public DragCallback stopDragCallback;

	private Vector3 lastPos;

	private Vector3 curPos;

	private Vector3 deltaPos;

	private Vector3 firstPosition;

	private Vector3 tweenTarget;

	public Entity entity;

	public DragDetect detector;

	private bool isWaitMove;

	private bool isGoingIn;

	private float y_up = 0.6f;

	private float deltaMove = 0.1f;

	private Vector3 dragScale = new Vector3(1.25f, 1.25f, 1f);

	private Vector3 targetMovePos;

	private Vector3 targetScale;

	private ITween<Vector3> tweenMove;

	private Camera mainCam;

	private float width;

	private bool isMoveOnBoard;

	public float minX;

	private Vector2 boxOffset;

	public bool IsCanClearLine;

	private float moveSpeed;

	private float moveNormalSpeed;

	private WaitForSeconds waitGoingIn = new WaitForSeconds(0.5f);

	private void Start()
	{
		mainCam = Camera.main;
		moveNormalSpeed = 40f;
	}

	public void SetIsEnable(bool isEnable)
	{
		this.isEnable = isEnable;
		if (!isEnable)
		{
			isDrag = false;
		}
	}

	private Vector3 GetClickPos()
	{
		if (UnityEngine.Input.touchCount > 0)
		{
			return mainCam.ScreenToWorldPoint(UnityEngine.Input.GetTouch(0).position);
		}
		return mainCam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
	}

	private void Update()
	{
		if (entity.isDestroy)
		{
			return;
		}
		if (isMoveOnBoard)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, targetMovePos, 10f * Time.deltaTime);
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, targetScale, 10f * Time.deltaTime);
			return;
		}
		if (isDrag)
		{
			curPos = GetClickPos();
			deltaPos = curPos - lastPos;
			targetMovePos += deltaPos;
			lastPos = curPos;
			Singleton<GameManager>.Instance.CheckShowShadow();
		}
		base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, targetScale, moveSpeed * Time.deltaTime);
		if (!isGoingIn)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, targetMovePos, moveSpeed * Time.deltaTime);
		}
	}

	private void OnMouseDown()
	{
		if (isGoingIn)
		{
			moveSpeed = moveNormalSpeed;
			isGoingIn = false;
		}
		if (!Singleton<GameManager>.instance.isCanDrag)
		{
			return;
		}
		Singleton<UIManager>.Instance.IsCanPause = false;
		if (!entity.isDestroy && !isDrag && Singleton<GameManager>.Instance.State == GameState.Playing)
		{
			isDrag = true;
			lastPos = GetClickPos();
			if (startDragCallback != null)
			{
				startDragCallback();
			}
			targetMovePos = lastPos;
			targetMovePos.x -= boxOffset.x;
			targetMovePos.y += y_up;
			targetMovePos.z = firstPosition.z;
			targetScale = dragScale;
			Singleton<GameManager>.Instance.curShape = base.gameObject.transform;
			ShapeBorder[] componentsInChildren = GetComponentsInChildren<ShapeBorder>();
			ShapeBorder[] array = componentsInChildren;
			foreach (ShapeBorder shapeBorder in array)
			{
				shapeBorder.Show(isShow: false);
			}
			Singleton<GameManager>.Instance.shapeEntity = entity;
			isWaitMove = true;
			Singleton<GameManager>.Instance.CreateShadow();
		}
	}

	private void OnMouseUp()
	{
		Singleton<UIManager>.Instance.IsCanPause = true;
		if (!entity.isDestroy && isDrag)
		{
			isDrag = false;
			if (stopDragCallback != null)
			{
				stopDragCallback();
			}
			Transform curShape = Singleton<GameManager>.Instance.curShape;
			if (!Singleton<GameManager>.Instance.TryPlaceShape())
			{
				isEnable = false;
				targetMovePos = firstPosition;
				targetScale = Vector3.one;
				Singleton<GameManager>.instance.CheckDeadPiece();
				Singleton<SoundManager>.instance.PlayMoveFailed();
			}
		}
	}

	public Vector3 PlaceFitOnBoard(Vector3 deltaMove)
	{
		deltaMove.z = 0f;
		targetMovePos += deltaMove;
		isMoveOnBoard = true;
		return targetMovePos;
	}

	private void StopTweens()
	{
		if (tweenMove != null)
		{
			if (tweenMove.isRunning())
			{
				tweenMove.stop(bringToCompletion: true);
			}
			tweenMove = null;
		}
	}

	public void SetNewGoingIn()
	{
		isGoingIn = false;
	}

	public IEnumerator IEWaitGoingIn(Vector3 posIn, Vector3 posStop, float time)
	{
		if (!isGoingIn)
		{
			isDrag = false;
			isGoingIn = true;
			isMoveOnBoard = false;
			firstPosition = posStop;
			targetMovePos = posStop;
			targetScale = Vector3.one;
			moveSpeed = (posStop - posIn).magnitude / time;
			BoxCollider2D box2d = GetComponent<BoxCollider2D>();
			Vector2 size = box2d.size;
			width = size.x;
			boxOffset = box2d.offset;
			base.transform.position = posIn;
			base.transform.ZKlocalPositionTo(posStop, time).setEaseType(EaseType.CircOut).start();
			yield return waitGoingIn;
			moveSpeed = moveNormalSpeed;
			isGoingIn = false;
		}
	}

	public void SetMouseDown()
	{
		OnMouseDown();
	}

	public void SetMouseUp()
	{
		OnMouseUp();
	}
}
