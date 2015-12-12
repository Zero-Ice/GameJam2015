using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	int turn;
	int turnIndex;
	float playerTurnTime;

	public int noPlayers;
	public int currentPlayerIndex;

	public List<Building> buildingList;
	public List<Player> playerList;

	bool showBuyMenu, showSellMenu;

	// Use this for initialization
	void Start () {
		turn = 0;
		turnIndex = 0;
		playerTurnTime = 30;
		currentPlayerIndex = 0;

		playerList = new List<Player> (noPlayers);
		buildingList = new List<Building> (30);

		for (int i = 0; i < 29; i++) {
			buildingList[i] = new Building();
			buildingList[i].type = Building.BuildingType.Company;
			buildingList[i].buildingIndex = i;
		}

		buildingList [0].type = Building.BuildingType.Start;
		buildingList [4].type = Building.BuildingType.RandomEvent;
		buildingList [11].type = Building.BuildingType.Tax;
		buildingList [15].type = Building.BuildingType.Jail;
		buildingList [19].type = Building.BuildingType.FairyGodMother;
		buildingList [26].type = Building.BuildingType.RandomEvent;

		playerList.Add (new Player());
		playerList.Add (new Player());
		playerList [0].turn = true;

		showBuyMenu = showSellMenu = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Current player
		Player player;
		player = playerList [currentPlayerIndex];

		playerTurnTime -= Time.deltaTime;

		// If timer runs up, move to the next player
		if (playerTurnTime < 0) {
			// Reset
			playerTurnTime = 25;
			player.End();
			// Go to next player
			++currentPlayerIndex;
			if(currentPlayerIndex > noPlayers) {
				currentPlayerIndex = 0;
			}
			// Set new current player
			player = playerList [currentPlayerIndex];
		}

		// Update current player's turn
		player.UpdateTurn ();

		// If player is done, start the next player's turn
		if (player.done) {
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

			if(currentPlayerIndex > noPlayers) {
				currentPlayerIndex = 0;
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
		}
	}

	public void BuyButton(){
		playerList [currentPlayerIndex].BuyStock ();
		playerList [currentPlayerIndex].buyDone = true;
	}

	public void BorrowButton(){
		playerList [currentPlayerIndex].BorrowStock ();
		playerList [currentPlayerIndex].buyDone = true;
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
		showSellMenu = true;

		Player player;
		player = playerList [currentPlayerIndex];
		//if(UI click sell) {
			//playerList[currentPlayerIndex].SellStock(
		//}

		// Player clicks end turn
		if (player.sellDone) {
			player.done = true;
		}
	}
}
