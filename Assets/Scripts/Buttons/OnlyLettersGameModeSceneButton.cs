using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script OnlyLettersGameModeSceneButton, representa o comportamento de um botão 
/// que vai para o modo de jogo de somente letras
/// </summary>
public class OnlyLettersGameModeSceneButton : MonoBehaviour
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
        SceneManager.LoadScene("OnlyLettersGameModeScene");
        GameManager.lastGameModeChosen = "OnlyLettersGameMode";
    }
}
