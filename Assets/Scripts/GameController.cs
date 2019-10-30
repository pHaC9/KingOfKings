using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Tamanho da grid")]
    public int vertical, horizontal, currentTile;
    public GameObject[] Casas;
    public GameObject player1, player2;
    public CardScript selectedcard;

    public enum TurnState{
        BeginTurn,
        PieceSelect,
        CardSelect,
        TileSelect,
        EndTurn
    }

    public TurnState turnState;

    // Update is called once per frame
    void Update()
    {
        TurnHandler();
        //Debug.Log(turnState);
    }

    private void MoverPeca(GameObject peca, int casa){
        peca.transform.DOMove(new Vector3(peca.transform.position.x, peca.transform.position.y+1, peca.transform.position.z),1).OnComplete(()=> {
            peca.transform.DOMove(new Vector3(Casas[casa].transform.position.x, peca.transform.position.y, Casas[casa].transform.position.z),1).OnComplete(() => {
                peca.transform.DOMove(new Vector3(peca.transform.position.x, 0.448f, peca.transform.position.z), 1).OnComplete(() => {
                    Debug.Log("Rodaaroda");
                    activeTurn = false;
                });
            });
        });
    }

    private int GetSelectedTile() {
        LayerMask mask = LayerMask.GetMask("Tabuleiro");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
            Debug.Log("Click Detected");
            StartCoroutine(Changetilecolor(hit.collider.gameObject));
            for(int i = 0; i<Casas.Length; i++ ){
                if (hit.collider.gameObject == Casas[i]) {
                    return i;
                }
            }            
        }
        return 0;
    }

    private bool CheckSelectedTile()
    {
        LayerMask mask = LayerMask.GetMask("Tabuleiro");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            Debug.Log("Click Detected");
            
            for (int i = 0; i < Casas.Length; i++)
            {
                if (hit.collider.gameObject == Casas[i] && hit.collider.gameObject.GetComponent<TileCheck>().canMove == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator Changetilecolor(GameObject tile) {
        tile.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        tile.GetComponent<Renderer>().material.color = Color.white;
    }

    public int downDifference;
    public int topDifference;

    private void HighlightMoveOptions(CardScript card) {
        

        for (int i = 0; i <= 5; i++)
        {
            if ((currentTile + i) % 5 == 0)
            {
                downDifference = -5 + i;
                topDifference = i;
            }
        }

        for (int i = 0; i < card.possibleMoves.Length; i++){

            Debug.Log(currentTile + card.possibleMoves[i]);
            if (currentTile + card.possibleMoves[i] >= 0 && currentTile + card.possibleMoves[i] <= 24) {

                if (currentTile + card.possibleMoves[i] < Casas.Length && card.possibleMoves[i] >= topDifference)
                {
                    Casas[currentTile + card.possibleMoves[i]].GetComponent<Renderer>().material.color = Color.green;
                    Casas[currentTile + card.possibleMoves[i]].GetComponent<TileCheck>().canMove = true;
                }
                else if (currentTile + card.possibleMoves[i] >= 0 && card.possibleMoves[i] < downDifference)
                {
                    Casas[currentTile + card.possibleMoves[i]].GetComponent<Renderer>().material.color = Color.green;
                    Casas[currentTile + card.possibleMoves[i]].GetComponent<TileCheck>().canMove = true;
                }
                else if ((currentTile + 1) % 5 != 0 && card.possibleMoves[i] > 0 && card.possibleMoves[i] < topDifference)
                {
                    Casas[currentTile + card.possibleMoves[i]].GetComponent<Renderer>().material.color = Color.green;
                    Casas[currentTile + card.possibleMoves[i]].GetComponent<TileCheck>().canMove = true;
                }
                else if (currentTile % 5 != 0 && card.possibleMoves[i] < 0 && card.possibleMoves[i] > downDifference)
                {
                    Casas[currentTile + card.possibleMoves[i]].GetComponent<Renderer>().material.color = Color.green;
                    Casas[currentTile + card.possibleMoves[i]].GetComponent<TileCheck>().canMove = true;
                }
            }
        }
    }

    GameObject currentPlayer;
    GameObject currentPiece;
    bool activeTurn = true;

    private void TurnHandler() {
        
        switch (turnState)
        {
            case TurnState.BeginTurn:                

                if (FindObjectOfType<TurnIndicator>().GetComponent<TurnIndicator>().blueTurn == true)
                {
                    currentPlayer = player1;
                }
                else
                {
                    currentPlayer = player2;
                }
                turnState = TurnState.CardSelect;
                break;

            case TurnState.CardSelect:                

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    turnState = TurnState.PieceSelect;
                }
                break;

            case TurnState.PieceSelect:                

                if (Input.GetKeyDown(KeyCode.X))
                {                    
                    currentPiece = currentPlayer;
                    LayerMask mask = LayerMask.GetMask("Tabuleiro");
                    RaycastHit hit;
                    if (Physics.Raycast(currentPiece.transform.position,new Vector3(0, -1, 0), out hit, mask))
                    {
                        for (int i = 0; i < Casas.Length; i++)
                        {
                            if (hit.collider.gameObject == Casas[i])
                            {
                                currentTile = i;
                            }
                        }
                        
                    }

                    HighlightMoveOptions(selectedcard);

                    turnState = TurnState.TileSelect;
                }
                
                break;

            case TurnState.TileSelect:                

                if (Input.GetMouseButton(0) && CheckSelectedTile() == true)
                {
                    MoverPeca(currentPiece, GetSelectedTile());
                    turnState = TurnState.EndTurn;
                }
                
                break;

            case TurnState.EndTurn:                

                ClearBoard();
                if (activeTurn == false)
                {
                    FindObjectOfType<TurnIndicator>().TurnChange();
                    activeTurn = true;
                    turnState = TurnState.BeginTurn;
                }

                break;
        }        

    }

    private void ClearBoard()
    {
        for (int i = 0; i < Casas.Length; i++)
        {
            Casas[i].GetComponent<TileCheck>().canMove = false;
            Casas[i].GetComponent<Renderer>().material.color = Color.white;
        }
    }


}
