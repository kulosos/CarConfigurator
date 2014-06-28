using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
//using UnityEditor;

public class Controller : MonoBehaviour {

	public static Controller instance;
	public CameraController camController;
	public Car currentCar;
	public ColorChange colorChange;
	public List<Car> carList = new List<Car>();
	public Vehicle selectVehicle;
	public Wheels wheels;
	public Transform carParent;

	// -------------------------------------------------------------------------

	public void init(){
	 
		this.setCurrentCar(carList[0]);
		//this.currentCar.init();
		this.camController.init();
		this.colorChange.init(this.currentCar);
		this.selectVehicle.init();
		this.wheels.init();
	}
	
	// ------------------------------------------------------------------------

	public void Start(){
		this.init();
		this.saveCurrentConfig();

		//Debug.Log ("ScreenWidth: " + Screen.width/2 + ", ScreenHeight: " + Screen.height/2);

		StartCoroutine(getImagesAsBase64String(false));
	}

	// ------------------------------------------------------------------------

	public void Awake(){

		Controller.instance = this;
	}

	// ------------------------------------------------------------------------

	public void Update(){

	}
		
	// -------------------------------------------------------------------------

	public void switchCar(string carId) {

		GameObject.Destroy(this.currentCar.gameObject);
		this.currentCar = null;

		foreach (Car car in this.carList) {

			if (car.id.Equals(carId)) {

				// calls first the setCurrentCar Method to give the returnValue to ColorChange - protects the NullRef in ColorChange
				this.colorChange.init(this.setCurrentCar(car));
				this.wheels.init();
			}
		}
	}

	// ------------------------------------------------------------------------

	public Car setCurrentCar(Car car){

		this.currentCar = (Car)GameObject.Instantiate(car);

		// set currentCar to child of parent
		this.currentCar.transform.parent = this.carParent;

		return this.currentCar;
	}

	// ------------------------------------------------------------------------

	public void saveCurrentConfig(){
	
		try{
			// root obj
			SceneData sceneData = new SceneData();

			// create and fill carDataObj with carParts
			CarData carData = new CarData();
			carData.carId = this.currentCar.id;
			carData.price = this.currentCar.price;
			carData.artNr = this.currentCar.artNr;

			foreach (CarPart cp in this.currentCar.GetComponentsInChildren<CarPart>()) {

				// Locator doesn't have an MeshRenderer, iterate over all CarParts expept Locators
				if (cp.carPartType != CarPartType.WheelLocator) {

					CarPartData cpd = new CarPartData ();
					cpd.partId = cp.id;
					cpd.partType = cp.carPartType;
					cpd.partColor = cp.getColor(); // return value needs to be an color not a string
					cpd.scale = cp.transform.localScale;
					cpd.price = cp.price;
					cpd.artNr = cp.artNr;

					carData.carPartData.Add (cpd);
				}
			}

			sceneData.carData = carData;

			XmlSerializer serializer = new XmlSerializer(typeof(SceneData));

			// Serialize to File
			//TextWriter tr = new StreamWriter (Application.dataPath + Path.DirectorySeparatorChar + "config.xml");
			//serializer.Serialize(tr, sceneData);
			//tr.Close();
			//Debug.Log ("saved to xml");

			// Serzialize to String
			StringWriter sw = new StringWriter();
			serializer.Serialize(sw, sceneData);

			Application.ExternalCall( "applyConfigToBrowser", sw.ToString() );
			Debug.Log(sw.ToString());
		}
		catch(Exception e){
			Debug.Log("Saving to xml failed.\n" + e);
		}
	}
	
	// ------------------------------------------------------------------------
	
