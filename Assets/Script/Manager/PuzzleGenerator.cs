using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
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

[System.Serializable]
public class PuzzleGenerator : MonoBehaviour
{
    [Header("Puzzle Dimension")]
    public int PuzzleX;
    public int PuzzleY;

    [Header("Generation Settings")]
    public GameObject PiceSpritePrefab;
    public Transform StartPosition;
    public Transform PuzzlePicesParent;
    Vector3 Offset = new Vector3(0, 0, 0);

    [Header("Puzzles Images")]
    public Texture2D[] PuzzlesImages;
    Sprite[] SelectedPuzzleSprites;
    public List<PuzzlePiece> InstantiatedPices = new List<PuzzlePiece>();

    public bool CanGenerate;

    private void Start()
    {
        CanGenerate = true;
        PuzzlesImages = Resources.LoadAll<Texture2D>("SlidingPuzzleImages");
    }

    /// <summary>
    /// Funzione che Genera il puzzle
    /// </summary>
    public void GeneratePuzzle()
    {
        if (CanGenerate)
        {
            int k = 0;
            SelectedPuzzleSprites = new Sprite[] { };
            SelectedPuzzleSprites = Resources.LoadAll<Sprite>("SlidingPuzzleImages\\" + PuzzlesImages[UnityEngine.Random.Range(0, PuzzlesImages.Count())].name);
            Debug.Log("Puzzle Randomizzato");
            InstantiatedPices.Clear();
            for (int i = 0; i < PuzzleX; i++)
            {
                for (int j = 0; j < PuzzleY; j++)
                {
                    SpriteRenderer InstantiatedSpriteRender = Instantiate(PiceSpritePrefab, new Vector3(StartPosition.position.x, StartPosition.position.y, StartPosition.position.z), Quaternion.identity, PuzzlePicesParent).GetComponent<SpriteRenderer>();
                    InstantiatedSpriteRender.sprite = SelectedPuzzleSprites[k];
                    InstantiatedSpriteRender.gameObject.GetComponent<BoxCollider2D>().size = InstantiatedSpriteRender.sprite.bounds.size;
                    PuzzlePiece InstantiatedPuzzlePice = InstantiatedSpriteRender.gameObject.GetComponent<PuzzlePiece>();
                    InstantiatedPuzzlePice.data = new PuzzlePieceData(InstantiatedSpriteRender.sprite, InstantiatedSpriteRender.gameObject, new Coordinates(i, j));
                    if (i == PuzzleX - 1 && j == PuzzleY - 1)
                        InstantiatedPuzzlePice.data.InvisiblePice = true;
                    InstantiatedPices.Add(InstantiatedPuzzlePice);
                    k++;
                }
            }
            k = 0;
            Debug.Log("Puzzle Inizializzato");
            ShufflePuzzlePices();
            for (int i = 0; i < PuzzleX; i++)
            {
                for (int j = 0; j < PuzzleY; j++)
                {
                    InstantiatedPices[k].gameObject.transform.position = new Vector3(StartPosition.position.x + Offset.x, StartPosition.position.y + Offset.y, StartPosition.position.z);
                    InstantiatedPices[k].data.ActualPosition = new Coordinates(i, j);
                    Offset.x += InstantiatedPices[k].data.PiceSprite.bounds.size.x * PiceSpritePrefab.transform.localScale.x;                    
                    if (i == PuzzleX - 1 && j == PuzzleY - 1)
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
