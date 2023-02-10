using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void NewGameRequested();

public class HUDScript : MonoBehaviour
{
    public event NewGameRequested NewGameRequested;

    public Image PlayerHealthImage;
    public Image OrganHealthImage;
    public TextMeshProUGUI OrganNameLabel;

    public GameObject GameOverPanel;
    public Button PlayAgainButton;
    public Button ExitGameButton;

    private void PlayAgainClicked()
    {
        NewGameRequested?.Invoke();
        ToggleGameOver(false);
    }

    private void ExitGameClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ToggleGameOver(bool gameOver)
    {
        PlayerHealthImage.enabled = !gameOver;
        OrganHealthImage.enabled = !gameOver;
        OrganNameLabel.enabled = !gameOver;
        GameOverPanel.SetActive(gameOver);
    }

    public void UpdatePlayerHealth(float percent)
    {
        PlayerHealthImage.fillAmount = percent;
        if(percent < .000000001)
        {
            ToggleGameOver(true);
        }
    }

    public void UpdateOrganHealth(float percent) => OrganHealthImage.fillAmount = percent;

    public void UpdateOrgan(BodyPart bodyPart)
    { 
        if(bodyPart == null)
        {
            OrganNameLabel.text = string.Empty;
            OrganNameLabel.enabled = false;
            OrganHealthImage.enabled = false;
        }
        else
        {
            OrganNameLabel.text = bodyPart.Name;
            OrganNameLabel.enabled = true;
            OrganHealthImage.enabled = true;
        }
    }
}
