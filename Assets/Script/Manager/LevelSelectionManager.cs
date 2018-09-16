using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject PuzzleLevelPrefab;
    public Transform Content;
    public int LastLevelUnlocked;
    public List<PuzzleScriptable> PuzzleList = new List<PuzzleScriptable>();

    public void Init()
    {
        PuzzleList = Resources.LoadAll<PuzzleScriptable>("PuzzleScriptable").ToList();
        PuzzleList = PuzzleList.OrderBy(p => p.PuzzleID).ToList();
        for (int i = 0; i < PuzzleList.Count; i++)
        {
            PuzzleLevelSelection NewLevel = Instantiate(PuzzleLevelPrefab, Content).GetComponent<PuzzleLevelSelection>();
            if (i < LastLevelUnlocked - 1)
            {
                NewLevel.Init(PuzzleList[i].DefaulImage, LockStatus.Unlocked, i);
            }
            else if (i == LastLevelUnlocked - 1)
            {
                NewLevel.Init(PuzzleList[i].OverlayImage, LockStatus.LastUnlocked, i);
            }
            else
            {
                NewLevel.Init(PuzzleList[i].OverlayImage, LockStatus.Locked, i);
            }
        }
    }

    public void PuzzleSelected(int _PuzzleID)
    {
		GameManager.instance.dm.Initialize (PuzzleList[_PuzzleID], _PuzzleID + 1);   
    }

}
