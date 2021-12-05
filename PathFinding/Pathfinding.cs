using System;
using System.Collections.Generic;
using static System.Math;

namespace HartLib.PathFinding
{
    public class PathFinding
    {
        Vector2i gridSize;
        PathCell[,] grid;

        /// <summary>
        /// <para> checkBlocking - choose what blocks movement including walls, enemies etc </para> 
        /// <para> getNeigbours - choose neigbouring tiles, may include teleports, stairs etc (and for example hexagonal maps) </para> 
        /// <para> getTileCost - additional cost to neigbour (default to 0) </para> 
        /// <para> Example of use: var path = pathfinding.FindPath(GridPos, TargetPos, (GridPos) => MapTiles.GetCellv(GridPos) == blockingTile, pathfinding.GetNeigboursDiagonal, (GridPos) => 0) </para> 
        /// </summary>
        public List<PathCell> FindPath(Vector2i startPos, Vector2i endPos, Func<Vector2i, bool> checkBlocking, Func<PathCell, List<PathCell>> getNeigbours, Func<Vector2i, int> getTileCost, bool checkLast = true)
        {
            int safeCheck = 3000;

            if (startPos.CheckIfInRange(gridSize) is false || endPos.CheckIfInRange(gridSize) is false)
                return new List<PathCell>();

            PathCell startCell = grid[startPos.x, startPos.y];
            PathCell endCell = grid[endPos.x, endPos.y];

            List<PathCell> openSet = new List<PathCell>();
            HashSet<PathCell> closedSet = new HashSet<PathCell>();
            openSet.Add(startCell);

            while (openSet.Count > 0 && safeCheck-- > 0)
            {
                PathCell curCell = openSet[0];

                // Check if there is a cell with lower F 
                for (int i = 1; i < openSet.Count; i++)
                    if (openSet[i].GetFCost() < curCell.GetFCost() || openSet[i].GetFCost() == curCell.GetFCost() && openSet[i].H < curCell.H)
                        curCell = openSet[i];


                openSet.Remove(curCell);
                closedSet.Add(curCell);

                // End Searching earlier
                if (curCell == endCell)
                {
                    if (checkLast && curCell.CheckForCollision(checkBlocking)) { return new List<PathCell>(); }
                    return RetracePath(startCell, endCell);
                }

                List<PathCell> neigbours = getNeigbours.Invoke(curCell);

                foreach (var neigbour in neigbours)
                {
                    if (closedSet.Contains(neigbour) || neigbour.CheckForCollision(checkBlocking))
                        continue;

                    var neigbhourPos = neigbour.GridPos;
                    int newCostToNeighbour = curCell.G + GetDistanceSquareGrid(curCell, neigbour);

                    if (newCostToNeighbour < neigbour.G || openSet.Contains(neigbour) is false)
                    {
                        neigbour.G = newCostToNeighbour;
                        neigbour.H = GetDistanceSquareGrid(neigbour, endCell);
                        neigbour.Parent = curCell;

                        if (openSet.Contains(neigbour) == false)
                            openSet.Add(neigbour);
                    }
                }

            }
            return new List<PathCell>();
        }

        public List<PathCell> RetracePath(PathCell startNode, PathCell endNode)
        {
            List<PathCell> path = new List<PathCell>();
            PathCell curCell = endNode;

            while (curCell != startNode)
            {
                if (curCell.Parent is null) { return new List<PathCell>(); }
                path.Add(curCell);
                curCell = curCell.Parent;
            }

            path.Reverse();
            return path;
        }

        public List<PathCell> GetNeigbours(PathCell cell)
        {
            List<PathCell> neigbours = new List<PathCell>();
            foreach (var dir in Utils.directions)
            {
                var neigbhourPos = cell.GridPos + dir.Value;
                if (neigbhourPos.CheckIfInRange(gridSize) && grid[neigbhourPos.x, neigbhourPos.y] != null)
                    neigbours.Add(grid[neigbhourPos.x, neigbhourPos.y]);
            }
            return neigbours;
        }

        public List<PathCell> GetNeigboursDiagonal(PathCell cell)
        {
            List<PathCell> neigbours = new List<PathCell>();
            foreach (var dir in Utils.directionsDiagonals)
            {
                var neigbhourPos = cell.GridPos + dir.Value;
                if (neigbhourPos.CheckIfInRange(gridSize) && grid[neigbhourPos.x, neigbhourPos.y] != null)
                    neigbours.Add(grid[neigbhourPos.x, neigbhourPos.y]);
            }
            return neigbours;
        }

        static public int GetDistanceSquareGrid(PathCell cellA, PathCell cellB)
        {
            int distantX = Math.Abs(cellA.GridPos.x - cellB.GridPos.x);
            int distantY = Math.Abs(cellA.GridPos.y - cellB.GridPos.y);

            if (distantX > distantY)
                return 14 * distantY + 10 * (distantX - distantY);
            else return 14 * distantX + 10 * (distantY - distantX);
        }

        public PathFinding(Vector2i _gridSize)
        {
            gridSize = _gridSize;

            grid = new PathCell[gridSize.x, gridSize.y];

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    grid[x, y] = new PathCell(new Vector2i(x, y));
                }
            }
        }
    }
}