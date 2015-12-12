using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public enum BuildingType
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
	float stockPrice = 5;
	public float finalStockPrice;
	float multiplier = 1;

	public BuildingType type;

	// Use this for initialization
	void Start () {
		finalStockPrice = stockPrice * multiplier;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Execute(Player player){
		switch (type) {
		case BuildingType.Company:
			player.playerState = Player.State.BUYMENU;
			break;
		case BuildingType.RandomEvent:
			Event ();
			player.playerState = Player.State.SELLMENU;
			break;
		case BuildingType.Jail:
			Jail (player);
			player.playerState = Player.State.SELLMENU;
			break;
		case BuildingType.FairyGodMother:
			FairyGodMother();
			break;

		}
	}

	void Jail(Player player){
		player.isJailed = true;
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
		//playerList [currentPlayerIndex].money -= turn * 5000;
	}

	void FairyGodMother(){

	}

	public void StockRandomChange(){
		stockPrice *= Random.Range(0.5f, 1.6f);
	}
}
