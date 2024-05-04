using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Text livesText; // Reference to the TMP Text component
    public PlayerController playerController; // Reference to the PlayerController script

    void Start()
    {
        UpdateLivesText(); // Update the TMP Text component with the initial number of lives
    }

    // Function to update the TMP Text component with the current number of lives
    public void UpdateLivesText()
    {
        livesText.text = "Lives: " + playerController.lives.ToString(); // Update the text with the current number of lives from PlayerController
    }
}