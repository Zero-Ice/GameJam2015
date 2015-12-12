using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building {

	public enum BuildingType
	{
		Start,
		Company,
		RandomEvent,
		Tax,
		FairyGodMother,
		Jail,
	};

	public Building(){

	}

	public string buildingName = "ayy";
	public int buildingIndex;
	float stockPrice = 5;
	public float finalStockPrice;
	float multiplier = 1;

	public BuildingType type;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Init(float multiplier, float stockPrice){
		this.multiplier = multiplier;
		this.stockPrice = stockPrice;
		finalStockPrice = stockPrice * multiplier;
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
			player.playerState = Player.State.SELLMENU;
			break;
		case BuildingType.Tax:
			Tax (player);
			player.playerState = Player.State.SELLMENU;
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
		default:
			break;
		}
	}
	
	void Event1(){
		GameManager manager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		int building1 = Random.Range(0, 30);
		int building2 = Random.Range(0, 30);

		manager.buildingList [building1].StockRandomChange ();
		manager.buildingList [building2].StockRandomChange ();
	}
	
	void Event2(){
		GameManager manager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		int building1 = Random.Range(0, 30);
		int building2 = Random.Range(0, 30);
		
		manager.buildingList [building1].stockPrice *= Random.Range(1f, 1.5f);
		manager.buildingList [building2].stockPrice *= Random.Range (1f, 1.5f);
	}
	
	void Event3(){
		GameManager manager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		int building1 = Random.Range(0, 30);
		int building2 = Random.Range(0, 30);
		
		manager.buildingList [building1].stockPrice *= Random.Range(0.5f, 1f);
		manager.buildingList [building2].stockPrice *= Random.Range (0.5f, 1f);
	}
	
	void Tax(Player player){
		player.money *= 0.85f;
	}

	void FairyGodMother(){
		int choice = Random.Range(0, 2);

		GameManager manager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		switch (choice) {
		case 0:
			foreach(Building x in manager.buildingList){
				x.stockPrice *= Random.Range(0.5f, 1f);
			}
			break;
		case 1:
			foreach(Building x in manager.buildingList){
				x.stockPrice *= Random.Range(1f, 1.5f);
			}
			break;
		}
	}

	public void StockRandomChange(){
		stockPrice *= Random.Range(0.5f, 1.6f);
	}
}
