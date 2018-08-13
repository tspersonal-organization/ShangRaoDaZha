using FrameworkForCSharp.NetWorks;
using UnityEngine;

public class DzPanelChat : UIBase<DzPanelChat>
{
    public GameObject btnClose;
    public UIButton SendContentBtn;
    public UIInput InputChat;
    public Transform FaceBase;
    public Transform TxtBase;
    
   
    void Start()
    {
        InputChat.onSubmit.Add(new EventDelegate(SendInputChat));
        UIEventListener.Get(btnClose).onClick = OnClick;
        UIEventListener.Get(SendContentBtn.gameObject).onClick = OnClick;

        foreach (Transform item in FaceBase)
            UIEventListener.Get(item.gameObject).onClick = OnClick;
        foreach (Transform item in TxtBase)
            UIEventListener.Get(item.gameObject).onClick = OnClick;
        
    }

    private void SendInputChat()
    {
        string fileName = "";
        if (InputChat.value == "" || InputChat.value == "请输入聊天内容。。。")
        { }
        else
        {
            fileName = "2@" + Player.Instance.guid + "@" + InputChat.value;
            ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
            UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
        }
    }

    void OnClick(GameObject go)
    {
        string fileName = "";
        switch (go.name)
        {
            case "BgMask":
                UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                break;
            case "SendSprite":
                if (InputChat.value == "" || InputChat.value == "请输入聊天内容。。。")
                { }
                else
                {
                    fileName = "2@" + Player.Instance.guid + "@" + InputChat.value;
                    ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
                    UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                }

                break;
            case "btnTxt":
                break;
            case "face1001":
            case "face1002":
            case "face1003":
            case "face1004":
            case "face1005":
            case "face1006":
            case "face1007":
                string faceID = go.name.Substring(4);
                fileName = "3@" + Player.Instance.guid + "@" + faceID;
                ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
                UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                break;
            case "ItemSprite0":
            case "ItemSprite1":
            case "ItemSprite2":
            case "ItemSprite3":
            case "ItemSprite4":
            case "ItemSprite5":
            case "ItemSprite6":
            case "ItemSprite7":
                string txtIndex = go.name.Substring(10);
                fileName = "5@" + Player.Instance.guid + "@" + txtIndex;
                ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
                UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                break;
            case "MItemSprite0":
            case "MItemSprite1":
            case "MItemSprite2":
            case "MItemSprite3":
            case "MItemSprite4":
            case "MItemSprite5":
            case "MItemSprite6":
            case "MItemSprite7":
            case "MItemSprite8":
                string txtIndex1 = go.name.Substring(11);
                fileName = "6@" + Player.Instance.guid + "@" + txtIndex1;
                ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
                UIManager.Instance.HideUiPanel(UIPaths.PanelChat);
                break;
        }
    }

}