	public void loadCurrentConfig(string xmlString){
	
		try{
			//StreamReader configStream = new StreamReader(path); //(Application.dataPath + Path.DirectorySeparatorChar + "config.xml");

			StringReader configStream = new StringReader(xmlString.ToString());

			XmlSerializer serializer = new XmlSerializer(typeof(SceneData));
			SceneData sd = (SceneData)serializer.Deserialize(configStream);

			// --- Load Car from CarData ---
			if(!sd.carData.carId.Equals(this.currentCar.id)){

				// load the car that is saved in the xml file, with the integer index of the car in comboListBox
				this.selectVehicle.instantiateCar(this.selectVehicle.getComboListIndexForCarId(sd.carData.carId));
			}


			// --- Load Wheels (CarParts) from CarPartData ---
			for(int i = 0; i < sd.carData.carPartData.Count; i++){

				for(int j = 0; j < Controller.instance.wheels.selectableWheels.Count; j++){

					// instantiate wheel if partId matches for CarPartData and SelectableWheels 
					if(sd.carData.carPartData[i].partId.Equals(Controller.instance.wheels.selectableWheels[j].id)){
						Controller.instance.wheels.instantiateCarPart( this.wheels.getComboListIndexForCarPartId(sd.carData.carPartData[i].partId) );
					}
				}
			}

			// --- Load and Set Wheel Scale from CarPartData ---
			foreach(CarPartData cp in sd.carData.carPartData){

				foreach(CarPart wheel in Controller.instance.wheels.currentInstantiatedWheels){

					// set scale
					if(cp.partId.Equals(wheel.id)){
						wheel.transform.localScale = new Vector3(cp.scale.x, cp.scale.y, cp.scale.z);
					}
				}
			}

			// --- Load and Set Colors for all CarParts from CarPartData ---
			foreach(CarPartData cpd in sd.carData.carPartData){

				//Debug.Log ("CarPartData Count: " + sd.carData.carPartData.Count);
				foreach (CarPart currentcp in this.currentCar.GetComponentsInChildren<CarPart>()) {
				
					if(cpd.partId.Equals(currentcp.id)){
						currentcp.setColor(cpd.partColor);
					}
				}
			}

			configStream.Close();
			Debug.Log ("XML loaded");
		}
		catch(Exception e){
			Debug.Log("Loading from xml failed.\n" + e);
		}
	}

	// -----------------------------------------------------------------------------

	public void getScreenshot(){

		StartCoroutine(getImagesAsBase64String(true));
		//getImagesAsBase64String(true);
	}

	// -----------------------------------------------------------------------------

	// ### PNG Screenshot as Base64 String ### 
	IEnumerator getPNGImageAsBase64String() {

		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		byte[] bytes = tex.EncodeToPNG();
		Destroy(tex);

		//Convert byte[] to Base64 String
		string base64String = Convert.ToBase64String(bytes);

		//Debug.Log ("Encoded text: " + base64String);
		Application.ExternalCall( "savePdf", base64String );
	}

	// -----------------------------------------------------------------------------

	// ### JPEG Screenshot as Base64 String ### 
	private IEnumerator getImagesAsBase64String(bool takeShot)
	{
		yield return new WaitForEndOfFrame ();

		int width = Screen.width-140;//(int)Math.Round((960-192)*0.7f);//Screen.width;
		int height = Screen.height-180;//(int)Math.Round((600 - 180)*0.7f);//Screen.height;

		Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);
		//tex.ReadPixels (new Rect (192, 180, width , height), 0, 0);
		tex.ReadPixels (new Rect (140, 180, width, height ), 0, 0);
		tex.Apply ();

		JPGEncoder encoder = new JPGEncoder (tex.GetPixels(), width, height, 100.0f); // Constructor JPGEncoder(Color[] pixels, int width, int height, float quality)
		encoder.doEncoding();

		//encoder is threaded; wait for it to finish
		/*while(!encoder.isDone){
			yield return null;
		}*/

		yield return new WaitForSeconds (0.5f);
		//Debug.Log ("Size: " + encoder.GetBytes().Length + " bytes");

		//Convert byte[] to Base64 String
		string base64String = Convert.ToBase64String (encoder.GetBytes ());

		//Debug.Log ("Encoded text: " + base64String);

		if (takeShot) {
			Application.ExternalCall ("savePdf", base64String);
		}

		Destroy (tex);
	}



}
