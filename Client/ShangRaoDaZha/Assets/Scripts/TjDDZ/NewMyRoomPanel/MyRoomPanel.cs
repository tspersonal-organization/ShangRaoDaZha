using FrameworkForCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRoomPanel : UIBase<MyRoomPanel>
{

    public UIButton CloseBtn;
    public GameObject RoomInfoItem;//房间信息预制体
    public GameObject ScrollParent;//父物体预制体
    // Use this for initialization
    void Start () {
        ClientToServerMsg.Send(FrameworkForCSharp.NetWorks.Opcodes.Client_GetAgentCreateXYQPRoomList);
        CloseBtn.onClick.Add(new EventDelegate(this.ClosePanel));

        GameEventDispatcher.Instance.addEventListener(EventIndex.AngentRoomListUpdata,this.SetInfo);
    }

    protected override void OnDestroy()
    {
        GameEventDispatcher.Instance.removeEventListener(EventIndex.AngentRoomListUpdata, this.SetInfo);
        base.OnDestroy();
    }

    private void ClosePanel()
    {
        UIManager.Instance.HideUIPanel(UIPaths.MyRoomPanel);
    }

    // Update is called once per frame
    void Update () {
		
	}

    List<GameObject> ItemList = new List<GameObject>();
    /// <summary>
    /// 设置房间列表信息
    /// </summary>
    public void SetInfo()
    {
        if (ItemList.Count != 0)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                Destroy(ItemList[i]);
            }
        }
        ItemList = new List<GameObject>();
        for (int i = 0; i < GameData.AngentRoomList.Count; i++)
        {
            GameObject g = GameObject.Instantiate(RoomInfoItem, ScrollParent.transform);
            g.SetActive(true);
            g.transform.localScale = Vector3.one;
            g.transform.Find("RoomIdLabel").GetComponent<UILabel>().text ="房间号:"+ GameData.AngentRoomList[i].RoomId.ToString();


            //DateTime dt = new DateTime(1970, 1, 1).AddSeconds(GameData.AngentRoomList[i].Time);
            //g.transform.FindChild("RoomTimeLabel").GetComponent<UILabel>().text = dt.ToString();

            DateTime dt = myFunction.Instance.fromSecondsFromGameBegin(GameData.AngentRoomList[i].Time);
            g.transform.Find("RoomTimeLabel").GetComponent<UILabel>().text = dt.ToString("MM/dd/ HH:mm:ss");


            //  g.transform.FindChild("RoomTimeLabel").GetComponent<UILabel>().text =   GameData.AngentRoomList[i].Time.ToString();
            g.transform.Find("RoomRuleLabel").GetComponent<UILabel>().text = "局数:"+GameData.AngentRoomList[i].RoomRound.ToString();
            if (GameData.AngentRoomList[i].PlayerCount == 4)
            {
                g.transform.Find("PlayingSprite").gameObject.SetActive(true);
                g.transform.Find("WaitingSprite").gameObject.SetActive(false);
                g.transform.GetComponent<UISprite>().spriteName = "BG_MyRoom_playing";
                g.transform.Find("DisMisBtnSprite").gameObject.SetActive(false);
            }
            else
            {
                g.transform.Find("PlayingSprite").gameObject.SetActive(false);
                g.transform.Find("WaitingSprite").gameObject.SetActive(true);
                g.transform.GetComponent<UISprite>().spriteName = "BG_MyRoom_waiting";
            }
            for (int j = 0; j < GameData.AngentRoomList[i].HeadNames.Count; j++)
            {
                g.transform.Find("playerlist").Find("PlayerOnePanel"+j.ToString()).gameObject.SetActive(true);
                DownloadImage.Instance.Download(g.transform.Find("playerlist").Find("PlayerOnePanel" + j.ToString()).Find("HeadSprite").GetComponent<UITexture>(), GameData.AngentRoomList[i].HeadNames[j]);
                g.transform.Find("playerlist").Find("PlayerOnePanel" + j.ToString()).Find("HeadSprite").GetComponent<UITexture>().width = 84;
                g.transform.Find("playerlist").Find("PlayerOnePanel" + j.ToString()).Find("HeadSprite").GetComponent<UITexture>().height = 84;
            }

            g.transform.localPosition = new Vector3(-233+460*(i%2),108-189*(i/2),0);//设置位置
            g.transform.GetComponent<myRoomItem>().RoomId = GameData.AngentRoomList[i].RoomId;
            g.transform.GetComponent<myRoomItem>().RoomRound = GameData.AngentRoomList[i].RoomRound;

            g.transform.GetComponent<myRoomItem>().info = GameData.AngentRoomList[i];
            ItemList.Add(g);
        }

      //  Debug.LogError("设置房间列表信息" + GameData.AngentRoomList.Count);
    }
}
