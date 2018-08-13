using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DragAndDropPuzzlePiceData
{
    public Sprite sprite;
    public int ID;

    public DragAndDropPuzzlePiceData (Sprite _sprite, int _ID)
    {
        sprite = _sprite;
        ID = _ID;
    }
}


public class DragAndDropPuzzleViewportUi : MonoBehaviour{

    [HideInInspector]
    public List<DragAndDropPuzzlePiceData> PuzzlePieces = new List<DragAndDropPuzzlePiceData>();
    public GameObject PuzzlePiecePrefab;
    public GameObject Content;

    // Use this for initialization
    public void SetPuzzlePieces (GameObject Puzzle) {

        for (int i = 0; i < Puzzle.transform.childCount; i++)
        {
            PuzzlePieces.Add(new DragAndDropPuzzlePiceData(Puzzle.transform.GetChild(i).GetComponent<DragAndDropPuzzleDestination>().spriteTendina, i));
        }

        ShuffleList(PuzzlePieces);

        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            GameObject InstantiatedPiece = Instantiate(PuzzlePiecePrefab, Content.transform);
            InstantiatedPiece.GetComponent<Image>().sprite = PuzzlePieces[i].sprite;
            InstantiatedPiece.GetComponent<DragAndDropPuzzlePice>().ID = PuzzlePieces[i].ID;
        }        
    }

    private void ShuffleList(List<DragAndDropPuzzlePiceData> ListToShuffle)
    {
        DragAndDropPuzzlePiceData temp;
        for (int i = 0; i < ListToShuffle.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(0, ListToShuffle.Count);
            temp = ListToShuffle[rnd];
            ListToShuffle[rnd] = ListToShuffle[i];
            ListToShuffle[i] = temp;
        }
    }

    public void DisablePice (GameObject ObjectToDisable)
    {
        int RemoveIndex = 0;
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            if (PuzzlePieces[i].sprite == ObjectToDisable.GetComponent<Image>().sprite)
            {
                RemoveIndex = i;
                break;
            }
        }
        PuzzlePieces.RemoveAt(RemoveIndex);
        ObjectToDisable.SetActive(false);
    }

}
