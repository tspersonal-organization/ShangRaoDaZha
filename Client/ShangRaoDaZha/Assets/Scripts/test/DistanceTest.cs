/*
* 开发人：滕俊
* 项目：
* 功能描述：
*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTest : MonoBehaviour {

    public UIButton Count;
    public UIInput latOne;
    public UIInput lonOne;
    public UIInput latTwo;
    public UIInput lonTwo;

    public UILabel Result;
    // Use this for initialization
    void Start () {
        Count.onClick.Add(new EventDelegate(this.CountDis));
	}

    private void CountDis()
    {
       double dis= Distance(double.Parse( latOne.value), double.Parse(lonOne.value), double.Parse(latTwo.value), double.Parse(lonTwo.value));

        Result.text = dis.ToString()+"千米";
    }

    // Update is called once per frame
    void Update () {
		
	}

    private const double EARTH_RADIUS1 = 6378.137;
    private static double rad(double d)
    {
        return d * Math.PI / 180.0;
    }
    public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
    {
        double radLat1 = rad(lat1);
        double radLat2 = rad(lat2);
        double a = radLat1 - radLat2;
        double b = rad(lng1) - rad(lng2);
        double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
         Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
        s = s * EARTH_RADIUS1;
        s = Math.Round(s * 10000) / 10000;
        return s;
    }




    static double EARTH_RADIUS = 6371.0;//km 地球半径 平均值，千米

    /// <summary>
    /// 给定的经度1，纬度1；经度2，纬度2. 计算2个经纬度之间的距离。
    /// </summary>
    /// <param name="lat1">经度1</param>
    /// <param name="lon1">纬度1</param>
    /// <param name="lat2">经度2</param>
    /// <param name="lon2">纬度2</param>
    /// <returns>距离（公里、千米）</returns>
    public static double Distance(double lat1, double lon1, double lat2, double lon2)
    {
        //用haversine公式计算球面两点间的距离。
        //经纬度转换成弧度
        lat1 = ConvertDegreesToRadians(lat1);
        lon1 = ConvertDegreesToRadians(lon1);
        lat2 = ConvertDegreesToRadians(lat2);
        lon2 = ConvertDegreesToRadians(lon2);

        //差值
        var vLon = Math.Abs(lon1 - lon2);
        var vLat = Math.Abs(lat1 - lat2);

        //h is the great circle distance in radians, great circle就是一个球体上的切面，它的圆心即是球心的一个周长最大的圆。
        var h = HaverSin(vLat) + Math.Cos(lat1) * Math.Cos(lat2) * HaverSin(vLon);

        var distance = 2 * EARTH_RADIUS * Math.Asin(Math.Sqrt(h));

        return distance;
    }
    public static double HaverSin(double theta)
    {
        var v = Math.Sin(theta / 2);
        return v * v;
    }


    /// <summary>
    /// 将角度换算为弧度。
    /// </summary>
    /// <param name="degrees">角度</param>
    /// <returns>弧度</returns>
    public static double ConvertDegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    public static double ConvertRadiansToDegrees(double radian)
    {
        return radian * 180.0 / Math.PI;
    }

}
