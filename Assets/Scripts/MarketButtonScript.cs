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

				//Debug.Log(marketData.transform.GetChild(j).GetChild(index).gameObject.name);
				marketInfo[i][j] = (marketData.transform.GetChild(j).GetChild(index).gameObject);

			}
		}
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

			// If player borrow, set color to red and set to show turns 
			if(player.CheckIfBorrowedStock(i)){
				marketInfo[i][1].GetComponent<RawImage>().color = Color.red;
				//marketInfo[i][1].transform.GetChild(0).GetComponent<Text>().color = Color.white;
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
				//marketInfo[i][1].transform.GetChild(0).GetComponent<Text>().color = Color.white;
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

	//1
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

	//2
	public void Sell2(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (2);
	}
	
	public void Return2(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (2);
	}

	//3
	public void Sell3(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (3);
	}
	
	public void Return3(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (3);
	}

	//4
	public void Sell4(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (5);
	}
	
	public void Return4(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (5);
	}

	//5
	public void Sell5(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (6);
	}
	
	public void Return5(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (6);
	}

	//6
	public void Sell6(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (7);
	}
	
	public void Return6(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (7);
	}

	//7
	public void Sell7(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (8);
	}
	
	public void Return7(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (8);
	}

	//8
	public void Sell8(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (9);
	}
	
	public void Return8(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (9);
	}

	//9
	public void Sell9(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (10);
	}
	
	public void Return9(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (10);
	}

	//10
	public void Sell10(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (12);
	}
	
	public void Return10(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (12);
	}

	//11
	public void Sell11(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (13);
	}
	
	public void Return11(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (13);
	}

	//12
	public void Sell12(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (14);
	}
	
	public void Return12(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (14);
	}

	//13
	public void Sell13(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (16);
	}
	
	public void Return13(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (16);
	}

	//14
	public void Sell14(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (17);
	}
	
	public void Return14(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (17);
	}

	//15
	public void Sell15(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (18);
	}
	
	public void Return15(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (18);
	}

	//16
	public void Sell16(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (20);
	}
	
	public void Return16(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (20);
	}

	//17
	public void Sell17(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (21);
	}
	
	public void Return17(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (21);
	}

	//18
	public void Sell18(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (22);
	}
	
	public void Return18(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (22);
	}

	//19
	public void Sell19(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (23);
	}
	
	public void Return19(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (23);
	}

	//20
	public void Sell20(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (24);
	}
	
	public void Return20(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (24);
	}

	//21
	public void Sell21(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (25);
	}
	
	public void Return21(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (25);
	}

	//22
	public void Sell22(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (27);
	}
	
	public void Return22(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (27);
	}

	//23
	public void Sell23(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (28);
	}
	
	public void Return23(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (28);
	}

	//24
	public void Sell24(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.SellStock (29);
	}
	
	public void Return24(){
		Player player;
		player = manager.playerList [manager.currentPlayerIndex];
		
		player.ReturnStock (29);
	}
}

