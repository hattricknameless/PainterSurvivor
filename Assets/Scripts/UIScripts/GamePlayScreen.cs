using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScreen : MonoBehaviour {

    //Scaling the plane to screen size is inspired by chatGPT
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float distance;
    [SerializeField] private RenderTexture renderTexture;

    private void Start() {
        mainCamera = Camera.main;
        ScaleToFitScreen();
    }

    private void ScaleToFitScreen() {
        // Ensure the quad is at the correct distance in front of the camera
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;

        // Calculate the height and width of the screen at the given distance
        float screenHeight = 2.0f * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float screenWidth = screenHeight * mainCamera.aspect;

        // Scale the Quad to match the calculated dimensions
        gameObject.transform.localScale = new Vector3(screenWidth, screenHeight, 1f);

        // Resize the Render Texture based on screen dimensions
        int newWidth = Mathf.CeilToInt(Screen.width);
        int newHeight = Mathf.CeilToInt(Screen.height);

        if (renderTexture.width != newWidth || renderTexture.height != newHeight) {
            renderTexture.Release(); // Release the current Render Texture to apply changes
            renderTexture.width = newWidth;
            renderTexture.height = newHeight;
            renderTexture.Create(); // Recreate the Render Texture with the new dimensions
        }
    }
}