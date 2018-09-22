using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class PuzzlePieceData
{
    public Sprite PiceSprite;
    public int SpriteID;
    public GameObject gameObject;
    public Coordinates RightPosition;
    public Coordinates ActualPosition;
    public bool InvisiblePice;

    public PuzzlePieceData(Sprite _sprite,int _SpriteID, GameObject _gameObject, Coordinates _right)
    {
        PiceSprite = _sprite;
        SpriteID = _SpriteID;
        gameObject = _gameObject;
        RightPosition = _right;
        InvisiblePice = false;
    }
}

[System.Serializable]
public class PuzzlePiece : MonoBehaviour
{
    [SerializeField]
    public PuzzlePieceData data;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        GameManager.instance.CheckIfCanMove(data);
    }

    public bool CheckPosition()
    {
        if (data.RightPosition.X == data.ActualPosition.X && data.RightPosition.Y == data.ActualPosition.Y)
            return true;
        return false;
    }
}
