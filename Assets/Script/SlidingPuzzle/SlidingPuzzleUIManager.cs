using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzleUIManager : MonoBehaviour {
    [Header("Win Screen References")]
    public GameObject WinScreen;

    private void OnDisable()
    {
        SlidingPuzzleManager.instance.ev.EndGame -= EndGame;
    }

    private void Start()
    {
        SlidingPuzzleManager.instance.ev.EndGame += EndGame;
    }

    private void EndGame()
    {
        WinScreen.SetActive(true);
    }
}
