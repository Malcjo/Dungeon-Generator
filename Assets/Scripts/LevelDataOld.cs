using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataOld
{
    public TextAsset levelJson;
    public Texture2D icon;

    public LevelDataOld(TextAsset data, Texture2D map)
    {
        levelJson = data;
        icon = map;
    }
}
