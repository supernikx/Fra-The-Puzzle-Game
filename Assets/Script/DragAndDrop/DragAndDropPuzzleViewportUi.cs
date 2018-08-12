using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropPuzzleViewportUi : MonoBehaviour{

    List<Sprite> PuzzlePieces = new List<Sprite>();
    public GameObject PuzzlePiecePrefab;
    public GameObject Content;

    // Use this for initialization
    public void SetPuzzlePieces (GameObject Puzzle) {

        for (int i = 0; i < Puzzle.transform.childCount; i++)
        {
            PuzzlePieces.Add(Puzzle.transform.GetChild(i).GetComponent<Image>().sprite);
        }

        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            GameObject InstantiatedPiece = Instantiate(PuzzlePiecePrefab, Content.transform);
            InstantiatedPiece.GetComponent<Image>().sprite = PuzzlePieces[i];
            InstantiatedPiece.GetComponent<DragAndDropPuzzlePice>().ID = i;
        }
	}


}
