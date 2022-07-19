using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class must match the structure of the JSON file that is getting parsed into the generator
/*
 * example if the Json file is structured
 * 
 *"rects": 
 * [{
 * "x": -2,
 * "y": -3,
 * "w": 5,
 * "h": 3
 * }]
 * 
 * Then the array within this class must match up with it's name and structure
 * public Rect[] rects; <---- this name needs to match exactly
 * 
 * [System.Serializable] <---- each class must have this above for the data to be able to be stored
 * public class Rect
 * {
 *       public float x;
 *       public float y;
 *       public float w;
 *       public float h;
 * }
 * This way when parsing through a JSON file unity knows where to store all the data that corresponds to each key
 * 
 * 
 */

[System.Serializable]
public class LevelData {

    public string version;
    public string title;
    public string story;
    public Rect[] rects;
    public Door[] doors;
    public Notes[] notes;
    public Columns[] columns;
    public Water[] water;

    [System.Serializable]
    public class Version
    {
        public string version;
    }
    [System.Serializable]
    public class Title
    {
        public string title;
    }
    [System.Serializable]
    public class Story
    {
        public string story;
    }

    [System.Serializable]
    public class Rect
    {
        public string name = "Rect";
        public int x;
        public int y;
        public int w;
        public int h;
        public bool rotunda = false;
    }
    [System.Serializable]
    public class Door
    {
        public float x = 0;
        public float y = 0;
        public Dir dir;
        public int type = 0;
    }
    [System.Serializable]
    public class Dir
    {
        public float x = 0;
        public float y = 0;
    }
    [System.Serializable]
    public class Notes
    {
        public string text;
        public string reference;
        public Pos pos;
    }
    [System.Serializable]
    public class Pos
    {
        public float x;
        public float y;
    }
    [System.Serializable]
    public class Columns
    {
        public float x;
        public float y;
    }
    [System.Serializable]
    public class Water
    {
        public float x;
        public float y;
    }

}
