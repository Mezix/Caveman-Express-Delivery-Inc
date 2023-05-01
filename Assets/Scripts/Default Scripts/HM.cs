using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public static class HM
{
    /// <summary>
    /// Only returns opposite if both have a value and none are zero
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static bool OppositeSignsInt(int x, int y)
    {
        if (x == 0 || y == 0) return false; // only return opposite if both are different
        else return ((x ^ y) < 0);
    }/// <summary>
     /// Only returns opposite if both have a value and none are zero
     /// </summary>
     /// <param name="x"></param>
     /// <param name="y"></param>
     /// <returns></returns>
    public static bool OppositeSignsFloat(float x, float y)
    {
        int xInt = Mathf.CeilToInt(x);
        int yInt = Mathf.CeilToInt(y);

        return OppositeSignsInt(xInt, yInt);
    }
    public static void RotateTransformToAngle(Transform t, Vector3 vec)
    {
        Quaternion q = new Quaternion();
        q.eulerAngles = vec;
        t.rotation = q;
    }
    public static void RotateLocalTransformToAngle(Transform t, Vector3 vec)
    {
        Quaternion q = new Quaternion();
        q.eulerAngles = vec;
        t.localRotation = q;
    }
    public static float GetAngle2DBetween(Vector3 from, Vector3 to)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(from.y - to.y, from.x - to.x);
    }

    public static RaycastHit2D Raycast2DAtPosition(Vector3 pos, int layerMask = 0)
    {
        //make sure we arent on the same plane as our object we are trying to hit
        pos.z = 1000;
        return Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, layerMask);
    }
    public static RaycastHit RaycastAtPosition(Vector3 startingPos, Vector3 dir, float length = Mathf.Infinity, int layerMask = -1)
    {
        RaycastHit hit;
        if (layerMask == -1) Physics.Raycast(startingPos, dir, out hit, length);
        else Physics.Raycast(startingPos, dir, out hit, length, layerMask);
        return hit;
    }
    public static RaycastHit2D RaycastToMouseCursor()
    {
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);
    }
    /// <summary>
    /// Find the LayerMask by using LayerMask.GetMask("layermask name here")
    /// </summary>
    /// <param name="layerMask"> </param>
    /// <returns></returns>
    public static RaycastHit2D RaycastToMouseCursor(int layerMask)
    {
        return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
    }

    public static string FloatToString(float f, int decimals = 0)
    {
        string formatString = "0.";
        for(int i = 0; i < decimals; i++)
        {
            formatString += "#";
        }
        string output = f.ToString(formatString);
        return output;
    }

    public static bool StringToBool(string s)
    {
        s = s.ToLower();
        if (s == "true") return true;
        if (s == "false") return false;
        Debug.Log("ERROR: Not a valid bool");
        return false;
    }

    public static Sprite GetSpriteFromSpritesheet(string spriteSheetPath, string spriteName)
    {
        // Load all sprites in atlas
        Sprite[] abilityIconsAtlas = Resources.LoadAll<Sprite>(spriteSheetPath);
        // Get specific sprite
        Sprite specificSprite = abilityIconsAtlas.Single(s => s.name == spriteName);
        return specificSprite;
    }

    public static float GetFloatWithRandomVariance(float valueStart, float variance)
    {
        return UnityEngine.Random.Range(valueStart-variance, valueStart + variance);
    }
}
