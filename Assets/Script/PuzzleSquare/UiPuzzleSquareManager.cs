using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPuzzleSquareManager : MonoBehaviour {
    [Header("Puzzle Square")]
    [Header("Win Screen References")]
    public GameObject WinScreen;
    public GameObject SelectedPicePrefab;
    GameObject SelectedPiceGO;
    Vector3 SelectedPiceDefaultPosition = new Vector3(100, 100, 100);

    private void OnDisable()
    {
        PuzzleSquareManager.instance.ev.EndGame -= EndGame;
    }

    private void Start()
    {
        SelectedPiceGO = Instantiate(SelectedPicePrefab, SelectedPiceDefaultPosition, Quaternion.identity);
        PuzzleSquareManager.instance.ev.EndGame += EndGame;
    }

    public void MoveSelected(Vector3 _position)
    {
        SelectedPiceGO.transform.position = new Vector3(_position.x, _position.y, 0);
    }

    public void DisableSelected()
    {
        SelectedPiceGO.transform.position = SelectedPiceDefaultPosition;
    }

    private void EndGame()
    {
        WinScreen.SetActive(true);
    }
}
