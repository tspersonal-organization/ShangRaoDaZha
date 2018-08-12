using UnityEngine;

public class DDOLSingleton<T> : MonoBehaviour where T : DDOLSingleton<T>
{
    protected static T _Instance = null;

    public static T Instance
    {
        get
        {
            if(_Instance == null)
            {
                GameObject go = GameObject.Find("DDOLSingleton");
                if(go == null)
                {
                    go = new GameObject("DDOLSingleton");
                    DontDestroyOnLoad(go);
                }
                _Instance = go.AddComponent<T>();
            }
            return _Instance;
        }
    }

    void OnApplicationQuit()
    {
        _Instance = null;
    }
}
