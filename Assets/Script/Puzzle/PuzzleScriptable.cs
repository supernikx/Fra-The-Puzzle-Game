using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzle", menuName = "Puzzle", order = 1)]
public class PuzzleScriptable : ScriptableObject {

    public Sprite DefaulImage;
    public Sprite OverlayImage;
    public Texture2D Puzzle;

}
