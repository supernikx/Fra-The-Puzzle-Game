using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public ScrollRect InventoryRect;

    public void EnableScroll (bool active)
    {
        InventoryRect.horizontal = active;
    }
}
