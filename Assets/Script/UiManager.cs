using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    [Header("Inventory References")]
    public ScrollRect InventoryRect;    
    public Button InventoryRectButton;    
    private Animator InventoryRectAnimator;
    private bool InventoryDown;

    private void Start()
    {
        InventoryRectAnimator = InventoryRect.GetComponent<Animator>();
        InventoryDown = false;
    }

    public void EnableScroll (bool active)
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
}
