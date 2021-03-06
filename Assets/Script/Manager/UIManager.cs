﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Menu Holders")]
    public GameObject MainMenu;
    public GameObject LevelSelection;
    public GameObject WinScreen;
    public GameObject DifficultySelectionPanel;
	public GameObject Gallery;
    public UiPauseManager PauseMng;
    public GameObject TutorialMenu;

    MenuType ActiveMenu = MenuType.MainMenu;

    private void OnEnable()
    {
        EventManager.EndLevel += ShowWinScreen;
    }
    private void OnDisable()
    {
        EventManager.EndLevel -= ShowWinScreen;
    }


    public void Init()
    {
        ToggleMenu(MenuType.MainMenu);
    }

    /// <summary>
    /// Funzione che attiva solo il menu passato come parametro
    /// </summary>
    /// <param name="_type"></param>
    public void ToggleMenu(MenuType _type)
    {
        DisableAllMenus();
        ActiveMenu = _type;
        if (_type == MenuType.None)
        {
            PauseMng.EnableArrow(true);
            return;
        }
        switch (_type)
        {
            case MenuType.MainMenu:
                GameManager.instance.PlayingPuzzle = null;
                GameManager.instance.DifficultySelected = null;
                MainMenu.SetActive(true);
                AudioManager.instance.TogglePlayVolume(false);
                AudioManager.instance.ToggleMenuVolume(true);
                break;
            case MenuType.LevelSelection:
                LevelSelection.SetActive(true);
                AudioManager.instance.TogglePlayVolume(false);
                AudioManager.instance.ToggleMenuVolume(true);
                break;
            case MenuType.PauseMenu:
                PauseMng.EnableArrow(true);
                if (EventManager.Pause != null)
                    EventManager.Pause();
                break;
            case MenuType.WinScreen:
                WinScreen.SetActive(true);
                break;
            case MenuType.DifficultyMenu:
                DifficultySelectionPanel.SetActive(true);
                AudioManager.instance.TogglePlayVolume(false);
                AudioManager.instance.ToggleMenuVolume(true);
                break;
			case MenuType.Gallery:
                PauseMng.EnableUpperCosina(false);
                Gallery.SetActive(true);
				AudioManager.instance.TogglePlayVolume(false);
				AudioManager.instance.ToggleMenuVolume(true);
				break;
            case MenuType.Tutorial:
                PauseMng.EnableUpperCosina(false);
                TutorialMenu.SetActive(true);
                AudioManager.instance.TogglePlayVolume(false);
                AudioManager.instance.ToggleMenuVolume(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Funzione che attiva solo l'index del menu passato come parametro 
    /// </summary>
    /// <param name="_type"></param>
    public void ToggleMenu(int _typeIndex)
    {
        DisableAllMenus();
        ActiveMenu = (MenuType)_typeIndex;
        if (_typeIndex == 0)
            return;
        switch (_typeIndex)
        {
            case (int)MenuType.MainMenu:
                GameManager.instance.PlayingPuzzle = null;
                GameManager.instance.DifficultySelected = null;
                MainMenu.SetActive(true);
                AudioManager.instance.TogglePlayVolume(false);
                AudioManager.instance.ToggleMenuVolume(true);
                break;
            case (int)MenuType.LevelSelection:
                LevelSelection.SetActive(true);
                AudioManager.instance.TogglePlayVolume(false);
                AudioManager.instance.ToggleMenuVolume(true);
                break;
            case (int)MenuType.PauseMenu:
                if (EventManager.Pause != null)
                    EventManager.Pause();
                break;
            case (int)MenuType.WinScreen:
                WinScreen.SetActive(true);
                break;
            case (int)MenuType.DifficultyMenu:
                DifficultySelectionPanel.SetActive(true);
                AudioManager.instance.TogglePlayVolume(false);
                AudioManager.instance.ToggleMenuVolume(true);
                break;
			case (int)MenuType.Gallery:
                PauseMng.EnableUpperCosina(false);
                Gallery.SetActive(true);
				AudioManager.instance.TogglePlayVolume(false);
				AudioManager.instance.ToggleMenuVolume(true);
				break;
            case (int)MenuType.Tutorial:
                PauseMng.EnableUpperCosina(false);
                TutorialMenu.SetActive(true);
                AudioManager.instance.TogglePlayVolume(false);
                AudioManager.instance.ToggleMenuVolume(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Funzione che ritorna il menu attivo
    /// </summary>
    /// <returns></returns>
    public MenuType GetActiveMenu()
    {
        return ActiveMenu;
    }

    /// <summary>
    /// Funzione che disabilita tutti i menu
    /// </summary>
    private void DisableAllMenus()
    {
        MainMenu.SetActive(false);
        LevelSelection.SetActive(false);
        WinScreen.SetActive(false);
        DifficultySelectionPanel.SetActive(false);
		Gallery.SetActive (false);
        PauseMng.EnableArrow(false);
        PauseMng.EnableUpperCosina(true);
        TutorialMenu.SetActive(false);

        if (GetActiveMenu() == MenuType.PauseMenu)
        {
            if (EventManager.UnPause != null)
                EventManager.UnPause();
        }
    }

    /// <summary>
    /// Funzione che chiama la coroutine per mostrare il WinScreen
    /// </summary>
    private void ShowWinScreen()
    {
        StartCoroutine(ShowWinScreenCoroutine());
    }

    /// <summary>
    /// Coroutine che mostra il winscreen
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowWinScreenCoroutine()
    {
        yield return new WaitForSeconds(1f);
        ToggleMenu(MenuType.WinScreen);
    }
}

public enum MenuType
{
    None = 0,
    MainMenu = 1,
    LevelSelection = 2,
    WinScreen = 3,
    PauseMenu = 4,
    DifficultyMenu = 5,
	Gallery = 6,
    Tutorial = 7,
}
