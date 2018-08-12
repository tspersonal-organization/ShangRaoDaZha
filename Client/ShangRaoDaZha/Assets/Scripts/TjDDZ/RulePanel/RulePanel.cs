using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulePanel : UIBase<RulePanel>
{
    public UIButton BackBtn;
    public GameObject TaoShangObj;
    public GameObject ZaiBaoObj;
    public GameObject WuTangHuObj;
    public GameObject NiuNiuObj;

    public UIButton WuTangHuBtn;
    public UIButton ZaiBaoBtn;
    public UIButton TaoShangBtn;
    public UIButton NiuNiuBtn;
    // Use this for initialization
    void Start () {
        BackBtn.onClick.Add(new EventDelegate(this.Close));
        WuTangHuBtn.onClick.Add(new EventDelegate(this.SetWuTangHuShow));
        ZaiBaoBtn.onClick.Add(new EventDelegate(SetZaiBaoShow));
        TaoShangBtn.onClick.Add(new EventDelegate(SetTaoShangShow));
        NiuNiuBtn.onClick.Add(new EventDelegate(SetNiuNiuShow));
        SetTaoShangShow();
    }

  

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// 牛牛点击
    /// </summary>
    private void SetNiuNiuShow()
    {
        NiuNiuObj.SetActive(true);
       
        TaoShangObj.SetActive(false);
        ZaiBaoObj.SetActive(false);
        WuTangHuObj.SetActive(false);
        WuTangHuBtn.transform.Find("Sprite").gameObject.SetActive(false);
        ZaiBaoBtn.transform.Find("Sprite").gameObject.SetActive(false);
        TaoShangBtn.transform.Find("Sprite").gameObject.SetActive(false);
        NiuNiuBtn.transform.Find("Sprite").gameObject.SetActive(true);
    }
    /// <summary>
    /// 设置显示
    /// </summary>
    /// <param name="index">1 无当户  2载保 3讨赏</param>
    public void SetTaoShangShow()
    {
                TaoShangObj.SetActive(true);
                ZaiBaoObj.SetActive(false);
                WuTangHuObj.SetActive(false);
                WuTangHuBtn.transform.Find("Sprite").gameObject.SetActive(false);
                ZaiBaoBtn.transform.Find("Sprite").gameObject.SetActive(false);
                TaoShangBtn.transform.Find("Sprite").gameObject.SetActive(true);

        NiuNiuObj.SetActive(false);
        NiuNiuBtn.transform.Find("Sprite").gameObject.SetActive(false);
    }

    public void SetWuTangHuShow()
    {
        TaoShangObj.SetActive(false);
        ZaiBaoObj.SetActive(false);
        WuTangHuObj.SetActive(true);
        WuTangHuBtn.transform.Find("Sprite").gameObject.SetActive(true);
        ZaiBaoBtn.transform.Find("Sprite").gameObject.SetActive(false);
        TaoShangBtn.transform.Find("Sprite").gameObject.SetActive(false);
        NiuNiuObj.SetActive(false);
        NiuNiuBtn.transform.Find("Sprite").gameObject.SetActive(false);
    }

    public void SetZaiBaoShow()
    {
        TaoShangObj.SetActive(false);
        ZaiBaoObj.SetActive(true);
        WuTangHuObj.SetActive(false);
        WuTangHuBtn.transform.Find("Sprite").gameObject.SetActive(false);
        ZaiBaoBtn.transform.Find("Sprite").gameObject.SetActive(true);
        TaoShangBtn.transform.Find("Sprite").gameObject.SetActive(false);
        NiuNiuObj.SetActive(false);
        NiuNiuBtn.transform.Find("Sprite").gameObject.SetActive(false);
    }
    private void Close()
    {
        UIManager.Instance.HideUIPanel(UIPaths.RulePanel);
    }
}
