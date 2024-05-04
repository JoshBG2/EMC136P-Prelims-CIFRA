using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Play or Restart the game
    public void Play()
    {
        SceneManager.LoadScene("Level 1");
    }

    // Return to the main menu
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Quit the game
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
