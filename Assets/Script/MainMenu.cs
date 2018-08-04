using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void LoadGame(int sceneindex)
    {
        SceneManager.LoadScene(sceneindex);
    }
}
