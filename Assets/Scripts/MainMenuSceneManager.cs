using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNormalModeButtonClick()
    {
        SceneManager.LoadScene("NormalGameModeScene");
    }

    public void OnTwoDecksModeButtonClick()
    {
        SceneManager.LoadScene("TwoDecksGameModeScene");
    }

    public void OnOnlyLettersButtonClick()
    {
        SceneManager.LoadScene("OnlyLettersGameModeScene");
    }

    public void OnCreditsButtonClick()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void OnQuitButtonClick()
    {
        // Essa condicional é necessária pois eu quero que o jogo pare quando eu estiver rodando no Unity
        // e que ele feche quando estiver rodando em uma janela
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
