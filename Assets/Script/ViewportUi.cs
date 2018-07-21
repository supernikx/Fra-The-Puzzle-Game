using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewportUi : MonoBehaviour {

    public List<Sprite> PuzzlePices = new List<Sprite>();
    public GameObject PuzzlePicePrefab;
    public GameObject Content;

	// Use this for initialization
	void Start () {
        foreach (Sprite pice in PuzzlePices)
        {
            Image InstantiatedPice = Instantiate(PuzzlePicePrefab, Content.transform).GetComponent<Image>();
            InstantiatedPice.sprite = pice;          
        }
	}
}
