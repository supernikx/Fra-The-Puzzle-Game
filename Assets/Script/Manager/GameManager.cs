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
    }

    private void Start()
    {
        ui.Init();

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
                    ui.ToggleMenu(MenuType.PauseMenu);
                    break;
            }
        }
    }

    public void StartLevel()
    {
        ui.ToggleMenu(MenuType.None);
        gen.GeneratePuzzle();
    }

    public void CheckIfCanMove(PuzzlePiceData _pice)
    {
        PuzzlePiceData PiceToCompare = null;
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

    public PuzzlePiceData GetPiceData(int x, int y)
    {
        foreach (PuzzlePice pice in gen.InstantiatedPices)
        {
            if (pice.data.ActualPosition.X == x && pice.data.ActualPosition.Y == y)
                return pice.data;
        }
        return null;
    }

    IEnumerator MovePice(PuzzlePiceData PiceToMove, PuzzlePiceData InvisiblePice)
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
        gen.CanGenerate = true;
        gen.GeneratePuzzle();
    }

    /// <summary>
    /// Funzione che apre nel browser la pagina instagram
    /// </summary>
    public void InstagramButton()
    {
        Application.OpenURL("https://www.instagram.com/fradesign.it/");
    }
}
