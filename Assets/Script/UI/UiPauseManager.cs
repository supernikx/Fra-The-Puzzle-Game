using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPauseManager : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    GameObject ArrowImage;
    [SerializeField]
    Image PuzzleImage;
    [SerializeField]
    GameObject UpperCosina;

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

    /// <summary>
    /// Funzione che abilita il menu di pausa
    /// </summary>
    public void EnablePause()
    {
        anim.SetTrigger("Pause");
        ArrowImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    /// <summary>
    /// Funzione che disabilita il menu di pausa
    /// </summary>
    public void DisablePause()
    {
        ArrowImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        anim.SetTrigger("UnPause");
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
}
