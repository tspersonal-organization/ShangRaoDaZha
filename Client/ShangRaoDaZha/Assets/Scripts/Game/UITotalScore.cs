using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITotalScore : UIBase<UITotalScore>
{
    public GameObject[] ItemArray;
    public GameObject btnShare;
    public GameObject btnRight;
    public Transform TranMyFrame;

    void Start ()
    {
        UIEventListener.Get(btnShare).onClick = OnClick;
        UIEventListener.Get(btnRight).onClick = OnClick;
        //for (int i = 0; i < GameData.m_PlayerInfoList.Count; i++)
        //{
        //    PlayerInfo info = GameData.m_PlayerInfoList[i];
        //    GameObject obj = ItemArray[info.pos - 1];
        //    obj.SetActive(true);
        //    obj.transform.Find("name").GetComponent<UILabel>().text = info.name;
        //    obj.transform.Find("score").GetComponent<UILabel>().text = info.score.ToString();
        //    DownloadImage.Instance.Download(obj.transform.Find("headImage").GetComponent<UITexture>(),info.headID);
        //}

        //ItemArray[0].transform.parent.GetComponent<UIGrid>().enabled = true;
        //TranMyFrame.parent = ItemArray[GameDataFunc.GetPlayerInfo(Player.Instance.guid).pos - 1].transform;
        //TranMyFrame.localPosition = Vector3.zero;
    }

    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        if (go == btnShare)
        {
            AuthorizeOrShare.Instance.ShareCapture();
        }
        else if (go == btnRight)
        {
            ManagerScene.Instance.LoadScene(SceneType.Main);
        }
    }
}
