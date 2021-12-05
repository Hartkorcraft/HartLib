using Godot;
using System;
using System.Collections.Generic;

public static class GodotExtensions
{
    public static void LerpOnPath(this Node2D node2D, Queue<(Vector2 pos, Action? action)> lerpPosAndActions, float smooth = 0.3f, float clip_range = 1f)
    {
        if (lerpPosAndActions.Count > 0)
        {
            var posAndAction = lerpPosAndActions.Peek();
            Vector2 new_pos = posAndAction.pos;
            node2D.Position = HartLib.Utils.Lerp(node2D.Position, new_pos, smooth);

            if (InClipRange(new_pos, clip_range))
            {
                posAndAction.action?.Invoke();
                lerpPosAndActions.Dequeue();
            }
        }

        bool InClipRange(Vector2 new_pos, float clip_range) =>
        (Mathf.Abs(node2D.Position.x - new_pos.x) <= clip_range && Mathf.Abs(node2D.Position.y - new_pos.y) <= clip_range);
    }
}
