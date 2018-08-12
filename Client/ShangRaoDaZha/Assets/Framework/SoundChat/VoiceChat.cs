using FrameworkForCSharp.NetWorks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChat : UIBase<VoiceChat>
{
    public GameObject MB;
    GameObject m_objCancelChat;
    GameObject m_objChat;

    float m_chatTime = 10.0f;
    bool m_IsStartChat = false;
    bool m_IsCancelChat = false;
    bool m_IsClickChat = false;
    bool isChatClick = false;//是否点击在语音聊天按钮上面
    bool isSendChat = false;

    UISprite m_ProgressImage;
    UISprite m_SoundSize;

    public static VoiceChat Instance = null;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        m_objCancelChat = transform.Find("Chat").Find("CancelChat").gameObject;
        m_objChat = transform.Find("Chat").Find("Base").gameObject;

        m_ProgressImage = m_objChat.transform.Find("progress").GetComponent<UISprite>();
        m_SoundSize = m_objChat.transform.Find("soundSize").GetComponent<UISprite>();

        UIEventListener.Get(transform.Find("Chat").Find("ButtonChat").gameObject).onDragOver = OnDragOver;
        UIEventListener.Get(transform.Find("Chat").Find("ButtonChat").gameObject).onPress = OnPress;

        UIEventListener.Get(MB).onDragOver = OnDragOver;
    }
    void Update()
    {
        if (m_IsStartChat && !isSendChat)
        {
            if (m_chatTime <= 0)
            {
                SendChatSound();
            }
            else
            {
                m_chatTime -= Time.deltaTime;
            }
            m_ProgressImage.fillAmount = m_chatTime / 10.0f;
            m_SoundSize.fillAmount = (float)VoiceUtility.GetInstance().Volume * 1.28f;
        }
    }

    void OnDragOver(GameObject go)
    {
        if (!m_IsStartChat) return;
        if (go.name == "ButtonChat")
        {
            m_objChat.SetActive(true);
            m_objCancelChat.SetActive(false);
            m_IsClickChat = true;
        }
        else if (go.name == MB.name)
        {
            m_objChat.SetActive(false);
            m_objCancelChat.SetActive(true);
            m_IsClickChat = false;
        }
    }

    void OnPress(GameObject go, bool isPress)
    {
        if (isPress)
        {
            if (go.name == "ButtonChat")
            {
                m_objChat.SetActive(true);
                m_objCancelChat.SetActive(false);
                m_IsStartChat = true;
                m_IsClickChat = true;
                m_IsCancelChat = false;
                m_chatTime = 10.0f;
                isChatClick = true;
                isSendChat = false;
                VoiceUtility.GetInstance().StartRecord(10);
            }
        }
        else
        {
            if (go.name == "ButtonChat")
            {
                Debug.Log("发送111");
                if (!m_IsClickChat) m_IsCancelChat = true;
                SendChatSound();
            }
        }
    }
    void SendChatSound()
    {
        m_IsStartChat = false;
        m_objChat.SetActive(false);
        m_objCancelChat.SetActive(false);
        isChatClick = false;

        if (!m_IsCancelChat && !isSendChat)
        {
            isSendChat = true;
            Debug.Log("发送");
            VoiceUtility.GetInstance().StopRecord();
            string fileName = GameData.m_TableInfo.id.ToString() + "@" + Player.Instance.guid.ToString() + "@" + DateTime.Now.Ticks.ToString();
            StartCoroutine(UploadMusic(VoiceUtility.GetInstance().GetLastRecordByteArray(), fileName, GameData.m_TableInfo.id.ToString()));
        }
    }

    //上传玩家说话声音
    IEnumerator UploadMusic(byte[] bytes, string fileName, string file)
    {
        fileName = "1@" + fileName;
        WWWForm newForm = new WWWForm();
        newForm.AddField("path", file);
        newForm.AddField("id", fileName);
        newForm.AddBinaryData("Sound", bytes, "photo.ogg");
        WWW w = new WWW(UIPaths.CHAT_UPLOAD_SOUND_PATH, newForm);
        while (!w.isDone)
        {
            Debug.Log("not isDone!");
            yield return new WaitForEndOfFrame();
        }
        ClientToServerMsg.Send(Opcodes.Client_PlayerSpeak, GameData.m_TableInfo.id, fileName);
        yield break;
    }

    /// <summary>
    /// 显示玩家说话
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="timeLength"></param>
    /// <returns></returns>
    public IEnumerator AsynSoundAnimation(Vector3 pos, float timeLength)
    {
        Transform tran = transform.Find("Chat").Find("WhoSound");
        tran.position = pos;
        tran.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeLength);
        tran.gameObject.SetActive(false);
        yield break;
    }

}
