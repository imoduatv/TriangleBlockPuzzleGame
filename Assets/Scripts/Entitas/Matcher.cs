using System;
using System.Collections.Generic;
using System.Text;

namespace Entitas
{
	public class Matcher : IAllOfMatcher, IAnyOfMatcher, INoneOfMatcher, ICompoundMatcher, IMatcher
	{
		private static IMatcher _matcherBoardEntities;

		private static IMatcher _matcherBoard;

		private static IMatcher _matcherBomb;

		private static IMatcher _matcherBonus;

		private static IMatcher _matcherBoxCollider2D;

		private static IMatcher _matcherBox;

		private static IMatcher _matcherChilds;

		private static IMatcher _matcherColor;

		private static IMatcher _matcherColorID;

		private static IMatcher _matcherDestroy;

		private static IMatcher _matcherEmpty;

		private static IMatcher _matcherFirstGrid;

		private static IMatcher _matcherGrid;

		private static IMatcher _matcherIndex;

		private static IMatcher _matcherLocalPosition;

		private static IMatcher _matcherOdd;

		private static IMatcher _matcherOldColorID;

		private static IMatcher _matcherParent;

		private static IMatcher _matcherPosition;

		private static IMatcher _matcherPrefabChild;

		private static IMatcher _matcherPrefab;

		private static IMatcher _matcherRigidbody2d;

		private static IMatcher _matcherScaleDirect;

		private static IMatcher _matcherScale;

		private static IMatcher _matcherSelect;

		private static IMatcher _matcherShape;

		private static IMatcher _matcherShapeShadow;

		private static IMatcher _matcherSortingLayer;

		private static IMatcher _matcherSprite;

		private static IMatcher _matcherSpriteRenderer;

		private static IMatcher _matcherTest;

		private static IMatcher _matcherText;

		private static IMatcher _matcherTransform;

		private static IMatcher _matcherType;

		private int[] _indices;

		private int[] _allOfIndices;

		private int[] _anyOfIndices;

		private int[] _noneOfIndices;

		private int _hash;

		private bool _isHashCached;

		public string[] componentNames;

		private string _toStringCache;

		public static IMatcher BoardEntities
		{
			get
			{
				if (_matcherBoardEntities == null)
				{
					Matcher matcher = (Matcher)AllOf(1);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherBoardEntities = matcher;
				}
				return _matcherBoardEntities;
			}
		}

		public static IMatcher Board
		{
			get
			{
				if (_matcherBoard == null)
				{
					Matcher matcher = (Matcher)AllOf(default(int));
					matcher.componentNames = ComponentIds.componentNames;
					_matcherBoard = matcher;
				}
				return _matcherBoard;
			}
		}

		public static IMatcher Bomb
		{
			get
			{
				if (_matcherBomb == null)
				{
					Matcher matcher = (Matcher)AllOf(2);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherBomb = matcher;
				}
				return _matcherBomb;
			}
		}

		public static IMatcher Bonus
		{
			get
			{
				if (_matcherBonus == null)
				{
					Matcher matcher = (Matcher)AllOf(3);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherBonus = matcher;
				}
				return _matcherBonus;
			}
		}

		public static IMatcher BoxCollider2D
		{
			get
			{
				if (_matcherBoxCollider2D == null)
				{
					Matcher matcher = (Matcher)AllOf(5);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherBoxCollider2D = matcher;
				}
				return _matcherBoxCollider2D;
			}
		}

		public static IMatcher Box
		{
			get
			{
				if (_matcherBox == null)
				{
					Matcher matcher = (Matcher)AllOf(4);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherBox = matcher;
				}
				return _matcherBox;
			}
		}

		public static IMatcher Childs
		{
			get
			{
				if (_matcherChilds == null)
				{
					Matcher matcher = (Matcher)AllOf(6);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherChilds = matcher;
				}
				return _matcherChilds;
			}
		}

		public static IMatcher Color
		{
			get
			{
				if (_matcherColor == null)
				{
					Matcher matcher = (Matcher)AllOf(7);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherColor = matcher;
				}
				return _matcherColor;
			}
		}

		public static IMatcher ColorID
		{
			get
			{
				if (_matcherColorID == null)
				{
					Matcher matcher = (Matcher)AllOf(8);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherColorID = matcher;
				}
				return _matcherColorID;
			}
		}

