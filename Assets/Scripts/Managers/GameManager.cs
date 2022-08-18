using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject GameUICanvas;
    public delegate void modeChangeDelegate(bool mode);
    public static event modeChangeDelegate onModeChange;
    public bool _isStart = false;
    public float heroCount;
    public int currentLevel;

    private void Start()
    {
        float heroCount;

        DontDestroyOnLoad(gameObject);
        PlayerPrefs.DeleteKey("heroes"); // BUNU EN SON SÝL HA
        //PlayerPrefs.SetString("heroes", "MeleeFighter_v1,MeleeFighter_v1,Wizard_v1,Wizard_v2,MeleeFighter_v2");
        currentLevel = PlayerPrefs.GetInt("level");
        if (PlayerPrefs.GetString("heroes").Length > 0)
        {
            heroCount = PlayerPrefs.GetString("heroes").Split(',').Length;
        }
        else
        {
            heroCount = 0;
            PlayerPrefs.SetString("heroes", "MeleeFighter_v1");
        }

        if (currentLevel == 0)
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("level", 1);
        }
        SceneManager.LoadScene("Level_" + currentLevel, LoadSceneMode.Additive);
    }
    public void isPlayModeOn()
    {
        _isStart = true;
        //GameUICanvas.SetActive(false);
        //onModeChange(_isStart);
        UIManager.Instance.CurrentState = UIManager.UIStates.Fight;
    }
    public void isPlayModeOff()
    {
        _isStart = false;
        //GameUICanvas.SetActive(true);
        //onModeChange(_isStart);
        UIManager.Instance.CurrentState = UIManager.UIStates.Main;
    }

    public string[] getHeroes()
    {
        return PlayerPrefs.GetString("heroes").Split(',');
    }

    public void buyHero(string heroName)
    {
        string[] heroes = getHeroes();
        string newHeroes = "";
        for (int i = 0; i < heroes.Length; i++)
        {
            newHeroes += heroes[i] + ",";
        }
        newHeroes += heroName;


        if (LevelManager.Instance.placeIfAvailable(heroName))
        {
            PlayerPrefs.SetString("heroes", newHeroes);
            //coin -= 20;
        }
    }
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoSingleton<GameManager>
//{
//    public GameObject GameUICanvas;
//    public delegate void modeChangeDelegate(bool mode);
//    public static event modeChangeDelegate onModeChange;
//    public bool _isStart = false;


//    public int currentLevel;

//    private void Start()
//    {
//        DontDestroyOnLoad(gameObject);
//        currentLevel = PlayerPrefs.GetInt("level");


//        if (currentLevel == 0)
//        {
//            currentLevel = 1;
//            PlayerPrefs.SetInt("level", 1);
//        }
//        SceneManager.LoadScene("Level_" + currentLevel, LoadSceneMode.Additive);
//    }
//    public void isPlayModeOn()
//    {
//        _isStart = true;
//        GameUICanvas.SetActive(false);
//        onModeChange(_isStart);
//    }
//    public void isPlayModeOff()
//    {
//        _isStart = false;
//        GameUICanvas.SetActive(true);       
//        onModeChange(_isStart);
//    }



//}
