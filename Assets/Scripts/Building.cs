using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public 	enum BuildingType
	{
		Start,
		Company,
		RandomEvent,
		Tax,
		FairyGodMother,
		Jail,
	};

	public string buildingName = "";
	public int buildingIndex;
	public float stockPrice = 5;
	public float finalStockPrice;
	public float multiplier = 1;

	List<Debt> debtors;

	public BuildingType type;

	// Use this for initialization
	void Start () {
		debtors = new List<Debt>();
		finalStockPrice = stockPrice * multiplier;
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
					x.player.ReturnStock(buildingIndex);
				} else {
					x.player.ReturnStock(buildingIndex);
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
		turns = 3;
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
