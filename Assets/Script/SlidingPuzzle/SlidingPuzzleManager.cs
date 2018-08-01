using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzleManager : MonoBehaviour
{
    #region Singleton
    public static SlidingPuzzleManager instance;
    #endregion

    [HideInInspector]
    public SlidingPuzzleGenerator gen;
    [HideInInspector]
    public SlidingPuzzleUIManager ui;
    [HideInInspector]
    public EventManager ev;

    bool GameEnded;
    bool Moving;

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
        gen = GetComponent<SlidingPuzzleGenerator>();
        ev = GetComponent<EventManager>();
        ui = GetComponent<SlidingPuzzleUIManager>();
    }

    private void Start()
    {
        GameEnded = false;
        Moving = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gen.GeneratePuzzle();
        }
    }

    public void CheckIfCanMove(SlidingPuzzlePiceData _pice)
    {
        SlidingPuzzlePiceData PiceToCompare = null;
        if (!GameEnded && !Moving)
        {
            if (!_pice.InvisiblePice)
            {
                bool swap = false;
                if (_pice.ActualPosition.X > 0 && !swap)
                {
                    if (((PiceToCompare = GetPiceData(_pice.ActualPosition.X - 1, _pice.ActualPosition.Y)) != null) && PiceToCompare.InvisiblePice)
                    {
                        swap = true;
                    }
                }
                if (_pice.ActualPosition.X < gen.PuzzleX - 1 && !swap)
                {
                    if (((PiceToCompare = GetPiceData(_pice.ActualPosition.X + 1, _pice.ActualPosition.Y)) != null) && PiceToCompare.InvisiblePice)
                    {
                        swap = true;
                    }
                }
                if (_pice.ActualPosition.Y < gen.PuzzleY - 1 && !swap)
                {
                    if (((PiceToCompare = GetPiceData(_pice.ActualPosition.X, _pice.ActualPosition.Y + 1)) != null) && PiceToCompare.InvisiblePice)
                    {
                        swap = true;
                    }
                }
                if (_pice.ActualPosition.Y > 0 && !swap)
                {
                    if (((PiceToCompare = GetPiceData(_pice.ActualPosition.X, _pice.ActualPosition.Y - 1)) != null) && PiceToCompare.InvisiblePice)
                    {
                        swap = true;
                    }
                }

                if (swap)
                {                    
                    StartCoroutine(MovePice(_pice,PiceToCompare));
                }

            }
            CheckPicePositions();
        }
    }

    public bool CheckPicePositions()
    {
        bool win = true;
        foreach (SlidingPuzzlePice pice in gen.InstantiatedPices)
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

    public SlidingPuzzlePiceData GetPiceData(int x, int y)
    {
        foreach (SlidingPuzzlePice pice in gen.InstantiatedPices)
        {
            if (pice.data.ActualPosition.X == x && pice.data.ActualPosition.Y == y)
                return pice.data;
        }
        return null;
    }

    IEnumerator MovePice(SlidingPuzzlePiceData PiceToMove, SlidingPuzzlePiceData InvisiblePice)
    {
        Moving = true;
        Vector2 tempPos = PiceToMove.gameObject.transform.position;
        Coordinates tempXY = PiceToMove.ActualPosition;
        while (Vector2.Distance(InvisiblePice.gameObject.transform.position , PiceToMove.gameObject.transform.position)>0.01f)
        {
            PiceToMove.gameObject.transform.position = Vector2.MoveTowards(PiceToMove.gameObject.transform.position, InvisiblePice.gameObject.transform.position, Time.deltaTime * 10f);
            yield return null;
        }
        PiceToMove.ActualPosition = InvisiblePice.ActualPosition;
        InvisiblePice.gameObject.transform.position = tempPos;
        InvisiblePice.ActualPosition = tempXY;
        Moving = false;
        CheckPicePositions();
    }

    private void EndGame()
    {
        GameEnded = true;
    }

    public void AgainButton()
    {
        GameEnded = false;
        foreach (SlidingPuzzlePice pice in new List<SlidingPuzzlePice>(gen.InstantiatedPices))
        {
            Destroy(pice.gameObject);
        }
        gen.CanGenerate = true;
        gen.GeneratePuzzle();
    }
}
