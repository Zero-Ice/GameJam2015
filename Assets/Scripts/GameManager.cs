using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	int turn;
	float playerTurnTime;
	public int noPlayers;
	int currentPlayerIndex;

	List<Building> buildingList;
	List<Player> playerList;

	// Use this for initialization
	void Start () {
		int turn = 25;
		float playerTurnTime = 30;
		currentPlayerIndex = 0;
		playerList = new List<Player> (noPlayers);
		buildingList = new List<Building> (24);
		playerList.Add (new Player());
		playerList.Add (new Player());
		playerList [0].turn = true;
	}
	
	// Update is called once per frame
	void Update () {
		playerTurnTime -= Time.deltaTime;

		if (playerTurnTime < 0) {
			playerTurnTime = 25;
			++currentPlayerIndex;
			if(currentPlayerIndex > noPlayers) {
				currentPlayerIndex = 0;
			}
		}

		playerList [currentPlayerIndex].UpdateTurn ();
		if (playerList [currentPlayerIndex].done) {
			++currentPlayerIndex;
			if(currentPlayerIndex > noPlayers) {
				currentPlayerIndex = 0;
			}
		}
	}

	// Player buy
	public void RunBuyMenu(){
		
	}
	
	// Player sell
	public void RunSellMenu(){
		
	}

	void Jail(){
		playerList [currentPlayerIndex].isJailed = true;
	}

	void Event(){
		int eventNo;

		eventNo = Random.Range (0, 3);

		switch (eventNo) {
		case 0:
			Event1();
			break;
		case 1:
			Event2 ();
			break;
		case 2:
			Event3 ();
			break;
		}
	}

	void Event1(){

	}

	void Event2(){

	}

	void Event3(){

	}

	void Tax(){
		playerList [currentPlayerIndex].money -= 10000;
	}
}
