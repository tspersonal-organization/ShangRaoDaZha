using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (instance == null) return;
        UIManager.instance.HideUIPanel(instance.gameObject);
        instance = null;
    }
}
