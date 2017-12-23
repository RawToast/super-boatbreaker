using UnityEngine;

public class MenuUI : MonoBehaviour {
    public void PlayButton() {
        Application.LoadLevel(1);
    }

    public void QuitButton() {
        Application.Quit();
    }
}