using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public string buildingName;
	public int buildingIndex;
	public float stockPrice;
	public float finalStockPrice;
	public float multiplier;

	List<Debt> debtors;


	// Use this for initialization
	void Start () {
		debtors = new List<Debt>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void AddDebtor(Player otherPlayer) {
		debtors.Add(new Debt(otherPlayer));
	}

	public void UpdateDebtors() {
		foreach (Debt x in debtors) {
			if(!x.UpdateTurn()){
				// If player has enough stock to return to the building
				if(x.player.stockDataList[buildingIndex].stocksBorrowed >= x.noStocksOwed) {
					x.player.stockDataList[buildingIndex].stocksBorrowed -= x.noStocksOwed;
					ClearDebtor(x);
				} else {
					// If player fails to pay in stocks, pay money
					x.player.money -= x.noStocksOwed * stockPrice;
				}
			}
		}
	}

	public void ClearDebtor(Player otherPlayer) {
		foreach (Debt x in debtors) {
			if(x.player == otherPlayer) {
				ClearDebtor(x);
				break;
			}
		}
	}

	void ClearDebtor(Debt otherDebt) {
		debtors.Remove (otherDebt);
	}
}

public class Debt : MonoBehaviour {
	
	public Player player;
	public float noStocksOwed;
	byte turns;
	
	public Debt(Player otherPlayer) {
		turns = 5;
		this.player = otherPlayer;
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
