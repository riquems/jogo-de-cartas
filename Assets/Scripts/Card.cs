using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Essa classe traz funções associadas e variavés associadas ao prefab do card.  
/// </summary>
public class Card : MonoBehaviour
{
    public GameObject front;
    public GameObject back;

    public string symbol; 
    public string type;
    public string color;
    public int rowNumber;
    private Vector3 position;
    public bool facingDown;
    public bool selected;
    public AudioSource audioSourceFlick;
    public AudioSource audioSourceDisplay;
    public AudioSource audioSourceRight;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    WithType da valor uma variavel pública (type) de acordo com uma entrada especifica de um tipo adequado
    */
    public Card WithType(string type)
    {
        this.type = type;
        return this;
    }
    /* 
    WithColor dá valor uma variavel pública (color) de acordo com uma entrada especifica de um tipo adequado
    */
    public Card WithColor(string color)
    {
        this.color = color;
        return this;
    }
    /* 
    InRow dá valor uma variavel pública (rowNumber) de acordo com uma entrada especifica do tipo adequado
    */
    public Card InRow(int rowNumber)
    {
        this.rowNumber = rowNumber;
        return this;
    }
    /*
       InPosition dá valor uma variavel pública (position) de acordo com uma entrada especifica do tipo adequado
    */
    public Card InPosition(Vector3 position)
    {
        this.transform.position = position;
        return this;
    }
    /*
    Create : cria um card de acordo com a Opção (cardOption), que representa o simbolo do card (Spades, hearts,
    Clubs, Diamonds), e de o nome do Card é de acordo com o rowNumber, symbol, type e color.
    Caso o card esteja virado para baixo( facingDown=true), ele ira virar (facingDown=false), e vice-versa
    */
    public Card Create(string cardOption, bool facingDown = true)
    {
        this.symbol = cardOption;

        this.name = $"{this.rowNumber} - {this.symbol} of {this.type} ({this.color})";

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
    revela um card alterando o valor da variavel facingDown
    */
    public void Reveal()
    {
        this.facingDown = false;
        this.back.GetComponent<SpriteRenderer>().sortingOrder = -1;
        this.front.GetComponent<SpriteRenderer>().sortingOrder = 0;
        audioSourceFlick.Play();
    }
    /*
    esconde um card alterando o valor da variavel facingDown
    */
    public void Hide()
    {
        this.facingDown = true;
        this.back.GetComponent<SpriteRenderer>().sortingOrder = 0;
        this.front.GetComponent<SpriteRenderer>().sortingOrder = -1;
        audioSourceFlick.Play();
    }
    /*
    verifica se o collider associado ao card foi clicado. 
    */
    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("Manager").SendMessage("OnCardClick", this);
    }
}