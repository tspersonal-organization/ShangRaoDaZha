using System.Collections;
using System.Text;
using UnityEngine;

public class AmapLocationManager : MonoBehaviour
{
    public static AmapLocationManager Instance = null;
    private int _nReqAmapLocationCount = 0;//请求定位的次数
    private int _nMAxReqAmapLocationCount = 10;//请求定位的最大次数

    private void Start()
    {
        Instance = this;
    }

    public void StartAmapLocation()
    {
        StartCoroutine(IeGetLocation());
    }

    IEnumerator IeGetLocation()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            string sError = "定位失败！请打开GPS";
            GlobalModule.Instance.OnOpenBubblingHint(sError);
            Debug.Log(sError);
            //GameData.Tips = sError;
            //UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            string sError = "定位失败！用户拒绝访问位置服务";
            GlobalModule.Instance.OnOpenBubblingHint(sError);
            Debug.Log(sError);
            //GameData.Tips = sError;
            //UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
            yield break;
        }
        Input.location.Start(10.0f, 10.0f);
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)  
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1)
        {
            string sError = "定位超时！";
            GlobalModule.Instance.OnOpenBubblingHint(sError);
            Debug.Log(sError);
            //GameData.Tips = "定位超时！";
            //GameData.Tips = sError;
            //UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
            yield break;
        }
        //this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
        //this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;

        if (_nReqAmapLocationCount > _nMAxReqAmapLocationCount)
            yield break;
        if (_nReqAmapLocationCount > 0)
            yield return new WaitForSeconds(3);
        GetLocationInfo();
    }

    private void GetLocationInfo()
    {
        _nReqAmapLocationCount++;
        string sError = "第" + _nReqAmapLocationCount + "次定位！";
        Debug.Log(sError);
#if UNITY_ANDROID
        AMapLocationInfo amap = AMapLocation.Instance.GetAmapLocationInfo();
        if (amap.NErrorCode == 0)
        {
            StringBuilder sb = new StringBuilder();
            string sErrorCode = "错误码为：" + amap.NErrorCode + "\n";
            Debug.Log("错误码为：" + sErrorCode);
            string sIdAddr = "地址为：" + amap.SIdAddr + "\n";
            Debug.Log("地址为：" + sIdAddr);
            string sCountry = "国家为：" + amap.SCountry + "\n";
            Debug.Log("国家为：" + sCountry);
            string sProvince = "省份为：" + amap.SProvince + "\n";
            Debug.Log("省份为：" + sProvince);
            string sCity = "城市为：" + amap.SCity + "\n";
            Debug.Log("城市为：" + sCity);
            string sDistrict = "城区为：" + amap.SDistrict + "\n";
            Debug.Log("城区为：" + sDistrict);
            string sStreet = "街道为：" + amap.SStreet + "\n";
            Debug.Log("街道为：" + sStreet);
            string sStreetNum = "街道牌号为：" + amap.SStreetNum + "\n";
            Debug.Log("街道牌号为：" + sStreetNum);
            string sCityCode = "城市编码为：" + amap.SCityCode + "\n";
            Debug.Log("城市编码为：" + sCityCode);
            string sAdCode = "地区编码为：" + amap.SAdCode + "\n";
            Debug.Log("地区编码为：" + sAdCode);
            string sAoiInfo = "定位AOI信息为：" + amap.SAoiInfo + "\n";
            Debug.Log("定位AOI信息为：" + sAoiInfo);
            string sLongitudes = "经度为：" + amap.Longitude + "\n";
            Debug.Log("经度为：" + sLongitudes);
            string sLatitudes = "纬度为：" + amap.Latitude + "\n";
            Debug.Log("纬度为：" + sLatitudes);

            sb.Append(sIdAddr);
            sb.Append(sCountry);
            sb.Append(sProvince);
            sb.Append(sCity);
            sb.Append(sDistrict);
            sb.Append(sStreet);
            sb.Append(sStreetNum);
            sb.Append(sCityCode);
            sb.Append(sAdCode);
            sb.Append(sAoiInfo);
            sb.Append(sLongitudes);
            sb.Append(sLatitudes);

            if (sIdAddr.Contains("未找到地址"))
            {
                Player.Instance.Address = "未找到地址！请检查定位信息...";
                StartAmapLocation();
            }
            else
            {
                Player.Instance.Address = amap.SIdAddr;
                //发送定位信息
                ClientToServerMsg.SetAdrress(amap.SIdAddr);
            }
        }
        else
        {
            GameData.Tips = ((ErrorMessage)amap.NErrorCode).ToString();
            UIManager.Instance.ShowUiPanel(UIPaths.PanelTips, OpenPanelType.MinToMax);
            SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
            Debug.Log(((ErrorMessage)amap.NErrorCode).ToString());
        }
#endif
    }
}
