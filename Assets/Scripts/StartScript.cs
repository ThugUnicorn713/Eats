using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject playButton;
    public GameObject startButton;

    public TextMeshProUGUI eatsvsEatsText;
    public TextMeshProUGUI explainText;
    public TextMeshProUGUI howToText;

    public string playScene = "GameScene";

    public void StartButton()
    {
        eatsvsEatsText.gameObject.SetActive(true);
        explainText.gameObject.SetActive(true);
        continueButton.SetActive(true);
        startButton.SetActive(false);
    }

    public void ContinueButton()
    {
        explainText.gameObject.SetActive(false);
        howToText.gameObject.SetActive(true);
        playButton.SetActive(true);
        continueButton.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(playScene);  
    }

}
