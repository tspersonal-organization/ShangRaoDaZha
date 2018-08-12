using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;

public class XMLManager
{

    static XMLManager _Instance = null;

    public static string WWW_PATH_WRITE = 
   #if UNITY_ANDROID && !UNITY_EDITOR  //安卓
   	Application.persistentDataPath;
   #elif UNITY_IPHONE && !UNITY_EDITOR
   	Application.persistentDataPath+"/";
   #elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
       Application.persistentDataPath;
   #else
      string.Empty;
   #endif

    public static string WWW_PATH_READ =
#if UNITY_ANDROID && !UNITY_EDITOR  //安卓
	"file://" + Application.persistentDataPath+ "/";
#elif UNITY_IPHONE && !UNITY_EDITOR
	"file:///" +Application.persistentDataPath+"/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
    "file:///" + Application.persistentDataPath+"/";
#else
   string.Empty;
#endif

    /// <summary>
    /// XML
    /// </summary>
    public static string DOWNLOAD_PATH = "http://game.youthgamer.com:92/ClientResource/";

    private List<string> LocalResVersion;
    private List<string> ServerResVersion;
    private List<string> NeedDownFiles;
    private bool NeedUpdateLocalVersionFile = false;

    string clientVersionManager = "clientVersionCompare.txt";
    string serverVersionManager = "VersionCompare.txt";
    public delegate void HandleFinishDownload(WWW www);

    public static XMLManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new XMLManager();
            }
            return _Instance;
        }
    }

    public void InitXml()
    {
        //初始化  
        LocalResVersion = new List<string>();
        ServerResVersion = new List<string>();
        NeedDownFiles = new List<string>();

        //加载本地version配置  
        CoroutineControl.Instance.StartCoroutine(DownLoad(WWW_PATH_READ + clientVersionManager, delegate (WWW localVersion)
        {
            //保存本地的version  
            ParseVersionFile(localVersion.text, LocalResVersion);
            //加载服务端version配置  
            CoroutineControl.Instance.StartCoroutine(DownLoad(DOWNLOAD_PATH + serverVersionManager, delegate (WWW serverVersion)
            {
                //保存服务端version  
                ParseVersionFile(serverVersion.text, ServerResVersion);
                //计算出需要重新加载的资源  
                CompareVersion();
                //加载需要更新的资源  
                DownLoadRes();

            }));

        }));
    }

    //依次加载需要更新的资源  
    private void DownLoadRes()
    {
        if (NeedDownFiles.Count == 0)
        {
            UpdateLocalVersionFile();
            return;
        }

        string[] file = NeedDownFiles[0].Split(':');
        NeedDownFiles.RemoveAt(0);

        CoroutineControl.Instance.StartCoroutine(this.DownLoad(DOWNLOAD_PATH + file[1], delegate (WWW w)
        {
            //将下载的资源替换本地就的资源  
            ReplaceLocalRes(file[1], w.bytes);
            DownLoadRes();
        }));
    }

    private void ReplaceLocalRes(string fileName, byte[] data)
    {
        string filePath = DOWNLOAD_PATH + fileName;
        FileStream stream = new FileStream(WWW_PATH_WRITE + "//" + fileName, FileMode.Create);
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
    }

    //更新本地的version配置  
    private void UpdateLocalVersionFile()
    {
        if (NeedUpdateLocalVersionFile)
        {
            StringBuilder versions = new StringBuilder();
            foreach (string item in ServerResVersion)
            {
                versions.Append(item).Append("\r\n");
            }

            FileStream stream = new FileStream(WWW_PATH_WRITE + "//" + clientVersionManager, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }
        //加载数据
        CoroutineControl.Instance.StartCoroutine(LoadXml());
    }

    private void CompareVersion()
    {
        for (int i = 0; i < ServerResVersion.Count; i++)
        {
            if (i < LocalResVersion.Count)
            {
                if (ServerResVersion[i] != LocalResVersion[i])
                {
                    NeedDownFiles.Add(ServerResVersion[i]);
                }
            }
            else
            {
                NeedDownFiles.Add(ServerResVersion[i]);
            }
        }
        //本次有更新，同时更新本地的version.txt  
        NeedUpdateLocalVersionFile = NeedDownFiles.Count > 0;
    }

    private void ParseVersionFile(string content, List<string> dict)
    {
        if (content == null || content.Length == 0)
        {
            return;
        }
        string[] items = content.Split(new String[] { "\r\n" }, StringSplitOptions.None);
        foreach (string item in items)
        {
            dict.Add(item);
        }

    }

    private IEnumerator DownLoad(string url, HandleFinishDownload finishFun)
    {
        WWW www = new WWW(url);
        yield return www;
        if (finishFun != null)
        {
            finishFun(www);
        }
        //if (www.error != null) Debug.LogError(www.error);
        www.Dispose();
    }

    /// <summary>
    /// 读取本地数据
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadXml()
    {
        InitGame.Instance.isXMLDone = true;
        
        yield break;
    }

}
