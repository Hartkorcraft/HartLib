using Godot;
using HartLib;
using System.Collections.Generic;

//* For finding things on map 
public static class MapFinding
{
    public static List<T> GetFromGridPos<T>(Vector2i _pos, World2D world2d, uint mask = 2147483647)
    {
        var colliders = GetColliderDictsFromGridPos(_pos, world2d, mask);
        var found = new List<T>();

        foreach (var collider in colliders)
        {
            if (collider["collider"] is Area2D && ((Area2D)collider["collider"]).GetParent() is T)
            {
                var objectFound = (T)(object)((Area2D)collider["collider"]).GetParent(); //Godot bug forces casting 
                found.Add(objectFound);
            }
        }
        return found;
    }

    public static bool CheckForCollisionOnGridPos(Vector2i _pos, World2D world2d, uint tileSize, uint mask = 2147483647)
    {
        var pos = (_pos * tileSize) + new Vector2((float)tileSize / 2, (float)tileSize / 2);

        Godot.Collections.Array result = world2d.DirectSpaceState.IntersectPoint(pos, 32, null, mask, true, true) ?? new Godot.Collections.Array();
        return result.Count > 0;
    }

    private static List<Godot.Collections.Dictionary> GetColliderDictsFromGridPos(Vector2i _pos, World2D world2d, uint tileSize, uint mask = 2147483647)
    {
        var pos = (_pos * tileSize) + new Vector2((float)tileSize / 2, (float)tileSize / 2);

        Godot.Collections.Array result = world2d.DirectSpaceState.IntersectPoint(pos, 32, null, mask, true, true) ?? new Godot.Collections.Array();

        var list = new List<Godot.Collections.Dictionary>();
        foreach (Godot.Collections.Dictionary collider_dict in result)
        {
            list.Add(collider_dict);
        }
        return list;
    }
}
