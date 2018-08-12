using FrameworkForCSharp.NetWorks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndroidOrIOSResult : MonoBehaviour
{
    void onInviteRoomID(string msg)
    {
        if (msg.Length == 6)
        {
            Player.Instance.shareRoomID = uint.Parse(msg);
            if (SceneManager.GetActiveScene().name == "02_Main")
            {
                ClientToServerMsg.Send(Opcodes.Client_PlayerEnterRoom, Player.Instance.shareRoomID, Input.location.lastData.latitude, Input.location.lastData.longitude);
                Player.Instance.shareRoomID = 0;
            }
        }
    }

    void onMask(string msg)
    {
        //Player.Instance.clientMask = msg;
    }
    public static void GetMask()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("getMask");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
    }
}
