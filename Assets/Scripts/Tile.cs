using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private bool shownCard = false; // IsCardTurn indicator  
    public Sprite originalCard; // Sprite choosen card
    public Sprite cardBack; // Sprite card backFace 
    public Sprite newCard; // card to update

    // Start is called before the first frame update
    void Start()
    {
        hideCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void OnMouseDown() 
    {
        print("Você clicou no Tile");
        if (shownCard == true){
            hideCard();
        }
        else {
            showCard();
        }
    }

    public void hideCard () 
    {
        GetComponent<SpriteRenderer>().sprite = cardBack;
        shownCard = false;
    }

    public void showCard()
    {
        GetComponent<SpriteRenderer>().sprite = originalCard;
        shownCard = true;
    }

    public void setOriginalCard() 
    {
        originalCard = newCard;
    }


}
