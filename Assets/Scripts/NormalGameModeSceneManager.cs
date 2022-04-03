using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
public class NormalGameModeSceneManager : MonoBehaviour
{
    public GameObject cardPrefab = null!;
    private List<Card> firstRowOfCards = null!;
    private List<Card> secondRowOfCards = null!;
    private int numberOfCardsPerRow = 13;
    private bool interactionBlocked = false;

    private Card? lastSelectedCard;
    private Stack<Card> selectedCards = new Stack<Card>();

    private bool timerIsRunning = false;
    private float timeRemaining = 0.0f;
    private Action? onTimerTimeout = null;

    private NumberText tries = null!;
    private NumberText matches = null!;
    private NumberText highscore = null!;

    // Start is called before the first frame update
    void Start()
    {
        this.Init();

        var dataToCreateCards = this.GenerateDataToCreateCards();
        dataToCreateCards.Shuffle();
        
        dataToCreateCards = dataToCreateCards.Take(numberOfCardsPerRow).ToList();
        this.firstRowOfCards = this.CreateRowOfCards(1, 0.125f, dataToCreateCards, "red");

        dataToCreateCards.Shuffle();
        this.secondRowOfCards = this.CreateRowOfCards(2, -0.125f, dataToCreateCards, "red");
    }

    void Init()
    {
        this.tries = GameObject.Find("Tries").GetComponent<NumberText>().Create(0);
        this.matches = GameObject.Find("Matches").GetComponent<NumberText>().Create(0);
        this.highscore = GameObject.Find("Highscore").GetComponent<NumberText>().Create(PlayerPrefs.GetInt("highscore"));
    }

    List<Card> CreateRowOfCards(int rowNumber, float positionYFromCenterInPercentage, List<CreateCardData> createCardsData, string color)
    {
        var rowOfCards = new List<Card>();

        for (int i = 0; i < numberOfCardsPerRow; i++)
        {
            float cardWidth = this.cardPrefab.GetComponent<BoxCollider2D>().size.x;
            float gap = 0.1f;

            float marginX = Screen.width * 0.075f;
            float marginY = Screen.height * 0.05f;
            float y = Screen.height / 2 + Screen.height * positionYFromCenterInPercentage;
            float z = Camera.main.transform.position.z * -1;

            Vector3 screenPosition = new Vector3(marginX, y, z);
            Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);
            position.x += i * ((cardWidth / 4) + gap);

            CreateCardData cardData = createCardsData[i];
            cardData.position = position;
            cardData.color = color;
            cardData.rowNumber = rowNumber;

            Card card = this.CreateCard(cardData);

            rowOfCards.Add(card);
        }

        return rowOfCards;
    }

    List<CreateCardData> GenerateDataToCreateCards()
    {
        string[] numberCards = Enumerable.Range(2, 9).Select(number => number.ToString()).ToArray();
        string[] specialCards = new string[] { "ace", "queen", "jack", "king" };
        string[] cardOptions = numberCards.Union(specialCards).ToArray();

        string[] cardTypes = new string[] { "spades", "hearts", "diamonds", "clubs" };

        var createCardsData = new List<CreateCardData>();

        foreach (string cardOption in cardOptions)
        {
            foreach (string cardType in cardTypes)
            {
                var cardData = new CreateCardData
                {
                    card = cardOption,
                    type = cardType
                };

                createCardsData.Add(cardData);
            }
        }

        return createCardsData;
    }

    Card CreateCard(CreateCardData data)
    {
        Card card = Instantiate(this.cardPrefab).GetComponent<Card>()
                .WithType(data.type)
                .WithColor(data.color)
                .InRow(data.rowNumber)
                .InPosition(data.position)
                .Create(data.card);

        return card;
    }

    public void OnCardClick(Card card)
    {
        if (this.interactionBlocked)
            return;

        if (card.selected)
            return;

        if (this.lastSelectedCard != null && card.rowNumber == this.lastSelectedCard.rowNumber)
            return;

        print($"Card {card.name} was clicked");

        this.SelectCard(card);
    }

    private void SelectCard(Card card)
    {
        card.Reveal();
        card.selected = true;

        if (selectedCards.Count == 0)
        {
            this.lastSelectedCard = card;
            this.selectedCards.Push(card);
        }
        else
        {
            Card cardSelectedBefore = this.selectedCards.Pop();

            Action? onTimerTimeout = null;

            if (this.CardEqualsCard(card, cardSelectedBefore))
            {
                print("Você acertou!!!");

                this.interactionBlocked = true;
                onTimerTimeout = () =>
                {
                    this.matches++;
                    this.tries++;
                    Destroy(card.gameObject);
                    Destroy(cardSelectedBefore.gameObject);
                    this.lastSelectedCard = null;
                    this.selectedCards.Clear();

                    this.CheckWin();

                    this.interactionBlocked = false;
                };
            }
            else
            {
                print("Você errou!!!");

                this.interactionBlocked = true;
                onTimerTimeout = () =>
                {
                    this.tries++;
                    this.DeselectCard(card);
                    this.DeselectCard(cardSelectedBefore);
                    this.lastSelectedCard = null;
                    this.selectedCards.Clear();

                    this.CheckWin();
                    this.interactionBlocked = false;
                };
            }

            this.SetTimeout(1, onTimerTimeout);
        }
    }

    private void CheckWin()
    {
        if (this.matches == this.numberOfCardsPerRow)
        {
            print("You won!");

            if (this.highscore == 0 || this.tries < this.highscore)
            {
                PlayerPrefs.SetInt("highscore", this.tries.GetValue());
                this.highscore.SetValue(this.tries.GetValue());
            }
        }
    }

    private void SetTimeout(float countdown, Action action)
    {
        this.onTimerTimeout = action;
        this.timeRemaining = countdown;
        this.timerIsRunning = true;
    }

    private void DeselectCard(Card card)
    {
        card.selected = false;
        card.Hide();
    }

    private bool CardEqualsCard(Card card1, Card card2)
    {
        return card1.symbol.Equals(card2.symbol);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.timerIsRunning)
        {
            if (this.timeRemaining > 0)
            {
                this.timeRemaining -= Time.deltaTime;
            }
            else
            {
                this.CancelTimer();
            }
        }
    }

    void CancelTimer()
    {
        this.timeRemaining = 0;
        this.timerIsRunning = false;

        if (this.onTimerTimeout != null)
            this.onTimerTimeout();
    }
}
