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

	public Player(){

	}

	public List<StockData> stockDataList = null;
	public List<Debt> debts = null;

	public float money;

	public bool turn, done, idleDone, rollDiceDone, buyDone, sellDone, isJailed;

	public State playerState;

	GameManager manager;

	public int currentBuildingIndex;
	Building currentBuilding;

	// Use this for initialization
	void Start () {
	}

	public void Init(){
		money = 200000;
		turn = done = idleDone = buyDone = rollDiceDone = sellDone = isJailed = false;
		
		playerState = State.IDLE;
		
		currentBuildingIndex = 0;
		
		stockDataList = new List<StockData> ();
		debts = new List<Debt>();
		
		this.manager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
		
		for (int i = 0; i < 30; i++) {
			StockData stockData = new StockData();
			stockData.building = manager.buildingList[i];
			stockDataList.Add(stockData);
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
		// To Do: Link Dice UI
		int diceroll1 = Random.Range (1, 7);
		int diceroll2 = Random.Range (1, 7);

		// To Do: Link Jailbreak UI, pay or roll double to escape

		// If player is jailed, check for same dice roll
		if (isJailed) {
			if(diceroll1 == diceroll2){
				isJailed = false;
			} else {
				rollDiceDone = true;
			}
		}

		// If player is not jailed, move the player
		if (!isJailed) {
			int prevIndex = currentBuildingIndex;
			currentBuildingIndex += (diceroll1 + diceroll2);
			if(currentBuildingIndex > 29){
				currentBuildingIndex -= 29;
			}

			currentBuilding = manager.buildingList [currentBuildingIndex];
			
			// switch for building that player lands on
			// Execute only once
			currentBuilding.Execute (this);

			rollDiceDone = true;
		}
	}

	public void BuyStock() {
		// If player has money to buy 1000 stocks
		if (money > manager.buildingList [currentBuildingIndex].finalStockPrice * 1000) {
			stockDataList [currentBuildingIndex].stocksBought += 1000;
			money -= manager.buildingList [currentBuildingIndex].finalStockPrice * 1000;
			stockDataList[currentBuildingIndex].priceWhenBorrowed = manager.buildingList[currentBuildingIndex].finalStockPrice;
		} else {
			// UI to tell that the player does not have money to buy stock
		}
	}

	public void SellStock(int buildingIndex) {
		if (CheckIfBoughtStock(buildingIndex)) {
			stockDataList [buildingIndex].Reset();
			money += 1000 * manager.buildingList [buildingIndex].finalStockPrice;
		} else if (CheckIfBorrowedStock(buildingIndex)) {
			stockDataList [buildingIndex].currentStocks = 0;
			money += 1000 * manager.buildingList [buildingIndex].finalStockPrice;
		}
	}

	public void BorrowStock(){
		// If player has not bought or borrowed from that company
		if (stockDataList [currentBuildingIndex].stocksBought > 0 || stockDataList [currentBuildingIndex].stocksBorrowed > 0) {
			AddDebt(manager.buildingList[currentBuildingIndex]);
			stockDataList [currentBuildingIndex].stocksBorrowed += 1000;
			stockDataList[currentBuildingIndex].currentStocks += 1000;
			stockDataList[currentBuildingIndex].priceWhenBorrowed = manager.buildingList[currentBuildingIndex].finalStockPrice;
		}
	}

	public void ReturnStock(int buildingIndex) {
		if (CheckIfBorrowedStock(buildingIndex)) {
			ClearDebtor (buildingIndex);

			// If player has kept borrowed stock
			if (stockDataList [buildingIndex].currentStocks >= 1000) {
				stockDataList [buildingIndex].Reset();
			} else {
				// Player auto buys stocks from market ( loses money ) and returns stock borrowed
				money -= manager.buildingList [buildingIndex].finalStockPrice * 1000;
				stockDataList [buildingIndex].Reset();
			}
		}
	}

	public bool CheckIfBoughtStock(int buildingIndex){
		if (stockDataList [buildingIndex].stocksBought > 0) {
			return true;
		} else {
				return false;
		}
	}

	public bool CheckIfBorrowedStock(int buildingIndex){
		if (stockDataList [buildingIndex].stocksBorrowed > 0) {
			return true;
		} else {
			return false;
		}
	}

	public void AddDebt(Building building) {
		debts.Add(new Debt(building));
	}

	public void UpdateDebtors() {
		foreach (Debt x in debts) {
			if(!x.UpdateTurn()){
				ReturnStock(x.building.buildingIndex);
			}
		}
	}
	
	public void ClearDebtor(int buildingIndex) {
		foreach (Debt x in debts) {
			if(x.building.buildingIndex == buildingIndex) {
				ClearDebtor(x);
				break;
			}
		}
	}
	
	void ClearDebtor(Debt otherDebt) {
		debts.Remove (otherDebt);
	}
}

public class StockData : MonoBehaviour {

	public StockData(){

	}

	public Building building = null;

	// Stocks bought from that company
	public float stocksBought = 0;

	// Stocks borrowed from that company
	public float stocksBorrowed = 0;

	// current stocks for return
	public float currentStocks = 0;

	// Price when borrowed / brought
	public float priceWhenBorrowed = 0;

	public void Reset(){
		stocksBought = 0;
		stocksBorrowed = 0;
		currentStocks = 0;
		priceWhenBorrowed = 0;
	}
}

public class Debt : MonoBehaviour {
	
	public Building building;
	public float noStocksOwed;
	public int turns;
	
	public Debt(Building building) {
		turns = 3;
		this.building = building;
	}
	
	public bool UpdateTurn() {
		--turns;
		if (turns > 0)
			return true;
		else {
			return false;
		}
	}
	
	void Start() {
		
	}
	
}

