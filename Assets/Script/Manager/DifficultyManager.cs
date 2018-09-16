using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyManager : MonoBehaviour {

	[Header("Images")]
	public Image PuzzleToPlayImage;
	public Image WinGuyImage;
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
			break;
		case (int)Difficulty.Normal:
			EasyStar.sprite = FilledStar;
			NormalStar.sprite = FilledStar;
			HardStar.sprite = EmptyStar;
			break;
		case (int)Difficulty.Hard:
			EasyStar.sprite = FilledStar;
			NormalStar.sprite = FilledStar;
			HardStar.sprite = FilledStar;
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