using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	int turn;
	float playerTurnTime;
	float noPlayers;
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
		playerList.Add (Player);
		playerList.Add (Player);
		playerList [0].turn = true;
	}
	
	// Update is called once per frame
	void Update () {
		playerList [currentPlayerIndex].UpdateTurn ();
		if (playerList [currentPlayerIndex].done) {
			++currentPlayerIndex;
			if(currentPlayerIndex > noPlayers) {
				currentPlayerIndex = 0;
			}
		}
	}
}
