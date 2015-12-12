using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MarketButtonScript : MonoBehaviour {
	public GameObject marketData;

	GameManager manager;

	GameObject[][] marketInfo = null;

	// Use this for initialization
	void Start () {
		this.manager = GameObject.Find ("Game Manager").GetComponent<GameManager>();

		for(int i = 0; i < 30; i++){
			for (int j = 0; j < 5; j++) {
				if(i == 0 || i == 4 || i == 11 || i == 15 || i == 19 || i == 26){
					marketInfo[i][j] = null;
					continue;
				}
				marketInfo[i][j] = marketData.transform.GetChild(j).GetChild(i).gameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OpenUI(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
			
		for (int i = 0; i < 30; i++) {
			if(i == 0 || i == 4 || i == 11 || i == 15 || i == 19 || i == 26){
				continue;
			}

			marketInfo[i][0].GetComponentInChildren<Text>().text = manager.buildingList[i].name;
			// If player borrow, set color to red and set to show turns 
			if(player.CheckIfBorrowedStock(i)){
				marketInfo[i][1].GetComponent<RawImage>().color = Color.red;
				marketInfo[i][1].GetComponentInChildren<Text>().color = Color.white;
				foreach(Debt debt in player.debts){
					if(debt.building.buildingIndex == i){
						marketInfo[i][1].GetComponentInChildren<Text>().text = debt.turns.ToString();
						break;
					}
				}
			} else if(player.CheckIfBoughtStock(i)){// Set color to blue and no text
				marketInfo[i][1].GetComponent<RawImage>().color = Color.blue;
				marketInfo[i][1].GetComponentInChildren<Text>().color = Color.white;
				marketInfo[i][1].GetComponentInChildren<Text>().text = "";
			} else { // If player has no relation with the building
				marketInfo[i][1].GetComponent<RawImage>().color = Color.white;
				marketInfo[i][1].GetComponentInChildren<Text>().text = "";
			}

			marketInfo[i][2].GetComponentInChildren<Text>().text = manager.playerList[manager.currentPlayerIndex].stockDataList[i].priceWhenBorrowed.ToString();
			marketInfo[i][3].GetComponentInChildren<Text>().text = manager.buildingList[i].finalStockPrice.ToString();
			marketInfo[i][4].GetComponentInChildren<Text>().text = 
			(manager.playerList[manager.currentPlayerIndex].stockDataList[i].priceWhenBorrowed - manager.buildingList[i].finalStockPrice).ToString();
		}
	}

	public void Sell1(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];

		player.SellStock (1);
	}

	public void Return1(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];

		player.ReturnStock (1);
	}
}
