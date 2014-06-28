using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Car : MonoBehaviour {

	public string id;
	public List<CarPart> carParts = new List<CarPart>(); 
	public float price;
	public string artNr;
	//private Color mainColor;
	
	// ------------------------------------------------------------------------
	
	public void init(){
	
		foreach(CarPart carpart in this.GetComponentsInChildren<CarPart>()){
			carpart.init();
			carParts.Add(carpart);
		}

		//Debug.Log("CarPartsCount: " + carParts.Count.ToString());
	}

	// ------------------------------------------------------------------------

	public List<CarPart> getCarParts(){

		return this.carParts;
	}
}
