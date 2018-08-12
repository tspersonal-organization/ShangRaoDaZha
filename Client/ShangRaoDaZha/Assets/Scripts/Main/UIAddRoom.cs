using FrameworkForCSharp.NetWorks;
using FrameworkForCSharp.Utils;
using UnityEngine;

public class UIAddRoom : UIBase<UIAddRoom>
{
    UILabel LBRoomID;
    string roomIDTxt = string.Empty;

    void Start()
    {
        LBRoomID = transform.Find("Base").Find("roomID").GetComponent<UILabel>();
        Debug.Log(myEnum.Instance.getEnumDescription(ResultCode.AuthCode_Check_Failed));
        foreach (Transform item in transform.Find("Base").Find("BtnBase"))
            UIEventListener.Get(item.gameObject).onClick = OnClick;
    }

    void OnClick(GameObject go)
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        switch (go.name)
        {
            case "btnClose":
                UIManager.Instance.HideUIPanel(UIPaths.UIPanel_AddRoom);
                break;
            case "btn0":
            case "btn1":
            case "btn2":
            case "btn3":
            case "btn4":
            case "btn5":
            case "btn6":
            case "btn7":
            case "btn8":
            case "btn9":
            case "btn100":
            case "btn99":
                ShowRoomID(int.Parse(go.name.Substring(3)));
                break;
        }
    }

    void ShowRoomID(int num)
    {
        if(num == 100)
        {
            roomIDTxt = "";
        }
        else if(num == 99 && roomIDTxt.Length > 0)
        {
            roomIDTxt = roomIDTxt.Substring(0,roomIDTxt.Length - 1);
        }
        else
        {
            if(roomIDTxt.Length < 6)
            {
                roomIDTxt += num.ToString();
                if (roomIDTxt.Length == 6) ClientToServerMsg.Send(Opcodes.Client_PlayerEnterRoom,uint.Parse(roomIDTxt), Input.location.lastData.latitude, Input.location.lastData.longitude);
            }
        }
        LBRoomID.text = roomIDTxt;
    }
}
