using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OpenPanelType
{
    None,
    MinToMax,//由小变大
}

public class UIManager : UIBase<UIManager>, IResourceListener
{
    /// <summary>
    /// UI名字和面板的映射关系
    /// </summary>
    private Dictionary<string, GameObject> nameUIDict = new Dictionary<string, GameObject>();
    private Dictionary<GameObject, string> uiNameDict = new Dictionary<GameObject, string>();

    bool isHidePanel = false;//是否需要关闭一个界面
    string openUIName;
    string closeUIName;
    string baseName;
    OpenPanelType openPanelType;

    /// <summary>
    /// 显示UI  没有就创建一个
    /// </summary>
    public void ShowUiPanel(string uiName)
    {
        openPanelType = OpenPanelType.None;
        if (nameUIDict.ContainsKey(uiName))
        {
            GameObject obj = nameUIDict[uiName];
            Debug.Log(uiName);
            obj.gameObject.SetActive(true);
            //iTween.ScaleFrom(obj, Vector2.zero, 0.5f);
            return;
        }
        ResourcesManager.Instance.Load(uiName, typeof(GameObject), this);
    }
    /// <summary>
    /// 显示UI  没有就创建一个
    /// </summary>
    public void ShowUiPanel(string uiName, OpenPanelType opType, string baseName = "Base")
    {
        openPanelType = opType;
        this.baseName = baseName;
        if (nameUIDict.ContainsKey(uiName))
        {
            GameObject obj = nameUIDict[uiName];
            obj.gameObject.SetActive(true);
            if (openPanelType != OpenPanelType.None)
                StartCoroutine(Play(obj));
            return;
        }
        ResourcesManager.Instance.Load(uiName, typeof(GameObject), this);
    }
    /// <summary>
    /// 显示已个UI 并且关闭一个UI
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="closeUiName"></param>
    public void ShowUiPanel(string uiName, string closeUiName)
    {
        isHidePanel = true;
        openUIName = uiName;
        closeUIName = closeUiName;
        if (nameUIDict.ContainsKey(uiName))
        {
            GameObject obj = nameUIDict[uiName];
            obj.gameObject.SetActive(true);
            return;
        }
        ResourcesManager.Instance.Load(uiName, typeof(GameObject), this);
    }

    /// <summary>
    /// 关闭UI 并且销毁
    /// </summary>
    /// <param name="uiName"></param>
    public void HideUiPanel(string uiName)
    {
        if (!nameUIDict.ContainsKey(uiName))
            return;
        GameObject obj = nameUIDict[uiName];
        nameUIDict.Remove(uiName);
        uiNameDict.Remove(obj);
        Destroy(obj);
    }

    /// <summary>
    /// 关闭UI 并且销毁
    /// </summary>
    /// <param name="obj"></param>
    public void HideUiPanel(GameObject obj)
    {
        if (!uiNameDict.ContainsKey(obj))
            return;
        string assetName = uiNameDict[obj];
        nameUIDict.Remove(assetName);
        uiNameDict.Remove(obj);
        Destroy(obj);
    }

    /// <summary>
    /// 加载UI完成 并且创建
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="asset"></param>
    public void OnLoaded(string assetName, object asset)
    {
        if (assetName == openUIName && isHidePanel)
        {
            isHidePanel = false;
            HideUiPanel(closeUIName);
        }
        GameObject obj = Instantiate<GameObject>(asset as GameObject);
        obj.transform.parent = GameObject.Find("2DRoot").transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        if (openPanelType != OpenPanelType.None)
            StartCoroutine(Play(obj));
        nameUIDict.Add(assetName, obj);
        uiNameDict.Add(obj, assetName);
    }

    IEnumerator Play(GameObject obj)
    {
        Transform tran = obj.transform.Find(baseName);
        if (tran == null)
        {
            Debug.LogError(obj.name);
        }

        switch (openPanelType)
        {
            case OpenPanelType.None:
                break;
            case OpenPanelType.MinToMax:
                obj.transform.localScale = Vector3.one;
                if (tran != null)
                    TweenScale.Begin(tran.gameObject, 0.1f, Vector3.one);
                break;
        }
        yield break;
    }

    public void Clear()
    {
        foreach (var dic in nameUIDict)
        {
            if (dic.Value != null)
            {
                Destroy(dic.Value);
            }
        }
        nameUIDict.Clear();
        uiNameDict.Clear();
    }

    protected override void OnDestroy()
    {

    }
}
