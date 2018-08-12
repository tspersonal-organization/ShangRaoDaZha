 using FrameworkForCSharp.NetWorks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppPurchasing : MonoBehaviour,IStoreListener
{
    public static string[] ProductIDs = new string[] 
    {
        "com.YouthGamer.XianYuGou.001",
        "com.YouthGamer.XianYuGou.002",
        "com.YouthGamer.XianYuGou.003",
        "com.YouthGamer.XianYuGou.004"
    };

    private static IStoreController m_Controller;//存储商品信息
    private static IAppleExtensions m_AppleExtensions;

    private static bool InternetAvailable;//是否初始化成功

    public static InAppPurchasing Instance = null;

    // Use this for initialization
    void Start ()
    {
        Instance = this;
        if (Application.platform == RuntimePlatform.IPhonePlayer) InitUnityPurchase();
	}
    public void InitUnityPurchase()//初始化方法
    {
        StandardPurchasingModule module = StandardPurchasingModule.Instance();//标准采购模块
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);//配置模式
        //ProductType的类型，Consumable是可以无限购买(比如水晶)，NonConsumable是只能购买一次(比如关卡)，Subscription是每月订阅(比如VIP)
        for (int i = 0; i < ProductIDs.Length; i++)
        {
            builder.AddProduct(ProductIDs[i], ProductType.Consumable);
        }
        //开始初始化
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_Controller = controller;
        InternetAvailable = true;//初始化成功
        Debug.Log("IAP初始化成功");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        InternetAvailable = false;
        if (error == InitializationFailureReason.PurchasingUnavailable)
        {
            Debug.Log("手机设置了禁止APP内购");
            GameData.ResultCodeStr = "手机设置了禁止APP内购";
            UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_Dialog, OpenPanelType.MinToMax);
        }
        Debug.Log("IAP初始化失败");
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        UIManager.Instance.HideUIPanel(UIPaths.LoadingInApp);
        Debug.Log("购买失败");
        GameData.ResultCodeStr = "购买失败!!!";
        UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_Dialog, OpenPanelType.MinToMax);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        UIManager.Instance.HideUIPanel(UIPaths.LoadingInApp);
        Debug.Log("transactionID:" + e.purchasedProduct.transactionID);
        Debug.Log("receipt:" + e.purchasedProduct.receipt);

        GameData.ResultCodeStr = "购买成功!!!";
        UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_Dialog, OpenPanelType.MinToMax);
        switch(e.purchasedProduct.definition.id)
        {
            case "com.YouthGamer.XianYuGou.001":
                Player.Instance.money += 6;
                break;
            case "com.YouthGamer.XianYuGou.002":
                Player.Instance.money += 32;
                break;
            case "com.YouthGamer.XianYuGou.003":
                Player.Instance.money += 75;
                break;
            case "com.YouthGamer.XianYuGou.004":
                Player.Instance.money += 150;
                break;
        }
        GameEventDispatcher.Instance.dispatchEvent(EventIndex.PlayerLivingDataChange);
        //ClientToServerMsg.Send(Opcodes.Client_AppleStore_Completed, e.purchasedProduct.transactionID, e.purchasedProduct.receipt);
        return  PurchaseProcessingResult.Complete;
    }

    public void BuyProduct(int productIndex)
    {
        if (productIndex >= ProductIDs.Length)
        {
            GameData.ResultCodeStr = "不存在该物品！！";
            UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_Dialog, OpenPanelType.MinToMax);
            return;
        }
        UIManager.Instance.ShowUIPanel(UIPaths.LoadingInApp);
        m_Controller.InitiatePurchase(ProductIDs[productIndex]);
    }
}
