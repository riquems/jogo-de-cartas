using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(this.OnClick);
    }

    private void OnClick()
    {
        // Essa condicional é necessária pois eu quero que o jogo pare quando eu estiver rodando no Unity
        // e que ele feche quando estiver rodando em uma janela
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
