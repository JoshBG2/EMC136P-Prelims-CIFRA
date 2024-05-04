using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivesManager : MonoBehaviour
{
    public TMP_Text livesText; 
    public PlayerController playerController; 

    void Start()
    {
        UpdateLivesText(); 
    }

    // CHANGE LIVES COUNTER
    public void UpdateLivesText()
    {
        livesText.text = "Lives: " + playerController.lives.ToString();
        Debug.Log("Updated Lives: " + playerController.lives);
    }
}