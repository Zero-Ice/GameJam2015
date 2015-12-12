using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MarketButtonScript : MonoBehaviour {
	public GameObject marketData;

	GameManager manager;

	GameObject[][] marketInfo = null;

	// Use this for initialization
	void Start () {
		marketInfo = new GameObject[30][];

		this.manager = GameObject.Find ("Game Manager").GetComponent<GameManager>();

		for(int i = 0; i < 30; i++){
			if(i == 0 || i == 4 || i == 11 || i == 15 || i == 19 || i == 26){
				continue;
			}

			marketInfo[i] = new GameObject[5];

			for (int j = 0; j < 5; j++) {
				int index = i;
				if(i > 0){
					index -= 1;
				}
				if(i > 4){
					index -= 1;
				}
				if(i > 11){
					index -= 1;
				}
				if(i > 15){
					index -= 1;
				}
				if(i > 19){
					index -= 1;
				}
				if(i > 26){
					index -= 1;
				}

				Debug.Log(marketData.transform.GetChild(j).GetChild(index).gameObject.name);
				marketInfo[i][j] = (marketData.transform.GetChild(j).GetChild(index).gameObject);

			}
		}
		Debug.Log("b");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenUI(){
		this.manager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
			
		for (int i = 0; i < 30; i++) {
			if(i == 0 || i == 4 || i == 11 || i == 15 || i == 19 || i == 26){
				continue;
			}

//			Debug.Log(marketInfo[i][j].transform.GetChild(0).GetComponent<Text>().text);

			marketInfo[i][0].transform.GetChild(0).GetComponent<Text>().text = manager.buildingList[i].buildingName;
			Debug.Log(marketInfo[i][0].transform.GetChild(0).GetComponent<Text>().text);

			// If player borrow, set color to red and set to show turns 
			if(player.CheckIfBorrowedStock(i)){
				marketInfo[i][1].GetComponent<RawImage>().color = Color.red;
				marketInfo[i][1].transform.GetChild(0).GetComponent<Text>().color = Color.white;
				foreach(Debt debt in player.debts){
					if(debt.building.buildingIndex == i){
						marketInfo[i][1].transform.GetChild(0).GetComponent<Text>().text = debt.turns.ToString();
						break;
					}
				}
				marketInfo[i][2].transform.GetChild(0).GetComponent<Text>().text = manager.playerList[manager.currentPlayerIndex].stockDataList[i].priceWhenBorrowed.ToString();
				marketInfo[i][3].transform.GetChild(0).GetComponent<Text>().text = manager.buildingList[i].finalStockPrice.ToString();
				marketInfo[i][4].transform.GetChild(0).GetComponent<Text>().text = 
					(manager.playerList[manager.currentPlayerIndex].stockDataList[i].priceWhenBorrowed - manager.buildingList[i].finalStockPrice).ToString();

			} else if(player.CheckIfBoughtStock(i)){// Set color to blue and no text
				marketInfo[i][1].GetComponent<RawImage>().color = Color.blue;
				marketInfo[i][1].transform.GetChild(0).GetComponent<Text>().color = Color.white;
				marketInfo[i][1].transform.GetChild(0).GetComponent<Text>().text = "";

				marketInfo[i][2].transform.GetChild(0).GetComponent<Text>().text = manager.playerList[manager.currentPlayerIndex].stockDataList[i].priceWhenBorrowed.ToString();
				marketInfo[i][3].transform.GetChild(0).GetComponent<Text>().text = manager.buildingList[i].finalStockPrice.ToString();
				marketInfo[i][4].transform.GetChild(0).GetComponent<Text>().text = 
					(manager.playerList[manager.currentPlayerIndex].stockDataList[i].priceWhenBorrowed - manager.buildingList[i].finalStockPrice).ToString();

			} else { // If player has no relation with the building
				marketInfo[i][1].GetComponent<RawImage>().color = Color.white;
				marketInfo[i][1].transform.GetChild(0).GetComponent<Text>().text = "";

				marketInfo[i][2].transform.GetChild(0).GetComponent<Text>().text = "";
				marketInfo[i][3].transform.GetChild(0).GetComponent<Text>().text = "";
				marketInfo[i][4].transform.GetChild(0).GetComponent<Text>().text = "";
			}
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
