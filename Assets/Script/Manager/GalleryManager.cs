using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour {

	public Transform Content;
	public Image GalleryImagePrefab;
	List<Image> GalleryImages = new List<Image> ();

	public void Init(){
		for (int i = 0; i < GameManager.instance.lvl.PuzzleList.Count; i++) {
			Image instantiatedGalleryImage = Instantiate (GalleryImagePrefab, Content);
			if (i < GameManager.instance.lvl.LastLevelUnlocked - 1)
				instantiatedGalleryImage.sprite = GameManager.instance.lvl.PuzzleList [i].DefaulImage;
			else
				instantiatedGalleryImage.sprite = GameManager.instance.lvl.PuzzleList [i].OverlayImage;
			instantiatedGalleryImage.SetNativeSize ();
			//GalleryImages.Add (instantiatedGalleryImage);
		}
	}

	public void Refresh(){
		for (int i = 0; i < GalleryImages.Count; i++) {
			if (i < GameManager.instance.lvl.LastLevelUnlocked - 1)
				GalleryImages[i].sprite = GameManager.instance.lvl.PuzzleList [i].DefaulImage;
			else
				GalleryImages[i].sprite = GameManager.instance.lvl.PuzzleList [i].OverlayImage;
			//GalleryImages[i].SetNativeSize ();
		}
	}

}
