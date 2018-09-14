using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [Header("Menu Holders")]
    public GameObject MainMenu;
    public GameObject LevelSelection;
    public GameObject WinScreen;
    MenuType ActiveMenu = MenuType.MainMenu;

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
            return;
        switch (_type)
        {
            case MenuType.MainMenu:
                MainMenu.SetActive(true);
                break;
            case MenuType.LevelSelection:
                LevelSelection.SetActive(true);
                break;
            case MenuType.PauseMenu:
                break;
            case MenuType.WinScreen:
                WinScreen.SetActive(true);
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
                MainMenu.SetActive(true);
                break;
            case (int)MenuType.LevelSelection:
                LevelSelection.SetActive(true);
                break;
            case (int)MenuType.PauseMenu:
                break;
            case (int)MenuType.WinScreen:
                WinScreen.SetActive(true);
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
    }
}

public enum MenuType
{
    None = 0,
    MainMenu = 1,
    LevelSelection = 2,
    WinScreen = 3,
    PauseMenu = 4,
}
