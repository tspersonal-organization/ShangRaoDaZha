using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using LitJson;

public class ToolsFunc
{
	/// <summary>
	/// 3D坐标到屏幕坐标
	/// </summary>
	/// <returns>The to U.</returns>
	/// <param name="point">Point.</param>
    public static Vector3 WorldToUI(Vector3 point)
    {
		Vector3 pt = Camera.main.WorldToScreenPoint(point);
        //我发现有时候UICamera.currentCamera 有时候currentCamera会取错，取的时候注意一下啊。
        Vector3 ff = UICamera.currentCamera.ScreenToWorldPoint(pt);
        //UI的话Z轴 等于0 
		ff.z = 0;
		return ff;
    }
	/// <summary>
	/// 秒转换成时间
	/// </summary>
	/// <returns>The time.</returns>
	/// <param name="total_time">Total_time.</param>
	public static string getTime(int total_time)
	{
        string str = "";
        str += (total_time / 3600 >= 10 ? (total_time / 3600).ToString() : "0" + (total_time / 3600).ToString()) + ":";
        str += ((total_time%3600)/60 >= 10 ?((total_time % 3600)/ 60).ToString():"0"+ ((total_time % 3600) / 60).ToString())+":";
        str += (total_time % 3600)%60 >= 10 ? ((total_time % 3600) % 60).ToString() : "0" + ((total_time % 3600)% 60).ToString();

        return str;
	}
    /// <summary>
    /// 获得首字母
    /// </summary>
    /// <param name="CnChar"></param>
    /// <returns></returns>
    public static string GetCharSpellCode(string CnChar)
    {
        long iCnChar;

        byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);

        //如果是字母，则直接返回 
        if (ZW.Length == 1)
        {
            return CnChar.ToUpper();
        }
        else
        {
            // get the array of byte from the single char 
            int i1 = (short)(ZW[0]);
            int i2 = (short)(ZW[1]);
            iCnChar = i1 * 256 + i2;
        }

        //expresstion 
        //table of the constant list 
        // 'A'; //45217..45252 
        // 'B'; //45253..45760 
        // 'C'; //45761..46317 
        // 'D'; //46318..46825 
        // 'E'; //46826..47009 
        // 'F'; //47010..47296 
        // 'G'; //47297..47613 

        // 'H'; //47614..48118 
        // 'J'; //48119..49061 
        // 'K'; //49062..49323 
        // 'L'; //49324..49895 
        // 'M'; //49896..50370 
        // 'N'; //50371..50613 
        // 'O'; //50614..50621 
        // 'P'; //50622..50905 
        // 'Q'; //50906..51386 

        // 'R'; //51387..51445 
        // 'S'; //51446..52217 
        // 'T'; //52218..52697 
        //没有U,V 
        // 'W'; //52698..52979 
        // 'X'; //52980..53640 
        // 'Y'; //53689..54480 
        // 'Z'; //54481..55289 

        // iCnChar match the constant 
        if ((iCnChar >= 45217) && (iCnChar <= 45252))
        {
            return "A";
        }
        else if ((iCnChar >= 45253) && (iCnChar <= 45760))
        {
            return "B";
        }
        else if ((iCnChar >= 45761) && (iCnChar <= 46317))
        {
            return "C";
        }
        else if ((iCnChar >= 46318) && (iCnChar <= 46825))
        {
            return "D";
        }
        else if ((iCnChar >= 46826) && (iCnChar <= 47009))
        {
            return "E";
        }
        else if ((iCnChar >= 47010) && (iCnChar <= 47296))
        {
            return "F";
        }
        else if ((iCnChar >= 47297) && (iCnChar <= 47613))
        {
            return "G";
        }
        else if ((iCnChar >= 47614) && (iCnChar <= 48118))
        {
            return "H";
        }
        else if ((iCnChar >= 48119) && (iCnChar <= 49061))
        {
            return "J";
        }
        else if ((iCnChar >= 49062) && (iCnChar <= 49323))
        {
            return "K";
        }
        else if ((iCnChar >= 49324) && (iCnChar <= 49895))
        {
            return "L";
        }
        else if ((iCnChar >= 49896) && (iCnChar <= 50370))
        {
            return "M";
        }

