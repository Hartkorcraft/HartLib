using static HartLib.Utils;
using System;
using System.Collections.Generic;

namespace HartLib
{
    //TODO It works nicely but needs refactoring 
    public class Fov
    {
        Func<Vector2i, bool> checkBlocking;

        //Todo Find quadrant by pos 
        //Todo recompute only specific quadrant
        public HashSet<Vector2i> Compute(Vector2i origin, uint rangeLimit, uint uncoverArround = 0)
        {
            HashSet<Vector2i> uncovered = UncoverAround(origin, uncoverArround);
            for (uint octant = 0; octant < 8; octant++) uncovered.UnionWith(Compute(octant, origin, rangeLimit, 1, new Slope(1, 1), new Slope(0, 1)));
            return uncovered;
        }

        public HashSet<Vector2i> Compute(Vector2i origin, uint rangeLimit, uint uncoverArround = 0, uint dir = 0)
        {
            HashSet<Vector2i> uncovered = UncoverAround(origin, uncoverArround);
            uncovered.UnionWith(Compute((8 - dir + 1) % 8, origin, rangeLimit, 1, new Slope(1, 1), new Slope(0, 1)));
            uncovered.UnionWith(Compute((8 - dir + 2) % 8, origin, rangeLimit, 1, new Slope(1, 1), new Slope(0, 1)));
            return uncovered;
        }

        public HashSet<Vector2i> UncoverAround(Vector2i pos, uint distance)
        {
            if (distance == 0) return new HashSet<Vector2i>();
            HashSet<Vector2i> uncovered = new HashSet<Vector2i>();

            for (int y = -(int)distance; y < distance + 1; y++)
            {
                for (int x = -(int)distance; x < distance + 1; x++)
                {
                    uncovered.Add(pos + new Vector2i(x, y));
                }
            }
            return uncovered;
        }

        struct Slope // represents the slope Y/X as a rational number
        {
            public Slope(uint y, uint x) { Y = y; X = x; }

            public bool Greater(uint y, uint x) { return Y * x > X * y; } // this > y/x
            public bool GreaterOrEqual(uint y, uint x) { return Y * x >= X * y; } // this >= y/x
            public bool Less(uint y, uint x) { return Y * x < X * y; } // this < y/x
            public bool LessOrEqual(uint y, uint x) { return Y * x <= X * y; } // this <= y/x

            public readonly uint X, Y;
        }

        bool BlocksLight(Vector2i pos, uint octant, Vector2i origin)
        {
            int nx = origin.x, ny = origin.y;
            switch (octant)
            {
                case 0: nx += pos.x; ny -= pos.y; break;
                case 1: nx += pos.y; ny -= pos.x; break;
                case 2: nx -= pos.y; ny -= pos.x; break;
                case 3: nx -= pos.x; ny -= pos.y; break;
                case 4: nx -= pos.x; ny += pos.y; break;
                case 5: nx -= pos.y; ny += pos.x; break;
                case 6: nx += pos.y; ny += pos.x; break;
                case 7: nx += pos.x; ny += pos.y; break;
            }
            return checkBlocking(new Vector2i(nx, ny));
        }

