using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EnhancedUI.EnhancedScroller
{
	[RequireComponent(typeof(ScrollRect))]
	public class EnhancedScroller : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IEventSystemHandler
	{
		public enum ScrollDirectionEnum
		{
			Vertical,
			Horizontal
		}

		public enum CellViewPositionEnum
		{
			Before,
			After
		}

		public enum ScrollbarVisibilityEnum
		{
			OnlyIfNeeded,
			Always,
			Never
		}

		private enum ListPositionEnum
		{
			First,
			Last
		}

		public enum TweenType
		{
			immediate,
			linear,
			spring,
			easeInQuad,
			easeOutQuad,
			easeInOutQuad,
			easeInCubic,
			easeOutCubic,
			easeInOutCubic,
			easeInQuart,
			easeOutQuart,
			easeInOutQuart,
			easeInQuint,
			easeOutQuint,
			easeInOutQuint,
			easeInSine,
			easeOutSine,
			easeInOutSine,
			easeInExpo,
			easeOutExpo,
			easeInOutExpo,
			easeInCirc,
			easeOutCirc,
			easeInOutCirc,
			easeInBounce,
			easeOutBounce,
			easeInOutBounce,
			easeInBack,
			easeOutBack,
			easeInOutBack,
			easeInElastic,
			easeOutElastic,
			easeInOutElastic
		}

		public ScrollDirectionEnum scrollDirection;

		public float spacing;

		public RectOffset padding;

		[SerializeField]
		private bool loop;

		[SerializeField]
		private ScrollbarVisibilityEnum scrollbarVisibility;

		public bool snapping;

		public bool snapOnlyOnEndDrag;

		public Action onSnapDone;

		public Action onBeginDrag;

		public bool forceChildExpandWidth = true;

		public bool forceChildExpandHeight = true;

		public TextAnchor childAnchor = TextAnchor.MiddleCenter;

		public float snapVelocityThreshold;

		public float snapWatchOffset;

		public float snapJumpToOffset;

		public float snapCellCenterOffset;

		public bool snapUseCellSpacing;

		public TweenType snapTweenType;

		public float snapTweenTime;

		public CellViewVisibilityChangedDelegate cellViewVisibilityChanged;

		public ScrollerScrolledDelegate scrollerScrolled;

		public ScrollerSnappedDelegate scrollerSnapped;

		public ScrollerScrollingChangedDelegate scrollerScrollingChanged;

		public ScrollerTweeningChangedDelegate scrollerTweeningChanged;

		private ScrollRect _scrollRect;

		private Vector2 _beginDragPosition;

		private int _dragDirection;

		private RectTransform _scrollRectTransform;

		private Scrollbar _scrollbar;

		private RectTransform _container;

		private HorizontalOrVerticalLayoutGroup _layoutGroup;

		private IEnhancedScrollerDelegate _delegate;

		private bool _reloadData;

		private bool _refreshActive;

		private SmallList<EnhancedScrollerCellView> _recycledCellViews = new SmallList<EnhancedScrollerCellView>();

		private LayoutElement _firstPadder;

		private LayoutElement _lastPadder;

		private RectTransform _recycledCellViewContainer;

		private SmallList<float> _cellViewSizeArray = new SmallList<float>();

		private SmallList<float> _cellViewOffsetArray = new SmallList<float>();

		private float _scrollPosition;

		private SmallList<EnhancedScrollerCellView> _activeCellViews = new SmallList<EnhancedScrollerCellView>();

		private int _activeCellViewsStartIndex;

		private int _activeCellViewsEndIndex;

		private int _loopFirstCellIndex;

		private int _loopLastCellIndex;

		private float _loopFirstScrollPosition;

		private float _loopLastScrollPosition;

		private float _loopFirstJumpTrigger;

		private float _loopLastJumpTrigger;

		private float _lastScrollRectSize;

		private bool _lastLoop;

		private int _snapCellViewIndex;

		private int _snapDataIndex;

		private bool _snapJumping;

		private bool _snapInertia;

		private ScrollbarVisibilityEnum _lastScrollbarVisibility;

		private bool _isDrag;

		private float _tweenTimeLeft;

		public IEnhancedScrollerDelegate Delegate
		{
			get
			{
				return _delegate;
			}
			set
			{
				_delegate = value;
				_reloadData = true;
			}
		}

		public float ScrollPosition
		{
			get
			{
				return _scrollPosition;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, GetScrollPositionForCellViewIndex(_cellViewSizeArray.Count - 1, CellViewPositionEnum.Before));
				if (_scrollPosition != value)
				{
					_scrollPosition = value;
					if (scrollDirection == ScrollDirectionEnum.Vertical)
					{
						_scrollRect.verticalNormalizedPosition = 1f - _scrollPosition / _ScrollSize;
					}
					else
					{
						_scrollRect.horizontalNormalizedPosition = _scrollPosition / _ScrollSize;
					}
					_refreshActive = true;
				}
			}
		}

		public bool Loop
		{
			get
			{
				return loop;
			}
			set
			{
				if (loop != value)
				{
					float scrollPosition = _scrollPosition;
					loop = value;
					_Resize(keepPosition: false);
					if (loop)
					{
						ScrollPosition = _loopFirstScrollPosition + scrollPosition;
					}
					else
					{
						ScrollPosition = scrollPosition - _loopFirstScrollPosition;
					}
					ScrollbarVisibility = scrollbarVisibility;
				}
			}
		}

		public ScrollbarVisibilityEnum ScrollbarVisibility
		{
			get
			{
				return scrollbarVisibility;
			}
			set
			{
				scrollbarVisibility = value;
				if (_scrollbar != null && _cellViewOffsetArray != null && _cellViewOffsetArray.Count > 0)
				{
					if (_cellViewOffsetArray.Last() < ScrollRectSize || loop)
					{
						_scrollbar.gameObject.SetActive(scrollbarVisibility == ScrollbarVisibilityEnum.Always);
					}
					else
					{
						_scrollbar.gameObject.SetActive(scrollbarVisibility != ScrollbarVisibilityEnum.Never);
					}
				}
			}
		}

		public Vector2 Velocity
		{
			get
			{
				return _scrollRect.velocity;
			}
			set
			{
				_scrollRect.velocity = value;
			}
		}

		public float LinearVelocity
		{
			get
			{
				float result;
				if (scrollDirection == ScrollDirectionEnum.Vertical)
				{
					Vector2 velocity = _scrollRect.velocity;
					result = velocity.y;
				}
				else
				{
					Vector2 velocity2 = _scrollRect.velocity;
					result = velocity2.x;
				}
				return result;
			}
			set
			{
				if (scrollDirection == ScrollDirectionEnum.Vertical)
				{
					_scrollRect.velocity = new Vector2(0f, value);
				}
				else
				{
					_scrollRect.velocity = new Vector2(value, 0f);
				}
			}
		}

		public bool IsScrolling
		{
			get;
			private set;
		}

		public bool IsTweening
		{
			get;
			private set;
		}

		public int StartCellViewIndex => _activeCellViewsStartIndex;

		public int EndCellViewIndex => _activeCellViewsEndIndex;

		public int StartDataIndex => _activeCellViewsStartIndex % NumberOfCells;

		public int EndDataIndex => _activeCellViewsEndIndex % NumberOfCells;

		public int NumberOfCells => (_delegate != null) ? _delegate.GetNumberOfCells(this) : 0;

		public ScrollRect ScrollRect => _scrollRect;

		public float ScrollRectSize
		{
			get
			{
				if (scrollDirection == ScrollDirectionEnum.Vertical)
				{
					return _scrollRectTransform.rect.height;
				}
				return _scrollRectTransform.rect.width;
			}
		}

		private float _ScrollSize
		{
			get
			{
				if (scrollDirection == ScrollDirectionEnum.Vertical)
				{
					return _container.rect.height - _scrollRectTransform.rect.height;
				}
				return _container.rect.width - _scrollRectTransform.rect.width;
			}
		}

		public EnhancedScrollerCellView GetCellView(EnhancedScrollerCellView cellPrefab)
		{
			EnhancedScrollerCellView enhancedScrollerCellView = _GetRecycledCellView(cellPrefab);
			if (enhancedScrollerCellView == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(cellPrefab.gameObject);
				enhancedScrollerCellView = gameObject.GetComponent<EnhancedScrollerCellView>();
				enhancedScrollerCellView.transform.SetParent(_container);
			}
			return enhancedScrollerCellView;
		}

		public void ReloadData()
		{
			_reloadData = false;
			_scrollPosition = 0f;
			_RecycleAllCells();
			if (_delegate != null)
			{
				_Resize(keepPosition: false);
			}
		}

		public void RefreshActiveCellViews()
		{
			for (int i = 0; i < _activeCellViews.Count; i++)
			{
				_activeCellViews[i].RefreshCellView();
			}
		}

		public void ClearRecycled()
		{
			for (int i = 0; i < _recycledCellViews.Count; i++)
			{
				UnityEngine.Object.DestroyImmediate(_recycledCellViews[i].gameObject);
			}
			_recycledCellViews.Clear();
		}

		public void ToggleLoop()
		{
			Loop = !loop;
		}

		public void JumpToDataIndex(int dataIndex, float scrollerOffset = 0f, float cellOffset = 0f, bool useSpacing = true, TweenType tweenType = TweenType.immediate, float tweenTime = 0f, Action jumpComplete = null)
		{
			float num = 0f;
			if (cellOffset != 0f)
			{
				float num2 = (_delegate == null) ? 0f : _delegate.GetCellViewSize(this, dataIndex);
				if (useSpacing)
				{
					num2 += spacing;
					if (dataIndex > 0 && dataIndex < NumberOfCells - 1)
					{
						num2 += spacing;
					}
				}
				num = num2 * cellOffset;
			}
			float num3 = 0f;
			float num4 = 0f - scrollerOffset * ScrollRectSize + num;
			if (loop)
			{
				float num5 = GetScrollPositionForCellViewIndex(dataIndex, CellViewPositionEnum.Before) + num4;
				float num6 = GetScrollPositionForCellViewIndex(dataIndex + NumberOfCells, CellViewPositionEnum.Before) + num4;
				float num7 = GetScrollPositionForCellViewIndex(dataIndex + NumberOfCells * 2, CellViewPositionEnum.Before) + num4;
				float num8 = Mathf.Abs(_scrollPosition - num5);
				float num9 = Mathf.Abs(_scrollPosition - num6);
				float num10 = Mathf.Abs(_scrollPosition - num7);
				num3 = ((num8 < num9) ? ((!(num8 < num10)) ? num7 : num5) : ((!(num9 < num10)) ? num7 : num6));
			}
			else
			{
				num3 = GetScrollPositionForDataIndex(dataIndex, CellViewPositionEnum.Before) + num4;
			}
			num3 = Mathf.Clamp(num3, 0f, GetScrollPositionForCellViewIndex(_cellViewSizeArray.Count - 1, CellViewPositionEnum.Before));
			if (useSpacing)
			{
				num3 = Mathf.Clamp(num3 - spacing, 0f, GetScrollPositionForCellViewIndex(_cellViewSizeArray.Count - 1, CellViewPositionEnum.Before));
			}
			StartCoroutine(TweenPosition(tweenType, tweenTime, ScrollPosition, num3, jumpComplete));
		}

		[Obsolete("This is an obsolete method, please use the version of this function with a cell offset.")]
		public void JumpToDataIndex(int dataIndex, CellViewPositionEnum position = CellViewPositionEnum.Before, bool useSpacing = true)
		{
			ScrollPosition = GetScrollPositionForDataIndex(dataIndex, position);
			if (useSpacing)
			{
				if (position == CellViewPositionEnum.Before)
				{
					ScrollPosition = _scrollPosition - spacing;
				}
				else
				{
					ScrollPosition = _scrollPosition + spacing;
				}
			}
		}

		public void Snap()
		{
			if (NumberOfCells != 0)
			{
				_snapJumping = true;
				LinearVelocity = 0f;
				_snapInertia = true;
				float position = ScrollPosition + ScrollRectSize * Mathf.Clamp01(snapWatchOffset);
				_snapCellViewIndex = GetCellViewIndexAtPosition(position);
				if (_dragDirection < 0)
				{
					_snapCellViewIndex++;
				}
				if (_snapCellViewIndex >= NumberOfCells)
				{
					_snapCellViewIndex = NumberOfCells - 1;
				}
				_snapDataIndex = _snapCellViewIndex % NumberOfCells;
				JumpToDataIndex(_snapDataIndex, snapJumpToOffset, snapCellCenterOffset, snapUseCellSpacing, snapTweenType, snapTweenTime, SnapJumpComplete);
			}
		}

		public void JumpToDataIndex(int dataIndex)
		{
			_snapJumping = true;
			JumpToDataIndex(dataIndex, snapJumpToOffset, snapCellCenterOffset, snapUseCellSpacing, snapTweenType, snapTweenTime, SnapJumpComplete);
		}

		public float GetScrollPositionForCellViewIndex(int cellViewIndex, CellViewPositionEnum insertPosition)
		{
			if (NumberOfCells == 0)
			{
				return 0f;
			}
			if (cellViewIndex == 0 && insertPosition == CellViewPositionEnum.Before)
			{
				return 0f;
			}
			if (cellViewIndex < _cellViewOffsetArray.Count)
			{
				if (insertPosition == CellViewPositionEnum.Before)
				{
					return _cellViewOffsetArray[cellViewIndex - 1] + spacing + (float)((scrollDirection != 0) ? padding.left : padding.top);
				}
				return _cellViewOffsetArray[cellViewIndex] + (float)((scrollDirection != 0) ? padding.left : padding.top);
			}
			return _cellViewOffsetArray[_cellViewOffsetArray.Count - 2];
		}

		public float GetScrollPositionForDataIndex(int dataIndex, CellViewPositionEnum insertPosition)
		{
			return GetScrollPositionForCellViewIndex((!loop) ? dataIndex : (_delegate.GetNumberOfCells(this) + dataIndex), insertPosition);
		}

		public int GetCellViewIndexAtPosition(float position)
		{
			return _GetCellIndexAtPosition(position, 0, _cellViewOffsetArray.Count - 1);
		}

		private void _Resize(bool keepPosition)
		{
			float scrollPosition = _scrollPosition;
			_cellViewSizeArray.Clear();
			float num = _AddCellViewSizes();
			if (loop)
			{
				if (num < ScrollRectSize)
				{
					int numberOfTimes = Mathf.CeilToInt(ScrollRectSize / num);
					_DuplicateCellViewSizes(numberOfTimes, _cellViewSizeArray.Count);
				}
				_loopFirstCellIndex = _cellViewSizeArray.Count;
				_loopLastCellIndex = _loopFirstCellIndex + _cellViewSizeArray.Count - 1;
				_DuplicateCellViewSizes(2, _cellViewSizeArray.Count);
			}
			_CalculateCellViewOffsets();
			if (scrollDirection == ScrollDirectionEnum.Vertical)
			{
				RectTransform container = _container;
				Vector2 sizeDelta = _container.sizeDelta;
				container.sizeDelta = new Vector2(sizeDelta.x, _cellViewOffsetArray.Last() + (float)padding.top + (float)padding.bottom);
			}
			else
			{
				RectTransform container2 = _container;
				float x = _cellViewOffsetArray.Last() + (float)padding.left + (float)padding.right;
				Vector2 sizeDelta2 = _container.sizeDelta;
				container2.sizeDelta = new Vector2(x, sizeDelta2.y);
			}
			if (loop)
			{
				_loopFirstScrollPosition = GetScrollPositionForCellViewIndex(_loopFirstCellIndex, CellViewPositionEnum.Before) + spacing * 0.5f;
				_loopLastScrollPosition = GetScrollPositionForCellViewIndex(_loopLastCellIndex, CellViewPositionEnum.After) - ScrollRectSize + spacing * 0.5f;
				_loopFirstJumpTrigger = _loopFirstScrollPosition - ScrollRectSize;
				_loopLastJumpTrigger = _loopLastScrollPosition + ScrollRectSize;
			}
			_ResetVisibleCellViews();
			if (keepPosition)
			{
				ScrollPosition = scrollPosition;
			}
			else if (loop)
			{
				ScrollPosition = _loopFirstScrollPosition;
			}
			else
			{
				ScrollPosition = 0f;
			}
			ScrollbarVisibility = scrollbarVisibility;
		}

		private float _AddCellViewSizes()
		{
			float num = 0f;
			for (int i = 0; i < NumberOfCells; i++)
			{
				_cellViewSizeArray.Add(_delegate.GetCellViewSize(this, i) + ((i != 0) ? _layoutGroup.spacing : 0f));
				num += _cellViewSizeArray[_cellViewSizeArray.Count - 1];
			}
			return num;
		}

		private void _DuplicateCellViewSizes(int numberOfTimes, int cellCount)
		{
			for (int i = 0; i < numberOfTimes; i++)
			{
				for (int j = 0; j < cellCount; j++)
				{
					_cellViewSizeArray.Add(_cellViewSizeArray[j] + ((j != 0) ? 0f : _layoutGroup.spacing));
				}
			}
		}

		private void _CalculateCellViewOffsets()
		{
			_cellViewOffsetArray.Clear();
			float num = 0f;
			for (int i = 0; i < _cellViewSizeArray.Count; i++)
			{
				num += _cellViewSizeArray[i];
				_cellViewOffsetArray.Add(num);
			}
		}

		private EnhancedScrollerCellView _GetRecycledCellView(EnhancedScrollerCellView cellPrefab)
		{
			for (int i = 0; i < _recycledCellViews.Count; i++)
			{
				if (_recycledCellViews[i].cellIdentifier == cellPrefab.cellIdentifier)
				{
					return _recycledCellViews.RemoveAt(i);
				}
			}
			return null;
		}

		private void _ResetVisibleCellViews()
		{
			_CalculateCurrentActiveCellRange(out int startIndex, out int endIndex);
			int num = 0;
			SmallList<int> smallList = new SmallList<int>();
			while (num < _activeCellViews.Count)
			{
				if (_activeCellViews[num].cellIndex < startIndex || _activeCellViews[num].cellIndex > endIndex)
				{
					_RecycleCell(_activeCellViews[num]);
					continue;
				}
				smallList.Add(_activeCellViews[num].cellIndex);
				num++;
			}
			if (smallList.Count == 0)
			{
				for (num = startIndex; num <= endIndex; num++)
				{
					_AddCellView(num, ListPositionEnum.Last);
				}
			}
			else
			{
				for (num = endIndex; num >= startIndex; num--)
				{
					if (num < smallList.First())
					{
						_AddCellView(num, ListPositionEnum.First);
					}
				}
				for (num = startIndex; num <= endIndex; num++)
				{
					if (num > smallList.Last())
					{
						_AddCellView(num, ListPositionEnum.Last);
					}
				}
			}
			_activeCellViewsStartIndex = startIndex;
			_activeCellViewsEndIndex = endIndex;
			_SetPadders();
		}

		private void _RecycleAllCells()
		{
			while (_activeCellViews.Count > 0)
			{
				_RecycleCell(_activeCellViews[0]);
			}
			_activeCellViewsStartIndex = 0;
			_activeCellViewsEndIndex = 0;
		}

		private void _RecycleCell(EnhancedScrollerCellView cellView)
		{
			_activeCellViews.Remove(cellView);
			_recycledCellViews.Add(cellView);
			cellView.transform.SetParent(_recycledCellViewContainer);
			cellView.dataIndex = 0;
			cellView.cellIndex = 0;
			cellView.active = false;
			if (cellViewVisibilityChanged != null)
			{
				cellViewVisibilityChanged(cellView);
			}
		}

		private void _AddCellView(int cellIndex, ListPositionEnum listPosition)
		{
			if (NumberOfCells != 0)
			{
				int dataIndex = cellIndex % NumberOfCells;
				EnhancedScrollerCellView cellView = _delegate.GetCellView(this, dataIndex, cellIndex);
				cellView.cellIndex = cellIndex;
				cellView.dataIndex = dataIndex;
				cellView.active = true;
				cellView.transform.SetParent(_container, worldPositionStays: false);
				cellView.transform.localScale = Vector3.one;
				cellView.transform.localPosition = Vector3.zero;
				LayoutElement layoutElement = cellView.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = cellView.gameObject.AddComponent<LayoutElement>();
				}
				if (scrollDirection == ScrollDirectionEnum.Vertical)
				{
					layoutElement.minHeight = _cellViewSizeArray[cellIndex] - ((cellIndex <= 0) ? 0f : _layoutGroup.spacing);
				}
				else
				{
					layoutElement.minWidth = _cellViewSizeArray[cellIndex] - ((cellIndex <= 0) ? 0f : _layoutGroup.spacing);
				}
				if (listPosition == ListPositionEnum.First)
				{
					_activeCellViews.AddStart(cellView);
				}
				else
				{
					_activeCellViews.Add(cellView);
				}
				switch (listPosition)
				{
				case ListPositionEnum.Last:
					cellView.transform.SetSiblingIndex(_container.childCount - 2);
					break;
				case ListPositionEnum.First:
					cellView.transform.SetSiblingIndex(1);
					break;
				}
				if (cellViewVisibilityChanged != null)
				{
					cellViewVisibilityChanged(cellView);
				}
			}
		}

		private void _SetPadders()
		{
			if (NumberOfCells != 0)
			{
				float num = _cellViewOffsetArray[_activeCellViewsStartIndex] - _cellViewSizeArray[_activeCellViewsStartIndex];
				float num2 = _cellViewOffsetArray.Last() - _cellViewOffsetArray[_activeCellViewsEndIndex];
				if (scrollDirection == ScrollDirectionEnum.Vertical)
				{
					_firstPadder.minHeight = num;
					_firstPadder.gameObject.SetActive(_firstPadder.minHeight > 0f);
					_lastPadder.minHeight = num2;
					_lastPadder.gameObject.SetActive(_lastPadder.minHeight > 0f);
				}
				else
				{
					_firstPadder.minWidth = num;
					_firstPadder.gameObject.SetActive(_firstPadder.minWidth > 0f);
					_lastPadder.minWidth = num2;
					_lastPadder.gameObject.SetActive(_lastPadder.minWidth > 0f);
				}
			}
		}

		private void _RefreshActive()
		{
			_refreshActive = false;
			Vector2 zero = Vector2.zero;
			if (loop)
			{
				if (_scrollPosition < _loopFirstJumpTrigger)
				{
					zero = _scrollRect.velocity;
					ScrollPosition = _loopLastScrollPosition - (_loopFirstJumpTrigger - _scrollPosition);
					_scrollRect.velocity = zero;
				}
				else if (_scrollPosition > _loopLastJumpTrigger)
				{
					zero = _scrollRect.velocity;
					ScrollPosition = _loopFirstScrollPosition + (_scrollPosition - _loopLastJumpTrigger);
					_scrollRect.velocity = zero;
				}
			}
			_CalculateCurrentActiveCellRange(out int startIndex, out int endIndex);
			if (startIndex != _activeCellViewsStartIndex || endIndex != _activeCellViewsEndIndex)
			{
				_ResetVisibleCellViews();
			}
		}

		private void _CalculateCurrentActiveCellRange(out int startIndex, out int endIndex)
		{
			startIndex = 0;
			endIndex = 0;
			float scrollPosition = _scrollPosition;
			float position = _scrollPosition + ((scrollDirection != 0) ? _scrollRectTransform.rect.width : _scrollRectTransform.rect.height);
			startIndex = GetCellViewIndexAtPosition(scrollPosition);
			endIndex = GetCellViewIndexAtPosition(position);
		}

		private int _GetCellIndexAtPosition(float position, int startIndex, int endIndex)
		{
			if (startIndex >= endIndex)
			{
				return startIndex;
			}
			int num = (startIndex + endIndex) / 2;
			if (_cellViewOffsetArray[num] + (float)((scrollDirection != 0) ? padding.left : padding.top) >= position)
			{
				return _GetCellIndexAtPosition(position, startIndex, num);
			}
			return _GetCellIndexAtPosition(position, num + 1, endIndex);
		}

		private void Awake()
		{
			_scrollRect = GetComponent<ScrollRect>();
			_scrollRectTransform = _scrollRect.GetComponent<RectTransform>();
			if (_scrollRect.content != null)
			{
				UnityEngine.Object.DestroyImmediate(_scrollRect.content.gameObject);
			}
			GameObject gameObject = new GameObject("Container", typeof(RectTransform));
			gameObject.transform.SetParent(_scrollRectTransform);
			gameObject.transform.localEulerAngles = Vector3.zero;
			if (scrollDirection == ScrollDirectionEnum.Vertical)
			{
				gameObject.AddComponent<VerticalLayoutGroup>();
			}
			else
			{
				gameObject.AddComponent<HorizontalLayoutGroup>();
			}
			_container = gameObject.GetComponent<RectTransform>();
			if (scrollDirection == ScrollDirectionEnum.Vertical)
			{
				_container.anchorMin = new Vector2(0f, 1f);
				_container.anchorMax = Vector2.one;
				_container.pivot = new Vector2(0.5f, 1f);
			}
			else
			{
				_container.anchorMin = Vector2.zero;
				_container.anchorMax = new Vector2(0f, 1f);
				_container.pivot = new Vector2(0.5f, 0.5f);
			}
			_container.offsetMax = Vector2.zero;
			_container.offsetMin = Vector2.zero;
			_container.localScale = Vector3.one;
			RectTransform container = _container;
			Vector3 localPosition = _container.localPosition;
			float x = localPosition.x;
			Vector3 localPosition2 = _container.localPosition;
			container.localPosition = new Vector2(x, localPosition2.y);
			_scrollRect.content = _container;
			if (scrollDirection == ScrollDirectionEnum.Vertical)
			{
				_scrollbar = _scrollRect.verticalScrollbar;
			}
			else
			{
				_scrollbar = _scrollRect.horizontalScrollbar;
			}
			_layoutGroup = _container.GetComponent<HorizontalOrVerticalLayoutGroup>();
			_layoutGroup.spacing = spacing;
			_layoutGroup.padding = padding;
			_layoutGroup.childAlignment = childAnchor;
			_layoutGroup.childForceExpandHeight = forceChildExpandHeight;
			_layoutGroup.childForceExpandWidth = forceChildExpandWidth;
			_scrollRect.horizontal = (scrollDirection == ScrollDirectionEnum.Horizontal);
			_scrollRect.vertical = (scrollDirection == ScrollDirectionEnum.Vertical);
			gameObject = new GameObject("First Padder", typeof(RectTransform), typeof(LayoutElement));
			gameObject.transform.SetParent(_container, worldPositionStays: false);
			_firstPadder = gameObject.GetComponent<LayoutElement>();
			gameObject = new GameObject("Last Padder", typeof(RectTransform), typeof(LayoutElement));
			gameObject.transform.SetParent(_container, worldPositionStays: false);
			_lastPadder = gameObject.GetComponent<LayoutElement>();
			gameObject = new GameObject("Recycled Cells", typeof(RectTransform));
			gameObject.transform.SetParent(_scrollRect.transform, worldPositionStays: false);
			_recycledCellViewContainer = gameObject.GetComponent<RectTransform>();
			_recycledCellViewContainer.gameObject.SetActive(value: false);
			_lastScrollRectSize = ScrollRectSize;
			_lastLoop = loop;
			_lastScrollbarVisibility = scrollbarVisibility;
		}

		public void SetPivotCenter(float x, float y)
		{
			if (!(_container == null))
			{
				_container.pivot = new Vector2(x, y);
			}
		}

		private void Update()
		{
			if (_reloadData)
			{
				ReloadData();
			}
			if ((loop && _lastScrollRectSize != ScrollRectSize) || loop != _lastLoop)
			{
				_Resize(keepPosition: true);
				_lastScrollRectSize = ScrollRectSize;
				_lastLoop = loop;
			}
			if (_lastScrollbarVisibility != scrollbarVisibility)
			{
				ScrollbarVisibility = scrollbarVisibility;
				_lastScrollbarVisibility = scrollbarVisibility;
			}
			if (LinearVelocity != 0f && !IsScrolling)
			{
				IsScrolling = true;
				if (scrollerScrollingChanged != null)
				{
					scrollerScrollingChanged(this, scrolling: true);
				}
			}
			else if (LinearVelocity == 0f && IsScrolling)
			{
				IsScrolling = false;
				if (scrollerScrollingChanged != null)
				{
					scrollerScrollingChanged(this, scrolling: false);
				}
			}
		}

		private void LateUpdate()
		{
			if (_refreshActive)
			{
				_RefreshActive();
			}
		}

		private void OnEnable()
		{
			_scrollRect.onValueChanged.AddListener(_ScrollRect_OnValueChanged);
		}

		private void OnDisable()
		{
			_scrollRect.onValueChanged.RemoveListener(_ScrollRect_OnValueChanged);
		}

		public void OnBeginDrag(PointerEventData data)
		{
			_beginDragPosition = data.pressPosition;
			_isDrag = true;
			if (onBeginDrag != null)
			{
				onBeginDrag();
			}
		}

		public void OnEndDrag(PointerEventData data)
		{
			_isDrag = false;
		}

		private void _ScrollRect_OnValueChanged(Vector2 val)
		{
			if (scrollDirection == ScrollDirectionEnum.Vertical)
			{
				_scrollPosition = (1f - val.y) * _ScrollSize;
			}
			else
			{
				_scrollPosition = val.x * _ScrollSize;
			}
			_refreshActive = true;
			if (scrollerScrolled != null)
			{
				scrollerScrolled(this, val, _scrollPosition);
			}
			if (snapOnlyOnEndDrag && !_isDrag && !_snapJumping)
			{
				float num = Mathf.Abs(LinearVelocity);
				if (num > 0f && num <= snapVelocityThreshold)
				{
					Snap();
				}
			}
			_RefreshActive();
		}

		private void SnapJumpComplete()
		{
			_snapJumping = false;
			if (scrollerSnapped != null)
			{
				scrollerSnapped(this, _snapCellViewIndex, _snapDataIndex);
			}
		}

		private IEnumerator TweenPosition(TweenType tweenType, float time, float start, float end, Action tweenComplete)
		{
			if (tweenType == TweenType.immediate || time == 0f)
			{
				ScrollPosition = end;
			}
			else
			{
				_scrollRect.velocity = Vector2.zero;
				IsTweening = true;
				if (scrollerTweeningChanged != null)
				{
					scrollerTweeningChanged(this, tweening: true);
				}
				_tweenTimeLeft = 0f;
				float newPosition = 0f;
				while (_tweenTimeLeft < time)
				{
					switch (tweenType)
					{
					case TweenType.linear:
						newPosition = linear(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.spring:
						newPosition = spring(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInQuad:
						newPosition = easeInQuad(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutQuad:
						newPosition = easeOutQuad(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutQuad:
						newPosition = easeInOutQuad(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInCubic:
						newPosition = easeInCubic(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutCubic:
						newPosition = easeOutCubic(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutCubic:
						newPosition = easeInOutCubic(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInQuart:
						newPosition = easeInQuart(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutQuart:
						newPosition = easeOutQuart(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutQuart:
						newPosition = easeInOutQuart(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInQuint:
						newPosition = easeInQuint(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutQuint:
						newPosition = easeOutQuint(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutQuint:
						newPosition = easeInOutQuint(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInSine:
						newPosition = easeInSine(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutSine:
						newPosition = easeOutSine(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutSine:
						newPosition = easeInOutSine(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInExpo:
						newPosition = easeInExpo(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutExpo:
						newPosition = easeOutExpo(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutExpo:
						newPosition = easeInOutExpo(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInCirc:
						newPosition = easeInCirc(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutCirc:
						newPosition = easeOutCirc(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutCirc:
						newPosition = easeInOutCirc(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInBounce:
						newPosition = easeInBounce(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutBounce:
						newPosition = easeOutBounce(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutBounce:
						newPosition = easeInOutBounce(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInBack:
						newPosition = easeInBack(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutBack:
						newPosition = easeOutBack(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutBack:
						newPosition = easeInOutBack(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInElastic:
						newPosition = easeInElastic(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeOutElastic:
						newPosition = easeOutElastic(start, end, _tweenTimeLeft / time);
						break;
					case TweenType.easeInOutElastic:
						newPosition = easeInOutElastic(start, end, _tweenTimeLeft / time);
						break;
					}
					if (loop)
					{
						if (end > start && newPosition > _loopLastJumpTrigger)
						{
							newPosition = _loopFirstScrollPosition + (newPosition - _loopLastJumpTrigger);
						}
						else if (start > end && newPosition < _loopFirstJumpTrigger)
						{
							newPosition = _loopLastScrollPosition - (_loopFirstJumpTrigger - newPosition);
						}
					}
					ScrollPosition = newPosition;
					_tweenTimeLeft += Time.unscaledDeltaTime;
					yield return null;
				}
				ScrollPosition = end;
			}
			tweenComplete?.Invoke();
			if (onSnapDone != null)
			{
				onSnapDone();
			}
			IsTweening = false;
			if (scrollerTweeningChanged != null)
			{
				scrollerTweeningChanged(this, tweening: false);
			}
		}

		private float linear(float start, float end, float val)
		{
			return Mathf.Lerp(start, end, val);
		}

		private static float spring(float start, float end, float val)
		{
			val = Mathf.Clamp01(val);
			val = (Mathf.Sin(val * 3.14159274f * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
			return start + (end - start) * val;
		}

		private static float easeInQuad(float start, float end, float val)
		{
			end -= start;
			return end * val * val + start;
		}

		private static float easeOutQuad(float start, float end, float val)
		{
			end -= start;
			return (0f - end) * val * (val - 2f) + start;
		}

		private static float easeInOutQuad(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val + start;
			}
			val -= 1f;
			return (0f - end) / 2f * (val * (val - 2f) - 1f) + start;
		}

		private static float easeInCubic(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val + start;
		}

		private static float easeOutCubic(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * (val * val * val + 1f) + start;
		}

		private static float easeInOutCubic(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val + start;
			}
			val -= 2f;
			return end / 2f * (val * val * val + 2f) + start;
		}

		private static float easeInQuart(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val * val + start;
		}

		private static float easeOutQuart(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return (0f - end) * (val * val * val * val - 1f) + start;
		}

		private static float easeInOutQuart(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val * val + start;
			}
			val -= 2f;
			return (0f - end) / 2f * (val * val * val * val - 2f) + start;
		}

		private static float easeInQuint(float start, float end, float val)
		{
			end -= start;
			return end * val * val * val * val * val + start;
		}

		private static float easeOutQuint(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * (val * val * val * val * val + 1f) + start;
		}

		private static float easeInOutQuint(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * val * val * val * val * val + start;
			}
			val -= 2f;
			return end / 2f * (val * val * val * val * val + 2f) + start;
		}

		private static float easeInSine(float start, float end, float val)
		{
			end -= start;
			return (0f - end) * Mathf.Cos(val / 1f * 1.57079637f) + end + start;
		}

		private static float easeOutSine(float start, float end, float val)
		{
			end -= start;
			return end * Mathf.Sin(val / 1f * 1.57079637f) + start;
		}

		private static float easeInOutSine(float start, float end, float val)
		{
			end -= start;
			return (0f - end) / 2f * (Mathf.Cos(3.14159274f * val / 1f) - 1f) + start;
		}

		private static float easeInExpo(float start, float end, float val)
		{
			end -= start;
			return end * Mathf.Pow(2f, 10f * (val / 1f - 1f)) + start;
		}

		private static float easeOutExpo(float start, float end, float val)
		{
			end -= start;
			return end * (0f - Mathf.Pow(2f, -10f * val / 1f) + 1f) + start;
		}

		private static float easeInOutExpo(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return end / 2f * Mathf.Pow(2f, 10f * (val - 1f)) + start;
			}
			val -= 1f;
			return end / 2f * (0f - Mathf.Pow(2f, -10f * val) + 2f) + start;
		}

		private static float easeInCirc(float start, float end, float val)
		{
			end -= start;
			return (0f - end) * (Mathf.Sqrt(1f - val * val) - 1f) + start;
		}

		private static float easeOutCirc(float start, float end, float val)
		{
			val -= 1f;
			end -= start;
			return end * Mathf.Sqrt(1f - val * val) + start;
		}

		private static float easeInOutCirc(float start, float end, float val)
		{
			val /= 0.5f;
			end -= start;
			if (val < 1f)
			{
				return (0f - end) / 2f * (Mathf.Sqrt(1f - val * val) - 1f) + start;
			}
			val -= 2f;
			return end / 2f * (Mathf.Sqrt(1f - val * val) + 1f) + start;
		}

		private static float easeInBounce(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			return end - easeOutBounce(0f, end, num - val) + start;
		}

		private static float easeOutBounce(float start, float end, float val)
		{
			val /= 1f;
			end -= start;
			if (val < 0.363636374f)
			{
				return end * (7.5625f * val * val) + start;
			}
			if (val < 0.727272749f)
			{
				val -= 0.545454562f;
				return end * (7.5625f * val * val + 0.75f) + start;
			}
			if ((double)val < 0.90909090909090906)
			{
				val -= 0.8181818f;
				return end * (7.5625f * val * val + 0.9375f) + start;
			}
			val -= 21f / 22f;
			return end * (7.5625f * val * val + 63f / 64f) + start;
		}

		private static float easeInOutBounce(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			if (val < num / 2f)
			{
				return easeInBounce(0f, end, val * 2f) * 0.5f + start;
			}
			return easeOutBounce(0f, end, val * 2f - num) * 0.5f + end * 0.5f + start;
		}

		private static float easeInBack(float start, float end, float val)
		{
			end -= start;
			val /= 1f;
			float num = 1.70158f;
			return end * val * val * ((num + 1f) * val - num) + start;
		}

		private static float easeOutBack(float start, float end, float val)
		{
			float num = 1.70158f;
			end -= start;
			val = val / 1f - 1f;
			return end * (val * val * ((num + 1f) * val + num) + 1f) + start;
		}

		private static float easeInOutBack(float start, float end, float val)
		{
			float num = 1.70158f;
			end -= start;
			val /= 0.5f;
			if (val < 1f)
			{
				num *= 1.525f;
				return end / 2f * (val * val * ((num + 1f) * val - num)) + start;
			}
			val -= 2f;
			num *= 1.525f;
			return end / 2f * (val * val * ((num + 1f) * val + num) + 2f) + start;
		}

		private static float easeInElastic(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			float num4 = 0f;
			if (val == 0f)
			{
				return start;
			}
			val /= num;
			if (val == 1f)
			{
				return start + end;
			}
			if (num4 == 0f || num4 < Mathf.Abs(end))
			{
				num4 = end;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = num2 / 6.28318548f * Mathf.Asin(end / num4);
			}
			val -= 1f;
			return 0f - num4 * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val * num - num3) * 6.28318548f / num2) + start;
		}

		private static float easeOutElastic(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			float num4 = 0f;
			if (val == 0f)
			{
				return start;
			}
			val /= num;
			if (val == 1f)
			{
				return start + end;
			}
			if (num4 == 0f || num4 < Mathf.Abs(end))
			{
				num4 = end;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = num2 / 6.28318548f * Mathf.Asin(end / num4);
			}
			return num4 * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val * num - num3) * 6.28318548f / num2) + end + start;
		}

		private static float easeInOutElastic(float start, float end, float val)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			float num4 = 0f;
			if (val == 0f)
			{
				return start;
			}
			val /= num / 2f;
			if (val == 2f)
			{
				return start + end;
			}
			if (num4 == 0f || num4 < Mathf.Abs(end))
			{
				num4 = end;
				num3 = num2 / 4f;
			}
			else
			{
				num3 = num2 / 6.28318548f * Mathf.Asin(end / num4);
			}
			if (val < 1f)
			{
				val -= 1f;
				return -0.5f * (num4 * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val * num - num3) * 6.28318548f / num2)) + start;
			}
			val -= 1f;
			return num4 * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val * num - num3) * 6.28318548f / num2) * 0.5f + end + start;
		}
	}
}
