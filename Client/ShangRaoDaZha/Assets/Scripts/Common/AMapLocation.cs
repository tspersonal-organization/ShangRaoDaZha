using System;
using UnityEngine;

/// <summary>
/// 错误码对照表
/// </summary>
public enum ErrorCode
{
    Error1_ = -1,//异常错误。
    Successful = 0,//定位成功。 ====>>>> 可以在定位回调里判断定位返回成功后再进行业务逻辑运算。
    Error1,//一些重要参数为空，如context。 ====>>>> 请对定位传递的参数进行非空判断。
    Error2,//定位失败，由于仅扫描到单个wifi，且没有基站信息。 ====>>>> 请重新尝试。
    Error3,//获取到的请求参数为空，可能获取过程中出现异常。 ====>>>> 请对所连接网络进行全面检查，请求可能被篡改。
    Error4,//请求服务器过程中的异常，多为网络情况差，链路不通导致。 ====>>>> 请检查设备网络是否通畅，检查通过接口设置的网络访问超时时间，建议采用默认的30秒。
    Error5,//请求被恶意劫持， ====>>>> 定位结果解析失败。您可以稍后再试，或检查网络链路是否存在异常。
    Error6,//定位服务返回定位失败。 ====>>>> 请获取errorDetail（通过getLocationDetail()方法获取）信息并参考定位常见问题进行解决。
    Error7,//KEY鉴权失败。 ====>>>> 请仔细检查key绑定的sha1值与apk签名sha1值是否对应，或通过高频问题查找相关解决办法。
    Error8,//Android exception常规错误。 ====>>>> 请将errordetail（通过getLocationDetail()方法获取）信息通过工单系统反馈给我们。
    Error9,//定位初始化时出现异常。 ====>>>> 请重新启动定位。
    Error10,//定位客户端启动失败。 ====>>>> 请检查AndroidManifest.xml文件是否配置了APSService定位服务。
    Error11,//定位时的基站信息错误。 ====>>>> 请检查是否安装SIM卡，设备很有可能连入了伪基站网络。
    Error12,//缺少定位权限。 ====>>>> 请在设备的设置中开启app的定位权限。
    Error13,//定位失败，由于未获得WIFI列表和基站信息，且GPS当前不可用。 ====>>>> 建议开启设备的WIFI模块，并将设备中插入一张可以正常工作的SIM卡，或者检查GPS是否开启；如果以上都内容都确认无误，请您检查App是否被授予定位权限。
    Error14,//GPS 定位失败，由于设备当前 GPS 状态差。 ====>>>> 建议持设备到相对开阔的露天场所再次尝试。
    Error15,//定位结果被模拟导致定位失败。 ====>>>> 如果您希望位置被模拟，请通过setMockEnable(true);方法开启允许位置模拟。
    Error16,//当前POI检索条件、行政区划检索条件下，无可用地理围栏。 ====>>>> 建议调整检索条件后重新尝试，例如调整POI关键字，调整POI类型，调整周边搜区域，调整行政区关键字等。
    Error17,//定位失败，错误码：17。
    Error18,//定位失败，由于手机WIFI功能被关闭同时设置为飞行模式。 ====>>>> 建议手机关闭飞行模式，并打开WIFI开关。
    Error19,//定位失败，由于手机没插sim卡且WIFI功能被关闭。 ====>>>> 建议手机插上sim卡，打开WIFI开关。
    Error20,//定位失败，错误码：20。
    Error21,//IO 操作异常
    Error22,//连接异常
    Error23,//连接超时
    Error24,//无效的参数
    Error25,//空指针异常
    Error26,//URL异常
    Error27,//未知主机
    Error28,//连接服务器失败
    Error29,//通信协议解析错误
    Error30,//http 连接失败
    Error31,//未知的错误
    Error32,//Key鉴权验证失败，请检查key绑定的sha1值、包名与apk信息是否对应，或通过高频问题查找相关解决办法。
    Error33,//没有获取到设备的定位权限
    Error34,//无法获取城市信息
    Error35,//当前ip请求次数超过配额
}

/// <summary>
/// 错误信息
/// </summary>
public enum ErrorMessage
{
    定位失败_异常错误 = -1,
    定位成功 = 0,
    定位失败_一些重要参数为空,
    定位失败_由于仅扫描到单个WIFI_且没有基站信息,
    定位失败_获取到的请求参数为空_可能获取过程中出现异常,
    定位失败_请求服务器过程中的异常_多为网络情况差_链路不通导致,
    定位失败_请求被恶意劫持,
    定位服务返回定位失败,
    定位失败_Key鉴权失败,
    定位失败_AndroidException常规错误,
    定位初始化时出现异常,
    定位客户端启动失败,
    定位时的基站信息错误,
    定位失败_缺少定位权限,
    定位失败_由于未获得WIFI列表和基站信息_且GPS当前不可用,
    GPS定位失败_由于设备当前GPS状态差,
    定位失败_定位结果被模拟导致定位失败,
    定位失败_当前POI检索条件_行政区划检索条件下_无可用地理围栏,
    定位失败_错误码_17,
    定位失败_由于手机WIFI功能被关闭同时设置为飞行模式,
    定位失败_由于手机没插sim卡且WIFI功能被关闭,
    定位失败_错误码_20,
    定位失败_IO操作异常,
    定位失败_连接异常,
    定位失败_连接超时,
    定位失败_无效的参数,
    定位失败_空指针异常,
    定位失败_URL异常,
    定位失败_未知主机,
    定位失败_连接服务器失败,
    定位失败_通信协议解析错误,
    定位失败_Http连接失败,
    定位失败_未知的错误,
    定位失败_Key鉴权验证失败_请检查Key绑定的sha1值_包名与Apk信息是否对应_或通过高频问题查找相关解决办法,
    定位失败_没有获取到设备的定位权限,
    定位失败_无法获取城市信息,
    定位失败_当前IP请求次数超过配额,
}

