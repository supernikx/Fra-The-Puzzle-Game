using UnityEngine;
using UnityEngine.UI;

public class DragAndDropPuzzleDestination : MonoBehaviour {

    public int ID;
    public Transform ImageCenter;
    public Sprite spriteTendina;

    public bool isFilled = false;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Fill () {
        image.enabled = true;
        isFilled = true;
	}
}
