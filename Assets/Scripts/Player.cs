using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public List<StockData> stockDataList;

	public float money;

	public bool turn, done, isJailed;

	State playerState;

	GameManager manager;

	enum State
	{
		IDLE,
		ROLLDICE,
		BUYMENU, // buy 
		SELLMENU,
	};

	// Use this for initialization
	void Start () {
		money = 200000;
		turn = false;
		done = false;
		isJailed = false;

		playerState = State.IDLE;

		this.manager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
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
	
	}

	// Wait for dice to roll finish and check which block player lands on
	void RunRollDice(){

	}

	public void BuyStock(int buildingIndex, Building otherBuilding) {
		stockDataList [buildingIndex].stocksBought += 1000;
	}

	public void BorrowStock(int buildingIndex, Building otherBuilding){
		otherBuilding.AddDebtor (this);
		stockDataList [buildingIndex].stocksBorrowed += 1000;
	}

	public void ReturnStock(int buildingIndex, Building otherBuilding) {
		otherBuilding.ClearDebtor (this);
		stockDataList [buildingIndex].stocksBorrowed -= 1000;
	}
}

public class StockData : MonoBehaviour {

	public Building building;
	// Stocks bought from that company
	public float stocksBought;
	// Stocks borrowed from that company
	public float stocksBorrowed;
}


