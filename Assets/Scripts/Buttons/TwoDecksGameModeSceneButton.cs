using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TwoDecksGameModeSceneButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(this.OnClick);
    }

    public void OnClick()
    {
        SceneManager.LoadScene("TwoDecksGameModeScene");
    }
}
