using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	int turn;
	int turnIndex;
	float playerTurnTime;

	public int noPlayers;
	int currentPlayerIndex;

	public List<Building> buildingList;
	List<Player> playerList;

	bool showBuyMenu, showSellMenu;

	// Use this for initialization
	void Start () {
		turn = 0;
		turnIndex = 0;
		playerTurnTime = 30;
		currentPlayerIndex = 0;

		playerList = new List<Player> (noPlayers);
		buildingList = new List<Building> (30);

		for (int i = 0; i < 30; i++) {
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

		showBuyMenu = false;
		showSellMenu = false;
	}
	
	// Update is called once per frame
	void Update () {
		Player player;
		player = playerList [currentPlayerIndex];

		playerTurnTime -= Time.deltaTime;

		// If timer runs up, move to the next player
		if (playerTurnTime < 0) {
			playerTurnTime = 25;
			++currentPlayerIndex;
			if(currentPlayerIndex > noPlayers) {
				currentPlayerIndex = 0;
			}
		}

		// Update current player's turn
		player.UpdateTurn ();

		// If player is done, start the next player's turn
		if (player.done) {
			player.done = false;
			++currentPlayerIndex;
			// Turn increment
			turnIndex += 1 / noPlayers;

			if(turnIndex >= 1) {
				turnIndex = 0;
				turn += 1;
			}

			if(turn % 2 == 0){
				foreach(Building x in buildingList){
					x.stockPrice *= Random.Range(0.5f, 1.6f);
				}
			}

			if(currentPlayerIndex > noPlayers) {
				currentPlayerIndex = 0;
			}
		}
	}

	// Player buy
	public void RunBuyMenu(){
		showBuyMenu = true;

		Player player;
		player = playerList [currentPlayerIndex];

		if (CheckIfPlayerCanBuy(player)) {

			// UI function here
			//if(UI click buy ) {
			//	playerList [currentPlayerIndex].BuyStock ();
			//} else if (UI click borrow) {
			//	playerList [currentPlayerIndex].BorrowStock ();
			//} else if (UI click close) {
			//	player.buyDone = true;
			//}
		} else { // If player is unable to perform any actions
			player.buyDone = true;
		}

		if (player.buyDone) {
			showBuyMenu = false;
			player.buyDone = false;
			player.playerState = Player.State.SELLMENU;
		}
	}

	bool CheckIfPlayerCanBuy(Player player) {
		if (player.stockDataList [player.currentBuildingIndex].stocksBorrowed == 0 && player.stockDataList [player.currentBuildingIndex].stocksBought == 0) {
			return true;
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


	}

	void Jail(){
		playerList [currentPlayerIndex].isJailed = true;
	}

	void Event(){
		int eventNo;

		eventNo = Random.Range (0, 3);

		switch (eventNo) {
		case 0:
			Event1();
			break;
		case 1:
			Event2 ();
			break;
		case 2:
			Event3 ();
			break;
		}
	}

	void Event1(){

	}

	void Event2(){

	}

	void Event3(){

	}

	void Tax(){
		playerList [currentPlayerIndex].money -= turn * 5000;
	}
}
