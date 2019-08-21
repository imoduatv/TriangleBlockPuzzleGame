public class ShapeData
{
	public int[,] map;

	private int colorID;

	public int shapeId;

	public ShapeData(int[,] map, int shapeId)
	{
		this.map = map;
		this.shapeId = shapeId;
	}
}
