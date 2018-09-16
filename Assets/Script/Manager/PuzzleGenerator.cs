using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Coordinates
{
    public int X;
    public int Y;
    public Coordinates(int _X, int _Y)
    {
        X = _X;
        Y = _Y;
    }
}

[Serializable]
public class DifficultySettings
{
    public Transform DifficultyParent;
    public Coordinates PuzzleDimension;
    public Coordinates PuzzleSize;
}

[Serializable]
public class PuzzleGenerator : MonoBehaviour
{
    [Header("Puzzle Difficulty")]
    [Header("Easy Settings"), SerializeField]
    public DifficultySettings PuzzleDifficultySettingsEasy;
    [Header("Normal Settings"), SerializeField]
    public DifficultySettings PuzzleDifficultySettingsNormal;
    [Header("Hard Settings"), SerializeField]
    public DifficultySettings PuzzleDifficultySettingsHard;
    [HideInInspector]
    public DifficultySettings SelectedDifficultySettings;

    [Header("Generation Settings")]
    public GameObject PiceSpritePrefab;
    public Transform PuzzleParent;
    Vector3 Offset = new Vector3(0, 0, 0);

    [Header("Puzzles Images")]
    public PuzzleScriptable SelectedPuzzle;
    List<Sprite> SelectedPuzzleSprites = new List<Sprite>();
    //Sprite[] SelectedPuzzleSprites;
    public List<PuzzlePiece> InstantiatedPices = new List<PuzzlePiece>();

    public bool CanGenerate;

    /// <summary>
    /// Funzione che istanzia il nuovo puzzle
    /// </summary>
	private void InstantieteNewPuzzle(PuzzleScriptable _SelectedPuzzle, Difficulty _DifficultySelected)
    {
        if (CanGenerate)
        {
            switch (_DifficultySelected)
            {
                case Difficulty.Easy:
                    SelectedDifficultySettings = PuzzleDifficultySettingsEasy;
                    break;
                case Difficulty.Normal:
                    SelectedDifficultySettings = PuzzleDifficultySettingsNormal;
                    break;
                case Difficulty.Hard:
                    SelectedDifficultySettings = PuzzleDifficultySettingsHard;
                    break;
            }
            Debug.Log("Difficoltà impostata");

            int k = 0;
            SelectedPuzzleSprites.Clear();
            SelectedPuzzle = _SelectedPuzzle;
            for (int j = SelectedDifficultySettings.PuzzleSize.Y - 1; j >= 0; j--)
            {
                for (int i = 0; i < SelectedDifficultySettings.PuzzleSize.X; i++)
                {
                    Sprite newSprite = Sprite.Create(SelectedPuzzle.Puzzle, new Rect(i * SelectedDifficultySettings.PuzzleDimension.X, SelectedDifficultySettings.PuzzleDimension.Y * j, SelectedDifficultySettings.PuzzleDimension.X, SelectedDifficultySettings.PuzzleDimension.Y), new Vector2(0.5f, 0.5f));
                    SelectedPuzzleSprites.Add(newSprite);
                }
            }
            Debug.Log("Pezzi Divisi");

            for (int i = 0; i < SelectedDifficultySettings.PuzzleSize.X; i++)
            {
                for (int j = 0; j < SelectedDifficultySettings.PuzzleSize.Y; j++)
                {
                    SpriteRenderer InstantiatedSpriteRender = Instantiate(PiceSpritePrefab, new Vector3(SelectedDifficultySettings.DifficultyParent.position.x, SelectedDifficultySettings.DifficultyParent.position.y, SelectedDifficultySettings.DifficultyParent.position.z), Quaternion.identity, PuzzleParent).GetComponent<SpriteRenderer>();
                    InstantiatedSpriteRender.sprite = SelectedPuzzleSprites[k];
                    InstantiatedSpriteRender.gameObject.GetComponent<BoxCollider2D>().size = InstantiatedSpriteRender.sprite.bounds.size;
                    PuzzlePiece InstantiatedPuzzlePice = InstantiatedSpriteRender.gameObject.GetComponent<PuzzlePiece>();
                    InstantiatedPuzzlePice.data = new PuzzlePieceData(InstantiatedSpriteRender.sprite, InstantiatedSpriteRender.gameObject, new Coordinates(i, j));
                    if (i == SelectedDifficultySettings.PuzzleSize.X - 1 && j == SelectedDifficultySettings.PuzzleSize.Y - 1)
                        InstantiatedPuzzlePice.data.InvisiblePice = true;
                    InstantiatedPices.Add(InstantiatedPuzzlePice);
                    k++;
                }
            }
            k = 0;
            Debug.Log("Puzzle Inizializzato");

            ShufflePuzzlePices();
            for (int i = 0; i < SelectedDifficultySettings.PuzzleSize.X; i++)
            {
                for (int j = 0; j < SelectedDifficultySettings.PuzzleSize.Y; j++)
                {
                    InstantiatedPices[k].gameObject.transform.position = new Vector3(SelectedDifficultySettings.DifficultyParent.position.x + Offset.x, SelectedDifficultySettings.DifficultyParent.position.y + Offset.y, SelectedDifficultySettings.DifficultyParent.position.z);
                    InstantiatedPices[k].data.ActualPosition = new Coordinates(i, j);
                    Offset.x += InstantiatedPices[k].data.PiceSprite.bounds.size.x * PiceSpritePrefab.transform.localScale.x;
                    if (i == SelectedDifficultySettings.PuzzleSize.X - 1 && j == SelectedDifficultySettings.PuzzleSize.Y - 1)
                    {
                        InstantiatedPices[k].gameObject.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    k++;
                }
                Offset.y -= InstantiatedPices[k - 1].data.PiceSprite.bounds.size.y * PiceSpritePrefab.transform.localScale.y;
                Offset.x = 0;
            }
            k = 0;
            Offset.x = 0;
            Offset.y = 0;
            CanGenerate = false;
            Debug.Log("Puzzle Generato");
            GameManager.instance.ui.ToggleMenu(MenuType.None);
        }
    }

    /// <summary>
    /// Funzione che distrugge il puzzle (se presente) e ne istanzia uno nuovo
    /// </summary>
	public void GeneratePuzzle(PuzzleScriptable _SelectedPuzzle, Difficulty _DifficultySelected)
    {
        InstantiatedPices.Clear();
        CanGenerate = true;
        InstantieteNewPuzzle(_SelectedPuzzle, _DifficultySelected);
    }

    public void DestroyPuzzle()
    {
        if (InstantiatedPices[0] != null)
        {
            foreach (PuzzlePiece p in InstantiatedPices)
            {
                Destroy(p.gameObject);
            }
        }
    }

    /// <summary>
    /// Funzione che randomizza la posizione dei pezzi del puzzle
    /// </summary>
    private void ShufflePuzzlePices()
    {
        PuzzlePiece temp;
        for (int i = 0; i < InstantiatedPices.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(0, InstantiatedPices.Count);
            temp = InstantiatedPices[rnd];
            InstantiatedPices[rnd] = InstantiatedPices[i];
            InstantiatedPices[i] = temp;
        }

        for (int i = 0; i < InstantiatedPices.Count; i++)
        {
            if (InstantiatedPices[i].data.InvisiblePice)
            {
                temp = InstantiatedPices[InstantiatedPices.Count - 1];
                InstantiatedPices[InstantiatedPices.Count - 1] = InstantiatedPices[i];
                InstantiatedPices[i] = temp;
            }
        }
    }
}
