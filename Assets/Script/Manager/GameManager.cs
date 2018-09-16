using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    #endregion

    [HideInInspector]
    public PuzzleGenerator gen;
    [HideInInspector]
    public UIManager ui;
    [HideInInspector]
    public EventManager ev;
    [HideInInspector]
    public LevelSelectionManager lvl;
	[HideInInspector]
	public DifficultyManager dm;

    PuzzleScriptable PlayingPuzzle;

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
        gen = FindObjectOfType<PuzzleGenerator>();
        ev = GetComponent<EventManager>();
        ui = FindObjectOfType<UIManager>();
        lvl = FindObjectOfType<LevelSelectionManager>();
		dm = FindObjectOfType<DifficultyManager> ();
    }

    private void Start()
    {
        ui.Init();
        lvl.Init();

        GameEnded = false;
        Moving = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (ui.GetActiveMenu())
            {
                case MenuType.MainMenu:
                    Application.Quit();
                    break;
                case MenuType.LevelSelection:
                    ui.ToggleMenu(MenuType.MainMenu);
                    break;
                case MenuType.WinScreen:
                    ui.ToggleMenu(MenuType.LevelSelection);
                    break;
                case MenuType.PauseMenu:
                    ui.ToggleMenu(MenuType.None);
                    break;
                case MenuType.None:
                    ui.ToggleMenu(MenuType.MainMenu);
                    break;
            }
        }
    }

    /// <summary>
    /// Funzione che starta il gioco
    /// </summary>
	public void StartGame(PuzzleScriptable _PuzzleToPlay, Coordinates _coords)
    {
        PlayingPuzzle = _PuzzleToPlay;
		gen.GeneratePuzzle(_PuzzleToPlay, _coords);
        ui.ToggleMenu(MenuType.None);
    }

    /// <summary>
    /// Funzione che controlla se il pezzo passato come parametro può spostarsi
    /// </summary>
    /// <param name="_pice"></param>
    public void CheckIfCanMove(PuzzlePieceData _pice)
    {
        PuzzlePieceData PiceToCompare = null;
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
                    StartCoroutine(MovePice(_pice, PiceToCompare));
                }

            }
            CheckPicePositions();
        }
    }

    /// <summary>
    /// Funzione che controlla se i pezzi sono al posto giusto
    /// </summary>
    /// <returns></returns>
    public bool CheckPicePositions()
    {
        bool win = true;
        foreach (PuzzlePiece pice in gen.InstantiatedPices)
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

    /// <summary>
    /// Funzione che ritorna il pezzo di puzzle corrispondente alle coordinate passate come parametro
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public PuzzlePieceData GetPiceData(int x, int y)
    {
        foreach (PuzzlePiece pice in gen.InstantiatedPices)
        {
            if (pice.data.ActualPosition.X == x && pice.data.ActualPosition.Y == y)
                return pice.data;
        }
        return null;
    }

    /// <summary>
    /// Coroutine che muove il pezzo toccato nel posto libero
    /// </summary>
    /// <param name="PiceToMove"></param>
    /// <param name="InvisiblePice"></param>
    /// <returns></returns>
    IEnumerator MovePice(PuzzlePieceData PiceToMove, PuzzlePieceData InvisiblePice)
    {
        Moving = true;
        Vector2 tempPos = PiceToMove.gameObject.transform.position;
        Coordinates tempXY = PiceToMove.ActualPosition;
        while (Vector2.Distance(InvisiblePice.gameObject.transform.position, PiceToMove.gameObject.transform.position) > 0.01f)
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

    /// <summary>
    /// Funzione che apre nel browser la pagina instagram
    /// </summary>
    public void InstagramButton()
    {
        Application.OpenURL("https://www.instagram.com/fradesign.it/");
    }

    /// <summary>
    /// Funzione che viene chiamata quando il gioco finisce
    /// </summary>
    private void EndGame()
    {
        Debug.Log("Gioco FInito");
    }
}
