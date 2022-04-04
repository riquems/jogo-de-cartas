using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script PlayAgainButton, representa o comportamento de um botão 
/// que repete o último modo de jogo jogado
/// </summary>
public class PlayAgainButton : MonoBehaviour
{
    /*
     * Método Awake
     * Representa a primeira parte de inicialização do script
     * independente se este está ativado ou não
     */
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(this.OnClick);
    }

    /*
     * Método OnClick
     * Representa a ação desejada quando o botão é clicado
     */
    public void OnClick()
    {
        if (GameManager.lastGameModeChosen != null)
        {
            SceneManager.LoadScene($"{GameManager.lastGameModeChosen}Scene");
        }
    }
}
