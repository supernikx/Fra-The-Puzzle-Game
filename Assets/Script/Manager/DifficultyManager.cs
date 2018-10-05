using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyManager : MonoBehaviour {

	public Button PlayButton;
	public Button WinGuyButton;

	[Header("Images")]
	public Image PuzzleToPlayImage;
	public Image EasyStar;
	public Image NormalStar;
	public Image HardStar;

	[Header("Sprites")]
	public Sprite EmptyStar;
	public Sprite FilledStar;

	[Header("Difficulty Coordinates")]
	public Coordinates EasyCoords;
	public Coordinates NormalCoords;
	public Coordinates HardCoords;

	public TextMeshProUGUI LevelNumberText;

	[HideInInspector]
	public Difficulty currentDifficulty;

	PuzzleScriptable selectedPuzzle;

	public void Initialize(PuzzleScriptable puzzle, int levelNumber)
	{
		GameManager.instance.ui.ToggleMenu (MenuType.DifficultyMenu);

		selectedPuzzle = puzzle;
		LevelNumberText.text = levelNumber.ToString ();
		PuzzleToPlayImage.sprite = puzzle.OverlayImage;

		SetDifficulty (0);
	}

	//Manca load dati salvati per controllo livello completato/da completare per determinare visione WinGuy
	public void SetDifficulty(int dIndex)
	{
		currentDifficulty = (Difficulty)dIndex;

		switch (dIndex) {
		case (int)Difficulty.Easy:
			EasyStar.sprite = FilledStar;
			NormalStar.sprite = EmptyStar;
			HardStar.sprite = EmptyStar;

			//se valore è 1 è già stato completato
			if (PlayerPrefs.GetInt (selectedPuzzle.PuzzleID.ToString () + Difficulty.Easy.ToString () + "status", 0) == 1) {
				WinGuyButton.gameObject.SetActive (true);
				PlayButton.gameObject.SetActive (false);
			} else {
				PlayButton.gameObject.SetActive (true);
				WinGuyButton.gameObject.SetActive (false);
			}

			break;
		case (int)Difficulty.Normal:
			EasyStar.sprite = FilledStar;
			NormalStar.sprite = FilledStar;
			HardStar.sprite = EmptyStar;

			//se valore è 1 è già stato completato
			if (PlayerPrefs.GetInt (selectedPuzzle.PuzzleID.ToString () + Difficulty.Normal.ToString () + "status", 0) == 1) {
				WinGuyButton.gameObject.SetActive (true);
				PlayButton.gameObject.SetActive (false);
			} else {
				PlayButton.gameObject.SetActive (true);
				WinGuyButton.gameObject.SetActive (false);
			}

			break;
		case (int)Difficulty.Hard:
			EasyStar.sprite = FilledStar;
			NormalStar.sprite = FilledStar;
			HardStar.sprite = FilledStar;

			//se valore è 1 è già stato completato
			if (PlayerPrefs.GetInt (selectedPuzzle.PuzzleID.ToString () + Difficulty.Hard.ToString () + "status", 0) == 1) {
				WinGuyButton.gameObject.SetActive (true);
				PlayButton.gameObject.SetActive (false);
			} else {
				PlayButton.gameObject.SetActive (true);
				WinGuyButton.gameObject.SetActive (false);
			}

			break;
		default:
			break;
		}
	}

	public void StartLevel()
	{
		if(selectedPuzzle)
			GameManager.instance.StartGame(selectedPuzzle, currentDifficulty);
	}

}

public enum Difficulty
{
    Easy,
    Normal,
    Hard
};