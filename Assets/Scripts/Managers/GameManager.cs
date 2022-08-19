using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject GameUICanvas;
    public delegate void modeChangeDelegate(bool mode);
    //public static event modeChangeDelegate onModeChange;
    public bool _isStart = false;
    private int MeleeFighterV1Price = 10;
    private int WizardV1Price = 15;
    public int coins = 0;
    public float heroCount;
    public int currentLevel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinsText;
    //public List<GameObject> _HeroesList = new List<GameObject>();
    public GameObject[] HeroesList;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("heroes");
        //PlayerPrefs.DeleteKey("coins");
        //PlayerPrefs.DeleteKey("level");
        float heroCount;
        coins = PlayerPrefs.GetInt("coins"); //setlenmemi� bir playerpref al�nmaya �al���rsa carsay�lan olarak 0 geliyor
        coinsText.text = coins.ToString();
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

        levelText.text = "LEVEL " + currentLevel;
        SceneManager.LoadScene("Level_" + currentLevel, LoadSceneMode.Additive); //GameUI sahnesini silmeden �zerine Level sahnesinide y�kledi
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("GameUI", LoadSceneMode.Single); //T�m sahneleri �nce kapat�r, ard�ndan GameUI sahnesiin yeniden y�kler, GameUI da zaten playerPrefsten currentLevel i alarak ona g�re level sahnesini y�kledi�i i�in bu bize restart etkisi olu�turur. 
    }
    public void NextLevel()
    {
        if(currentLevel == 3) //son level i de kazand���m�z� ifa eder
        {
            Debug.Log("You have defeated all enemies!");

            PlayerPrefs.DeleteKey("heroes");
            PlayerPrefs.DeleteKey("coins");
            PlayerPrefs.DeleteKey("level");

             SceneManager.LoadScene("GameUI", LoadSceneMode.Single); //Ard�ndan varolan t�m sahneleri silip GameUI sahnesini ba�tan y�kledik, GameUI da level sahnesini playerprefsten g�ncel level i sorgulayarak y�kledi�i i�in her �ey t�k�r t�k�r t�k�r t�k�r i�ledi, bkz: bu scriptin Start() �.

            return;
        }
        PlayerPrefs.SetInt("level", currentLevel + 1); //�nce haf�zadaki g�ncel level i sonrakine ayarlad�k
        SceneManager.LoadScene("GameUI", LoadSceneMode.Single); //Ard�ndan varolan t�m sahneleri silip GameUI sahnesini ba�tan y�kledik, GameUI da level sahnesini playerprefsten g�ncel level i sorgulayarak y�kledi�i i�in her �ey t�k�r t�k�r t�k�r t�k�r i�ledi, bkz: bu scriptin Start() �.
    }

    public void isPlayModeOn()
    {
        _isStart = true;
        HeroesList = GameObject.FindGameObjectsWithTag("Hero");
        //GameUICanvas.SetActive(false);
        //onModeChange(_isStart);
        //UIManager.Instance.StartCoroutine(C)
        UIManager.Instance.getTotalMaxHealth(HeroesList);
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
        int heroCost = 0;
        if (heroName == "Wizard_v1")
        {
            heroCost = WizardV1Price;
        }else if (heroName == "MeleeFighter_v1")
        {
            heroCost = MeleeFighterV1Price;
        }

        if (coins - heroCost >= 0)
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

                if (heroName == "Wizard_v1")
                {
                    calculateCoin(-WizardV1Price); //sat�n al�nan karaktere g�re coins i azalt
                }
                else if (heroName == "MeleeFighter_v1")
                {
                    calculateCoin(-MeleeFighterV1Price); //sat�n al�nan karaktere g�re coins i azalt
                }
            }
        }
        
    }

    public void updateHeroListAfterMerge(string name, string mergedName)
    {
        string[] heroes = getHeroes();
        List<String> heroesList = heroes.ToList<String>();

        heroesList.Add(mergedName);

        string newHeroSet = "";
        string result = "";

        int counter = 0;
        foreach (string heroName in heroesList)
        {
            if(heroName == name && counter < 2)
            {
                counter += 1;
                continue;   
            }

            newHeroSet += heroName + ",";
        }
        string substr = newHeroSet.Substring(newHeroSet.Length - 1);
        if (substr == ",")
        {
            result = newHeroSet.Remove(newHeroSet.Length - 1);
        }

        Debug.Log("result:" + result);

        PlayerPrefs.SetString("heroes", result);
    }

    public void calculateCoin(int balance) //i�ine ald��� miktar� coinse ekler, oyun kapan�p a�t���nda kaybolmamas� i�in playerprefs e setler
    {
        coins += balance;
        PlayerPrefs.SetInt("coins", coins);
        coinsText.text = coins.ToString();
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
