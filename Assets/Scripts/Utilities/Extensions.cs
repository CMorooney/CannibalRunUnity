using UnityEngine;

public static class Extensions
{
    public static Vector3 WithZ(this Vector3 v3, float z = 0) => new Vector3(v3.x, v3.y, 0);

    public static bool CompareTagsOR(this Collider2D collider, params string[] tags)
    {
        foreach(var t in tags)
        {
            if(collider.CompareTag(t))
            {
                return true;
            }
        }

        return false;
    }

    public static bool CompareTagsAND(this Collider2D collider, params string[] tags)
    {
        foreach(var t in tags)
        {
            if(collider.CompareTag(t) == false)
            {
                return false;
            }
        }

        return true;
    }
}

