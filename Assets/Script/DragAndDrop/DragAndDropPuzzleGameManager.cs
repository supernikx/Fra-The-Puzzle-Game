using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragAndDropPuzzleGameManager : MonoBehaviour
{

    public static DragAndDropPuzzleGameManager instance;

    [HideInInspector]
    public DragAndDropPuzzleUIManager ui;
    [HideInInspector]
    public DragAndDropPuzzleViewportUi viewportUi;
    [HideInInspector]
    public EventManager ev;

    [Header("Puzzle List")]
    public GameObject[] Puzzles;

    public Transform canvas;
    
    List<DragAndDropPuzzleDestination> puzzleDestinations = new List<DragAndDropPuzzleDestination>();

    private void Awake()
    {
        instance = this;
        ev = GetComponent<EventManager>();
        ui = GetComponent<DragAndDropPuzzleUIManager>();
        viewportUi = FindObjectOfType<DragAndDropPuzzleViewportUi>();
    }

    private void Start()
    {
        if (viewportUi)
        {
            GameObject InstantiatedPuzzle = Instantiate(Puzzles[Random.Range(0, Puzzles.Length)], canvas);
            viewportUi.SetPuzzlePieces(InstantiatedPuzzle);
            for (int i = 0; i < InstantiatedPuzzle.transform.childCount; i++)
            {
                puzzleDestinations.Add(InstantiatedPuzzle.transform.GetChild(i).GetComponent<DragAndDropPuzzleDestination>());
            }
        }
        else
            print("viewport not found");
    }

    public bool CheckDistance(Vector2 _PiecePosition, int PuzzlePieceID)//invece che vector2 come imput, prendo DragAndDropPuzzlePiece.
    {
        float distance = 0.0f;
        Transform destination = GetPuzzlePieceDestination(PuzzlePieceID);

        if (destination)
            distance = Vector2.Distance(_PiecePosition, Camera.main.ScreenToWorldPoint(destination.position));
        else
        {
            print("Error in checking distance");
            return false;
        }

        if (distance <= 1f)//oltre che checkare position, controllo ID.
        {
            foreach (var puzzleDest in puzzleDestinations)
            {
                if (PuzzlePieceID == puzzleDest.ID && !puzzleDest.isFilled)
                {
                    puzzleDest.Fill();
                    return true;
                }
            }
        }
        return false;
    }

    public Transform GetPuzzlePieceDestination(int _ID)
    {
        foreach (var puzzleDest in puzzleDestinations)
        {
            if (puzzleDest.ID == _ID)
                return puzzleDest.ImageCenter;
        }
        return null;
    }

    public void CheckEndGame()
    {
        if (viewportUi.PuzzlePieces.Count == 0)
        {
            if (ev.EndGame != null)
                ev.EndGame();
        }
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
