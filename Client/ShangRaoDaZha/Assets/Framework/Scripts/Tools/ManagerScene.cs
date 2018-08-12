using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    InitGame,
    Login,
    Main,
    Game,
    GameJinBi,
    WDHGame,
    WDHGameJinBi,
    ZBGame,
    ZBGameJinBi,
    NiuNiu,
}

public class ManagerScene
{
    static ManagerScene instnce = null;
    public  SceneType currentSceneType;
    
    public static ManagerScene Instance
    {
        get
        {
            if (instnce == null)
                instnce = new ManagerScene();
            return instnce;
        }
    }
    void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadScene(SceneType type)
    {
        switch (type)
        {
            case SceneType.InitGame:
                currentSceneType = SceneType.InitGame;
                break;
            case SceneType.Login:
                LoadScene("01_Login");
                currentSceneType = SceneType.Login;
                break;
            case SceneType.Main:
                LoadScene("02_Main");
                currentSceneType = SceneType.Main;
                break;
            case SceneType.Game:
                LoadScene("03_Game");
                currentSceneType = SceneType.Game;
                break;
            case SceneType.GameJinBi:
                LoadScene("06_GameJinBi");
                currentSceneType = SceneType.GameJinBi;
                break;
            case SceneType.WDHGame:
                LoadScene("04_WuDangHuGame");
                currentSceneType = SceneType.WDHGame;
                break;
            case SceneType.WDHGameJinBi:
                LoadScene("07_WuDangHuGamJinBi");
                currentSceneType = SceneType.WDHGameJinBi;
                break;
            case SceneType.ZBGame:
                LoadScene("05_ZaiBaoGame");
                currentSceneType = SceneType.ZBGame;
                break;
            case SceneType.ZBGameJinBi:
                LoadScene("08_ZaiBaoGameJinBi");
                currentSceneType = SceneType.ZBGameJinBi;
                break;
            case SceneType.NiuNiu:
                LoadScene("09_NiuNiu");
                currentSceneType = SceneType.NiuNiu;
                break;
            default:
                Log.Debug("没有找到场景" + type.ToString());
                break;
        }
    }
}
