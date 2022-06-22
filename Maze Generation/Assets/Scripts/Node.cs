public class Node
{
    // This Script Holds The Details For A Node
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public Node cameFromNode;

    public bool isWalkable;

    public Node(int x, int y, bool isWalkable)
    {
        this.x = x;
        this.y = y;
        hCost = 0;
        this.isWalkable = isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
