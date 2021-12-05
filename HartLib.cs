using Godot;
using System;
using System.Collections.Generic;

namespace HartLib
{


    public static class Utils
    {
        public static Random rng = new Random(0);

        public static float RandomFloat(float minimum, float maximum)
            => (float)(rng.NextDouble() * (maximum - minimum) + minimum);

        public static float GetDistance(Vector2i vec1, Vector2i vec2)
            => Mathf.Sqrt(Mathf.Pow((vec2.x - vec1.x), 2) + Mathf.Pow((vec2.y - vec1.x), 2));

        public static bool CheckIfInRange(this Vector2 pos, Vector2 range)
            => (pos.x >= 0 && pos.y >= 0 && pos.x < range.x && pos.y < range.y);

        public static bool CheckIfInRange(this Vector2i pos, Vector2i range)
            => (pos.x >= 0 && pos.y >= 0 && pos.x < range.x && pos.y < range.y);

        public static Vector2 CartToIso(Vector2i pos)
            => new Vector2((pos.x - pos.y), (pos.x + pos.y) / 2);

        public static Vector2 CartToIso(Vector2i pos, Vector2i TileSize)
            => new Vector2((pos.x - pos.y) * (TileSize.x / 2), (pos.x + pos.y) * (TileSize.y / 2));

        public static Vector2i MouseToCart(Vector2 mousePos, TileMap tiles)
            => new Vector2i(tiles.WorldToMap(mousePos));

        public static float Lerp(float firstFloat, float secondFloat, float by)
            => firstFloat * (1 - by) + secondFloat * by;

        public static Vector2 Lerp(Vector2 firstVector, Vector2 secondVector, float by)
            => new Vector2(Lerp(firstVector.x, secondVector.x, by), Lerp(firstVector.y, secondVector.y, by));

        public static string ReplaceStringAt(this string str, int index, int length, string replace)
            => str.Remove(index, Math.Min(length, str.Length - index)).Insert(index, replace);

        public static string[] DivideStringIntoLines(string input)
            => input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        public static List<string> DivideStringIntoLinesList(string input)
            => new List<string>(input.Split(','));

        public static string NewLine
            => "\r\n";

        //* Godot stuff  
        public static List<string> ReadTextData(string dir)
        {
            var f = new Godot.File();
            f.Open(dir, Godot.File.ModeFlags.Read);

            string text = f.GetAsText();
            f.Close();
            var _data = text.Split(new[] { '\r', '\n' });
            var data = new List<string>();
            foreach (var line in _data)
            {
                data.Add(line);
            }
            return data;
        }
        //Func<Vector2, HashSet<Main.TileTypes>, bool> checkForBlocking = (pos, blocking) => { return blocking.Contains((Main.TileTypes)Main.mapTiles.GetCellv(pos)); };

        public static bool CheckForTileType<TileType>(Vector2i pos, HashSet<TileType> tiles, TileMap tileMap)
            => tiles.Contains((TileType)(object)tileMap.GetCellv(pos));

        public static bool CheckForTileType<TileType>(Vector2 pos, HashSet<TileType> tiles, TileMap tileMap)
            => tiles.Contains((TileType)(object)tileMap.GetCellv(pos));

        public static int Round_Nearest_Up(float n)
            => Mathf.FloorToInt(n + 0.5f);
        public static int Round_Nearest_Down(float n)
            => Mathf.CeilToInt(n - 0.5f);
        public static double Round_Nearest_Up(double n)
            => Math.Ceiling(n + 0.5f);
        public static double Round_Nearest_Down(double n)
            => Math.Floor(n - 0.5);


        public static Dictionary<Dir, Vector2i> directions = new Dictionary<Dir, Vector2i>()
        {
            {   Dir.Up, new Vector2i(0,1)                   },
            {   Dir.Down, new Vector2i(0,-1)                },
            {   Dir.Left, new Vector2i(-1,0)                },
            {   Dir.Right, new Vector2i(1,0)                },
        };

        public static Dictionary<DirDiagonal, Vector2i> directionsDiagonals = new Dictionary<DirDiagonal, Vector2i>()
        {
            {   DirDiagonal.Up, new Vector2i(0,1)           },
            {   DirDiagonal.Down, new Vector2i(0,-1)        },
            {   DirDiagonal.Left, new Vector2i(-1,0)        },
            {   DirDiagonal.Right, new Vector2i(1,0)        },
            {   DirDiagonal.UpLeft, new Vector2i(-1,1)      },
            {   DirDiagonal.UpRight, new Vector2i(1,1)      },
            {   DirDiagonal.DownLeft, new Vector2i(-1,-1)   },
            {   DirDiagonal.DownRight, new Vector2i(1,-1)   }
        };

        public static System.Array GetEnumValues<TEnum>() => Enum.GetValues(typeof(TEnum));

        public enum Axis { Horizontal, Vertical }
        public enum Dir { Up, Right, Down, Left }
        public enum DirDiagonal { Up, Right, Down, Left, UpLeft, UpRight, DownLeft, DownRight }

        public static List<Vector2i> GetAroundPos(Vector2i pos)
            => new List<Vector2i>()
                {
                    new Vector2i(1, 0),
                    new Vector2i(0, 1),
                    new Vector2i(-1, 0),
                    new Vector2i(0, -1),
                };

        public static List<Vector2i> GetAroundPosDiagonals(Vector2i pos)
            => new List<Vector2i>()
                {
                    new Vector2i(1, 0),
                    new Vector2i(1, 1),
                    new Vector2i(0, 1),
                    new Vector2i(-1, 1),
                    new Vector2i(-1, 0),
                    new Vector2i(-1, -1),
                    new Vector2i(0, -1),
                    new Vector2i(1, -1)
                };
    }
}