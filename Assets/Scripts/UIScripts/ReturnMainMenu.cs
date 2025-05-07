using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour {

    private const string MAIN_MENU_NAME = "MainMenuScene";

    public void ReturnToMenu() {
        SceneManager.LoadScene(MAIN_MENU_NAME, LoadSceneMode.Single);
    }
}
