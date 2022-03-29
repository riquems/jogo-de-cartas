using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void ShowCard()
    {
        //Instantiate(card, new Vector3(0,0,0), Quaternion.identity);
        addCard();
    }
    void addCard() {
        GameObject c =  (GameObject)(Instantiate(card, new Vector3(0,0,0), Quaternion.identity)); 
    }
}