		public static IMatcher Destroy
		{
			get
			{
				if (_matcherDestroy == null)
				{
					Matcher matcher = (Matcher)AllOf(9);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherDestroy = matcher;
				}
				return _matcherDestroy;
			}
		}

		public static IMatcher Empty
		{
			get
			{
				if (_matcherEmpty == null)
				{
					Matcher matcher = (Matcher)AllOf(10);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherEmpty = matcher;
				}
				return _matcherEmpty;
			}
		}

		public static IMatcher FirstGrid
		{
			get
			{
				if (_matcherFirstGrid == null)
				{
					Matcher matcher = (Matcher)AllOf(11);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherFirstGrid = matcher;
				}
				return _matcherFirstGrid;
			}
		}

		public static IMatcher Grid
		{
			get
			{
				if (_matcherGrid == null)
				{
					Matcher matcher = (Matcher)AllOf(12);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherGrid = matcher;
				}
				return _matcherGrid;
			}
		}

		public static IMatcher Index
		{
			get
			{
				if (_matcherIndex == null)
				{
					Matcher matcher = (Matcher)AllOf(13);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherIndex = matcher;
				}
				return _matcherIndex;
			}
		}

		public static IMatcher LocalPosition
		{
			get
			{
				if (_matcherLocalPosition == null)
				{
					Matcher matcher = (Matcher)AllOf(14);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherLocalPosition = matcher;
				}
				return _matcherLocalPosition;
			}
		}

		public static IMatcher Odd
		{
			get
			{
				if (_matcherOdd == null)
				{
					Matcher matcher = (Matcher)AllOf(15);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherOdd = matcher;
				}
				return _matcherOdd;
			}
		}

		public static IMatcher OldColorID
		{
			get
			{
				if (_matcherOldColorID == null)
				{
					Matcher matcher = (Matcher)AllOf(16);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherOldColorID = matcher;
				}
				return _matcherOldColorID;
			}
		}

		public static IMatcher Parent
		{
			get
			{
				if (_matcherParent == null)
				{
					Matcher matcher = (Matcher)AllOf(17);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherParent = matcher;
				}
				return _matcherParent;
			}
		}

		public static IMatcher Position
		{
			get
			{
				if (_matcherPosition == null)
				{
					Matcher matcher = (Matcher)AllOf(18);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherPosition = matcher;
				}
				return _matcherPosition;
			}
		}

		public static IMatcher PrefabChild
		{
			get
			{
				if (_matcherPrefabChild == null)
				{
					Matcher matcher = (Matcher)AllOf(20);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherPrefabChild = matcher;
				}
				return _matcherPrefabChild;
			}
		}

		public static IMatcher Prefab
		{
			get
			{
				if (_matcherPrefab == null)
				{
					Matcher matcher = (Matcher)AllOf(19);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherPrefab = matcher;
				}
				return _matcherPrefab;
			}
		}

		public static IMatcher Rigidbody2d
		{
			get
			{
				if (_matcherRigidbody2d == null)
				{
					Matcher matcher = (Matcher)AllOf(21);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherRigidbody2d = matcher;
				}
				return _matcherRigidbody2d;
			}
		}

		public static IMatcher ScaleDirect
		{
			get
			{
				if (_matcherScaleDirect == null)
				{
					Matcher matcher = (Matcher)AllOf(23);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherScaleDirect = matcher;
				}
				return _matcherScaleDirect;
			}
		}

		public static IMatcher Scale
		{
			get
			{
				if (_matcherScale == null)
				{
					Matcher matcher = (Matcher)AllOf(22);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherScale = matcher;
				}
				return _matcherScale;
			}
		}

		public static IMatcher Select
		{
			get
			{
				if (_matcherSelect == null)
				{
					Matcher matcher = (Matcher)AllOf(24);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherSelect = matcher;
				}
				return _matcherSelect;
			}
		}

		public static IMatcher Shape
		{
			get
			{
				if (_matcherShape == null)
				{
					Matcher matcher = (Matcher)AllOf(25);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherShape = matcher;
				}
				return _matcherShape;
			}
		}

		public static IMatcher ShapeShadow
		{
			get
			{
				if (_matcherShapeShadow == null)
				{
					Matcher matcher = (Matcher)AllOf(26);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherShapeShadow = matcher;
				}
				return _matcherShapeShadow;
			}
		}

