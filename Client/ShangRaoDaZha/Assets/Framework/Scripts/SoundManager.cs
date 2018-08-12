using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 剩余管理类
/// </summary>
public class SoundManager : UIBase<SoundManager>,IResourceListener
{
    public GameObject[] soundArray;
    int CurIndex = 0;
  public   AudioSource m_AudioSource;
    float waitTime = 0;
    /// <summary>
    /// 保存已经加载过的声音
    /// </summary>
    Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

	void Start ()
    {
        CurIndex = 0;
        m_AudioSource = gameObject.GetComponent<AudioSource>();
        //if (Player.Instance.GameBGSoundOff) m_AudioSource.volume = Player.Instance.GameBGSoundValue;
        //else m_AudioSource.volume = 0;
        SetBGVolume();
	}
    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="fileName"></param>
    public void PlaySound(string fileName,float _waitTime = 0)
    {
        waitTime = _waitTime;
        if (Player.Instance.GameEffectSoundOff)//是否关闭了特效音
        {
            if (audioDict.ContainsKey(fileName))
            {
                StartCoroutine(AsynPlaySound(audioDict[fileName]));
                return;
            }
            //没有就加载
            ResourcesManager.Instance.Load(fileName,typeof(object),this);
        }
    }
    string bgPaht = string.Empty;
    public void ChangeBGSound(string fileName)
    {
        bgPaht = fileName;
        if (audioDict.ContainsKey(fileName))
        {
            m_AudioSource.clip = audioDict[fileName];
            m_AudioSource.Play();
            return;
        }
        ResourcesManager.Instance.Load(fileName, typeof(object), this);
    }
    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="fileName"></param>
    public void PlaySound(string fileName,byte sex)
    {
        waitTime = 0;
        if (Player.Instance.GameEffectSoundOff)//是否关闭了特效音
        {
            if (sex == 1) fileName = string.Format(fileName, "boy");
            else fileName = string.Format(fileName, "girl");

            if (audioDict.ContainsKey(fileName))
            {
                StartCoroutine(AsynPlaySound(audioDict[fileName]));
                return;
            }
            //没有就加载
            ResourcesManager.Instance.Load(fileName, typeof(object), this);
        }
    }
    /// <summary>
    /// 创建并且播放声音
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    IEnumerator AsynPlaySound(AudioClip clip)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        GameObject go = soundArray[CurIndex];
        go.SetActive(true);
        CurIndex++;
        if (CurIndex >= soundArray.Length)
            CurIndex = 0;
        //sw.Stop();
        //Log.Debug("运行时间:"+sw.Elapsed.TotalMilliseconds);
        go.transform.parent = transform;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        AudioSource audio = go.GetComponent<AudioSource>();
        audio.volume = Player.Instance.GameEffectSoundValue;
        audio.clip = clip;
        yield return new WaitForSeconds(waitTime);
        audio.Play();
        yield return new WaitForSeconds(clip.length);
        go.SetActive(false);
        yield break;
    }
    public void SetBGVolume()
    {
        //m_AudioSource.volume = Player.Instance.GameBGSoundValue;
        if (Player.Instance.GameBGSoundOff)
        {
          //  Player.Instance.GameBGSoundValue=0.1f;
            m_AudioSource.volume = Player.Instance.GameBGSoundValue;
        }
           
          else
              CloseBGSound();
    }
    public void CloseBGSound()
    {
        m_AudioSource.volume = 0;
    }
    /// <summary>
    /// 声音是否已经加载完成
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="asset"></param>
    public void OnLoaded(string assetName, object asset)
    {
        AudioClip chip = asset as AudioClip;
        audioDict.Add(assetName, asset as AudioClip);
        if (assetName == bgPaht)
        {
            m_AudioSource.clip = chip;
            m_AudioSource.Play();
        }
        else StartCoroutine(AsynPlaySound(chip));
    }

    protected override void OnDestroy()
    {

    }
}
