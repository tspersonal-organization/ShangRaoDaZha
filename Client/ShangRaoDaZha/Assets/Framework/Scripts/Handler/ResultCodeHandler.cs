﻿using UnityEngine;
using FrameworkForCSharp.NetWorks;
using UnityEngine.SceneManagement;

public class ResultCodeHandler
{
    public ResultCodeHandler()
    {
        Handler.addServerHandler(Opcodes.Server_ResultCode, OnResultCode);
    }

    void OnResultCode(NetworkMessage message)
    {
        ushort temp = message.readUInt16();
        ResultCode code = (ResultCode)temp;
        Opcodes op = (Opcodes)message.readUInt16();
        switch (code)
        {
            case ResultCode.Successed:
                Succeed(op);
                break;
            default:
                Loser(code);
                break;
        }
    }

    /// <summary>
    /// 失败返回
    /// </summary>
    /// <param name="code"></param>
    void Loser(ResultCode code)
    {
        string sError = ResultCodeString.GetResultString((ushort)code);
        GameData.ResultCodeStr = sError;
        //UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
        GlobalModule.Instance.OnOpenBubblingHint(sError);
        Log.Debug("错误返回：" + sError);
        switch (code)
        {
            case ResultCode.RoomNotExist:
                if (ManagerScene.Instance.currentSceneType != SceneType.Main)
                {
                    ManagerScene.Instance.LoadScene(SceneType.Main);
                }
                break;
        }
    }

    void Succeed(Opcodes op)
    {
        //Client_Apply_For_Club
        switch (op)
        {
            case Opcodes.Client_Apply_For_Club:
                GameData.ResultCodeStr = "申请发送成功";

                UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
                DzPanelJoinMoment.Instance.gameObject.SetActive(false);

                break;
            case Opcodes.Client_Club_Invite_Player:
                GameData.ResultCodeStr = "邀请成功";
             
                UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
                DzPanelJoinMomentInvite.Instance.gameObject.SetActive(false);

                break;
            case Opcodes.Client_Change_Club_Name://改变俱乐部名称
                GameData.ResultCodeStr = "修改成功";
                ClientToServerMsg.GetClubList();//申请俱乐部列表
                ClientToServerMsg.GetClubInfo((uint)GameData.CurrentClubInfo.Id);//再次申请当前选的俱乐部的信息
                ClubReNamePanelControl.Instance.gameObject.SetActive(false);
                ClubMastorControl.Instance.gameObject.SetActive(false);
                UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);

                break;

            case Opcodes.Client_Club_Config_AutoRoom://开放配置返回
                GameData.ResultCodeStr = "创建房间配置成功";
                ClientToServerMsg.GetClubInfo((uint)GameData.CurrentClubInfo.Id);//再次申请当前选的俱乐部的信息
                CreatRoomPanel.Instance.gameObject.SetActive(false);
                ClubMastorControl.Instance.gameObject.SetActive(false);
                UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);

                break;
            case Opcodes.Client_SetInviteGuid:
                GameData.ResultCodeStr = "绑定邀请人成功";
                UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
                UIManager.Instance.HideUiPanel(UIPaths.InvitePanel);
                // SoundManager.Instance.PlaySound(UIPaths.SOUND_CHARGE);
                break;
            case Opcodes.Client_ExchangDiamondToCard:
                GameData.ResultCodeStr = "兑换成功";
                UIManager.Instance.ShowUiPanel(UIPaths.PanelDialog, OpenPanelType.MinToMax);
             
                // SoundManager.Instance.PlaySound(UIPaths.SOUND_CHARGE);
                break;

                //case Opcodes.Client_SendMoney:
                //    GameData.ResultCodeStr = "转增成功！";
                //    UIManager.Instance.ShowUiPanel(UIPaths.ShowStringDialog, OpenPanelType.MinToMax);
                //    SoundManager.Instance.PlaySound(UIPaths.SOUND_CHARGE);
                //    break;
                //case Opcodes.Client_PlayerChooseWantPartnerResult:
                //    UIManager.Instance.HideUiPanel(UIPaths.AskHuoZhuang);
                //    break;
                //case Opcodes.Client_SetInviteGuid:
                //    GameData.ResultCodeStr = "绑定代理人成功！";
                //    UIManager.Instance.ShowUiPanel(UIPaths.ShowStringDialog, OpenPanelType.MinToMax);
                //    UIManager.Instance.HideUiPanel(UIPaths.AddRoom);
                //    break;
                //case Opcodes.Client_ParentWantBackMoneyResult:
                //    if (AskRecycleMoney.Instance != null) AskRecycleMoney.Instance.OnOperateSucceed();
                //    break;
        }
    }

}