		public static IMatcher SortingLayer
		{
			get
			{
				if (_matcherSortingLayer == null)
				{
					Matcher matcher = (Matcher)AllOf(27);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherSortingLayer = matcher;
				}
				return _matcherSortingLayer;
			}
		}

		public static IMatcher Sprite
		{
			get
			{
				if (_matcherSprite == null)
				{
					Matcher matcher = (Matcher)AllOf(28);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherSprite = matcher;
				}
				return _matcherSprite;
			}
		}

		public static IMatcher SpriteRenderer
		{
			get
			{
				if (_matcherSpriteRenderer == null)
				{
					Matcher matcher = (Matcher)AllOf(29);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherSpriteRenderer = matcher;
				}
				return _matcherSpriteRenderer;
			}
		}

		public static IMatcher Test
		{
			get
			{
				if (_matcherTest == null)
				{
					Matcher matcher = (Matcher)AllOf(30);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherTest = matcher;
				}
				return _matcherTest;
			}
		}

		public static IMatcher Text
		{
			get
			{
				if (_matcherText == null)
				{
					Matcher matcher = (Matcher)AllOf(31);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherText = matcher;
				}
				return _matcherText;
			}
		}

		public static IMatcher Transform
		{
			get
			{
				if (_matcherTransform == null)
				{
					Matcher matcher = (Matcher)AllOf(32);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherTransform = matcher;
				}
				return _matcherTransform;
			}
		}

		public static IMatcher Type
		{
			get
			{
				if (_matcherType == null)
				{
					Matcher matcher = (Matcher)AllOf(33);
					matcher.componentNames = ComponentIds.componentNames;
					_matcherType = matcher;
				}
				return _matcherType;
			}
		}

		public int[] indices
		{
			get
			{
				if (_indices == null)
				{
					_indices = mergeIndices();
				}
				return _indices;
			}
		}

		public int[] allOfIndices => _allOfIndices;

		public int[] anyOfIndices => _anyOfIndices;

		public int[] noneOfIndices => _noneOfIndices;

		private Matcher()
		{
		}

		IAnyOfMatcher IAllOfMatcher.AnyOf(params int[] indices)
		{
			_anyOfIndices = distinctIndices(indices);
			_indices = null;
			return this;
		}

		IAnyOfMatcher IAllOfMatcher.AnyOf(params IMatcher[] matchers)
		{
			return ((IAllOfMatcher)this).AnyOf(mergeIndices(matchers));
		}

		public INoneOfMatcher NoneOf(params int[] indices)
		{
			_noneOfIndices = distinctIndices(indices);
			_indices = null;
			return this;
		}

		public INoneOfMatcher NoneOf(params IMatcher[] matchers)
		{
			return NoneOf(mergeIndices(matchers));
		}

		public bool Matches(Entity entity)
		{
			bool flag = _allOfIndices == null || entity.HasComponents(_allOfIndices);
			bool flag2 = _anyOfIndices == null || entity.HasAnyComponent(_anyOfIndices);
			bool flag3 = _noneOfIndices == null || !entity.HasAnyComponent(_noneOfIndices);
			return flag && flag2 && flag3;
		}

		private int[] mergeIndices()
		{
			int capacity = ((_allOfIndices != null) ? _allOfIndices.Length : 0) + ((_anyOfIndices != null) ? _anyOfIndices.Length : 0) + ((_noneOfIndices != null) ? _noneOfIndices.Length : 0);
			List<int> list = new List<int>(capacity);
			if (_allOfIndices != null)
			{
				list.AddRange(_allOfIndices);
			}
			if (_anyOfIndices != null)
			{
				list.AddRange(_anyOfIndices);
			}
			if (_noneOfIndices != null)
			{
				list.AddRange(_noneOfIndices);
			}
			return distinctIndices(list);
		}

		private static int[] mergeIndices(IMatcher[] matchers)
		{
			int[] array = new int[matchers.Length];
			int i = 0;
			for (int num = matchers.Length; i < num; i++)
			{
				IMatcher matcher = matchers[i];
				if (matcher.indices.Length != 1)
				{
					throw new MatcherException(matcher);
				}
				array[i] = matcher.indices[0];
			}
			return array;
		}

