using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OutLog : MonoBehaviour {
    static List<string> mLines = new List<string>();//屏幕上输出
    static List<string> mWriteTxt = new List<string>();
    private string outpath;//= Application.persistentDataPath + "/outLog.txt";

    public static OutLog Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。
        outpath = Application.persistentDataPath + "/outLog.txt";
        //每次启动客户端删除之前保存的Log
        //if (System.IO.File.Exists(outpath))
        //{
        //    File.Delete(outpath);
        //}
        //在这里做一个Log的监听
       // Application.RegisterLogCallback(HandleLog);
       
    }

    void Update()
    {
        //因为写入文件的操作必须在主线程中完成
        if (mWriteTxt.Count > 0)
        {
            string[] temp = mWriteTxt.ToArray();
            foreach (string t in temp)
            {
                using (StreamWriter writer = new StreamWriter(outpath, true, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteTxt.Remove(t);
            }
        }
    }

    /// <summary>
    /// 删除原来的
    /// </summary>
    public void Reset()
    {
        if (System.IO.File.Exists(outpath))
        {
            File.Delete(outpath);
        }
        mWriteTxt = new List<string>();
    }
    /// <summary>
    /// 打印信息
    /// </summary>
    /// <param name="str"></param>
    public void AddLogInfo(string str)
    {
        mWriteTxt.Add(str);
    }
    /// <summary>
    /// 打印信息
    /// </summary>
    /// <param name="logString"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
 public  static  void HandleLog(string logString, string stackTrace, LogType type)
    {
        mWriteTxt.Add(logString);
        if (type == LogType.Error || type == LogType.Exception)
        {
            Log(logString);
            Log(stackTrace);
        }
    }

    //这里我把错误的信息保存起来，用来输出在手机屏幕上
    static public void Log(params object[] objs)
    {
        string text = "";
        for (int i = 0; i < objs.Length; ++i)
        {
            if (i == 0)
            {
                text += objs[i].ToString();
            }
            else
            {
                text += ", " + objs[i].ToString();
            }
        }
        if (Application.isPlaying)
        {
            if (mLines.Count > 20)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);

        }
    }

    void OnGUI()
    {
      
        for (int i = 0, imax = mLines.Count; i < imax; ++i)
        {
            GUILayout.Label(mLines[i]);
        }
       
        GUIStyle ss = new GUIStyle();
        ss.fontSize = 30;
        ss.normal.textColor = new Color(1, 0, 0);
        GUILayout.Label(outpath, ss);
        //if (GUILayout.Button("打印"))
        //{
        //    mWriteTxt = new List<string>() { "sdsdsdsdsdsd","rtrtrtrtrtr","ddfgfgfgfg","hjkjkjkjkjkjkjk","wewe3434343","56y5656565565yy5y5","uiuo8ioioioioioioi"};
        //}
    }
}
