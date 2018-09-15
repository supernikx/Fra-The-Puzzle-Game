using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleLevelSelection : MonoBehaviour
{

    [Header("References")]
    [SerializeField]
    Image PuzzleImage;
    [SerializeField]
    Image LockerImage;
    [SerializeField]
    Sprite ClosedLockerSprite;
    [SerializeField]
    Sprite OpenLockerSprite;

    [Header("Other Variables")]
    [SerializeField]
    int PuzzleID;
    [SerializeField]
    bool Locked;
    LevelSelectionManager lvlMng;

    public void Init(Sprite _PuzzleSprite, LockStatus _LockStatus, int _PuzzleID)
    {
        PuzzleID = _PuzzleID;
        lvlMng = GameManager.instance.lvl;
        PuzzleImage.sprite = _PuzzleSprite;
        switch (_LockStatus)
        {
            case LockStatus.Locked:
                Locked = true;
                LockerImage.sprite = ClosedLockerSprite;
                break;
            case LockStatus.LastUnlocked:
                Locked = false;
                LockerImage.sprite = OpenLockerSprite;
                break;
            case LockStatus.Unlocked:
                Locked = false;
                LockerImage.enabled = false;
                break;
        }
    }

    /// <summary>
    /// Funzione che comunica al levelselectionmanager il puzzle che è stato selezionato
    /// </summary>
    public void OnPuzzleSelected()
    {
        if (!Locked)
            lvlMng.PuzzleSelected(PuzzleID);
        else
            Debug.Log("Livello Bloccato");
    }

}

public enum LockStatus
{
    Locked,
    LastUnlocked,
    Unlocked,
}
