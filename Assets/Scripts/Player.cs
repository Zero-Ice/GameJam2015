using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public enum State
	{
		IDLE,
		ROLLDICE,
		BUYMENU, // buy 
		SELLMENU,
	};

	public List<StockData> stockDataList;

	public float money;

	public bool turn, done, idleDone, rollDiceDone, buyDone, sellDone, isJailed;

	public State playerState;

	GameManager manager;

	public int currentBuildingIndex;
	Building currentBuilding;

	// Use this for initialization
	void Start () {
		money = 200000;
		turn = done = idleDone = buyDone = rollDiceDone = sellDone = isJailed = false;

		playerState = State.IDLE;

		currentBuildingIndex = 0;

		stockDataList = new List<StockData> (30);

		this.manager = GameObject.Find ("Game Manager").GetComponent<GameManager>();

		for (int i = 0; i < 30; i++) {
			stockDataList.Add(new StockData());
			stockDataList[i].building = manager.buildingList[i];
		}


	}

	// Update is called once per frame
	void Update () {
		
	}

	public void End(){
		playerState = State.IDLE;
		turn = done = idleDone = buyDone = rollDiceDone = isJailed = false;
	}

	public void UpdateTurn() {
		switch (playerState) {
		case State.IDLE:
			RunIdle ();
			break;
		case State.ROLLDICE:
			RunRollDice();
			break;
		case State.BUYMENU:
			buyDone = false;
			manager.RunBuyMenu();
			break;
		case State.SELLMENU:
			manager.RunSellMenu();
			break;
		}
	}

	// Wait for player to roll dice
	void RunIdle(){
		// Player will click on a button which sets idle done, then transition to roll dice state
		if (idleDone) {
			playerState = State.ROLLDICE;
		}
	}

	// Wait for dice to roll finish and check which block player lands on
	void RunRollDice(){
		int diceroll = Random.Range (1, 7) + Random.Range (1, 7);
		currentBuildingIndex += diceroll;
		currentBuilding = manager.buildingList [currentBuildingIndex];

		// switch for building that player lands on
	}

	public void BuyStock() {
		// If player has money to buy 1000 stocks
		if (money > manager.buildingList [currentBuildingIndex].finalStockPrice * 1000) {
			stockDataList [currentBuildingIndex].stocksBought += 1000;
			money -= manager.buildingList [currentBuildingIndex].finalStockPrice * 1000;
		} else {
			// UI to tell that the player does not have money to buy stock
		}
	}

	public void SellStock(int buildingIndex) {
		stockDataList [buildingIndex].stocksBought = 0;
		money += 1000 * manager.buildingList [buildingIndex].finalStockPrice;
	}

	public void BorrowStock(){
		// If player has not bought or borrowed from that company
		if (stockDataList [currentBuildingIndex].stocksBought > 0 || stockDataList [currentBuildingIndex].stocksBorrowed > 0) {
			currentBuilding.AddDebtor (this);
			stockDataList [currentBuildingIndex].stocksBorrowed += 1000;
			stockDataList[currentBuildingIndex].currentStocks += 1000;
			stockDataList[currentBuildingIndex].priceWhenBorrowed = manager.buildingList[currentBuildingIndex].finalStockPrice;
		}
	}

	public void ReturnStock(int buildingIndex) {
		manager.buildingList[buildingIndex].ClearDebtor (this);
		stockDataList [buildingIndex].stocksBorrowed = 0;

		// If player has kept borrowed stock
		if (stockDataList [buildingIndex].currentStocks >= 1000) {
			stockDataList[buildingIndex].currentStocks = 0;
		} else {
			// Player auto buys stocks from market ( loses money ) and returns stock borrowed
			money -= manager.buildingList [buildingIndex].finalStockPrice * 1000;
		}
	}
}

public class StockData : MonoBehaviour {

	public Building building = null;

	// Stocks bought from that company
	public float stocksBought = 0;

	// Stocks borrowed from that company
	public float stocksBorrowed = 0;

	// current stocks for return
	public float currentStocks = 0;

	public float priceWhenBorrowed = 0;
}


