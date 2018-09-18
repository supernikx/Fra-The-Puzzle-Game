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
    public GameObject PieceSpritePrefab;
    public Transform PuzzleParent;
    Vector3 Offset = new Vector3(0, 0, 0);

    [Header("Puzzles Images")]
    public PuzzleScriptable SelectedPuzzle;
    List<Sprite> SelectedPuzzleSprites = new List<Sprite>();
    public List<PuzzlePiece> InstantiatedPieces = new List<PuzzlePiece>();
    SpriteRenderer InvisiblePieceSpriteRenderer;
    Sprite InvisibleSprite;

    public bool CanGenerate;

    private void OnEnable()
    {
        EventManager.EndLevel += ShowLastPiece;
    }
    private void OnDisable()
    {
        EventManager.EndLevel -= ShowLastPiece;
    }

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
                    SpriteRenderer InstantiatedSpriteRender = Instantiate(PieceSpritePrefab, new Vector3(SelectedDifficultySettings.DifficultyParent.position.x, SelectedDifficultySettings.DifficultyParent.position.y, SelectedDifficultySettings.DifficultyParent.position.z), Quaternion.identity, PuzzleParent).GetComponent<SpriteRenderer>();
                    InstantiatedSpriteRender.sprite = SelectedPuzzleSprites[k];
                    InstantiatedSpriteRender.gameObject.GetComponent<BoxCollider2D>().size = InstantiatedSpriteRender.sprite.bounds.size;
                    PuzzlePiece InstantiatedPuzzlePice = InstantiatedSpriteRender.gameObject.GetComponent<PuzzlePiece>();
                    InstantiatedPuzzlePice.data = new PuzzlePieceData(InstantiatedSpriteRender.sprite, InstantiatedSpriteRender.gameObject, new Coordinates(i, j));
                    if (i == SelectedDifficultySettings.PuzzleSize.X - 1 && j == SelectedDifficultySettings.PuzzleSize.Y - 1)
                        InstantiatedPuzzlePice.data.InvisiblePice = true;
                    InstantiatedPieces.Add(InstantiatedPuzzlePice);
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
                    InstantiatedPieces[k].gameObject.transform.position = new Vector3(SelectedDifficultySettings.DifficultyParent.position.x + Offset.x, SelectedDifficultySettings.DifficultyParent.position.y + Offset.y, SelectedDifficultySettings.DifficultyParent.position.z);
                    InstantiatedPieces[k].data.ActualPosition = new Coordinates(i, j);
                    Offset.x += InstantiatedPieces[k].data.PiceSprite.bounds.size.x * PieceSpritePrefab.transform.localScale.x;
                    if (InstantiatedPieces[k].data.InvisiblePice)
                    {
                        InvisiblePieceSpriteRenderer = InstantiatedPieces[k].gameObject.GetComponent<SpriteRenderer>();
                        InvisibleSprite = InvisiblePieceSpriteRenderer.sprite;
                        InvisiblePieceSpriteRenderer.sprite = null;

                    }
                    k++;
                }
                Offset.y -= InstantiatedPieces[k - 1].data.PiceSprite.bounds.size.y * PieceSpritePrefab.transform.localScale.y;
                Offset.x = 0;
            }
            k = 0;
            Offset.x = 0;
            Offset.y = 0;
            CanGenerate = false;
            Debug.Log("Puzzle Generato");
            GameManager.instance.ui.ToggleMenu(MenuType.None);
			AudioManager.instance.ToggleMenuVolume (false);
			AudioManager.instance.TogglePlayVolume (true);
        }
    }

    /// <summary>
    /// Funzione che distrugge il puzzle (se presente) e ne istanzia uno nuovo
    /// </summary>
	public void GeneratePuzzle(PuzzleScriptable _SelectedPuzzle, Difficulty _DifficultySelected)
    {
        InstantiatedPieces.Clear();
        CanGenerate = true;
        InstantieteNewPuzzle(_SelectedPuzzle, _DifficultySelected);
    }

    /// <summary>
    /// Funzione che distrugge il puzzle attuale
    /// </summary>
    public void DestroyPuzzle()
    {
        if (InstantiatedPieces[0] != null)
        {
            foreach (PuzzlePiece p in InstantiatedPieces)
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
        for (int i = 0; i < InstantiatedPieces.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(0, InstantiatedPieces.Count);
            temp = InstantiatedPieces[rnd];
            InstantiatedPieces[rnd] = InstantiatedPieces[i];
            InstantiatedPieces[i] = temp;
        }

        for (int i = 0; i < InstantiatedPieces.Count; i++)
        {
            if (InstantiatedPieces[i].data.InvisiblePice)
            {
                temp = InstantiatedPieces[InstantiatedPieces.Count - 1];
                InstantiatedPieces[InstantiatedPieces.Count - 1] = InstantiatedPieces[i];
                InstantiatedPieces[i] = temp;
            }
        }
    }

    /// <summary>
    /// Funzione che mostra l'ultimo pezzo del puzzle (quello invisibile)
    /// </summary>
    private void ShowLastPiece()
    {
        InvisiblePieceSpriteRenderer.sprite = InvisibleSprite;
    }
}
