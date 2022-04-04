using UnityEngine;

/// <summary>
/// Script MyCameraBehaviour, diz sobre o comportamento da minha camera em algumas cenas
/// </summary>
public class MyCameraBehaviour : MonoBehaviour
{
    /*
     * Método Awake
     * Representa a primeira parte de inicialização do script
     * independente se este está ativado ou não
     */
    private void Awake()
    {
        this.GetComponent<Camera>().backgroundColor = new Color(26.0f / 255.0f, 89.0f / 255.0f, 31.0f / 255.0f);
    }
}
