using System;

namespace HartLib.PathFinding
{
    public class PathCell
    {
        public Vector2i GridPos { get; set; }
        public PathCell? Parent { get; set; }
        public int G { get; set; } // Estimated d istance To Start Node
        public int H { get; set; } // Estimated distance To End Node
        int F => G + H;
        
        public int GetFCost(Func<int>? getTileCost = null) => F + getTileCost?.Invoke() ?? 0;
        public bool CheckForCollision(Func<Vector2i, bool> checkBlocking) => checkBlocking.Invoke(GridPos);

        public PathCell(Vector2i gridPos) => this.GridPos = gridPos;
    }
}