public struct Vector2Int
{
    public int x, y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("[{0},{1}]", x, y);
    }
}