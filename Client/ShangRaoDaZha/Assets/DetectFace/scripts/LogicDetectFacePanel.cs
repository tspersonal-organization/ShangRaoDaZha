using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cn.sharesdk.unity3d;
using ExifLib;
using FrameworkForCSharp.NetWorks;
using UnityEngine;
using UnityEngine.UI;

public class LogicDetectFacePanel : UIBase<LogicDetectFacePanel> {

    private const string DEFAULT_IMAGE_NAME = "picture.png";

    CommentController commentCon;

    public UIButton btnCloseStep1;
    public UIButton btnCloseStep2;
	public UIButton btnTakePhoto;

    public UIButton btnShare;

    public GameObject panelStep1;
    public GameObject panelStep2;

    const double EXPIRED_SECONDS = 2592000;

    [SerializeField]
    public GameObject _imageResult;

    public UIButton FaceBtn;

    public UILabel txtPoints;
    public UILabel txtAge;
    public UILabel txtMotion;

    public UILabel txtName;

    public UILabel txtComments;


    public UILabel txtRewards;

    public GameObject imgRewards;

    public int curPoitns = 0;

    //请求服务器 获得奖励
    public void RewardUserRequest()
    {
        //获得奖励的等级
        string rewardLevel = LogicRewardsAppearance.getRewardsLevel(curPoitns);

        uint a = uint.Parse(rewardLevel);
        switch (a)
        {
            case 1:
                uint index = 1;
                ClientToServerMsg.Send(Opcodes.Client_Selfie, index);
                break;
            case 2:
                uint index1 = 2;
                ClientToServerMsg.Send(Opcodes.Client_Selfie, index1);
                break;
            case 3:
                uint index2 = 3;
                ClientToServerMsg.Send(Opcodes.Client_Selfie, index2);
                break;
        }
    }

    // Use this for initialization
    void Start () {

        commentCon = new CommentController();

        if(btnCloseStep1!=null)
        btnCloseStep1.onClick.Add(new EventDelegate(this.close));
        if(btnCloseStep2!=null)
        btnCloseStep2.onClick.Add(new EventDelegate(this.close));


        setParams();

        jumpBackToStep1();
    }

    public void OnBtnShareClick(){
        //if (btnCloseStep2 != null)
            //btnCloseStep2.GetComponent<UISprite>().enabled = false;

        AuthorizeOrShare.shareFunctionName = AuthorizeOrShare.SFN_DETECTFACE;
        AuthorizeOrShare.Instance.ShareCapture(
            new PlatformType[] {PlatformType.WeChatMoments } );
    }

    void OnEnable()
    {
        EventCenter.instance.AddListener<DetectFaceSharedEvent>(this.OnSharedHandler);
        EventCenter.instance.AddListener<ShareFailureEvent>(this.OnShareFailureHandler);
    }

    void OnDisable()
    {
        EventCenter.instance.RemoveListener<DetectFaceSharedEvent>(this.OnSharedHandler);
        EventCenter.instance.RemoveListener<ShareFailureEvent>(this.OnShareFailureHandler);
    }

