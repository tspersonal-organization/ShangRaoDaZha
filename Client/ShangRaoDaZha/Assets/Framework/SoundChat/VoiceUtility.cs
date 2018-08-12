using UnityEngine;
using System.Collections;
using System;
using FrameworkForCSharp.Utils;

public class VoiceUtility : MonoBehaviour
{

    public int RecordSampleRate;//录音采样率
    public AudioSource VoiceSource;//播放录音的AudioSource
    public AudioClip PcTestClip;//PC端测试的音效
    private byte[] LastRecordedByteArray;//上次录音保存的字节流
    private static VoiceUtility _instance;
    private AudioClip clip;//存放音频的AudioClip
    private bool HasAccess = true;//是否有录音权限
    private bool HasDevice = true;//是否有录音设备
    bool isPlay = false;
    private float totalRecordedTime = 0f;
    private DateTime lastRecordStartTime;
    float m_Time;
    // Use this for initialization
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //StartCoroutine(InitialSettings());
    }

    public static VoiceUtility GetInstance()
    {
        return _instance;
    }

    void Update()
    {
        if(GameData.m_VoiceQueue.Count > 0 && !isPlay)
        {
            isPlay = true;
            StartCoroutine(DownloadMusic());
        }
    }

    //播放玩家声音
    public IEnumerator DownloadMusic()
    {
        string[] strs = GameData.m_VoiceQueue.Dequeue().Split('@');
        string path = strs[1] + "@" + strs[2] + "@" + strs[3] + "@"+strs[4];
        //Debug.Log(InitData.CHAT_SOUND_DOWN_PATH + GameData.m_TableChats[0].id.ToString() + "/" + path + ".sound");
        WWW ww = new WWW(UIPaths.CHAT_DOWN_SOUND_PATH+ strs[0] +"/"+ path+".sound");
        while (!ww.isDone)
        {
             yield return new WaitForEndOfFrame();
        }
        if (ww.error == null)
        {
           //bool isPlaySound = false;
           //if (ulong.Parse(strs[2]) == Player.Instance.guid) isPlaySound = false;
           //else isPlaySound = true;
            AudioClip ac = PlayVoice(ww.bytes, true);
            if (GameData.GlobleRoomType == RoomType.PK)
            {
                if (DDZMainGame.Instance != null) DDZMainGame.Instance.onShowSoundAnimation(ulong.Parse(strs[3]), ac.length);
            }
            else if (GameData.GlobleRoomType == RoomType.WDH)
            {
                WuDangHuGame.Instance.onShowSoundAnimation(ulong.Parse(strs[3]), ac.length);
            }
          
            yield return new WaitForSeconds(ac.length);
        }
        isPlay = false;
        yield break;
    }
    /// <summary>
    /// 开始录音
    /// </summary>
    /// <param name="MaxSeconds">最长的时间/秒</param>
    public void StartRecord(int MaxSeconds)
    {
        if (!Application.isMobilePlatform)
        {
            clip = PcTestClip;
        }
        else
        {
            if (HasDevice && HasAccess)
            {
                if (Application.isMobilePlatform)
                {
                    clip = Microphone.Start(null, false, MaxSeconds, RecordSampleRate);
                    lastRecordStartTime = DateTime.Now;
                }
            }
        }
    }

    /// <summary>
    /// 停止录音
    /// </summary>
    public void StopRecord()
    {
        totalRecordedTime = (float)(DateTime.Now - lastRecordStartTime).TotalSeconds;
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
        }
        SetLastRecordByteArray();
    }

    /// <summary>
    /// 获取刚刚录取的音频字节流
    /// </summary>
    /// <returns></returns>
    public byte[] GetLastRecordByteArray()
    {
        return LastRecordedByteArray;
    }

    /// <summary>
    /// 播放指定字节流的音频
    /// </summary>
    /// <param name="VoiceByteArray">指定音频字节流</param>
    public AudioClip PlayVoice(byte[] VoiceByteArray,bool isPlay = true)
    {
       AudioClip result = AudioClip.Create("receviedSound", VoiceByteArray.Length / 2, 1, 8000, false, false);
        //AudioClip result = Microphone.Start(null, false, 1, 8000);
        float[] floatArray = new float[VoiceByteArray.Length / 2];
        for (int i = 0; i < floatArray.Length; i++)
        {
            byte[] tmpByte = new byte[2];
            tmpByte[0] = VoiceByteArray[i * 2];
            tmpByte[1] = VoiceByteArray[i * 2 + 1];
            short tmpShort = BitConverter.ToInt16(tmpByte, 0);
            floatArray[i] = (float)tmpShort / 32767f;
        }
        result.SetData(floatArray, 0);
        if (isPlay)
        {
            VoiceSource.clip = result;
            VoiceSource.Play();
            
        }
        m_Time = result.length;
        return result;
    }

     public float Volume
     {
         get
         {
             if (Microphone.IsRecording(null))
             {
                 // 采样数
                 int sampleSize = 128;
                 float[] samples = new float[sampleSize];
                 int startPosition = Microphone.GetPosition(null) - (sampleSize + 1);
                // 得到数据
                clip.GetData(samples, startPosition);
     
                 // Getting a peak on the last 128 samples
                 float levelMax = 0;
                 for (int i = 0; i<sampleSize; ++i)
                 {
                     float wavePeak = samples[i];
                     if (levelMax<wavePeak)
                         levelMax = wavePeak;
                 }
     
                 return levelMax;
             }
             return 0;
         }
     }

    private void SetLastRecordByteArray()
    {
        if (clip == null)
        {
            return;
        }

        float[] samples = new float[RecordSampleRate * Mathf.CeilToInt(totalRecordedTime)];

        clip.GetData(samples, 0);


        byte[] tempResult = new byte[samples.Length * 2];
        //Int16[] intData = new Int16[samples.Length];   
        //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]   

        int rescaleFactor = 32767; //to convert float to Int16   

        for (int i = 0; i < samples.Length; i++)
        {
            short temshort = (short)(samples[i] * rescaleFactor);

            byte[] temdata = System.BitConverter.GetBytes(temshort);

            tempResult[i * 2] = temdata[0];
            tempResult[i * 2 + 1] = temdata[1];


        }
        LastRecordedByteArray = tempResult;
        if (tempResult == null || tempResult.Length <= 0)
        {
            return;
        }
    }

    IEnumerator InitialSettings()
    {
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
            HasAccess = true;
        }

        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            HasAccess = false;
            yield break;
        }

        if (Microphone.devices.Length == 0)
        {
            HasDevice = false;
            yield break;
        }
    }
}
