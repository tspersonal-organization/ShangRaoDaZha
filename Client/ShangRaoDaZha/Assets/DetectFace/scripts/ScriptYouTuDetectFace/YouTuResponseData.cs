using System;
using UnityEngine;

    [Serializable]
    public class YouTuResponseData
    {
        public int session_id;
        public int image_height;
        public int image_width;
        public FaceObject[] face;
        public int errorcode;
        public string errormsg;

        public static YouTuResponseData fromJson(string aJson)
        {
            return JsonUtility.FromJson<YouTuResponseData>(aJson);
        }
 
    }
    
    [Serializable]
    public class FaceObject
    {
        public int age;
        public int beauty;
        public int expression;
    }