    public void OnShareFailureHandler(ShareFailureEvent evt){
        if (btnCloseStep2 != null)
            btnCloseStep2.GetComponent<UISprite>().enabled = true;

        GameData.ResultCodeStr = "分享失败!";
        UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_Dialog, OpenPanelType.MinToMax);
    }

    //分享成功的回调
    public void OnSharedHandler(DetectFaceSharedEvent evt){
        if (btnCloseStep2 != null)
            btnCloseStep2.GetComponent<UISprite>().enabled = true;

        RewardUserRequest();

        GameData.ResultCodeStr = "分享成功!";
        UIManager.Instance.ShowUIPanel(UIPaths.UIPanel_Dialog,OpenPanelType.MinToMax);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void close(){
        UIManager.Instance.HideUIPanel(ConstDetectFace.FacePanel);
    }


    void takePhoto()
    {
        showlog("启动拍摄");
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Native.Instance.CameraPicture(DEFAULT_IMAGE_NAME, PictureLoaded, 512);
        }
        else 
        {
            testLocalPhoto();
        }
    }

    void testLocalPhoto2(){

        object textureObj = Resources.Load("test2",typeof(Texture2D));
        Texture2D texture = (Texture2D)textureObj;

        //texture.anisoLevel = 1;
        //texture.filterMode = FilterMode.Bilinear;
        //texture.wrapMode = TextureWrapMode.Clamp;

        //scale to small size
        int aScaleSize = MediaBase.getAScaleSize(texture.width);
        TextureScale.Bilinear(texture, texture.width / aScaleSize, texture.height / aScaleSize);

        JpegInfo jpi = ExifReader.ReadJpeg(texture.EncodeToJPG(), "Foo");
        int angle = 0;
        switch (jpi.Orientation)
        {
            case ExifOrientation.TopLeft:
                break;
            case ExifOrientation.TopRight:
                angle = -90;
                break;
            case ExifOrientation.BottomLeft:
                angle = 90;
                break;
            case ExifOrientation.BottomRight:
                angle = 180;
                break;
        }
        texture = TextureRotate.RotateImage(texture, angle);

        Sprite picture = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1);
        PictureLoaded(true, "", picture);
    }

    void testLocalPhoto()
    {
        string url = "file:///Users/leon49/Documents/test2.jpg";
        LoadPicture(url);
    }

    private void LoadPicture(string path)
    {
        StartCoroutine(LoadPictureCoroutine(path));
    }

    IEnumerator LoadPictureCoroutine(string path)
    {
        WWW www = new WWW(path);

        if (www.size == 0)
        {
            showlog("size 0");
            yield return new WaitForSeconds(1.0f);
            LoadPicture(path);
        }
        else if (!string.IsNullOrEmpty(www.error))
        {
            showlog("error");
            PictureLoaded(false, null, null);
        }
        else
        {
            //ExifLib.JpegInfo jpi = ExifLib.ExifReader.ReadJpeg(www.bytes, "test2.jpg");
            //showlog("EXIF: " + jpi.Orientation);

            //Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.RGB24, false);

            //texture.anisoLevel = 1;
            //texture.filterMode = FilterMode.Bilinear;
            //texture.wrapMode = TextureWrapMode.Clamp;

            //www.LoadImageIntoTexture(texture);

            //TextureScale.Bilinear(texture, texture.width / 5, texture.height / 5);
            //texture = TextureRotate.RotateImage(texture, -90);


            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.RGB24, false);

            texture.anisoLevel = 1;
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;

            www.LoadImageIntoTexture(texture);

            //scale to small size
            int aScaleSize = MediaBase.getAScaleSize(texture.width);
            TextureScale.Bilinear(texture, texture.width / aScaleSize, texture.height / aScaleSize);

            JpegInfo jpi = ExifReader.ReadJpeg(www.bytes, "Foo");
            int angle = 0;
            switch (jpi.Orientation)
            {
                case ExifOrientation.TopLeft:
                    break;
                case ExifOrientation.TopRight:
                    angle = -90;
                    break;
                case ExifOrientation.BottomLeft:
                    angle = 90;
                    break;
                case ExifOrientation.BottomRight:
                    angle = 180;
                    break;
            }
            texture = TextureRotate.RotateImage(texture, angle);

            Sprite picture = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1);
            PictureLoaded(true, path, picture);
        }
    }

    private void PictureLoaded(bool success, string aPath, Sprite aSpr)
    {
        if (success)
        {
			Texture2D aTex = aSpr.texture;

            float widthSquare = 240;

            UITexture tex = _imageResult.GetComponent<UITexture>();
            tex.mainTexture = aTex;


            if( aTex.width > aTex.height ){
                tex.width = (int)((float)aTex.width/((float)aTex.height/widthSquare));
                tex.height =(int) widthSquare;
            }else{
                tex.width = (int)widthSquare;
                tex.height = (int)((float)aTex.height /((float)aTex.width / widthSquare));
            }
            
            showlog("拍摄成功! ");

            detectfaceRequest(aTex.EncodeToJPG());


            print("SUCCESS: " + aTex.width + "x" + aTex.height + "px");
        }
    }

    void detectfaceRequest(byte[] inArray)
    {
        if( txtName!=null )
            txtName.text = Player.Instance.otherName;

        jumpToStep2();

        showlog("数组长度 " + inArray.Length);

        string expired = Utility.UnixTime(EXPIRED_SECONDS);
        string methodName = "youtu/api/detectface";
        StringBuilder postData = new StringBuilder();
        string pars = "\"app_id\":\"" + Conf.Instance().APPID +
                      "\",\"image\":\"" + Convert.ToBase64String(inArray.ToArray()) +
                      "\",\"mode\":1";
        postData.Append("{");
        postData.Append(pars);
        postData.Append("}");
        string result = Http.HttpPost(methodName, postData.ToString(), Auth.appSign(expired, Conf.Instance().USER_ID));

        showlog("http 请求完成" + " 长度" + result.Length);

        print(result);
        faceDetectedHandler(result);
    }

    void faceDetectedHandler(string result){
        YouTuResponseData tData = YouTuResponseData.fromJson(result);
        showlog(JsonUtility.ToJson(tData));
        if (tData != null && tData.errorcode == 0)
        {
            try
            {
                //测试
                //tData.face[0].beauty = 71;
                if (tData.face[0].beauty > 98) tData.face[0].beauty = 98;     

                txtPoints.text = tData.face[0].beauty.ToString();
                txtAge.text = tData.face[0].age.ToString();
                string strMotion = "";
                int exp = tData.face[0].expression;
                if (exp < 30)
                {
                    strMotion = "黯然伤神";
                }
                else if (exp > 30 && exp < 70)
                {
                    strMotion = "微微一笑";
                }
                else
                {
                    strMotion = "开怀大笑";
                }
                txtMotion.text = strMotion;

                //评语
                txtComments.text = commentCon.GetCommentText(tData.face[0].beauty);
                //播放对应的声音
                commentCon.PlayCommentSound(tData.face[0].beauty);

                ShowRewards(tData.face[0].beauty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    void ShowRewards(int aPoints){
        curPoitns = aPoints;
        EventCenter.instance.Raise(new ShowRewardsEvent(aPoints));
    }

    void showlog(string aStr)
    {
        print(aStr);
    }

    void setParams()
    {
        string appid = "10110438";
        string secretId = "AKIDKsJbhcgtGZoslToKUUJxRkvwtnjVrcu2";
        string secretKey = "86xeJpBMPLqIcnpkW9e4OdkgRft20mNm";
        string userid = "274333243";

        Conf.Instance().setAppInfo(appid, secretId, secretKey, userid, Conf.Instance().YOUTU_END_POINT);
    }

    private void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void jumpToStep2()
    {
        panelStep1.SetActive(false);
        panelStep2.SetActive(true);
    }

    public void jumpBackToStep1()
    {
        panelStep1.SetActive(true);
        panelStep2.SetActive(false);
        txtPoints.text = "";
        txtAge.text = "";
        txtMotion.text = "";
    }
}
