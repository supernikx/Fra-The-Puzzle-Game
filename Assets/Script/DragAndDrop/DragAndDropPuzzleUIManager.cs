using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropPuzzleUIManager : MonoBehaviour {
    [Header("Inventory References")]
    public ScrollRect InventoryRect;
    public Button InventoryRectButton;
    private Animator InventoryRectAnimator;
    private bool InventoryDown;

    [Header("End Game References")]
    public GameObject EndGamePanel;

    private void OnDisable()
    {
        DragAndDropPuzzleGameManager.instance.ev.EndGame += EndGame;
    }

    private void Start()
    {
        DragAndDropPuzzleGameManager.instance.ev.EndGame += EndGame;
        InventoryRectAnimator = InventoryRect.GetComponent<Animator>();
        InventoryDown = false;
        EndGamePanel.SetActive(false);
    }

    public void EnableScroll(bool active)
    {
        InventoryRect.horizontal = active;
    }

    public void UpDownInventory()
    {
        if (InventoryDown)
        {
            InventoryRectAnimator.SetTrigger("Up");
            InventoryDown = false;
        }
        else
        {
            InventoryRectAnimator.SetTrigger("Down");
            InventoryDown = true;
        }
        StartCoroutine(UpDownInventoryCoroutine());
    }

    private IEnumerator UpDownInventoryCoroutine()
    {
        InventoryRectButton.interactable = false;
        yield return new WaitForSecondsRealtime(0.35f);
        InventoryRectButton.interactable = true;
    }

    private void EndGame()
    {
        EndGamePanel.SetActive(true);
        EnableScroll(false);
        InventoryRectButton.enabled = false;
    }

}
