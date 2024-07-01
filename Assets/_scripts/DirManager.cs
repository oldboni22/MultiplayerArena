using BonBon;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class DirManager
{
    private readonly static Dictionary<float,CharDir> _moveToDirX = new Dictionary<float, CharDir>() 
    { 
        {-1,CharDir.Left },
        {1,CharDir.Right}
    };
    private readonly static Dictionary<float,CharDir> _moveToDirY = new Dictionary<float, CharDir>() 
    { 
        {-1,CharDir.Down },
        {1,CharDir.Up}
    };


    public static CharDir GetDirX(float x)
    {
        Debug.Log(x);
        return _moveToDirX.GetValueOrDefault(x);
    }
    
    public static CharDir GetDirY(float y)
    {
        return _moveToDirY.GetValueOrDefault(y);
    }
    public static CharDir GetDir(Vector2 vector)
    {
        if (vector.x != 0)
            return _moveToDirX.GetValueOrDefault(vector.x);
        else
            return _moveToDirY.GetValueOrDefault(vector.y);
    }
}
