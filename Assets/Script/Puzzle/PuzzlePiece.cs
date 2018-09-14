using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzlePieceData
{
    public Sprite PiceSprite;
    public GameObject gameObject;
    public Coordinates RightPosition;
    public Coordinates ActualPosition;
    public bool InvisiblePice;

    public PuzzlePieceData(Sprite _sprite, GameObject _gameObject, Coordinates _right)
    {
        PiceSprite = _sprite;
        gameObject = _gameObject;
        RightPosition = _right;
        InvisiblePice = false;
    }
}

public class PuzzlePiece : MonoBehaviour {

    public PuzzlePieceData data;

    private void OnMouseDown()
    {
        GameManager.instance.CheckIfCanMove(data);
    }

    public bool CheckPosition()
    {
        if (data.RightPosition.X == data.ActualPosition.X && data.RightPosition.Y == data.ActualPosition.Y)
            return true;
        return false;
    }
}
