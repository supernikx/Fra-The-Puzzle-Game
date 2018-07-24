using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzlePiceData
{
    public Sprite PiceSprite;
    public GameObject gameObject;
    public Coordinates RightPosition;
    public Coordinates ActualPosition;

    public PuzzlePiceData(Sprite _sprite, GameObject _gameObject, Coordinates _right)
    {
        PiceSprite = _sprite;
        gameObject = _gameObject;
        RightPosition = _right;
    }
}

public class PuzzlePice : MonoBehaviour {

    public PuzzlePiceData data;

    private void OnMouseDown()
    {
        Debug.Log(data.RightPosition.X + " " + data.RightPosition.Y);
        PuzzleSquareManager.instance.PicePressed(data);
    }

    public bool CheckPosition()
    {
        if (data.RightPosition.X == data.ActualPosition.X && data.RightPosition.Y == data.ActualPosition.Y)
            return true;
        return false;
    }
}
