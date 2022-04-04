#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script referente ao modo de jogo de dois baralhos
/// </summary>
public class TwoDecksGameModeSceneManager : MonoBehaviour
{
    public GameObject cardPrefab = null!; // Prefab da carta

    private const int numberOfCardsPerGroup = 13; // Número de cartas por grupo
    private const int numberOfGroups = 2; // Número de grupos
    private const int maxNumberOfMatches = (numberOfCardsPerGroup * numberOfGroups) / 2; // Número de pares possíveis
    private bool interactionBlocked = false; // Se a interação do jogador está bloqueada

    private Card? lastSelectedCard; // Variável que guarda o último card selecionado
    private Stack<Card> selectedCards = new Stack<Card>(); // Pilha de cards selecionados

    private bool timerIsRunning = false; // Se o timer está rodando
    private float timeRemaining = 0.0f; // Tempo faltante do timer
    private Action? onTimerTimeout = null; // Função para executar quando o timer termina

    private NumberText tries = null!; // Informação na tela que diz o número de tentativas
    private NumberText matches = null!; // Informação na tela que diz o número de matches
    private NumberText highscore = null!; // Informação na tela que diz o record da última vez que esse modo de jogo foi jogado

    /*
     * Método que é chamado antes do primeiro frame ser renderizado
     */
    void Start()
    {
        this.Init();

        var dataToCreateCards = this.GenerateDataToCreateCards();
        dataToCreateCards.Shuffle();

        dataToCreateCards = dataToCreateCards.Take(numberOfCardsPerGroup).ToList();
        this.CreateRowOfCards(0, 0.125f, dataToCreateCards, "red");

        dataToCreateCards.Shuffle();
        this.CreateRowOfCards(1, -0.125f, dataToCreateCards, "blue");
    }

    /*
     * Método chamado no Start para atribuir referências necessárias por esse script
     */
    void Init()
    {
        this.tries = GameObject.Find("Tries").GetComponent<NumberText>().Create(0);
        this.matches = GameObject.Find("Matches").GetComponent<NumberText>().Create(0);
        this.highscore = GameObject.Find("Highscore").GetComponent<NumberText>().Create(PlayerPrefs.GetInt("highscore_" + SceneManager.GetActiveScene().name));
    }

    /*
     * Método para criar uma linha de cartas
     */
    List<Card> CreateRowOfCards(int rowNumber, float positionYFromCenterInPercentage, List<CreateCardData> createCardsData, string color)
    {
        var rowOfCards = new List<Card>();

        for (int i = 0; i < numberOfCardsPerGroup; i++)
        {
            float cardWidth = this.cardPrefab.GetComponent<BoxCollider2D>().size.x;
            float gap = 0.1f;

            float marginX = Screen.width * 0.05f;
            float marginY = Screen.height * 0.05f;
            float y = Screen.height / 2 + Screen.height * positionYFromCenterInPercentage;
            float z = Camera.main.transform.position.z * -1;

            Vector3 screenPosition = new Vector3(marginX, y, z);
            Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);
            position.x += i * ((cardWidth / 4) + gap);

            CreateCardData cardData = createCardsData[i];
            cardData.position = position;
            cardData.color = color;
            cardData.groupNumber = rowNumber;

            Card card = this.CreateCard(cardData);

            rowOfCards.Add(card);
        }

        return rowOfCards;
    }

    /*
     * Método para criar dados para a criação de cartas
     */
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

    /*
     * Método que cria uma carta
     */
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

    /*
     * Método acionado quando uma carta é clicada
     */
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

    /*
     * Método dessa classe que cuida da seleção de uma carta
     */
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
        if (this.matches == maxNumberOfMatches)
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