		private static string[] getComponentNames(IMatcher[] matchers)
		{
			int i = 0;
			for (int num = matchers.Length; i < num; i++)
			{
				Matcher matcher = matchers[i] as Matcher;
				if (matcher != null && matcher.componentNames != null)
				{
					return matcher.componentNames;
				}
			}
			return null;
		}

		private static void setComponentNames(Matcher matcher, IMatcher[] matchers)
		{
			string[] array = getComponentNames(matchers);
			if (array != null)
			{
				matcher.componentNames = array;
			}
		}

		private static int[] distinctIndices(IEnumerable<int> indices)
		{
			HashSet<int> hashSet = new HashSet<int>(indices);
			int[] array = new int[hashSet.Count];
			hashSet.CopyTo(array);
			Array.Sort(array);
			return array;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType() || obj.GetHashCode() != GetHashCode())
			{
				return false;
			}
			Matcher matcher = (Matcher)obj;
			if (!equalIndices(matcher.allOfIndices, _allOfIndices))
			{
				return false;
			}
			if (!equalIndices(matcher.anyOfIndices, _anyOfIndices))
			{
				return false;
			}
			if (!equalIndices(matcher.noneOfIndices, _noneOfIndices))
			{
				return false;
			}
			return true;
		}

		private static bool equalIndices(int[] i1, int[] i2)
		{
			if (i1 == null != (i2 == null))
			{
				return false;
			}
			if (i1 == null)
			{
				return true;
			}
			if (i1.Length != i2.Length)
			{
				return false;
			}
			int j = 0;
			for (int num = i1.Length; j < num; j++)
			{
				if (i1[j] != i2[j])
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			if (!_isHashCached)
			{
				int hashCode = GetType().GetHashCode();
				hashCode = applyHash(hashCode, _allOfIndices, 3, 53);
				hashCode = applyHash(hashCode, _anyOfIndices, 307, 367);
				hashCode = (_hash = applyHash(hashCode, _noneOfIndices, 647, 683));
				_isHashCached = true;
			}
			return _hash;
		}

		private static int applyHash(int hash, int[] indices, int i1, int i2)
		{
			if (indices != null)
			{
				int j = 0;
				for (int num = indices.Length; j < num; j++)
				{
					hash ^= indices[j] * i1;
				}
				hash ^= indices.Length * i2;
			}
			return hash;
		}

		public static IAllOfMatcher AllOf(params int[] indices)
		{
			Matcher matcher = new Matcher();
			matcher._allOfIndices = distinctIndices(indices);
			return matcher;
		}

		public static IAllOfMatcher AllOf(params IMatcher[] matchers)
		{
			Matcher matcher = (Matcher)AllOf(mergeIndices(matchers));
			setComponentNames(matcher, matchers);
			return matcher;
		}

		public static IAnyOfMatcher AnyOf(params int[] indices)
		{
			Matcher matcher = new Matcher();
			matcher._anyOfIndices = distinctIndices(indices);
			return matcher;
		}

		public static IAnyOfMatcher AnyOf(params IMatcher[] matchers)
		{
			Matcher matcher = (Matcher)AnyOf(mergeIndices(matchers));
			setComponentNames(matcher, matchers);
			return matcher;
		}

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (_allOfIndices != null)
				{
					appendIndices(stringBuilder, "AllOf", _allOfIndices, componentNames);
				}
				if (_anyOfIndices != null)
				{
					if (_allOfIndices != null)
					{
						stringBuilder.Append(".");
					}
					appendIndices(stringBuilder, "AnyOf", _anyOfIndices, componentNames);
				}
				if (_noneOfIndices != null)
				{
					appendIndices(stringBuilder, ".NoneOf", _noneOfIndices, componentNames);
				}
				_toStringCache = stringBuilder.ToString();
			}
			return _toStringCache;
		}

		private static void appendIndices(StringBuilder sb, string prefix, int[] indexArray, string[] componentNames)
		{
			sb.Append(prefix);
			sb.Append("(");
			int num = indexArray.Length - 1;
			int i = 0;
			for (int num2 = indexArray.Length; i < num2; i++)
			{
				int num3 = indexArray[i];
				if (componentNames == null)
				{
					sb.Append(num3);
				}
				else
				{
					sb.Append(componentNames[num3]);
				}
				if (i < num)
				{
					sb.Append(", ");
				}
			}
			sb.Append(")");
		}
	}
}
