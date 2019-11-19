using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour {
	public int[] possibleMovesX, possibleMovesY;

    public Sprite MOVE1;
    public Sprite MOVE2;
    public Sprite MOVE3;
    public Sprite MOVE4;
    public Sprite MOVE5;

    private Sprite[] Deck;

    private Sprite CardImage;
    private GameObject gameController;

    private void Start()
    {
        InitDeck();
        ShuffleCards();
    }

    private void Update()
    {
        CardImage = GetComponent<Image>().sprite;
    }

    private void DrawCard() //XXX - Método para puxar a próxima carta do array "Deck" antes do turno do jogador começar - XXX
    {
        
    }

    private void ShuffleCards() //Embaralha a ordem das cartas com o algoritmo de Knuth
    {        
        for (int t = 0; t < Deck.Length; t++)
        {
            Sprite tmp = Deck[t];
            int r = Random.Range(t, Deck.Length);
            Deck[t] = Deck[r];
            Deck[r] = tmp;
        }
    }

    public void CheckCard() //Checa quais são as cartas mostradas e troca as movimentações de acordo
    {
        //gameController.GetComponent<GameController>().selectedcard = this;
        if (CardImage == MOVE1)//Arquivo Move_0
        {
            possibleMovesX[0] = 1;
            possibleMovesX[1] = 1;
            possibleMovesX[2] = 0;

            possibleMovesY[0] = 0;
            possibleMovesY[1] = 1;
            possibleMovesY[2] = -1;
        }
        else if (CardImage == MOVE2)//Arquivo Move_1
        {
            possibleMovesX[0] = -1;
            possibleMovesX[1] = 1;
            possibleMovesX[2] = 0;

            possibleMovesY[0] = 0;
            possibleMovesY[1] = 1;
            possibleMovesY[2] = -1;
        }
        else if(CardImage == MOVE3)//Arquivo Move_2
        {
            possibleMovesX[0] = -1;
            possibleMovesX[1] = 1;
            possibleMovesX[2] = -1;

            possibleMovesY[0] = 1;
            possibleMovesY[1] = 0;
            possibleMovesY[2] = -1;
        }
        else if(CardImage == MOVE4)//Arquivo Move_3
        {
            possibleMovesX[0] = -1;
            possibleMovesX[1] = 1;
            possibleMovesX[2] = 0;

            possibleMovesY[0] = 1;
            possibleMovesY[1] = 2;
            possibleMovesY[2] = -1;
        }
        else //Arquivo Move_4
        {
            possibleMovesX[0] = -1;
            possibleMovesX[1] = 1;
            possibleMovesX[2] = 1;

            possibleMovesY[0] = 0;
            possibleMovesY[1] = 1;
            possibleMovesY[2] = -1;
        }
    }

    private void InitDeck() //Inicializa o array "Deck", colocando as Sprites nas posições iniciais antes de embaralhá-las
    {
        Deck = new Sprite[15];
        Deck[0] = MOVE1;
        Deck[1] = MOVE1;
        Deck[2] = MOVE1;
        Deck[3] = MOVE2;
        Deck[4] = MOVE2;
        Deck[5] = MOVE2;
        Deck[6] = MOVE3;
        Deck[7] = MOVE3;
        Deck[8] = MOVE3;
        Deck[9] = MOVE4;
        Deck[10] = MOVE4;
        Deck[11] = MOVE4;
        Deck[12] = MOVE5;
        Deck[13] = MOVE5;
        Deck[14] = MOVE5;
    } 
}
