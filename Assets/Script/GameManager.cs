using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    [HideInInspector]
    public UiManager ui;

    public Transform testdistance;

    private void Awake()
    {
        instance = this;
        ui = GetComponent<UiManager>();
    }

    public bool CheckDistance(Vector2 _PicePosition)
    {
        float distance = Vector2.Distance(_PicePosition, testdistance.position);
        Debug.Log(distance);
        if (distance <= 0.5f)
        {
            return true;
        }
        return false;
    }
}
