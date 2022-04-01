using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CardsManager : MonoBehaviour
{
    public GameObject card ;    //card to discart

    // Start is called before the first frame update
    void Start()
    {
        ShowCard();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*mostra o card na tela*/
    public  void ShowCard()
    {
        //Instantiate(card, new Vector3(0,0,0), Quaternion.identity);
        for (int i=0; i<13; i++) {
            addCard(i);
        }
    }
    void addCard(int rank ) {

        float scaleOriginalCard = card.transform.localScale.x;
        float xScaleFactor = ((500*scaleOriginalCard)/100.0f);
        GameObject center =  GameObject.Find("centerReference");
        // Vector3  newPosition = new Vector3(center.transform.position.x + ((rank- 13 / 2)), center.transform.position.y, center.transform.position.z);
        Vector3  newPosition = new Vector3(center.transform.position.x + ((rank- 13 / 2)*xScaleFactor), center.transform.position.y, center.transform.position.z);
        // GameObject c =  (GameObject)(Instantiate(card, new Vector3(0,0,0), Quaternion.identity));
        // GameObject c =  (GameObject)(Instantiate(card, new Vector3(rank*2.0f,0,0), Quaternion.identity));
        GameObject c =  (GameObject)(Instantiate(card, newPosition, Quaternion.identity));
        c.tag = "" + rank; 
        c.name= "" + rank;
        string cardName ="";
        string cardNumber ="";
        if (rank == 0) {
            cardNumber = "ace";
        }
        else if ( rank==10){
            cardNumber = "jack";
        }
        else if (rank == 11){
            cardNumber = "queen";
        }
        else if (rank == 12){
            cardNumber = "king";
        }
        else{
            cardNumber = "" + (rank+1);
            cardName = cardNumber + "_of_clubs";
        }
        Sprite s1 = (Sprite)(Resources.Load<Sprite>(cardName));
        print("S1: " + s1);
        GameObject.Find(""+rank).GetComponent<Tile>().setOriginalCard(s1);
    }
}
