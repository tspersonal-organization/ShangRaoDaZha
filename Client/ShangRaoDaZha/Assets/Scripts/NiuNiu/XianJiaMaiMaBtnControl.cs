/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XianJiaMaiMaBtnControl : MonoBehaviour {

    private int Pos;//对应买码的位置

    private UIButton ChipOneBtn;//五倍
    private UIButton ChipTwoBtn;//十倍
    private UIButton ChipThreeBtn;//十倍s


    private GameObject MaskBtn;//遮罩按钮
                                 // Use this for initialization
    void Start () {
        MaskBtn = transform.Find("Mask").gameObject;
        UIEventListener.Get(MaskBtn).onClick = this.OnClick;
        //MaskBtn.onClick.Add(new EventDelegate(()=>
        //{
        //    transform.FindChild("BgSprite").gameObject.SetActive(false);
        //    MaskBtn.gameObject.SetActive(false);
        //    Debug.Log("--=-=-=-=-=-=");
        //}));

        if (Pos != NiuNiuGame.Instance.SelfPos)
        {
            ChipOneBtn = transform.Find("BgSprite/Chip0").GetComponent<UIButton>();
            ChipTwoBtn = transform.Find("BgSprite/Chip1").GetComponent<UIButton>();
            ChipThreeBtn = transform.Find("BgSprite/Chip2").GetComponent<UIButton>();

            UIEventListener.Get(ChipOneBtn.gameObject).onClick = this.Click;
            UIEventListener.Get(ChipTwoBtn.gameObject).onClick = this.Click;
            UIEventListener.Get(ChipThreeBtn.gameObject).onClick = this.Click;
        }
       

        transform.GetComponent<UIButton>().onClick.Add(new EventDelegate(this.ChipOtherClick));
      

    }

    private void OnClick(GameObject go)
    {
        transform.Find("BgSprite").gameObject.SetActive(false);
        MaskBtn.gameObject.SetActive(false);
        Debug.Log("--=-=-=-=-=-=");
    }

    private void Click(GameObject go)
    {
        ChipPanelControl.Instance.OtherChipDic = new Dictionary<byte, uint>();
        if (go == ChipOneBtn.gameObject)
        {
            ChipPanelControl.Instance.OtherChipDic[(byte)Pos] = GameData.m_TableInfo.CanChipList[0];
          
        }
        else if (go == ChipTwoBtn.gameObject)
        {
            ChipPanelControl.Instance.OtherChipDic[(byte)Pos] = GameData.m_TableInfo.CanChipList[1];
           // ClientToServerMsg.SendDropChip(GameData.m_TableInfo.CanChipList[1], ChipPanelControl.Instance.OtherChipDic);
        }
        else if (go == ChipThreeBtn.gameObject)
        {
            ChipPanelControl.Instance.OtherChipDic[(byte)Pos] = GameData.m_TableInfo.CanChipList[2];
           // ClientToServerMsg.SendDropChip(GameData.m_TableInfo.CanChipList[1], ChipPanelControl.Instance.OtherChipDic);
        }
        ChipPanelControl.Instance.SendChip();
        Reset();
    }


    public  void Reset()
    {
        ChipOneBtn.gameObject.SetActive(false);
        ChipTwoBtn.gameObject.SetActive(false);
        ChipThreeBtn.gameObject.SetActive(false);
        ChipOneBtn.transform.parent.gameObject.SetActive(false);
        MaskBtn.gameObject.SetActive(false);
        ChipOneBtn.transform.localPosition = new Vector3(-135,0,0);
        ChipTwoBtn.transform.localPosition = new Vector3(5, 0, 0);
        ChipThreeBtn.transform.localPosition = new Vector3(141, 0, 0);
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 点击
    /// </summary>
    private void ChipOtherClick()
    {
        if (Pos == NiuNiuGame.Instance.SelfPos)//点击自己门前的不买
        {
            NiuNiuGame.Instance.HideChipPanel();
            return;
        }
      GameObject g=  transform.Find("BgSprite").gameObject;
        g.SetActive(true);
        MaskBtn.gameObject.SetActive(true);
        ChipOneBtn.gameObject.SetActive(false);
        ChipTwoBtn.gameObject.SetActive(false);
        ChipThreeBtn.gameObject.SetActive(false);
        ChipOneBtn.transform.localPosition = new Vector3(-135, 0, 0);
        ChipTwoBtn.transform.localPosition = new Vector3(5, 0, 0);
        ChipThreeBtn.transform.localPosition = new Vector3(141, 0, 0);
        for (int i = 0; i < GameData.m_TableInfo.CanChipList.Count; i++)//可下那些基础分
        {
          g.transform.Find("Chip" + i.ToString()).gameObject.SetActive(true);
          g. transform.Find("Chip" + i.ToString()).Find("Label").GetComponent<UILabel>().text = GameData.m_TableInfo.CanChipList[i].ToString() + "分";
        }
        if (GameData.m_TableInfo.CanChipList.Count == 2)
        {
            g.transform.Find("Chip0").localPosition = new Vector3(-82, 0, 0);
           g. transform.Find("Chip1").localPosition = new Vector3(82, 0, 0);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}


    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="pos"></param>
    public void SetValue(int pos)
    {
        this.Pos = pos;
    }
}
