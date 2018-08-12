using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

/// <summary>
/// 通用方法
/// </summary>
public class Utility
{
    /// <summary>
    /// 字符串转字节数组
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] StrToByteArr(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    /// <summary>
    /// 字节数组转字符串
    /// </summary>
    /// <param name="byteArray"></param>
    /// <returns></returns>
    public static string ByteArrToStr(byte[] byteArray)
    {
        return Encoding.UTF8.GetString(byteArray);
    }

    /// <summary>
    /// UnixTime时间戳
    /// </summary>
    /// <param name="expired">有效期（单位：秒）</param>
    /// <returns></returns>
    public static string UnixTime(double expired = 0)
    {
        var time = (DateTime.Now.AddSeconds(expired).ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        return time.ToString();
    }

    /// <summary>
    /// 字节数组合并
    /// </summary>
    /// <param name="byte1"></param>
    /// <param name="byte2"></param>
    /// <returns></returns>
    public static byte[] JoinByteArr(byte[] byte1, byte[] byte2)
    {
        byte[] full = new byte[byte1.Length + byte2.Length];
        Stream s = new MemoryStream();
        s.Write(byte1, 0, byte1.Length);
        s.Write(byte2, 0, byte2.Length);
        s.Position = 0;
        int r = s.Read(full, 0, full.Length);
        if (r > 0)
        {
            return full;
        }
        throw new Exception("读取错误!");
    }

    public static string ImgBase64(string localFileName)
    {
        WWW localFile = new WWW("file:///" + localFileName);

        if (localFile.error == null)
            Debug.Log("Loaded file successfully");
        else
        {
            Debug.Log("Open file error: " + localFile.error);
        }

        return Convert.ToBase64String(localFile.bytes.ToArray());
    }


    /// <summary>
    /// 图片转Base64
    /// </summary>
    /// <param name="path">图片路径</param>
    /// <param name="isWebImg">是否网络图片 默认 false </param>
    /// <returns>Base64</returns>
//        public static string ImgBase64(string path )
//        {
//            
//            Image img;
//
//            if (!File.Exists(path))
//            {
//                throw new Exception("文件不存在!");
//            }
//            img = Image.FromFile(path);
//            
//            MemoryStream ms = new MemoryStream();
//            string file_etx = Path.GetExtension(path).ToLower();
//            switch (file_etx)
//            {
//                case ".jpg":
//                    img.Save(ms, Jpeg);
//                    break;
//                case ".png":
//                    img.Save(ms, ImageFormat.Png);
//                    break;
//                case ".gif":
//                    img.Save(ms, ImageFormat.Gif);
//                    break;
//                case ".bmp":
//                    img.Save(ms, ImageFormat.Bmp);
//                    break;
//                default:
//                    img.Save(ms, ImageFormat.Jpeg);
//                    break;
//            }
//            return Convert.ToBase64String(ms.ToArray());
//        }
}