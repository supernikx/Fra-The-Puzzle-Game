using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPauseManager : MonoBehaviour
{
    Animator anim;
	[SerializeField]
	Button PauseButton;
    [SerializeField]
    GameObject ArrowImage;
    [SerializeField]
    Image PuzzleImage;
    [SerializeField]
    GameObject UpperCosina;

    UIManager ui;
    [SerializeField]
    float SwipeResistence;
    Vector3 TouchInitialPosition;

    private void OnEnable()
    {
        EventManager.Pause += EnablePause;
        EventManager.UnPause += DisablePause;
    }
    private void OnDisable()
    {
        EventManager.Pause -= EnablePause;
        EventManager.UnPause -= DisablePause;
    }

    private void Awake()
    {        
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        ui = GameManager.instance.ui;
        UpperCosina.GetComponent<Button>().onClick.AddListener(() => ui.ToggleMenu(MenuType.PauseMenu));
    }

    /// <summary>
    /// Funzione che abilita il menu di pausa
    /// </summary>
    public void EnablePause()
    {
        anim.SetTrigger("Pause");
        ArrowImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        UpperCosina.GetComponent<Button>().onClick.RemoveAllListeners();
        UpperCosina.GetComponent<Button>().onClick.AddListener(() => ui.ToggleMenu(MenuType.None));
    }

    /// <summary>
    /// Funzione che disabilita il menu di pausa
    /// </summary>
    public void DisablePause()
    {
        ArrowImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        anim.SetTrigger("UnPause");
        UpperCosina.GetComponent<Button>().onClick.RemoveAllListeners();
        UpperCosina.GetComponent<Button>().onClick.AddListener(() => ui.ToggleMenu(MenuType.PauseMenu));
    }

    /// <summary>
    /// Funzione che abilita/disabilita la freccia
    /// </summary>
    /// <param name="_enable"></param>
    public void EnableArrow(bool _enable)
    {
        if (_enable)
            ArrowImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        ArrowImage.SetActive(_enable);
		PauseButton.enabled = _enable;
    }

    /// <summary>
    /// Funzione che abilita la cosina in alto
    /// </summary>
    /// <param name="_enable"></param>
    public void EnableUpperCosina(bool _enable)
    {
        UpperCosina.SetActive(_enable);
    }

    /// <summary>
    /// Funzione che carica la preview del puzzle selezionato
    /// </summary>
    /// <param name="_puzzle"></param>
    public void LoadPuzzleSprite(PuzzleScriptable _puzzle)
    {
        PuzzleImage.sprite = _puzzle.DefaulImage;
    }

    /// <summary>
    /// Funzione che esce dal gioco e torna al menu
    /// </summary>
    public void ExitButton()
    {
        EventManager.UnPause();
        GameManager.instance.gen.SavePuzzleStatus();
        GameManager.instance.gen.DestroyPuzzle();
        GameManager.instance.ui.ToggleMenu(MenuType.MainMenu);
    }

    /// <summary>
    /// Funzione che ricomincia il puzzle cancellando i salvataggi
    /// </summary>
    public void RestartButton()
    {
        GameManager.instance.RestartGame();
        if (EventManager.UnPause != null)
            EventManager.UnPause();
    }

    #region Input
    /// <summary>
    /// Funzione che registra la posizione in cui è iniziato il drag
    /// </summary>
    public void BeginDrag()
    {
        if (ui.GetActiveMenu() == MenuType.None || ui.GetActiveMenu() == MenuType.PauseMenu)
        {
            TouchInitialPosition = Input.mousePosition;
        }
        else
        {
            Debug.Log("Non puoi mettere in pausa ora");
        }
    }

    /// <summary>
    /// Funzione che controlla se è stato eseguito un drag e la direzione verso cui è stato eseguito
    /// </summary>
    public void EndDrag()
    {
        if (ui.GetActiveMenu() == MenuType.None || ui.GetActiveMenu() == MenuType.PauseMenu)
        {
            Vector2 deltaSwipe = TouchInitialPosition - Input.mousePosition;
            if (Mathf.Abs(deltaSwipe.y) > SwipeResistence)
            {
                if (deltaSwipe.y < 0)
                {
                    if (ui.GetActiveMenu() == MenuType.PauseMenu)
                    {
                        ui.ToggleMenu(MenuType.None);
                    }
                }
                else
                {
                    if (ui.GetActiveMenu() == MenuType.None)
                    {
                        ui.ToggleMenu(MenuType.PauseMenu);
                    }
                }
            }
        }
    }
    #endregion
}
