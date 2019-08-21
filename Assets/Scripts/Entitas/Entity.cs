using Components;
using Dta.TenTen;
using Entitas.Serialization.Blueprints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Entitas
{
	public class Entity
	{
		public delegate void EntityChanged(Entity entity, int index, IComponent component);

		public delegate void ComponentReplaced(Entity entity, int index, IComponent previousComponent, IComponent newComponent);

		public delegate void EntityReleased(Entity entity);

		private static readonly Destroy destroyComponent = new Destroy();

		private static readonly Empty emptyComponent = new Empty();

		private static readonly Odd oddComponent = new Odd();

		private static readonly Select selectComponent = new Select();

		internal int _creationIndex;

		internal bool _isEnabled = true;

		private readonly int _totalComponents;

		private readonly IComponent[] _components;

		private readonly Stack<IComponent>[] _componentPools;

		private readonly PoolMetaData _poolMetaData;

		private IComponent[] _componentsCache;

		private int[] _componentIndicesCache;

		private string _toStringCache;

		public readonly HashSet<object> owners = new HashSet<object>();

		public BoardEntities boardEntities => (BoardEntities)GetComponent(1);

		public bool hasBoardEntities => HasComponent(1);

		public Board board => (Board)GetComponent(0);

		public bool hasBoard => HasComponent(0);

		public Bomb bomb => (Bomb)GetComponent(2);

		public bool hasBomb => HasComponent(2);

		public Bonus bonus => (Bonus)GetComponent(3);

		public bool hasBonus => HasComponent(3);

		public Components.BoxCollider2D boxCollider2D => (Components.BoxCollider2D)GetComponent(5);

		public bool hasBoxCollider2D => HasComponent(5);

		public Box box => (Box)GetComponent(4);

		public bool hasBox => HasComponent(4);

		public Childs childs => (Childs)GetComponent(6);

		public bool hasChilds => HasComponent(6);

		public Components.Color color => (Components.Color)GetComponent(7);

		public bool hasColor => HasComponent(7);

		public ColorID colorID => (ColorID)GetComponent(8);

		public bool hasColorID => HasComponent(8);

		public bool isDestroy
		{
			get
			{
				return HasComponent(9);
			}
			set
			{
				if (value != isDestroy)
				{
					if (value)
					{
						AddComponent(9, destroyComponent);
					}
					else
					{
						RemoveComponent(9);
					}
				}
			}
		}

		public bool isEmpty
		{
			get
			{
				return HasComponent(10);
			}
			set
			{
				if (value != isEmpty)
				{
					if (value)
					{
						AddComponent(10, emptyComponent);
					}
					else
					{
						RemoveComponent(10);
					}
				}
			}
		}

		public FirstGrid firstGrid => (FirstGrid)GetComponent(11);

		public bool hasFirstGrid => HasComponent(11);

		public Components.Grid grid => (Components.Grid)GetComponent(12);

		public bool hasGrid => HasComponent(12);

		public Index index => (Index)GetComponent(13);

		public bool hasIndex => HasComponent(13);

		public LocalPosition localPosition => (LocalPosition)GetComponent(14);

		public bool hasLocalPosition => HasComponent(14);

		public bool isOdd
		{
			get
			{
				return HasComponent(15);
			}
			set
			{
				if (value != isOdd)
				{
					if (value)
					{
						AddComponent(15, oddComponent);
					}
					else
					{
						RemoveComponent(15);
					}
				}
			}
		}

		public OldColorID oldColorID => (OldColorID)GetComponent(16);

		public bool hasOldColorID => HasComponent(16);

		public Parent parent => (Parent)GetComponent(17);

		public bool hasParent => HasComponent(17);

		public Position position => (Position)GetComponent(18);

		public bool hasPosition => HasComponent(18);

		public PrefabChild prefabChild => (PrefabChild)GetComponent(20);

		public bool hasPrefabChild => HasComponent(20);

		public Prefab prefab => (Prefab)GetComponent(19);

		public bool hasPrefab => HasComponent(19);

		public Rigidbody2d rigidbody2d => (Rigidbody2d)GetComponent(21);

		public bool hasRigidbody2d => HasComponent(21);

		public ScaleDirect scaleDirect => (ScaleDirect)GetComponent(23);

		public bool hasScaleDirect => HasComponent(23);

		public Scale scale => (Scale)GetComponent(22);

		public bool hasScale => HasComponent(22);

		public bool isSelect
		{
			get
			{
				return HasComponent(24);
			}
			set
			{
				if (value != isSelect)
				{
					if (value)
					{
						AddComponent(24, selectComponent);
					}
					else
					{
						RemoveComponent(24);
					}
				}
			}
		}

		public Components.Shape shape => (Components.Shape)GetComponent(25);

		public bool hasShape => HasComponent(25);

		public ShapeShadow shapeShadow => (ShapeShadow)GetComponent(26);

		public bool hasShapeShadow => HasComponent(26);

		public Components.SortingLayer sortingLayer => (Components.SortingLayer)GetComponent(27);

		public bool hasSortingLayer => HasComponent(27);

		public Components.Sprite sprite => (Components.Sprite)GetComponent(28);

		public bool hasSprite => HasComponent(28);

		public Components.SpriteRenderer spriteRenderer => (Components.SpriteRenderer)GetComponent(29);

		public bool hasSpriteRenderer => HasComponent(29);

		public Test test => (Test)GetComponent(30);

		public bool hasTest => HasComponent(30);

		public Components.Text text => (Components.Text)GetComponent(31);

		public bool hasText => HasComponent(31);

		public Components.Transform transform => (Components.Transform)GetComponent(32);

		public bool hasTransform => HasComponent(32);

		public Components.Type type => (Components.Type)GetComponent(33);

		public bool hasType => HasComponent(33);

		public int totalComponents => _totalComponents;

		public int creationIndex => _creationIndex;

		public bool isEnabled => _isEnabled;

		public Stack<IComponent>[] componentPools => _componentPools;

		public PoolMetaData poolMetaData => _poolMetaData;

		public int retainCount => owners.Count;

		public event EntityChanged OnComponentAdded;

		public event EntityChanged OnComponentRemoved;

		public event ComponentReplaced OnComponentReplaced;

		public event EntityReleased OnEntityReleased;

		public Entity(int totalComponents, Stack<IComponent>[] componentPools, PoolMetaData poolMetaData = null)
		{
			_totalComponents = totalComponents;
			_components = new IComponent[totalComponents];
			_componentPools = componentPools;
			if (poolMetaData != null)
			{
				_poolMetaData = poolMetaData;
				return;
			}
			string[] array = new string[totalComponents];
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				array[i] = i.ToString();
			}
			_poolMetaData = new PoolMetaData("No Pool", array, null);
		}

		public Entity AddBoardEntities(Entity[,] newBoard)
		{
			BoardEntities boardEntities = CreateComponent<BoardEntities>(1);
			boardEntities.board = newBoard;
			return AddComponent(1, boardEntities);
		}

		public Entity ReplaceBoardEntities(Entity[,] newBoard)
		{
			BoardEntities boardEntities = CreateComponent<BoardEntities>(1);
			boardEntities.board = newBoard;
			ReplaceComponent(1, boardEntities);
			return this;
		}

		public Entity RemoveBoardEntities()
		{
			return RemoveComponent(1);
		}

		public Entity AddBoard(int newRows, int newCols, float newX, float newY, float newWidth, float newHeight, UnityEngine.Transform newTranform)
		{
			Board board = CreateComponent<Board>(0);
			board.rows = newRows;
			board.cols = newCols;
			board.x = newX;
			board.y = newY;
			board.width = newWidth;
			board.height = newHeight;
			board.tranform = newTranform;
			return AddComponent(0, board);
		}

		public Entity ReplaceBoard(int newRows, int newCols, float newX, float newY, float newWidth, float newHeight, UnityEngine.Transform newTranform)
		{
			Board board = CreateComponent<Board>(0);
			board.rows = newRows;
			board.cols = newCols;
			board.x = newX;
			board.y = newY;
			board.width = newWidth;
			board.height = newHeight;
			board.tranform = newTranform;
			ReplaceComponent(0, board);
			return this;
		}

		public Entity RemoveBoard()
		{
			return RemoveComponent(0);
		}

		public Entity AddBomb(int newTime)
		{
			Bomb bomb = CreateComponent<Bomb>(2);
			bomb.time = newTime;
			return AddComponent(2, bomb);
		}

		public Entity ReplaceBomb(int newTime)
		{
			Bomb bomb = CreateComponent<Bomb>(2);
			bomb.time = newTime;
			ReplaceComponent(2, bomb);
			return this;
		}

		public Entity RemoveBomb()
		{
			return RemoveComponent(2);
		}

		public Entity AddBonus(int newPoint)
		{
			Bonus bonus = CreateComponent<Bonus>(3);
			bonus.point = newPoint;
			return AddComponent(3, bonus);
		}

		public Entity ReplaceBonus(int newPoint)
		{
			Bonus bonus = CreateComponent<Bonus>(3);
			bonus.point = newPoint;
			ReplaceComponent(3, bonus);
			return this;
		}

		public Entity RemoveBonus()
		{
			return RemoveComponent(3);
		}

		public Entity AddBoxCollider2D(UnityEngine.BoxCollider2D newData)
		{
			Components.BoxCollider2D boxCollider2D = CreateComponent<Components.BoxCollider2D>(5);
			boxCollider2D.data = newData;
			return AddComponent(5, boxCollider2D);
		}

		public Entity ReplaceBoxCollider2D(UnityEngine.BoxCollider2D newData)
		{
			Components.BoxCollider2D boxCollider2D = CreateComponent<Components.BoxCollider2D>(5);
			boxCollider2D.data = newData;
			ReplaceComponent(5, boxCollider2D);
			return this;
		}

		public Entity RemoveBoxCollider2D()
		{
			return RemoveComponent(5);
		}

		public Entity AddBox(float newWidth, float newHeight)
		{
			Box box = CreateComponent<Box>(4);
			box.width = newWidth;
			box.height = newHeight;
			return AddComponent(4, box);
		}

		public Entity ReplaceBox(float newWidth, float newHeight)
		{
			Box box = CreateComponent<Box>(4);
			box.width = newWidth;
			box.height = newHeight;
			ReplaceComponent(4, box);
			return this;
		}

		public Entity RemoveBox()
		{
			return RemoveComponent(4);
		}

		public Entity AddChilds(List<Entity> newData)
		{
			Childs childs = CreateComponent<Childs>(6);
			childs.data = newData;
			return AddComponent(6, childs);
		}

		public Entity ReplaceChilds(List<Entity> newData)
		{
			Childs childs = CreateComponent<Childs>(6);
			childs.data = newData;
			ReplaceComponent(6, childs);
			return this;
		}

		public Entity RemoveChilds()
		{
			return RemoveComponent(6);
		}

		public Entity AddColor(UnityEngine.Color newData)
		{
			Components.Color color = CreateComponent<Components.Color>(7);
			color.data = newData;
			return AddComponent(7, color);
		}

		public Entity ReplaceColor(UnityEngine.Color newData)
		{
			Components.Color color = CreateComponent<Components.Color>(7);
			color.data = newData;
			ReplaceComponent(7, color);
			return this;
		}

		public Entity RemoveColor()
		{
			return RemoveComponent(7);
		}

		public Entity AddColorID(int newData)
		{
			ColorID colorID = CreateComponent<ColorID>(8);
			colorID.data = newData;
			return AddComponent(8, colorID);
		}

		public Entity ReplaceColorID(int newData)
		{
			ColorID colorID = CreateComponent<ColorID>(8);
			colorID.data = newData;
			ReplaceComponent(8, colorID);
			return this;
		}

		public Entity RemoveColorID()
		{
			return RemoveComponent(8);
		}

		public Entity IsDestroy(bool value)
		{
			isDestroy = value;
			return this;
		}

		public Entity IsEmpty(bool value)
		{
			isEmpty = value;
			return this;
		}

		public Entity AddFirstGrid(Entity newData)
		{
			FirstGrid firstGrid = CreateComponent<FirstGrid>(11);
			firstGrid.data = newData;
			return AddComponent(11, firstGrid);
		}

		public Entity ReplaceFirstGrid(Entity newData)
		{
			FirstGrid firstGrid = CreateComponent<FirstGrid>(11);
			firstGrid.data = newData;
			ReplaceComponent(11, firstGrid);
			return this;
		}

		public Entity RemoveFirstGrid()
		{
			return RemoveComponent(11);
		}

		public Entity AddGrid(int newRow, int newCol)
		{
            Components.Grid grid = CreateComponent<Components.Grid>(12);
			grid.row = newRow;
			grid.col = newCol;
			return AddComponent(12, grid);
		}

		public Entity ReplaceGrid(int newRow, int newCol)
		{
            Components.Grid grid = CreateComponent<Components.Grid>(12);
			grid.row = newRow;
			grid.col = newCol;
			ReplaceComponent(12, grid);
			return this;
		}

		public Entity RemoveGrid()
		{
			return RemoveComponent(12);
		}

		public Entity AddIndex(int newData)
		{
			Index index = CreateComponent<Index>(13);
			index.data = newData;
			return AddComponent(13, index);
		}

		public Entity ReplaceIndex(int newData)
		{
			Index index = CreateComponent<Index>(13);
			index.data = newData;
			ReplaceComponent(13, index);
			return this;
		}

		public Entity RemoveIndex()
		{
			return RemoveComponent(13);
		}

		public Entity AddLocalPosition(float newX, float newY)
		{
			LocalPosition localPosition = CreateComponent<LocalPosition>(14);
			localPosition.x = newX;
			localPosition.y = newY;
			return AddComponent(14, localPosition);
		}

		public Entity ReplaceLocalPosition(float newX, float newY)
		{
			LocalPosition localPosition = CreateComponent<LocalPosition>(14);
			localPosition.x = newX;
			localPosition.y = newY;
			ReplaceComponent(14, localPosition);
			return this;
		}

		public Entity RemoveLocalPosition()
		{
			return RemoveComponent(14);
		}

		public Entity IsOdd(bool value)
		{
			isOdd = value;
			return this;
		}

		public Entity AddOldColorID(int newData)
		{
			OldColorID oldColorID = CreateComponent<OldColorID>(16);
			oldColorID.data = newData;
			return AddComponent(16, oldColorID);
		}

		public Entity ReplaceOldColorID(int newData)
		{
			OldColorID oldColorID = CreateComponent<OldColorID>(16);
			oldColorID.data = newData;
			ReplaceComponent(16, oldColorID);
			return this;
		}

		public Entity RemoveOldColorID()
		{
			return RemoveComponent(16);
		}

		public Entity AddParent(UnityEngine.Transform newData)
		{
			Parent parent = CreateComponent<Parent>(17);
			parent.data = newData;
			return AddComponent(17, parent);
		}

		public Entity ReplaceParent(UnityEngine.Transform newData)
		{
			Parent parent = CreateComponent<Parent>(17);
			parent.data = newData;
			ReplaceComponent(17, parent);
			return this;
		}

		public Entity RemoveParent()
		{
			return RemoveComponent(17);
		}

		public Entity AddPosition(float newX, float newY)
		{
			Position position = CreateComponent<Position>(18);
			position.x = newX;
			position.y = newY;
			return AddComponent(18, position);
		}

		public Entity ReplacePosition(float newX, float newY)
		{
			Position position = CreateComponent<Position>(18);
			position.x = newX;
			position.y = newY;
			ReplaceComponent(18, position);
			return this;
		}

		public Entity RemovePosition()
		{
			return RemoveComponent(18);
		}

		public Entity AddPrefabChild(GameObject newGameObject)
		{
			PrefabChild prefabChild = CreateComponent<PrefabChild>(20);
			prefabChild.gameObject = newGameObject;
			return AddComponent(20, prefabChild);
		}

		public Entity ReplacePrefabChild(GameObject newGameObject)
		{
			PrefabChild prefabChild = CreateComponent<PrefabChild>(20);
			prefabChild.gameObject = newGameObject;
			ReplaceComponent(20, prefabChild);
			return this;
		}

		public Entity RemovePrefabChild()
		{
			return RemoveComponent(20);
		}

		public Entity AddPrefab(GameObject newGameObject)
		{
			Prefab prefab = CreateComponent<Prefab>(19);
			prefab.gameObject = newGameObject;
			return AddComponent(19, prefab);
		}

		public Entity ReplacePrefab(GameObject newGameObject)
		{
			Prefab prefab = CreateComponent<Prefab>(19);
			prefab.gameObject = newGameObject;
			ReplaceComponent(19, prefab);
			return this;
		}

		public Entity RemovePrefab()
		{
			return RemoveComponent(19);
		}

		public Entity AddRigidbody2d(Rigidbody2D newData)
		{
			Rigidbody2d rigidbody2d = CreateComponent<Rigidbody2d>(21);
			rigidbody2d.data = newData;
			return AddComponent(21, rigidbody2d);
		}

		public Entity ReplaceRigidbody2d(Rigidbody2D newData)
		{
			Rigidbody2d rigidbody2d = CreateComponent<Rigidbody2d>(21);
			rigidbody2d.data = newData;
			ReplaceComponent(21, rigidbody2d);
			return this;
		}

		public Entity RemoveRigidbody2d()
		{
			return RemoveComponent(21);
		}

		public Entity AddScaleDirect(float newX, float newY)
		{
			ScaleDirect scaleDirect = CreateComponent<ScaleDirect>(23);
			scaleDirect.x = newX;
			scaleDirect.y = newY;
			return AddComponent(23, scaleDirect);
		}

		public Entity ReplaceScaleDirect(float newX, float newY)
		{
			ScaleDirect scaleDirect = CreateComponent<ScaleDirect>(23);
			scaleDirect.x = newX;
			scaleDirect.y = newY;
			ReplaceComponent(23, scaleDirect);
			return this;
		}

		public Entity RemoveScaleDirect()
		{
			return RemoveComponent(23);
		}

		public Entity AddScale(float newX, float newY)
		{
			Scale scale = CreateComponent<Scale>(22);
			scale.x = newX;
			scale.y = newY;
			return AddComponent(22, scale);
		}

		public Entity ReplaceScale(float newX, float newY)
		{
			Scale scale = CreateComponent<Scale>(22);
			scale.x = newX;
			scale.y = newY;
			ReplaceComponent(22, scale);
			return this;
		}

		public Entity RemoveScale()
		{
			return RemoveComponent(22);
		}

		public Entity IsSelect(bool value)
		{
			isSelect = value;
			return this;
		}

		public Entity AddShape(Dta.TenTen.Shape newData)
		{
			Components.Shape shape = CreateComponent<Components.Shape>(25);
			shape.data = newData;
			return AddComponent(25, shape);
		}

		public Entity ReplaceShape(Dta.TenTen.Shape newData)
		{
			Components.Shape shape = CreateComponent<Components.Shape>(25);
			shape.data = newData;
			ReplaceComponent(25, shape);
			return this;
		}

		public Entity RemoveShape()
		{
			return RemoveComponent(25);
		}

		public Entity AddShapeShadow(Entity newShape)
		{
			ShapeShadow shapeShadow = CreateComponent<ShapeShadow>(26);
			shapeShadow.shape = newShape;
			return AddComponent(26, shapeShadow);
		}

		public Entity ReplaceShapeShadow(Entity newShape)
		{
			ShapeShadow shapeShadow = CreateComponent<ShapeShadow>(26);
			shapeShadow.shape = newShape;
			ReplaceComponent(26, shapeShadow);
			return this;
		}

		public Entity RemoveShapeShadow()
		{
			return RemoveComponent(26);
		}

		public Entity AddSortingLayer(int newData)
		{
			Components.SortingLayer sortingLayer = CreateComponent<Components.SortingLayer>(27);
			sortingLayer.data = newData;
			return AddComponent(27, sortingLayer);
		}

		public Entity ReplaceSortingLayer(int newData)
		{
			Components.SortingLayer sortingLayer = CreateComponent<Components.SortingLayer>(27);
			sortingLayer.data = newData;
			ReplaceComponent(27, sortingLayer);
			return this;
		}

		public Entity RemoveSortingLayer()
		{
			return RemoveComponent(27);
		}

		public Entity AddSprite(UnityEngine.Sprite newData)
		{
			Components.Sprite sprite = CreateComponent<Components.Sprite>(28);
			sprite.data = newData;
			return AddComponent(28, sprite);
		}

		public Entity ReplaceSprite(UnityEngine.Sprite newData)
		{
			Components.Sprite sprite = CreateComponent<Components.Sprite>(28);
			sprite.data = newData;
			ReplaceComponent(28, sprite);
			return this;
		}

		public Entity RemoveSprite()
		{
			return RemoveComponent(28);
		}

		public Entity AddSpriteRenderer(UnityEngine.SpriteRenderer newData)
		{
			Components.SpriteRenderer spriteRenderer = CreateComponent<Components.SpriteRenderer>(29);
			spriteRenderer.data = newData;
			return AddComponent(29, spriteRenderer);
		}

		public Entity ReplaceSpriteRenderer(UnityEngine.SpriteRenderer newData)
		{
			Components.SpriteRenderer spriteRenderer = CreateComponent<Components.SpriteRenderer>(29);
			spriteRenderer.data = newData;
			ReplaceComponent(29, spriteRenderer);
			return this;
		}

		public Entity RemoveSpriteRenderer()
		{
			return RemoveComponent(29);
		}

		public Entity AddTest(int newData)
		{
			Test test = CreateComponent<Test>(30);
			test.data = newData;
			return AddComponent(30, test);
		}

		public Entity ReplaceTest(int newData)
		{
			Test test = CreateComponent<Test>(30);
			test.data = newData;
			ReplaceComponent(30, test);
			return this;
		}

		public Entity RemoveTest()
		{
			return RemoveComponent(30);
		}

		public Entity AddText(UnityEngine.UI.Text newData)
		{
			Components.Text text = CreateComponent<Components.Text>(31);
			text.data = newData;
			return AddComponent(31, text);
		}

		public Entity ReplaceText(UnityEngine.UI.Text newData)
		{
			Components.Text text = CreateComponent<Components.Text>(31);
			text.data = newData;
			ReplaceComponent(31, text);
			return this;
		}

		public Entity RemoveText()
		{
			return RemoveComponent(31);
		}

		public Entity AddTransform(UnityEngine.Transform newData)
		{
			Components.Transform transform = CreateComponent<Components.Transform>(32);
			transform.data = newData;
			return AddComponent(32, transform);
		}

		public Entity ReplaceTransform(UnityEngine.Transform newData)
		{
			Components.Transform transform = CreateComponent<Components.Transform>(32);
			transform.data = newData;
			ReplaceComponent(32, transform);
			return this;
		}

		public Entity RemoveTransform()
		{
			return RemoveComponent(32);
		}

		public Entity AddType(BoardType newKind)
		{
			Components.Type type = CreateComponent<Components.Type>(33);
			type.kind = newKind;
			return AddComponent(33, type);
		}

		public Entity ReplaceType(BoardType newKind)
		{
			Components.Type type = CreateComponent<Components.Type>(33);
			type.kind = newKind;
			ReplaceComponent(33, type);
			return this;
		}

		public Entity RemoveType()
		{
			return RemoveComponent(33);
		}

		public Entity AddComponent(int index, IComponent component)
		{
			if (!_isEnabled)
			{
				throw new EntityIsNotEnabledException("Cannot add component '" + _poolMetaData.componentNames[index] + "' to " + this + "!");
			}
			if (HasComponent(index))
			{
				throw new EntityAlreadyHasComponentException(index, "Cannot add component '" + _poolMetaData.componentNames[index] + "' to " + this + "!", "You should check if an entity already has the component before adding it or use entity.ReplaceComponent().");
			}
			_components[index] = component;
			_componentsCache = null;
			_componentIndicesCache = null;
			_toStringCache = null;
			if (this.OnComponentAdded != null)
			{
				this.OnComponentAdded(this, index, component);
			}
			return this;
		}

		public Entity RemoveComponent(int index)
		{
			if (!_isEnabled)
			{
				throw new EntityIsNotEnabledException("Cannot remove component '" + _poolMetaData.componentNames[index] + "' from " + this + "!");
			}
			if (!HasComponent(index))
			{
				throw new EntityDoesNotHaveComponentException(index, "Cannot remove component '" + _poolMetaData.componentNames[index] + "' from " + this + "!", "You should check if an entity has the component before removing it.");
			}
			replaceComponent(index, null);
			return this;
		}

		public Entity ReplaceComponent(int index, IComponent component)
		{
			if (!_isEnabled)
			{
				throw new EntityIsNotEnabledException("Cannot replace component '" + _poolMetaData.componentNames[index] + "' on " + this + "!");
			}
			if (HasComponent(index))
			{
				replaceComponent(index, component);
			}
			else if (component != null)
			{
				AddComponent(index, component);
			}
			return this;
		}

		private void replaceComponent(int index, IComponent replacement)
		{
			IComponent component = _components[index];
			if (component == replacement)
			{
				if (this.OnComponentReplaced != null)
				{
					this.OnComponentReplaced(this, index, component, replacement);
				}
				return;
			}
			_components[index] = replacement;
			_componentsCache = null;
			GetComponentPool(index).Push(component);
			if (replacement == null)
			{
				_componentIndicesCache = null;
				_toStringCache = null;
				if (this.OnComponentRemoved != null)
				{
					this.OnComponentRemoved(this, index, component);
				}
			}
			else if (this.OnComponentReplaced != null)
			{
				this.OnComponentReplaced(this, index, component, replacement);
			}
		}

		public IComponent GetComponent(int index)
		{
			if (!HasComponent(index))
			{
				throw new EntityDoesNotHaveComponentException(index, "Cannot get component '" + _poolMetaData.componentNames[index] + "' from " + this + "!", "You should check if an entity has the component before getting it.");
			}
			return _components[index];
		}

		public IComponent[] GetComponents()
		{
			if (_componentsCache == null)
			{
				List<IComponent> list = new List<IComponent>(16);
				int i = 0;
				for (int num = _components.Length; i < num; i++)
				{
					IComponent component = _components[i];
					if (component != null)
					{
						list.Add(component);
					}
				}
				_componentsCache = list.ToArray();
			}
			return _componentsCache;
		}

		public int[] GetComponentIndices()
		{
			if (_componentIndicesCache == null)
			{
				List<int> list = new List<int>(16);
				int i = 0;
				for (int num = _components.Length; i < num; i++)
				{
					if (_components[i] != null)
					{
						list.Add(i);
					}
				}
				_componentIndicesCache = list.ToArray();
			}
			return _componentIndicesCache;
		}

		public bool HasComponent(int index)
		{
			return _components[index] != null;
		}

		public bool HasComponents(int[] indices)
		{
			int i = 0;
			for (int num = indices.Length; i < num; i++)
			{
				if (_components[indices[i]] == null)
				{
					return false;
				}
			}
			return true;
		}

		public bool HasAnyComponent(int[] indices)
		{
			int i = 0;
			for (int num = indices.Length; i < num; i++)
			{
				if (_components[indices[i]] != null)
				{
					return true;
				}
			}
			return false;
		}

		public void RemoveAllComponents()
		{
			_toStringCache = null;
			int i = 0;
			for (int num = _components.Length; i < num; i++)
			{
				if (_components[i] != null)
				{
					replaceComponent(i, null);
				}
			}
		}

		public Stack<IComponent> GetComponentPool(int index)
		{
			Stack<IComponent> stack = _componentPools[index];
			if (stack == null)
			{
				stack = new Stack<IComponent>();
				_componentPools[index] = stack;
			}
			return stack;
		}

		public IComponent CreateComponent(int index, System.Type type)
		{
			Stack<IComponent> componentPool = GetComponentPool(index);
			return (IComponent)((componentPool.Count <= 0) ? Activator.CreateInstance(type) : componentPool.Pop());
		}

		public T CreateComponent<T>(int index) where T : new()
		{
			Stack<IComponent> componentPool = GetComponentPool(index);
			return (componentPool.Count <= 0) ? new T() : ((T)componentPool.Pop());
		}

		internal void destroy()
		{
			RemoveAllComponents();
			this.OnComponentAdded = null;
			this.OnComponentReplaced = null;
			this.OnComponentRemoved = null;
			_isEnabled = false;
		}

		internal void removeAllOnEntityReleasedHandlers()
		{
			this.OnEntityReleased = null;
		}

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				StringBuilder stringBuilder = new StringBuilder().Append("Entity_").Append(_creationIndex).Append("(*")
					.Append(retainCount)
					.Append(")")
					.Append("(");
				IComponent[] components = GetComponents();
				int num = components.Length - 1;
				int i = 0;
				for (int num2 = components.Length; i < num2; i++)
				{
					stringBuilder.Append(components[i].GetType().Name.RemoveComponentSuffix());
					if (i < num)
					{
						stringBuilder.Append(", ");
					}
				}
				stringBuilder.Append(")");
				_toStringCache = stringBuilder.ToString();
			}
			return _toStringCache;
		}

		public Entity Retain(object owner)
		{
			if (!owners.Add(owner))
			{
				throw new EntityIsAlreadyRetainedByOwnerException(this, owner);
			}
			return this;
		}

		public void Release(object owner)
		{
			if (!owners.Remove(owner))
			{
				throw new EntityIsNotRetainedByOwnerException(this, owner);
			}
			if (owners.Count == 0 && this.OnEntityReleased != null)
			{
				this.OnEntityReleased(this);
			}
		}

		public Entity ApplyBlueprint(Blueprint blueprint, bool replaceComponents = false)
		{
			int i = 0;
			for (int num = blueprint.components.Length; i < num; i++)
			{
				ComponentBlueprint componentBlueprint = blueprint.components[i];
				if (replaceComponents)
				{
					ReplaceComponent(componentBlueprint.index, componentBlueprint.CreateComponent(this));
				}
				else
				{
					AddComponent(componentBlueprint.index, componentBlueprint.CreateComponent(this));
				}
			}
			return this;
		}
	}
}
