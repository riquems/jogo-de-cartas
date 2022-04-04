using UnityEngine;

/// <summary>
/// Script MyCameraBehaviour, diz sobre o comportamento da minha camera em algumas cenas
/// </summary>
public class MyCameraBehaviour : MonoBehaviour
{
    /*
     * M�todo Awake
     * Representa a primeira parte de inicializa��o do script
     * independente se este est� ativado ou n�o
     */
    private void Awake()
    {
        this.GetComponent<Camera>().backgroundColor = new Color(26.0f / 255.0f, 89.0f / 255.0f, 31.0f / 255.0f);
    }
}
