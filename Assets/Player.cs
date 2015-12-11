using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public List<StockData> stockDataList;

	public float money;

	public bool turn, done;

	// Use this for initialization
	void Start () {
		money = 200000;
		turn = false;
		done = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void UpdateTurn() {

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


