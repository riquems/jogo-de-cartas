using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable
public class OnlyLettersGameModeSceneManager : MonoBehaviour
{
    public GameObject cardPrefab = null!;
    private const int numberOfCardsPerRow = 4;
    private const int numberOfRows = 4;
    private const int numberOfGroups = 2;
    private const int numberOfPossibleMatches = ((numberOfCardsPerRow * numberOfRows) * numberOfGroups) / 2;
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

        for (int i = 0; i < numberOfRows; i++)
        {
            this.CreateRowOfCards(0, i, dataToCreateCards, "red");
        }

        dataToCreateCards.Shuffle();
        
        for (int i = 0; i < numberOfRows; i++)
        {
            this.CreateRowOfCards(1, i, dataToCreateCards, "blue");
        }
    }

    void Init()
    {
        this.tries = GameObject.Find("Tries").GetComponent<NumberText>().Create(0);
        this.matches = GameObject.Find("Matches").GetComponent<NumberText>().Create(0);
        this.highscore = GameObject.Find("Highscore").GetComponent<NumberText>().Create(PlayerPrefs.GetInt("highscore_" + SceneManager.GetActiveScene().name));
    }

    List<Card> CreateRowOfCards(int groupNumber, int rowNumber, List<CreateCardData> createCardsData, string color)
    {
        var rowOfCards = new List<Card>();

        for (int i = 0; i < numberOfCardsPerRow; i++)
        {
            float cardWidth = this.cardPrefab.GetComponent<BoxCollider2D>().size.x;
            float gap = 0.1f;

            float marginX = Screen.width * 0.200f;
            float marginY = Screen.height * 0.05f;
            float y = (Screen.height / 2) + (Screen.height * 0.22f) - (rowNumber * 90);
            float z = Camera.main.transform.position.z * -1;

            Vector3 screenPosition = new Vector3(marginX, y, z);
            Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);
            position.x += i * ((cardWidth / 4) + gap) + (7.5f * groupNumber);

            CreateCardData cardData = createCardsData[i + (rowNumber * numberOfCardsPerRow)];
            cardData.position = position;
            cardData.color = color;
            cardData.groupNumber = groupNumber;

            Card card = this.CreateCard(cardData);

            rowOfCards.Add(card);
        }

        return rowOfCards;
    }

    List<CreateCardData> GenerateDataToCreateCards()
    {
        string[] numberCards = Enumerable.Range(2, 9).Select(number => number.ToString()).ToArray();
        string[] specialCards = new string[] { "ace", "queen", "jack", "king" };
        string[] cardOptions = specialCards;

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
                .InGroup(data.groupNumber)
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

        if (this.lastSelectedCard != null && card.groupNumber == this.lastSelectedCard.groupNumber)
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
        if (this.matches == numberOfPossibleMatches)
        {
            print("You won!");

            if (this.highscore == 0 || this.tries < this.highscore)
            {
                PlayerPrefs.SetInt("highscore_" + SceneManager.GetActiveScene().name, this.tries.GetValue());
                this.highscore.SetValue(this.tries.GetValue());
            }

            SceneManager.LoadScene("VictoryScene");
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
        return card1.symbol.Equals(card2.symbol) &&
               card1.type.Equals(card2.type);
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