        Vector2i SetVisible(Vector2i pos, uint octant, Vector2i origin)
        {
            int nx = origin.x, ny = origin.y;
            switch (octant)
            {
                case 0: nx += pos.x; ny -= pos.y; break;
                case 1: nx += pos.y; ny -= pos.x; break;
                case 2: nx -= pos.y; ny -= pos.x; break;
                case 3: nx -= pos.x; ny -= pos.y; break;
                case 4: nx -= pos.x; ny += pos.y; break;
                case 5: nx -= pos.y; ny += pos.x; break;
                case 6: nx += pos.y; ny += pos.x; break;
                case 7: nx += pos.x; ny += pos.y; break;
            }

            var newPos = new Vector2i(nx, ny);
            return newPos;
        }
        HashSet<Vector2i> Compute(uint octant, Vector2i origin, uint rangeLimit, uint x, Slope top, Slope bottom)
        {
            HashSet<Vector2i> uncovered = new HashSet<Vector2i>();
            for (; x <= (uint)rangeLimit; x++)
            {
                uint topY;
                if (top.X == 1)
                {
                    topY = x;
                }
                else
                {

                    topY = ((x * 2 - 1) * top.Y + top.X) / (top.X * 2);

                    if (BlocksLight(new Vector2i(x, topY), octant, origin))
                    {
                        if (top.GreaterOrEqual(topY * 2 + 1, x * 2) && !BlocksLight(new Vector2i(x, topY + 1), octant, origin)) topY++;

                    }
                    else
                    {
                        uint ax = x * 2;
                        if (BlocksLight(new Vector2i(x + 1, topY + 1), octant, origin)) ax++;
                        if (top.Greater(topY * 2 + 1, ax)) topY++;
                    }
                }
                uint bottomY;
                if (bottom.Y == 0)
                {
                    bottomY = 0;
                }
                else
                {
                    bottomY = ((x * 2 - 1) * bottom.Y + bottom.X) / (bottom.X * 2);

                    if (bottom.GreaterOrEqual(bottomY * 2 + 1, x * 2)
                        && BlocksLight(new Vector2i(x, bottomY), octant, origin)
                        && !BlocksLight(new Vector2i(x, bottomY + 1), octant, origin))
                    {
                        bottomY++;
                    }
                }

                int wasOpaque = -1;

                for (uint y = topY; (int)y >= (int)bottomY; y--)
                {
                    if (rangeLimit < 0 || GetDistance(new Vector2i(0, 0), new Vector2i(x, y)) <= rangeLimit)
                    {
                        bool isOpaque = BlocksLight(new Vector2i(x, y), octant, origin);
                        //bool isVisible = isOpaque || ((y != topY || top.Greater(y * 4 - 1, x * 4 + 1)) && (y != bottomY || bottom.Less(y * 4 + 1, x * 4 - 1)));

                        // NOTE: if you want the algorithm to be either fully or mostly symmetrical, replace the line above with the
                        // following line (and uncomment the Slope.LessOrEqual method). the line ensures that a clear tile is visible
                        // only if there's an unobstructed line to its center. if you want it to be fully symmetrical, also remove
                        // the "isOpaque ||" part and see NOTE comments further down
                        bool isVisible = ((y != topY || top.GreaterOrEqual(y, x)) && (y != bottomY || bottom.LessOrEqual(y, x)));

                        if (isVisible)
                        { uncovered.Add(SetVisible(new Vector2i(x, y), octant, origin)); }

                        if (x != rangeLimit)
                        {
                            if (isOpaque)
                            {
                                if (wasOpaque == 0)
                                {
                                    uint nx = x * 2, ny = y * 2 + 1;
                                    //if (BlocksLight(new Vector2ui(x, y + 1), octant, origin)) nx--;
                                    if (top.Greater(ny, nx))
                                    {
                                        if (y == bottomY) { bottom = new Slope(ny, nx); break; }
                                        else Compute(octant, origin, rangeLimit, x + 1, top, new Slope(ny, nx));
                                    }
                                    else
                                    {
                                        if (y == bottomY) return uncovered;
                                    }
                                }
                                wasOpaque = 1;
                            }
                            else
                            {
                                if (wasOpaque > 0)
                                {
                                    uint nx = x * 2, ny = y * 2 + 1;
                                    //if (BlocksLight(new Vector2ui(x + 1, y + 1), octant, origin)) nx++;

                                    if (bottom.GreaterOrEqual(ny, nx)) return uncovered;
                                    top = new Slope(ny, nx);
                                }
                                wasOpaque = 0;
                            }
                        }
                    }
                }


                if (wasOpaque != 0) break;
            }
            return uncovered;
        }
        public Fov(Func<Vector2i, bool> _checkBlocking)
        {
            checkBlocking = _checkBlocking;
            // for example: Func<Vector2i, HashSet<Main.TileTypes>, bool> checkForBlocking = (pos, blocking) => { return blocking.Contains((Main.TileTypes)Main.mapTiles.GetCellv(pos )); };
        }
    }


}