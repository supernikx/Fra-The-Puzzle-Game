using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropPuzzleGameManager : MonoBehaviour {

    public static DragAndDropPuzzleGameManager instance;
    [HideInInspector]
    public DragAndDropPuzzleUIManager ui;
    public Transform testdistance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ui = GetComponent<DragAndDropPuzzleUIManager>();
    }

    public bool CheckDistance(Vector2 _PicePosition)
    {
        float distance = Vector2.Distance(_PicePosition, testdistance.position);
        if (distance <= 0.5f)
        {
            return true;
        }
        return false;
    }
}
