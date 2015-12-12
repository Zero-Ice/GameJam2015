using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MarketButtonScript : MonoBehaviour {
	GameObject[][] marketInfo;

	public GameObject[] texts;
	public GameObject[] texts1;
	public GameObject[] texts2;
	public GameObject[] texts3;
	public GameObject[] texts4;

	GameManager manager;

	// Use this for initialization
	void Start () {
		this.manager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
		marketInfo [0] = texts;
		marketInfo [1] = texts1;
		marketInfo [2] = texts2;
		marketInfo [3] = texts3;
		marketInfo [4] = texts4;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OpenUI(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
			
		for (int i = 0; i < 29; i++) {
			if(i == 0 || i == 4 || i == 11 || i == 15 || i == 19 || i == 26){
				continue;
			}

			marketInfo[i][0].GetComponent<Text>().text = manager.buildingList[i].name;
			// If player borrow, set color to red and set to show turns 
			if(player.CheckIfBorrowedStock(i)){
				marketInfo[i][1].GetComponent<Text>().color = Color.red;
				foreach(Debt debt in player.debts){
					if(debt.building.buildingIndex == i){
						marketInfo[i][1].GetComponent<Text>().text = debt.turns.ToString();
						break;
					}
				}
			} else if(player.CheckIfBoughtStock(i)){// Set color to blue and no text
				marketInfo[i][1].GetComponent<Text>().color = Color.blue;
				marketInfo[i][1].GetComponent<Text>().text = "";
			} else { // If player has no relation with the building
				marketInfo[i][1].GetComponent<Text>().color = Color.black;
				marketInfo[i][1].GetComponent<Text>().text = "";
			}

			marketInfo[i][2].GetComponent<Text>().text = manager.playerList[manager.currentPlayerIndex].stockDataList[i].priceWhenBorrowed.ToString();
			marketInfo[i][3].GetComponent<Text>().text = manager.buildingList[i].finalStockPrice.ToString();
			marketInfo[i][4].GetComponent<Text>().text = 
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
