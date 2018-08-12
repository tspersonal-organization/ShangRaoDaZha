using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCodeString
{
    static ArrayList resultString = new ArrayList();

    public static void Init()
    {
        ReadTxt();
    }
    static void ReadTxt()
    {
        TextAsset ta = Resources.Load<TextAsset>("Txt/ResultCodeString");
        string[] items = ta.text.Split(new String[] { "\r\n" }, StringSplitOptions.None);
        for (int i = 0; i < items.Length; i++)
        {
            resultString.Add(items[i]);
        }
    }
    public static string GetResultString(ushort index)
    {
        if (index < resultString.Count)
            return resultString[index].ToString();
        else
            return "未知错误:"+index;
    }
}