        else if ((iCnChar >= 50371) && (iCnChar <= 50613))
        {
            return "N";
        }
        else if ((iCnChar >= 50614) && (iCnChar <= 50621))
        {
            return "O";
        }
        else if ((iCnChar >= 50622) && (iCnChar <= 50905))
        {
            return "P";
        }
        else if ((iCnChar >= 50906) && (iCnChar <= 51386))
        {
            return "Q";
        }
        else if ((iCnChar >= 51387) && (iCnChar <= 51445))
        {
            return "R";
        }
        else if ((iCnChar >= 51446) && (iCnChar <= 52217))
        {
            return "S";
        }
        else if ((iCnChar >= 52218) && (iCnChar <= 52697))
        {
            return "T";
        }
        else if ((iCnChar >= 52698) && (iCnChar <= 52979))
        {
            return "W";
        }
        else if ((iCnChar >= 52980) && (iCnChar <= 53640))
        {
            return "X";
        }
        else if ((iCnChar >= 53689) && (iCnChar <= 54480))
        {
            return "Y";
        }
        else if ((iCnChar >= 54481) && (iCnChar <= 55289))
        {
            return "Z";
        }
        else return ("?");
    }
    /// <summary>
    /// 解析URL 获得IP
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetServerIP(string url)
    {
        string str = url.Substring(0,7);
        if (str == "192.168") return url;
        IPHostEntry ipHost = Dns.GetHostEntry(url);
        string serverIP = "";
        switch (ipHost.AddressList[0].AddressFamily)
        {
            case AddressFamily.InterNetwork:
                ConnServer.m_IsIpv6 = false;
                break;
            case AddressFamily.InterNetworkV6:
                ConnServer.m_IsIpv6 = true;
                break;

        }
        serverIP = ipHost.AddressList[0].ToString();
        return serverIP;
    }
    /// <summary>
    /// 获得字符串长度
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetStringLength(string str)
    {
        int len = 0;
        foreach (char item in str)
        {
            len += (int)item > 127 ? 2 : 1;
        }
        return len;
    }
    /// <summary>
    /// 获得缩略显示
    /// </summary>
    /// <returns></returns>
    public static string GetSuoNueShowText(int number)
    {
        string str = "";
        string suffix = "";
        int integ = 0;
        if (number < 1000)
        {
            return number.ToString();
        }
        else if (number >= 1000 && number < 10000)
        {
            integ = (int)(number / 1000);
            suffix = "K";
        }
        else if (number >= 10000 && number < 1000000)
        {
            integ = (int)(number / 10000);
            suffix = "W";
        }
        else if (number >= 1000000)
        {
            integ = (int)(number / 1000000);
            suffix = "M";
        }
        str += integ.ToString();
        str +=  suffix;
        return str;
    }
    /// <summary>
    /// 设置分享图标
    /// </summary>
    public static void SetShareIcon()
    {
        Texture2D texture = Resources.Load<Texture2D>("ICON");
        File.WriteAllBytes(Application.persistentDataPath + "/icon.png", texture.EncodeToPNG());
    }

    #region 文件读取
    ///**
    ///* path：文件创建目录
    ///* name：文件的名称
    ///*  info：写入的内容
    ///*/
    public static void CreateFile(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开
            sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }
    /**
     * path：读取文件的路径
     * name：读取文件的名称
     */
    public static ArrayList LoadFile(string path, string name)
    {
        //使用流的形式读取
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取
            //将每一行的内容存入数组链表容器中
            arrlist.Add(line);
        }
        //关闭流
        sr.Close();
        //销毁流
        sr.Dispose();
        //将数组链表容器返回
        return arrlist;
    }
    /**
     * path：删除文件的路径
     * name：删除文件的名称
     */
    public static void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);

    }
    #endregion

    #region json读取
    /// <summary>
    /// 读Json格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text"></param>
    /// <param name="outList"></param>
    public static void ReadJson<T>(string text,ref List<T> outList)
    {
        JsonData data = JsonMapper.ToObject(text);
        foreach (JsonData json in data)
        {
            T t = JsonUtility.FromJson<T>(json.ToJson());
            outList.Add(t);
        }
    }
    #endregion
}
