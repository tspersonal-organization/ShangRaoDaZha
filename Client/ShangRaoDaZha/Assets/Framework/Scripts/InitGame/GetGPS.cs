using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGPS : MonoBehaviour {

    public string gps_info = "";
    public int flash_num = 1;

    public static GetGPS Instance = null;
    // Use this for initialization  
    void Awake()
    {
        //StartCoroutine(StartGPS());
        Instance = this;
    }

    public void InitGPS()
    {
        StartCoroutine(StartGPS());
    }

    void StopGPS()
    {
        Input.location.Stop();
    }

    IEnumerator StartGPS()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置  
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            this.gps_info = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
        }
        else
        {
            Input.location.Start(10.0f, 10.0f);
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                // 暂停协同程序的执行(1秒)  
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < 1)
            {
                this.gps_info = "Init GPS service time out";
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                this.gps_info = "位置服务失败（用户拒绝访问位置服务）";
            }
            else
            {
                //this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
                //this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
                yield return new WaitForSeconds(100);
            }
        }
        yield break;
    }

}
