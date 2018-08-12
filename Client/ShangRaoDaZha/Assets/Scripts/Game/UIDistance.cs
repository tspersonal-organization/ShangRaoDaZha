using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkForCSharp.Utils;

public class UIDistance : UIBase<UIDistance>
{

	// Use this for initialization
	void Start ()
    {
        UIEventListener.Get(transform.Find("MB").gameObject).onClick = OnClick;
        UILabel lb = transform.Find("Base").Find("desc").GetComponent<UILabel>();
        if(GameData.m_PlayerInfoList.Count > 1)
        {
            string desc = "";
            for (int i = 0; i < GameData.m_PlayerInfoList.Count - 1; i++)
            {
                PlayerInfo targetInfo = GameData.m_PlayerInfoList[i];
                for (int k = i+1; k < GameData.m_PlayerInfoList.Count; k++)
                {
                    PlayerInfo info = GameData.m_PlayerInfoList[k];
                    float dis = (float)ToolsFuncElse.Distance(targetInfo.N, targetInfo.E, info.N, info.E);
                    desc += "[ffff00]"+targetInfo.name + "[-] 距离 [ffff00]" + info.name + "[-] [ff0000]" + dis +"[-] 千米 \n";
                }
            }
            lb.text = desc;
        }
	}
    void OnClick(GameObject go)
    {
        UIManager.Instance.HideUIPanel(UIPaths.UIPanel_Distance);
    }
}
