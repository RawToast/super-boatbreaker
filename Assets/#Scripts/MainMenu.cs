﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayButton() {
        Grd.Level.ResetLevels();
        Grd.Level.NextLevel();
    }

    public void QuitButton() {
        Application.Quit();
    }
}