using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public enum UIStates { Main, Settings, CardHero, Fight, Defeat, Victory }; //UI States
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

    public GameObject MeleeCards;
    public GameObject WizardCards;

    public GameObject DefeatPanel;
    public GameObject VictoryPanel;

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
                DefeatPanel.SetActive(false);
                VictoryPanel.SetActive(false);
                break;
            case UIStates.Settings:
                SettingsPanel.SetActive(true);
                MainPanel.SetActive(true);
                LevelText.SetActive(true);
                HeroCardPanel.SetActive(false);
                TotalLifeBar.SetActive(false);
                DefeatPanel.SetActive(false);
                VictoryPanel.SetActive(false);
                break;
            case UIStates.CardHero:
                HeroCardPanel.SetActive(true);
                MainPanel.SetActive(true);
                LevelText.SetActive(true);
                SettingsPanel.SetActive(false);
                TotalLifeBar.SetActive(false);
                DefeatPanel.SetActive(false);
                VictoryPanel.SetActive(false);
                break;
            case UIStates.Fight:
                TotalLifeBar.SetActive(true);
                LevelText.SetActive(true);
                MainPanel.SetActive(false);
                SettingsPanel.SetActive(false);
                HeroCardPanel.SetActive(false);
                DefeatPanel.SetActive(false);
                VictoryPanel.SetActive(false);
                break;
            case UIStates.Defeat:

                DefeatPanel.SetActive(true);
                VictoryPanel.SetActive(false);
                TotalLifeBar.SetActive(false);
                LevelText.SetActive(false);
                MainPanel.SetActive(false);
                SettingsPanel.SetActive(false);
                HeroCardPanel.SetActive(false);
                break;
            case UIStates.Victory:

                VictoryPanel.SetActive(true);
                DefeatPanel.SetActive(false);
                TotalLifeBar.SetActive(false);
                LevelText.SetActive(false);
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

    public void OnDefeat()
    {
        CurrentState = UIStates.Defeat;
    }
    
    public void OnVictory()
    {
        CurrentState=UIStates.Victory;
    }

    public void ShowMeleeCards()
    {
        MeleeCards.SetActive(true);
        WizardCards.SetActive(false);
    }

    public void ShowWizardCards()
    {
        WizardCards.SetActive(true);
        MeleeCards.SetActive(false);
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
