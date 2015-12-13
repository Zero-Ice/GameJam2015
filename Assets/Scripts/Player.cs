using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {

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
	
	public Building currentBuilding;

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
	void Update () 
	{
		
	}

	public void End(){
		playerState = State.IDLE;
		turn = done = idleDone = buyDone = sellDone = rollDiceDone = isJailed = false;
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
		if (!manager.canMove)
		{
		// To Do: Link Dice UI
		int diceroll1 = Random.Range (1, 7);
		int diceroll2 = Random.Range (1, 7);

			Debug.Log("Dice roll");

		// If player is jailed, check for same dice roll
		if (isJailed) {
			if(diceroll1 == diceroll2){
				isJailed = false;
			} else {
				rollDiceDone = true;
			}
		}

		int diceResults = diceroll1 + diceroll2;
		
		// If player is not jailed, move the player
		if (!isJailed) {
			manager.prevIndex = currentBuildingIndex;

			manager.timerPerUnit = (float)(1f/diceResults);
			manager.timeMoveLeft = (float)(1f/diceResults);

			// Initial target 
			manager.blockTargetMove = currentBuildingIndex + 1;
			if(manager.blockTargetMove > 29){
					manager.blockTargetMove -= 30;
				}

			// Final target
			manager.blockFinalTarget = diceResults + currentBuildingIndex;
			if(manager.blockFinalTarget > 29){
					manager.blockFinalTarget -= 30;
			}
				Debug.Log(manager.blockFinalTarget.ToString());
			manager.diceResult = diceResults;
			
			//manager.blocksLeftToMove = currentBuildingIndex;

			manager.canMove = true;

			currentBuilding = manager.buildingList [currentBuildingIndex];
			
			// switch for building that player lands on
			// Execute only once
			//currentBuilding.Execute (this);
			}
		}
	}

	public bool BuyStock() {
		// If player has money to buy 1000 stocks
		if (money > manager.buildingList [currentBuildingIndex].finalStockPrice * 1000) {
			stockDataList [currentBuildingIndex].stocksBought += 1000;
			money -= manager.buildingList [currentBuildingIndex].finalStockPrice * 1000;
			stockDataList[currentBuildingIndex].priceWhenBorrowed = manager.buildingList[currentBuildingIndex].finalStockPrice;
			return true;
		} else {
			// UI to tell that the player does not have money to buy stock
			return false;
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

	public bool BorrowStock(){
		// If player has not bought or borrowed from that company

		AddDebt(manager.buildingList[currentBuildingIndex]);
		stockDataList [currentBuildingIndex].stocksBorrowed += 1000;
		stockDataList[currentBuildingIndex].currentStocks += 1000;
		stockDataList[currentBuildingIndex].priceWhenBorrowed = manager.buildingList[currentBuildingIndex].finalStockPrice;
		return true;
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

public class StockData {

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

public class Debt {
	
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

