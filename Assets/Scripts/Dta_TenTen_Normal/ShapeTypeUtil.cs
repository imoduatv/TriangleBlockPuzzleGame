using System;

namespace Dta.TenTen.Normal
{
	public class ShapeTypeUtil
	{
		private static ShapeType[] VALUES;

		public static ShapeType[] GetValues()
		{
			if (VALUES == null)
			{
				VALUES = (Enum.GetValues(typeof(ShapeType)) as ShapeType[]);
			}
			return VALUES;
		}

		public static ShapeType GetRandom()
		{
			return GetValues()[MathUtils.Random(0, GetValues().Length - 1)];
		}

		public static int[,] GetShape(ShapeType shape)
		{
			switch (shape)
			{
			case ShapeType.SQUARE:
				return new int[2, 2]
				{
					{
						1,
						1
					},
					{
						1,
						1
					}
				};
			case ShapeType.HORIZONTAL2:
				return new int[1, 2]
				{
					{
						1,
						1
					}
				};
			case ShapeType.HORIZONTAL3:
				return new int[1, 3]
				{
					{
						1,
						1,
						1
					}
				};
			case ShapeType.HORIZONTAL4:
			{
				int[,] obj = new int[1, 4]
				{
					{
						1,
						1,
						1,
						1
					}
				};
				int[,] array = obj;
				return obj;
			}
			case ShapeType.HORIZONTAL5:
				return new int[1, 5]
				{
					{
						1,
						1,
						1,
						1,
						1
					}
				};
			case ShapeType.SINGLE:
				return new int[1, 1]
				{
					{
						1
					}
				};
			case ShapeType.VERTICAL2:
				return new int[2, 1]
				{
					{
						1
					},
					{
						1
					}
				};
			case ShapeType.VERTICAL3:
				return new int[3, 1]
				{
					{
						1
					},
					{
						1
					},
					{
						1
					}
				};
			case ShapeType.VERTICAL4:
				return new int[4, 1]
				{
					{
						1
					},
					{
						1
					},
					{
						1
					},
					{
						1
					}
				};
			case ShapeType.VERTICAL5:
				return new int[5, 1]
				{
					{
						1
					},
					{
						1
					},
					{
						1
					},
					{
						1
					},
					{
						1
					}
				};
			case ShapeType.LEFT_L5:
				return new int[3, 3]
				{
					{
						1,
						1,
						1
					},
					{
						1,
						0,
						0
					},
					{
						1,
						0,
						0
					}
				};
			case ShapeType.RIGHT_L5:
				return new int[3, 3]
				{
					{
						0,
						0,
						1
					},
					{
						0,
						0,
						1
					},
					{
						1,
						1,
						1
					}
				};
			case ShapeType.UP_L5:
				return new int[3, 3]
				{
					{
						1,
						0,
						0
					},
					{
						1,
						0,
						0
					},
					{
						1,
						1,
						1
					}
				};
			case ShapeType.DOWN_L5:
				return new int[3, 3]
				{
					{
						1,
						1,
						1
					},
					{
						0,
						0,
						1
					},
					{
						0,
						0,
						1
					}
				};
			case ShapeType.RIGHT_L3:
				return new int[2, 2]
				{
					{
						0,
						1
					},
					{
						1,
						1
					}
				};
			case ShapeType.UP_L3:
				return new int[2, 2]
				{
					{
						1,
						1
					},
					{
						0,
						1
					}
				};
			case ShapeType.LEFT_L3:
				return new int[2, 2]
				{
					{
						1,
						1
					},
					{
						1,
						0
					}
				};
			case ShapeType.DOWN_L3:
				return new int[2, 2]
				{
					{
						1,
						0
					},
					{
						1,
						1
					}
				};
			case ShapeType.THREEXTHREE:
				return new int[3, 3]
				{
					{
						1,
						1,
						1
					},
					{
						1,
						1,
						1
					},
					{
						1,
						1,
						1
					}
				};
			default:
				return null;
			}
		}
	}
}
