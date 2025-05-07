using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour {

    [SerializeField] private string targetSceneName;
    
    //If player enters the portal, Load the main menu
    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            //If targetSceneName is ExitGame, exit the game
            if (targetSceneName == "ExitGame") {
                Application.Quit();
            }
            //Else load the designated scene
            else {
                SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single);
            }
        }
    }
}