/// <summary>
/// 安卓返回数据组成的地址结构
/// </summary>
public struct AMapLocationInfo
{
    public int NErrorCode;//错误码
    public string SErrorCode;//错误码

    public string SIdAddr;//地址
    public string SCountry;//国家
    public string SProvince;//省份为
    public string SCity;//城市为
    public string SDistrict;//城区为
    public string SStreet;//街道为
    public string SStreetNum;//街道牌号为
    public string SCityCode;//城市编码为
    public string SAdCode;//地区编码为
    public string SAoiInfo;//定位AOI信息为
    public double Longitude;//经度
    public double Latitude;//纬度
}

/// <summary>
/// 调用Android的获取地址的代码
/// </summary>
public class AMapLocation
{
    private AndroidJavaClass _jc;
    private AndroidJavaObject _jo;

    // 定义一个静态变量来保存类的实例
    private static AMapLocation _instance;

    // 定义一个标识确保线程同步
    private static readonly object locker = new object();

    // 定义私有构造函数，使外界不能创建该类实例
    private AMapLocation()
    {

    }

    /// <summary>
    /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
    /// </summary>
    /// <returns></returns>
    public static AMapLocation Instance
    {
        get
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            if (_instance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (_instance == null)
                    {
                        _instance = new AMapLocation();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 返回地址结构
    /// </summary>
    /// <returns></returns>
    public AMapLocationInfo GetAmapLocationInfo()
    {
        _jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _jo = _jc.GetStatic<AndroidJavaObject>("currentActivity");

        AMapLocationInfo amap = new AMapLocationInfo();
        try
        {
            amap.NErrorCode = GetErrorCode();
            amap.SErrorCode = ((ErrorMessage)amap.NErrorCode).ToString();

            amap.SIdAddr = GetLocationInfo();
            amap.SCountry = GetCountry();
            amap.SProvince = GetProvince();
            amap.SCity = GetCity();
            amap.SDistrict = GetDistrict();
            amap.SStreet = GetStreet();
            amap.SStreetNum = GetStreetNum();
            amap.SCityCode = GetCityCode();
            amap.SAdCode = GetAdCode();
            amap.SAoiInfo = GetAoiInfo();
            amap.Longitude = GetLongitude();
            amap.Latitude = GetLatitude();

            return amap;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            amap.NErrorCode = (int)ErrorCode.Error1_;
            amap.SErrorCode = ((ErrorMessage) (int) ErrorCode.Error1_).ToString();
            return amap;
        }
    }
    //在调用其他之前应该先调用该方法，因为在android中该方法会实例化定位SDK，而其他方法不会
    public int GetErrorCode()
    {
        return _jo.Call<int>("GetErrorCode");
    }
    //获取纬度
    public double GetLatitude()
    {
        return _jo.Call<double>("GetLatitude");
    }
    //获取经度
    public double GetLongitude()
    {
        return _jo.Call<double>("GetLongitude");
    }
    //获取精度
    public double GetAccuracys()
    {
        return _jo.Call<double>("GetAccuracys");
    }
    //获取具体地址
    public string GetLocationInfo()
    {
        string s = _jo.Call<string>("GetInfo");
        //if (s.Length > 32)
        //{
        //    string sNew = s.Substring(0, 32);
        //    return sNew;
        //}
        //else
        //{
        //    return s;
        //}
        return s;
    }
    //获得国家信息
    public string GetCountry()
    {
        return _jo.Call<string>("GetCountry");
    }
    //获取省信息
    public string GetProvince()
    {
        return _jo.Call<string>("GetProvince");
    }
    //获取城市信息
    public string GetCity()
    {
        return _jo.Call<string>("GetCity");
    }
    //获取城区信息
    public string GetDistrict()
    {
        return _jo.Call<string>("GetDistrict");
    }
    //获取街道信息
    public string GetStreet()
    {
        return _jo.Call<string>("GetStreet");
    }
    //获取街道牌号信息
    public string GetStreetNum()
    {
        return _jo.Call<string>("GetStreetNum");
    }
    //获取城市编码
    public string GetCityCode()
    {
        return _jo.Call<string>("GetCityCode");
    }
    //获取地区编码
    public string GetAdCode()
    {
        return _jo.Call<string>("GetAdCode");
    }
    //获取定位AOI信息
    public string GetAoiInfo()
    {
        return _jo.Call<string>("GetAoiName");
    }
}
