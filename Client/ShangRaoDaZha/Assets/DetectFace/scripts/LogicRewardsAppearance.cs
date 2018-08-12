using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicRewardsAppearance : MonoBehaviour {

    public const int VALUE_REWARDS_GOLD = 1;
    public const int VALUE_REWARDS_DIAMOND = 2;
    public const int VALUE_REWARDS_ROOM_CARDS = 3;

    const int LEVEL_ONE_POINTS = 70;
    const int LEVEL_TWO_POINTS = 80;

    private string mRewardsLevel = REWARDS_LEVEL_ONE;
    private GameObject mRewardsGameObject;
    private UILabel mRewardsTxt;

    public const string REWARDS_LEVEL_ONE = "1";
    public const string REWARDS_LEVEL_TWO = "2";
    public const string REWARDS_LEVEL_THREE = "3";

    public UILabel txtGold;
    public UILabel txtDiamond;
    public UILabel txtRoomCards;

    public UILabel txtGoldStep1;
    public UILabel txtDiamondStep1;
    public UILabel txtRoomCardsStep1;

    public GameObject goGold;
    public GameObject goDiamond;
    public GameObject goRoomCards;

    public string RewardsLevel{
        get{return mRewardsLevel;}
        set{
            mRewardsLevel = value;
            updateUI();
        }
    }

    public void updateUI(){
        goGold.SetActive(false);
        goDiamond.SetActive(false);
        goRoomCards.SetActive(false);

        mRewardsGameObject = goRoomCards;


        switch(mRewardsLevel){
            case REWARDS_LEVEL_ONE:
                {
                    //mRewardsGameObject = goGold;
                    //mRewardsTxt = txtGold;
                    txtRoomCards.text = VALUE_REWARDS_GOLD.ToString()+ "房卡";
                    break;
                }
            case REWARDS_LEVEL_TWO:
                {
                    //mRewardsGameObject = goDiamond;
                    //mRewardsTxt = txtDiamond;
                    txtRoomCards.text = VALUE_REWARDS_DIAMOND.ToString()+ "房卡";
                    break;
                }
            case REWARDS_LEVEL_THREE:
                {
                    //mRewardsGameObject = goRoomCards;
                    //mRewardsTxt = txtRoomCards;

                    txtRoomCards.text = VALUE_REWARDS_ROOM_CARDS.ToString()+ "房卡";
                    break;
                }
        }

        mRewardsGameObject.SetActive(true);
    }

    private void OnEnable()
    {
        EventCenter.instance.AddListener<ShowRewardsEvent>(OnShowRewardsEventHandler);
    }

    private void OnDisable()
    {
        EventCenter.instance.RemoveListener<ShowRewardsEvent>(OnShowRewardsEventHandler);
    }

    public void OnShowRewardsEventHandler(ShowRewardsEvent evt){
        RewardsLevel = getRewardsLevel(evt.points);
    }

    public static string getRewardsLevel(int aPoints){
        string tmpLevel = REWARDS_LEVEL_ONE;
        if (aPoints < LEVEL_ONE_POINTS)
        {
            tmpLevel = REWARDS_LEVEL_ONE;
        }
        else if (aPoints >= LEVEL_ONE_POINTS && aPoints < LEVEL_TWO_POINTS)
        {
            tmpLevel = REWARDS_LEVEL_TWO;
        }
        else
        {
            tmpLevel = REWARDS_LEVEL_THREE;
        }
        return tmpLevel;
    }

    private void Start()
    {
        txtGold.text = VALUE_REWARDS_GOLD.ToString() + "房卡";
        txtDiamond.text = VALUE_REWARDS_DIAMOND.ToString() + "房卡";
        txtRoomCards.text = VALUE_REWARDS_ROOM_CARDS.ToString() + "房卡";

        txtGoldStep1.text = VALUE_REWARDS_GOLD.ToString()+ "房卡";
        txtDiamondStep1.text = VALUE_REWARDS_DIAMOND.ToString() + "房卡";
        txtRoomCardsStep1.text = VALUE_REWARDS_ROOM_CARDS.ToString()+ "房卡";
    }


    private void Update()
    {

    }
}
