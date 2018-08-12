using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinBiDataControl  {

    private static JinBiDataControl _instance;
    public static JinBiDataControl Instance
    {
        get { if (_instance == null)
            {
                _instance = new JinBiDataControl();
            }

            return _instance;
        }
       
    }

    public uint TaoShangLimitGold;
    public uint TaoSHangRate;

    public uint WDHLimitGold;
    public uint WDHRate;

    public uint ZBLimitGold;
    public uint ZBRate;
}
