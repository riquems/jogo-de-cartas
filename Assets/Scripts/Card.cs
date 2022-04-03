using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card WithType(string type)
    {
        this.type = type;
        return this;
    }

    public Card WithColor(string color)
    {
        this.color = color;
        return this;
    }

    public Card InRow(int rowNumber)
    {
        this.rowNumber = rowNumber;
        return this;
    }

    public Card InPosition(Vector3 position)
    {
        this.transform.position = position;
        return this;
    }

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

    public void Reveal()
    {
        this.facingDown = false;
        this.back.GetComponent<SpriteRenderer>().sortingOrder = -1;
        this.front.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public void Hide()
    {
        this.facingDown = true;
        this.back.GetComponent<SpriteRenderer>().sortingOrder = 0;
        this.front.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("Manager").SendMessage("OnCardClick", this);
    }
}
