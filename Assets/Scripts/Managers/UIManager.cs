using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public enum UIStates { Main, Settings, CardHero, Fight }; //UI States
    public UIStates CurrentState;
    public GameObject MainPanel;
    public GameObject SettingsPanel; //Ayarlar butonuna t?kland???nda aç?lacak panel
    public GameObject HeroCardPanel; //Hero card butonuna t?kland???nda aç?lacak panel
    public GameObject TotalLifeBar;
    public GameObject LevelText;
    public Image HealthBar;
    float maxTotalHealth;
    float currentTotalHealth;
    GameObject[] _aliveHeroes;

    private void Awake()
    {
        CurrentState = UIStates.Main;
    }

    private void Update()
    {

        // UI durumlar?n?n kontrolü
        switch (CurrentState)
        {
            case UIStates.Main:
                MainPanel.SetActive(true);
                LevelText.SetActive(true);
                SettingsPanel.SetActive(false);
                HeroCardPanel.SetActive(false);
                TotalLifeBar.SetActive(false);
                break;
            case UIStates.Settings:
                SettingsPanel.SetActive(true);
                MainPanel.SetActive(true);
                LevelText.SetActive(true);
                HeroCardPanel.SetActive(false);
                TotalLifeBar.SetActive(false);
                break;
            case UIStates.CardHero:
                HeroCardPanel.SetActive(true);
                MainPanel.SetActive(true);
                LevelText.SetActive(true);
                SettingsPanel.SetActive(false);
                TotalLifeBar.SetActive(false);
                break;
            case UIStates.Fight:
                TotalLifeBar.SetActive(true);
                LevelText.SetActive(true);
                MainPanel.SetActive(false);
                SettingsPanel.SetActive(false);
                HeroCardPanel.SetActive(false);
                break;
        }
        _aliveHeroes = GameObject.FindGameObjectsWithTag("Hero");
        if(_aliveHeroes != null)
        {
            getTotalCurrentHealth();
        }
    }

    public void OnMainMenu()
    {
        CurrentState = UIStates.Main;
    }

    public void OnSettings()
    {
        //Settings butonununa t?klan?ld???nda
        CurrentState = UIStates.Settings;
    }

    public void OnCardHero()
    {
        //Hero card butonuna t?kland???nda
        CurrentState = UIStates.CardHero;
    }
    public void getTotalCurrentHealth()
    {
        currentTotalHealth = 0f;
        foreach (var hero in _aliveHeroes)
        {
            currentTotalHealth += hero.GetComponent<Fighter>().CurrentHealth;
        }
        HealthBar.fillAmount = currentTotalHealth / maxTotalHealth;
    }
    public void getTotalMaxHealth(GameObject[] heroList)
    {
        maxTotalHealth = 0;
        foreach (var hero in heroList)
        {
            maxTotalHealth += hero.GetComponent<Fighter>().characterSO.MaxHealth;
        }
    }
}
