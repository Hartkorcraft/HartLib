using Godot;
using System;

namespace HartLib
{
    //TODO Add override Equals and GetHashCode
    public struct Vector2i
    {
        public int x { get; set; }
        public int y { get; set; }

        public static Vector2i Zero = new Vector2i(0, 0);
        public static Vector2i Up = new Vector2i(0, 1);
        public static Vector2i Down = new Vector2i(0, -1);
        public static Vector2i Left = new Vector2i(-1, 0);
        public static Vector2i Right = new Vector2i(1, 0);


        public override string ToString() => $"({x}, {y})";
        public int AddValues => x + y;
        public float XoverY => (float)x / y;

        public static implicit operator Vector2(Vector2i v) => new Vector2(v.x, v.y);

        public Vector2ui Vec2ui() => new Vector2ui(Math.Abs(x), Math.Abs(y));

        public static bool operator ==(Vector2i vec1, Vector2i vec2) => vec1.x == vec2.x && vec1.y == vec2.y;
        public static bool operator !=(Vector2i vec1, Vector2i vec2) => !(vec1 == vec2);
        public static bool operator ==(Vector2i vec1, Vector2 vec2) => vec1 == new Vector2i(vec2);
        public static bool operator !=(Vector2i vec1, Vector2 vec2) => !(vec1 == vec2);

        public static Vector2i operator *(Vector2i vec1, int value) => new Vector2i(vec1.x * value, vec1.y * value);
        public static Vector2i operator *(Vector2i vec1, uint value) => vec1 * (int)value;
        public static Vector2i operator *(Vector2i vec1, Vector2i vec2) => new Vector2i(vec1.x * vec2.x, vec1.y * vec2.y);
        public static Vector2i operator /(Vector2i vec1, int value) => new Vector2i(vec1.x / value, vec1.y / value);

        public static Vector2i operator -(Vector2i vec1, Vector2i vec2) => new Vector2i(vec1.x - vec2.x, vec1.y - vec2.y);
        public static Vector2i operator +(Vector2i vec1, Vector2i vec2) => new Vector2i(vec1.x + vec2.x, vec1.y + vec2.y);

        public Vector2i(int _x, int _y) => (x, y) = (_x, _y);
        public Vector2i(Vector2 vec) => (x, y) = ((int)vec.x, (int)vec.y);
        public Vector2i(Vector2i vec) => (x, y) = (vec.x, vec.y);
        public Vector2i(Vector2ui vec) => (x, y) = ((int)vec.x, (int)vec.y);
        public Vector2i(uint _x, uint _y) => (x, y) = (Math.Abs((int)_x), Math.Abs((int)_y));

    }

    public struct Vector2ui //TODO 
    {
        public uint x { get; set; }
        public uint y { get; set; }

        public static Vector2ui Zero = new Vector2ui(0, 0);

        public override string ToString() => $"({x}, {y})";
        public uint AddValues => x + y;

        //To Vector2
        public Vector2 Vec2() => new Vector2(x, y);
        public Vector2i Vec2i() => new Vector2i(x, y);


        public static bool operator ==(Vector2ui vec1, Vector2ui vec2) => (vec1.x == vec2.x && vec1.y == vec2.y);
        public static bool operator !=(Vector2ui vec1, Vector2ui vec2) => !(vec1 == vec2);
        public static bool operator ==(Vector2ui vec1, Vector2 vec2) => (vec1.x == (int)vec2.x && vec1.y == (int)vec2.y);
        public static bool operator !=(Vector2ui vec1, Vector2 vec2) => !(vec1 == vec2);

        public static Vector2i operator *(Vector2ui vec1, int value) => new Vector2i((int)vec1.x * value, (int)vec1.y * value);
        public static Vector2i operator *(Vector2ui vec1, Vector2i vec2) => new Vector2i((int)vec1.x * vec2.x, (int)vec1.y * vec2.y);
        public static Vector2i operator /(Vector2ui vec1, int value) => new Vector2i((int)vec1.x / value, (int)vec1.y / value);

        public static Vector2i operator -(Vector2ui vec1, Vector2i vec2) => new Vector2i((int)vec1.x - vec2.x, (int)vec1.y - vec2.y);
        public static Vector2ui operator +(Vector2ui vec1, Vector2ui vec2) => new Vector2ui(vec1.x + vec2.x, vec1.y + vec2.y);


        public Vector2ui(uint _x, uint _y) => (x, y) = (_x, _y);
        public Vector2ui(int _x, int _y)
        {
            x = (uint)Math.Abs(_x);
            y = (uint)Math.Abs(_y);
        }
        public Vector2ui(Vector2 vec)
        {
            x = (uint)Math.Abs(vec.x);
            y = (uint)Math.Abs(vec.y);
        }
        public Vector2ui(Vector2ui vec)
        {
            x = vec.x;
            y = vec.y;
        }
        public Vector2ui(Vector2i vec)
        {
            x = (uint)Math.Abs(vec.x);
            y = (uint)Math.Abs(vec.y);
        }
    }


}