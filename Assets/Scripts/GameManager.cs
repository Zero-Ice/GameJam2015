using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	[SerializeField] GameObject market;
	[SerializeField] GameObject buyMenu;
	[SerializeField] float[] stockPrice;
	[SerializeField] float[] multiplier;
	public GameObject[] playerSprite;
	public Transform wayPoints;

	int turn, turnIndex;
	float playerTurnTime;

	public int noPlayers;
	public int currentPlayerIndex;

	public List<Building> buildingList = null;
	public List<Player> playerList = null;

	public int prevIndex, blockTargetMove, blocksLeftToMove, diceResult;
	public float timerPerUnit, timeMoveLeft;
	public bool canMove;



	bool showBuyMenu, showSellMenu;

	// Use this for initialization
	void Start () {
		turn = 0;
		turnIndex = 0;
		playerTurnTime = 30;
		currentPlayerIndex = 0;
		noPlayers = 2;

		buildingList = new List<Building> ();

		for (int i = 0; i < 30; i++) {
			Building building;
			building = new Building();
			building.type = Building.BuildingType.Company;
			building.buildingIndex = i;
			//buildingList[i].Init(multiplier[i], stockPrice[i]);
			building.Init(1f, 5f);
			buildingList.Add(building);
		}

		playerList = new List<Player> ();

		for (int i = 0; i < noPlayers; i++) {
			Player player = new Player();
			player.Init();
			playerList.Add(player);
		}

		buildingList [0].type = Building.BuildingType.Start;
		buildingList [4].type = Building.BuildingType.RandomEvent;
		buildingList [11].type = Building.BuildingType.Tax;
		buildingList [15].type = Building.BuildingType.Jail;
		buildingList [19].type = Building.BuildingType.FairyGodMother;
		buildingList [26].type = Building.BuildingType.RandomEvent;

		int playerStart = Random.Range (0, noPlayers);
		currentPlayerIndex = playerStart;

		playerList [playerStart].turn = true;

		showBuyMenu = showSellMenu = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (showSellMenu) {
			market.SetActive (true);
		} else {
			market.SetActive(false);
		}
		if (showBuyMenu) {
			buyMenu.SetActive (true);
		} else {
			buyMenu.SetActive(false);
		}
		// Current player

		Player player;	
		player = playerList [currentPlayerIndex];

		playerTurnTime -= Time.deltaTime;

		// If timer runs up, move to the next player
		if (playerTurnTime < 0) {
			// Reset
			playerTurnTime = 30;
			player.End();
			// Go to next player
			++currentPlayerIndex;

			if(currentPlayerIndex >= noPlayers) {
				currentPlayerIndex = 0;
			}

			// Set new current player
			player = playerList [currentPlayerIndex];
		}

		Debug.Log (player.playerState.ToString ());
		// Update current player's turn
		player.UpdateTurn ();

		// If player is done, start the next player's turn
		if (player.done) {
			showBuyMenu = false;
			showSellMenu = false;

			player.End();
			++currentPlayerIndex;

			// Turn increment
			turnIndex += 1 / noPlayers;

			if(turnIndex >= 1) {
				turnIndex = 0;
				turn += 1;
			}

			// Randomize the stocks of every building every 2 turns
			if(turn % 2 == 0){
				foreach(Building x in buildingList){
					x.StockRandomChange();
				}
			}

			if(currentPlayerIndex >= noPlayers) {
				currentPlayerIndex = 0;
			}
		}
		
		if(canMove)
		{
			Debug.Log (blocksLeftToMove + "Dice result");
			Debug.Log (blockTargetMove + "block Target");
//			Debug.Log(prevIndex);
			timeMoveLeft -= Time.deltaTime;
			
			//playerSprite[currentPlayerIndex].transform.position = wayPoints.GetChild(blockTargetMove).position;
			playerSprite[currentPlayerIndex].transform.position = Vector3.Lerp(playerSprite[currentPlayerIndex].transform.position, wayPoints.GetChild(blockTargetMove).position, Time.deltaTime * 20);
			if (timeMoveLeft <= 0)
			{
				timeMoveLeft = timerPerUnit;
				if (blocksLeftToMove != 0)
				{
					
					blockTargetMove = diceResult - (blocksLeftToMove - 1);
					blocksLeftToMove -= 1;
					
					if (blockTargetMove > 29)
						blockTargetMove = prevIndex + blocksLeftToMove - 30;
				}
				else
				{
					Debug.Log("End");
					canMove = false;
					playerList [currentPlayerIndex].rollDiceDone = true;
					playerList [currentPlayerIndex].currentBuildingIndex = blockTargetMove;
				}
			}
		}
	}

	// Player buy
	public void RunBuyMenu(){

		Player player;
		player = playerList [currentPlayerIndex];

		if (CheckIfPlayerCanBuy(player)) {
			// Display the buy menu 
			showBuyMenu = true;
		} else { // If player is unable to buy or borrow
			player.buyDone = true;
		}

		// Transition to sell state
		if (player.buyDone) {
			showBuyMenu = false;
			player.buyDone = false;
			player.playerState = Player.State.SELLMENU;
			return;
		}
	}

	public void BuyButton(){
		if (showBuyMenu) {
			if(playerList [currentPlayerIndex].BuyStock ()){
				playerList [currentPlayerIndex].buyDone = true;
			}
		}
	}

	public void BorrowButton(){
		if (showBuyMenu) {
			if(playerList [currentPlayerIndex].BorrowStock ()){
				playerList [currentPlayerIndex].buyDone = true;
			}
		}
	}

	// Function to check that player has not bought or borrowed from that building
	bool CheckIfPlayerCanBuy(Player player) {
		if (player.stockDataList [player.currentBuildingIndex].stocksBorrowed == 0 && player.stockDataList [player.currentBuildingIndex].stocksBought == 0) {
			return true;
		} else {
			return false;
		}
	}

	// Player sell
	public void RunSellMenu(){
		Player player;
		player = playerList [currentPlayerIndex];

		// Player clicks end turn
		if (player.sellDone) {
			player.done = true;
		}
	}

	// UI Button Function to get out of idle state
	public void RollDice(){
		playerList [currentPlayerIndex].idleDone = true;
	}

	public void OpenMarket(){
		GetComponent<MarketButtonScript> ().OpenUI ();
		showSellMenu = true;
	}

	public void CloseMarket(){
		showSellMenu = false;
	}

	public void EndTurn(){
		playerList [currentPlayerIndex].sellDone = true;
	}
}
