using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public float tempoAnimacao;

	[HideInInspector] public int[] currentTile;

	public GameObject[] Linha1;
	public GameObject[] Linha2;
	public GameObject[] Linha3;
	public GameObject[] Linha4;
	public GameObject[] Linha5;
	private GameObject[,] Casas;

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




	private void Start() {

		Casas = new GameObject[5, 5];
		currentTile = new int[2];

		for (int i = 0; i < 5; i++ ) {
			Casas[0, i] = Linha1[i];
			Casas[1, i] = Linha2[i];
			Casas[2, i] = Linha3[i];
			Casas[3, i] = Linha4[i];
			Casas[4, i] = Linha5[i];
		}
	}



    
	void Update()
    {
        //APENAS PARA TESTES - RESETAR A CENA
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        TurnHandler();
        //Debug.Log(turnState);
    }




	private void MoverPeca(GameObject peca, GameObject casa){

		Vector3 acimaDoTileAntigo = new Vector3(peca.transform.position.x, peca.transform.position.y + 1, peca.transform.position.z);
		Vector3 acimaDoNovoTile = new Vector3(casa.transform.position.x, peca.transform.position.y + 1, casa.transform.position.z);
		Vector3 posicaoFinal = new Vector3(casa.transform.position.x, peca.transform.position.y, casa.transform.position.z);

		peca.transform.DOMove(acimaDoTileAntigo, tempoAnimacao / 3).OnComplete(()=> {
			peca.transform.DOMove(acimaDoNovoTile, tempoAnimacao / 3).OnComplete(() => {
				peca.transform.DOMove(posicaoFinal, tempoAnimacao / 3).OnComplete(() => {
                    activeTurn = false;
                });
            });
        });
    }




	private GameObject GetSelectedTile() {
        LayerMask mask = LayerMask.GetMask("Tabuleiro");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
            StartCoroutine(Changetilecolor(hit.collider.gameObject));

            for(int i = 0; i<5; i++ )
				for (int j = 0; j < 5; j++ ) 
	                if (hit.collider.gameObject == Casas[i,j]) 
	                    return Casas[i,j];
        }
		return null;
    }




    private bool CheckSelectedTile()
    {
        LayerMask mask = LayerMask.GetMask("Tabuleiro");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)){
			GameObject hcgo = hit.collider.gameObject;
			bool canMove = hcgo.GetComponent<TileCheck>().canMove;

			if ( canMove ) 
				for (int i = 0; i < 5 ; i++)
					for (int j = 0; j < 5; j++ ) 
	                	if ( hcgo == Casas[i,j])
	                	    return true;
        }
        return false;
    }




    IEnumerator Changetilecolor(GameObject tile) {
        tile.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        tile.GetComponent<Renderer>().material.color = Color.white;
    }
    


    private void HighlightMoveOptions(CardScript card) {
		int[] pmX = card.possibleMovesX;
		int[] pmY = card.possibleMovesY;
		int cX = currentTile[0];
		int cY = currentTile[1];

		int sinal = TurnIndicator.indicator.blueTurn ? 1 : -1;
		// XXX
		// AVALIAR SE O SINAL TÁ MUDANDO A CADA TURNO 
		for (int i = 0; i < pmX.Length; i++ ) {
			int candidatoX = cX + sinal * pmX[i];
			int candidatoY = cY + sinal * pmY[i];

			if ( candidatoX >=0 && candidatoX<5 && candidatoY>=0 && candidatoY<5 ){
				GameObject c = Casas[candidatoX, candidatoY];
	            c.GetComponent<Renderer>().material.color = Color.green;
	            c.GetComponent<TileCheck>().canMove = true;
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
                    if (Physics.Raycast(currentPiece.transform.position,new Vector3(0, -1, 0), out hit, mask)){
						GameObject hcgo = hit.collider.gameObject;
											 
						for (int i = 0; i < 5; i++)
							for (int j = 0; j < 5; j++ ) 
								if (hcgo== Casas[i,j]){
									currentTile[0] = i;
									currentTile[1] = j;
								}
                    }
					// XXX
					// IF p1, highlight na selectedcard do player 1, ELSE highlight na selectedcard do player 2
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
        for (int i = 0; i < 5; i++)
			for (int j = 0; j < 5; j++ ) {
				Casas[i,j].GetComponent<TileCheck>().canMove = false;
				Casas[i,j].GetComponent<Renderer>().material.color = Color.white;
			}        
    }
}
