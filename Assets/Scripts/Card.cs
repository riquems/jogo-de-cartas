using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Essa classe traz funções associadas e variavés associadas ao prefab do card.
/// </summary>
public class Card : MonoBehaviour
{
    public GameObject front { get; set; } // Parte da frente do card
    public GameObject back { get; set; } // Parte de trás do card

    public string symbol { get; set; } // Símbolo do card (ex: 2, 3, 4, queen, jack, ace)
    public string type { get; set; } // Tipo do card (ex: spades, hearts, diamonds, clubs)
    public string color { get; set; } // Cor do card (blue, red)
    public int groupNumber { get; set; } // Grupo a qual o card pertence
    public bool facingDown { get; set; } // Se o card está virado para baixo
    public bool selected { get; set; } // Se o card está selecionado
    public AudioSource audioSourceFlick { get; set; } // Som que toca quando o card é virado (e desvirado)

    /*
     * WithType da valor uma variavel pública (type) de acordo com uma entrada especifica de um tipo adequado
     */

    public Card WithType(string type)
    {
        this.type = type;
        return this;
    }

    /*
     * WithColor dá valor uma variavel pública (color) de acordo com uma entrada especifica de um tipo adequado
     */
    public Card WithColor(string color)
    {
        this.color = color;
        return this;
    }

    /*
     * InGroup dá valor uma variavel pública (groupNumber) de acordo com uma entrada especifica do tipo adequado
     */
    public Card InGroup(int groupNumber)
    {
        this.groupNumber = groupNumber;
        return this;
    }

    /*
     * InPosition dá valor uma variavel pública (position) de acordo com uma entrada especifica do tipo adequado
     */
    public Card InPosition(Vector3 position)
    {
        this.transform.position = position;
        return this;
    }

    /*
     * Create : cria um card de acordo com o card option (2, 3, queen, jack, king, ace)
     * e dá o nome do Card de acordo com o rowNumber, symbol, type e color.
     * Caso o card esteja virado para baixo(facingDown=true), ele ira virar (facingDown=false), e vice-versa
     */
    public Card Create(string cardOption, bool facingDown = true)
    {
        this.symbol = cardOption;

        this.name = $"{this.groupNumber} - {this.symbol} of {this.type} ({this.color})";

        this.front = GameObject.Find($"{this.name}/Front");
        this.back = GameObject.Find($"{this.name}/Back");

        Sprite cardFrontSprite = Resources.Load<Sprite>($"Cartas/{this.symbol}_of_{this.type}");
        Sprite cardBackSprite = Resources.Load<Sprite>($"Cartas/{this.color}CardBackFixed");

        this.front.GetComponent<SpriteRenderer>().sprite = cardFrontSprite;
        this.back.GetComponent<SpriteRenderer>().sprite = cardBackSprite;

        if (facingDown) 
            this.Hide();
        else
            this.Reveal();
        return this;
    }

    /*
     * Revela um card alterando o valor da variavel facingDown
     */
    public void Reveal()
    {
        this.facingDown = false;
        this.back.GetComponent<SpriteRenderer>().sortingOrder = -1;
        this.front.GetComponent<SpriteRenderer>().sortingOrder = 0;
        audioSourceFlick.Play();
    }

    /*
     * Esconde um card alterando o valor da variavel facingDown
     */
    public void Hide()
    {
        this.facingDown = true;
        this.back.GetComponent<SpriteRenderer>().sortingOrder = 0;
        this.front.GetComponent<SpriteRenderer>().sortingOrder = -1;
        audioSourceFlick.Play();
    }

    /*
     * Método acionado quando há um clique no card
     */
    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("Manager").SendMessage("OnCardClick", this);
    }
}
