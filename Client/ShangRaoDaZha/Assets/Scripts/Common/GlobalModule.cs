using System.Collections.Generic;
using UnityEngine;

public class GlobalModule : MonoBehaviour
{
    [SerializeField] private Transform _goBubblingBase; //提示框的父对象
    [SerializeField] private GameObject _goBubbling; //提示框
    [SerializeField] private List<GameObject> _listBubbling = new List<GameObject>();

    private float _fLeaveTime = 0;
    private float _fDelayTime = 3;
    private float _fHeightForBubblingHint = 62; //提示框的高度
    private float _fMoveY; //提示框移动的Y轴距离
    private float _StayTime = 2;//停留的时间

    private static GlobalModule _instance = null;

    public static GlobalModule Instance
    {
        get { return _instance; }
    }

    private void Start()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        _fHeightForBubblingHint = _goBubbling.GetComponent<UIWidget>().localSize.y;
        _fMoveY = _fHeightForBubblingHint + 5f;

        //为其赋值
        for (int i = 0; i < _goBubblingBase.childCount; i++)
        {
            GameObject go = _goBubblingBase.GetChild(i).gameObject;
            _listBubbling.Add(go);
            go.SetActive(false);
        }
    }

    //打开冒泡提示框
    public void OnOpenBubblingHint(string sContent)
    {
        GameObject go = null;
        //循环一次，看看是否有未使用的提示框
        for (int i = 0; i < _listBubbling.Count; i++)
        {
            if (!_listBubbling[i].activeInHierarchy)
            {
                go = _listBubbling[i];
                break;
            }
        }
        if (go == null)
        {
            go = _listBubbling[_listBubbling.Count - 1];
        }

        _listBubbling.Remove(go);
        iTween.Stop(go);
        go.SetActive(false);
        go.transform.localPosition = Vector3.zero;
        go.SetActive(true);
        _listBubbling.Insert(0, go);

        UILabel lab = go.transform.GetChild(0).GetComponent<UILabel>();
        lab.text = sContent;
        go.GetComponent<UISprite>().width = lab.width + 100;
        //播放透明度动画
        SetBubblingAlphaTween(go);
        //播放移动动画
        for (int i = 0; i < _listBubbling.Count; i++)
        {
            if (_listBubbling[i].activeInHierarchy)
                SetBubblingMoveTween(_listBubbling[i], i);
        }

    }

    //设置冒泡框的移动动画
    private void SetBubblingMoveTween(GameObject go, int nIndex)
    {
        //            Hashtable hash = new Hashtable();
        //            hash.Add("x", 0);
        //            hash.Add("y", fOffset);
        //            hash.Add("z", 0);
        //            hash.Add("time", 0.35);
        //            iTween.MoveTo(go, hash);
        //            Log.Debug("移动前位置为::" + go.transform.localPosition);
        //            Log.Debug("移动后位置为:" + nIndex * _fMoveY);
        go.GetComponent<TweenPosition>().ResetToBeginning();
        //            go.GetComponent<TweenPosition>().from = go.transform.localPosition;
        float fOffset = ((nIndex - 1) >= 0 ? nIndex - 1 : 0) * _fMoveY;
        go.GetComponent<TweenPosition>().from = new Vector3(0, fOffset, 0);
        go.GetComponent<TweenPosition>().to = new Vector3(0, nIndex * _fMoveY, 0);
        go.GetComponent<TweenPosition>().duration = 0.35f;
        go.GetComponent<TweenPosition>().PlayForward();
        go.GetComponent<TweenPosition>().onFinished.Clear();
        go.GetComponent<TweenPosition>().SetOnFinished(delegate
        {
            go.transform.localPosition = new Vector3(0, nIndex * _fMoveY, 0);
        });
    }

    //设置冒泡框的渐变动画
    private void SetBubblingAlphaTween(GameObject go)
    {
        //只适用于Texture
        //            Hashtable hash = new Hashtable();
        //            hash.Add("alpha", 0);
        //            hash.Add("includechildren", true);
        //            hash.Add("time", 2);
        //            hash.Add("delay", 3);
        //            hash.Add("oncomplete", "AlphaAnimation1End");
        //            hash.Add("oncompleteparams", "end");
        //            hash.Add("oncompletetarget", go);
        //            iTween.FadeTo(go, hash);
        go.GetComponent<TweenAlpha>().ResetToBeginning();
        go.GetComponent<TweenAlpha>().from = 1;
        go.GetComponent<TweenAlpha>().to = 0;
        go.GetComponent<TweenAlpha>().duration = 1;
        go.GetComponent<TweenAlpha>().delay = _StayTime;
        go.GetComponent<TweenAlpha>().PlayForward();
        go.GetComponent<TweenAlpha>().onFinished.Clear();
        go.GetComponent<TweenAlpha>().SetOnFinished(delegate
        {
            go.SetActive(false);
        });
    }

    //透明动画播放完毕
    private void AlphaAnimation1End(GameObject go)
    {
        go.SetActive(false);
    }


    //关闭冒泡提示框
    public void OnCloseBubbling()
    {
        _listBubbling.Clear();
        for (int i = 0; i < _goBubblingBase.childCount; i++)
        {
            GameObject go = _goBubblingBase.GetChild(i).gameObject;
            _listBubbling.Add(go);
            iTween.Stop(go);
            go.SetActive(false);
        }
    }
}
