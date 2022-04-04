using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayAgainButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(this.OnClick);
    }

    public void OnClick()
    {
        if (GameManager.lastGameModeChosen != null)
        {
            SceneManager.LoadScene($"{GameManager.lastGameModeChosen}Scene");
        }
    }
}
