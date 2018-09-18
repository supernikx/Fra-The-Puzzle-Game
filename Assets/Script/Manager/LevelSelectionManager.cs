using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject PuzzleLevelPrefab;
    public Transform Content;
    public int LastLevelUnlocked;
    public List<PuzzleScriptable> PuzzleList = new List<PuzzleScriptable>();
    List<PuzzleLevelSelection> InstantiatedLevels = new List<PuzzleLevelSelection>();

    public void Init()
    {
        LastLevelUnlocked = PlayerPrefs.GetInt("LastLevelUnlocked", 1);
        PuzzleList = Resources.LoadAll<PuzzleScriptable>("PuzzleScriptable").ToList();
        PuzzleList = PuzzleList.OrderBy(p => p.PuzzleID).ToList();
        for (int i = 0; i < PuzzleList.Count; i++)
        {
            PuzzleLevelSelection NewLevel = Instantiate(PuzzleLevelPrefab, Content).GetComponent<PuzzleLevelSelection>();
            if (i < LastLevelUnlocked - 1)
            {
                NewLevel.Init(PuzzleList[i].DefaulImage, LockStatus.Unlocked, PuzzleList[i].PuzzleID);
            }
            else if (i == LastLevelUnlocked - 1)
            {
                NewLevel.Init(PuzzleList[i].OverlayImage, LockStatus.LastUnlocked, PuzzleList[i].PuzzleID);
            }
            else
            {
                NewLevel.Init(PuzzleList[i].OverlayImage, LockStatus.Locked, PuzzleList[i].PuzzleID);
            }
            InstantiatedLevels.Add(NewLevel);
        }
    }

    public void PuzzleSelected(int _PuzzleID)
    {
        GameManager.instance.dm.Initialize(PuzzleList[_PuzzleID - 1], _PuzzleID);
    }

    /// <summary>
    /// Funzione che sblocca il puzzle successivo
    /// </summary>
    public void UnlockNextPuzzle(PuzzleScriptable _PuzzleCompleted)
    {
        if (InstantiatedLevels[LastLevelUnlocked - 1].GetPuzzleID() == _PuzzleCompleted.PuzzleID)
        {
            InstantiatedLevels[LastLevelUnlocked - 1].CompletePuzzle(PuzzleList[LastLevelUnlocked - 1].DefaulImage);
            LastLevelUnlocked++;
            InstantiatedLevels[LastLevelUnlocked - 1].UnlockPuzzle();
            PlayerPrefs.SetInt("LastLevelUnlocked", LastLevelUnlocked);
        }
    }
}
