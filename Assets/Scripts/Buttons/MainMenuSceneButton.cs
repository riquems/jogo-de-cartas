using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script MainMenuSceneButton, representa o comportamento de um botão que vai para o menu principal
/// </summary>
public class MainMenuSceneButton : MonoBehaviour
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
        SceneManager.LoadScene("MainMenuScene");
    }
}
