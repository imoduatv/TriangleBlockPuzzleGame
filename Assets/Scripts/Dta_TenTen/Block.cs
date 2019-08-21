namespace Dta.TenTen
{
	public class Block
	{
		private int color;

		private int x;

		private int y;

		public Block()
		{
			color = -1;
			x = -1;
			y = -1;
		}

		public Block(int c)
		{
			color = c;
		}

		public Block(int c, int x, int y)
		{
			color = c;
			this.x = x;
			this.y = y;
		}

		public Block(Block block)
		{
			color = block.Color();
			x = block.GetX();
			y = block.GetY();
		}

		public int GetX()
		{
			return x;
		}

		public int GetY()
		{
			return y;
		}

		public int Color()
		{
			return color;
		}
	}
}
