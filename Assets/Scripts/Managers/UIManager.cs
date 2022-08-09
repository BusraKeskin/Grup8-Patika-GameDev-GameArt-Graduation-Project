using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIStates { Main, Settings, CardHero }; //UI States
    public UIStates CurrentState;
    public GameObject MainPanel;
    public GameObject SettingsPanel; //Ayarlar butonuna t?kland???nda aç?lacak panel
    public GameObject HeroCardPanel; //Hero card butonuna t?kland???nda aç?lacak panel

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
                SettingsPanel.SetActive(false);
                HeroCardPanel.SetActive(false);
                break;
            case UIStates.Settings:
                SettingsPanel.SetActive(true);
                MainPanel.SetActive(true);
                HeroCardPanel.SetActive(false);
                break;
            case UIStates.CardHero:
                HeroCardPanel.SetActive(true);
                MainPanel.SetActive(true);
                SettingsPanel.SetActive(false);
                break;
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        EventManager.loadSameScene?.Invoke(); //Event calistirma yontemi
    //    }
    //}


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
}
