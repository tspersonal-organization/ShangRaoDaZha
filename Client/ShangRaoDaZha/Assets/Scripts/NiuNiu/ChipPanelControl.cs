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

public class ChipPanelControl : MonoBehaviour {


    public static ChipPanelControl Instance;
    public UIButton ChipOneBtn;//五倍
    public UIButton ChipTwoBtn;//十倍
    public UIButton ChipThreeBtn;//十倍

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        UIEventListener.Get(ChipOneBtn.gameObject).onClick = this.Click;
        UIEventListener.Get(ChipTwoBtn.gameObject).onClick = this.Click;
        UIEventListener.Get(ChipThreeBtn.gameObject).onClick = this.Click;
    }

  public   Dictionary<byte, uint> OtherChipDic = new Dictionary<byte, uint>();//给其他人下的注
    public uint ChipChose=0;//自己下注选择
    private void Click(GameObject go)
    {
        if (!GameData.m_TableInfo.EnXianJiaMaiMA)
        {
            if (go == ChipOneBtn.gameObject)
            {
                ChipChose = GameData.m_TableInfo.CanChipList[0];
                ClientToServerMsg.SendDropChip(GameData.m_TableInfo.CanChipList[0], OtherChipDic);
            }
            else if (go == ChipTwoBtn.gameObject)
            {
                ChipChose = GameData.m_TableInfo.CanChipList[1];
                ClientToServerMsg.SendDropChip(GameData.m_TableInfo.CanChipList[1], OtherChipDic);
            }
            else if (go == ChipThreeBtn.gameObject)
            {
                ChipChose = GameData.m_TableInfo.CanChipList[2];
                ClientToServerMsg.SendDropChip(GameData.m_TableInfo.CanChipList[2], OtherChipDic);
            }
          //  NiuNiuGame.Instance.ChipPanel.SetActive(false);
        }
        else
        {

            if (go == ChipOneBtn.gameObject)
            {
                ChipChose = GameData.m_TableInfo.CanChipList[0];
                ClientToServerMsg.SendDropChip(GameData.m_TableInfo.CanChipList[0], OtherChipDic);
            }
            else if (go == ChipTwoBtn.gameObject)
            {
                ChipChose = GameData.m_TableInfo.CanChipList[1];
                ClientToServerMsg.SendDropChip(GameData.m_TableInfo.CanChipList[1], OtherChipDic);
            }
            else if (go == ChipThreeBtn.gameObject)
            {
                ChipChose = GameData.m_TableInfo.CanChipList[2];
                ClientToServerMsg.SendDropChip(GameData.m_TableInfo.CanChipList[2], OtherChipDic);
            }

           
        }

        ChipOneBtn.gameObject.SetActive(false);
        ChipTwoBtn.gameObject.SetActive(false);
        ChipThreeBtn.gameObject.SetActive(false);

    }


    /// <summary>
    /// 发送下注
    /// </summary>
    public void SendChip()
    {
        ClientToServerMsg.SendDropChip(ChipChose, OtherChipDic);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
