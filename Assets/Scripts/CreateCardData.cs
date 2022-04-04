using UnityEngine;

/// <summary>
/// Essa classe referencia os parâmetros associados para se criar um Card.
/// </summary>
public class CreateCardData
{
    public string card { get; set; } // Símbolo do card (ex: 2, 3, 4, queen, jack, ace)
    public string type { get; set; } // Tipo do card (ex: spades, hearts, diamonds, clubs)
    public string color { get; set; } // Cor do card (blue, red)
    public int groupNumber { get; set; } // Número do grupo a que o card pertence
    public Vector3 position { get; set; } // Posição para se criar o card
}
