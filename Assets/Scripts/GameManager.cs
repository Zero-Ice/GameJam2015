using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	[SerializeField] GameObject market;
	[SerializeField] GameObject buyMenu;
	[SerializeField] float[] stockPrice;
	[SerializeField] float[] multiplier;
	[SerializeField] GameObject player1UI;
	[SerializeField] GameObject player2UI;
	public GameObject[] playerSprite;
	public Transform wayPoints;

	int turn, turnIndex;
	float playerTurnTime;

	public int noPlayers;
	public int currentPlayerIndex;

	public List<Building> buildingList = null;
	public List<Player> playerList = null;

	public int prevIndex, blockTargetMove, blockFinalTarget, blocksLeftToMove, diceResult;
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

		canMove = false;
		timeMoveLeft = 5;

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

		player1UI.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);	
		player2UI.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);	

		switch (playerStart) {
		case 0:
			player1UI.GetComponent<Image> ().color = new Color(1, 1, 1, 1f);
			break;
		case 1:
			player2UI.GetComponent<Image> ().color = new Color(1, 1, 1, 1f);
			break;
		}

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
		
		// Update current player's turn
		player.UpdateTurn ();

		if(canMove)
		{
			timeMoveLeft -= Time.deltaTime;
			
			//playerSprite[currentPlayerIndex].transform.position = wayPoints.GetChild(blockTargetMove).position;
			playerSprite[currentPlayerIndex].transform.position = Vector3.Lerp(playerSprite[currentPlayerIndex].transform.position, wayPoints.GetChild(blockTargetMove).position, Time.deltaTime * 20);
			if (timeMoveLeft <= 0)
			{
				timeMoveLeft = timerPerUnit;
				
				if(blockTargetMove != blockFinalTarget){
					blockTargetMove += 1;
					if (blockTargetMove > 29)
						blockTargetMove = 0;
				}
				else
				{
					canMove = false;
					
					player.rollDiceDone = true;
					player.currentBuildingIndex = blockFinalTarget;
					player.currentBuilding = buildingList [player.currentBuildingIndex];
					player.currentBuilding.Execute(player);
				}
			}
		}

		// If player is done, start the next player's turn
		if (player.done) {
			// Reset values
			showBuyMenu = false;
			showSellMenu = false;

			playerTurnTime = 30;
			player.End();

			// Change the non-active player UI to translucent
			switch (currentPlayerIndex) {
			case 0:
				player1UI.GetComponent<Image> ().color = new Color(1, 1, 1, 0.3f);
				break;
			case 1:
				player2UI.GetComponent<Image> ().color = new Color(1, 1, 1, 0.3f);
				break;
			}

			// Next player
			++currentPlayerIndex;
			if(currentPlayerIndex >= noPlayers) {
				currentPlayerIndex = 0;
			}

			// Set the new active player UI to opaque
			switch (currentPlayerIndex) {
			case 0:
				player1UI.GetComponent<Image> ().color = new Color(1, 1, 1, 1f);
				break;
			case 1:
				player2UI.GetComponent<Image> ().color = new Color(1, 1, 1, 1f);
				break;
			}

			Debug.Log("Button Player End Turn");

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
		}
		// If timer runs up, move to the next player
		else if (playerTurnTime < 0) {
			showBuyMenu = false;
			showSellMenu = false;

			// Reset
			playerTurnTime = 30;
			player.End();
			// Go to next player

			switch (currentPlayerIndex) {
			case 0:
				player1UI.GetComponent<Image> ().color = new Color(1, 1, 1, 0.3f);
				break;
			case 1:
				player2UI.GetComponent<Image> ().color = new Color(1, 1, 1, 0.3f);
				break;
			}

			++currentPlayerIndex;
			if(currentPlayerIndex >= noPlayers) {
				currentPlayerIndex = 0;
			}

			switch (currentPlayerIndex) {
			case 0:
				player1UI.GetComponent<Image> ().color = new Color(1, 1, 1, 1f);
				break;
			case 1:
				player2UI.GetComponent<Image> ().color = new Color(1, 1, 1, 1f);
				break;
			}

			Debug.Log("Timer Run Out Player End Turn");
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
			Debug.Log("Buy Done");
			showBuyMenu = false;
			player.buyDone = false;
			player.playerState = Player.State.SELLMENU;
		}
	}

	public void BuyButton(){
		Player player = playerList [currentPlayerIndex];
		if (showBuyMenu && player.buyDone == false && player.playerState == Player.State.BUYMENU) {
			if(player.BuyStock ()){
				player.buyDone = true;
				showBuyMenu = false;
			}
		}
	}

	public void BorrowButton(){
		Player player = playerList [currentPlayerIndex];
		if (showBuyMenu && player.buyDone == false && player.playerState == Player.State.BUYMENU) {
			if(player.BorrowStock ()){
				player.buyDone = true;
				showBuyMenu = false;
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
		if (playerList [currentPlayerIndex].playerState == Player.State.IDLE) {
			playerList [currentPlayerIndex].idleDone = true;
		}
	}

	public void OpenMarket(){
		if (playerList [currentPlayerIndex].playerState == Player.State.SELLMENU) {
			GetComponent<MarketButtonScript> ().OpenUI ();
			showSellMenu = true;
		}
	}

	public void CloseMarket(){
		showSellMenu = false;
	}

	public void EndTurn(){
		if (playerList [currentPlayerIndex].playerState == Player.State.SELLMENU) {
			playerList [currentPlayerIndex].sellDone = true;
		}
	}
}
