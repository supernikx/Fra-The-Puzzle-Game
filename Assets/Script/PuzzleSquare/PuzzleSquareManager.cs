using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSquareManager : MonoBehaviour {
    #region Singleton
    public static PuzzleSquareManager instance;
    #endregion

    [HideInInspector]
    public PuzzleGenerator gen;
    [HideInInspector]
    public UiPuzzleSquareManager ui;
    [HideInInspector]
    public EventManager ev;

    PuzzlePiceData PiceSelected;
    bool GameEnded;

    private void OnEnable()
    {
       ev.EndGame += EndGame;
    }
    private void OnDisable()
    {
        ev.EndGame -= EndGame;
    }

    private void Awake()
    {
        instance = this;
        gen = GetComponent<PuzzleGenerator>();
        ev = GetComponent<EventManager>();
        ui = GetComponent<UiPuzzleSquareManager>();
    }

    private void Start()
    {
        PiceSelected = null;
        GameEnded = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gen.GeneratePuzzle();
        }
    }

    public void PicePressed(PuzzlePiceData _pice)
    {
        if (!GameEnded)
        {
            if (PiceSelected != null)
            {
                if (_pice != PiceSelected)
                {
                    Vector3 tempPos = _pice.gameObject.transform.position;
                    Coordinates tempXY = _pice.ActualPosition;
                    _pice.gameObject.transform.position = PiceSelected.gameObject.transform.position;
                    _pice.ActualPosition = PiceSelected.ActualPosition;
                    PiceSelected.gameObject.transform.position = tempPos;
                    PiceSelected.ActualPosition = tempXY;
                    PiceSelected = null;
                    CheckPicePositions();
                    Debug.Log("Swap");
                    ui.DisableSelected();
                }
                else
                {
                    PiceSelected = null;
                    ui.DisableSelected();
                }
            }
            else
            {
                PiceSelected = _pice;
                ui.MoveSelected(_pice.gameObject.transform.position);
            }
        }
    }

    public bool CheckPicePositions()
    {
        bool win = true;
        foreach (PuzzlePice pice in gen.InstantiatedPices)
        {
            if (!pice.CheckPosition())
            {
                win = false;
                break;
            }
        }

        if (win)
        {
            if (ev.EndGame != null)
                ev.EndGame();
            return true;
        }
        return false;
    }

    private void EndGame()
    {
        GameEnded = true;
    }

    public void AgainButton()
    {
        GameEnded = false;
        foreach (PuzzlePice pice in new List<PuzzlePice>(gen.InstantiatedPices))
        {
            Destroy(pice.gameObject);
        }
        gen.Generate = true;
        gen.GeneratePuzzle();
    }
}
