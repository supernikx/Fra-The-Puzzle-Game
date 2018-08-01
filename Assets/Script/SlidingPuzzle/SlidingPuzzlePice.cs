using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlidingPuzzlePiceData
{
    public Sprite PiceSprite;
    public GameObject gameObject;
    public Coordinates RightPosition;
    public Coordinates ActualPosition;
    public bool InvisiblePice;

    public SlidingPuzzlePiceData(Sprite _sprite, GameObject _gameObject, Coordinates _right)
    {
        PiceSprite = _sprite;
        gameObject = _gameObject;
        RightPosition = _right;
        InvisiblePice = false;
    }
}

public class SlidingPuzzlePice : MonoBehaviour {

    public SlidingPuzzlePiceData data;

    private void OnMouseDown()
    {
        Debug.Log(data.RightPosition.X + " " + data.RightPosition.Y);
        SlidingPuzzleManager.instance.CheckIfCanMove(data);
    }

    public bool CheckPosition()
    {
        if (data.RightPosition.X == data.ActualPosition.X && data.RightPosition.Y == data.ActualPosition.Y)
            return true;
        return false;
    }
}
