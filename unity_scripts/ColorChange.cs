using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorChange : MonoBehaviour {

	public 	Texture2D 	colorTexture;
	public Texture sliderGreen, sliderRed, sliderBlue;
	private	List<Material> carPartColor;
	//private CarData carData;
	public Car currentCar;
	private bool changeAlpha;
	

	private Rect colorPanelRect ;//= new Rect (10, 300, 100, 100);
	private	Rect previewRect = new Rect (150, 300, 100, 100);

	private Color currentColorPickerColor;

	private Color originBodyColor, originPaneColor, originRimColor;

	private float textureUpos;
	private float textureVpos;

	private float colorSliderR, colorSliderG, colorSliderB;

	public bool showColorPicker, isCarNew, allowToReset = false;

	public GUIStyle redBackground, greenBackground, blueBackground, sliderStyle;

	// Use this for initialization
	public void init (Car car) {
		carPartColor = new List<Material> ();
		changeAlpha = false;

		// gets the currentCar object
		this.currentCar = car;
		isCarNew = true;
		showColorPicker = false;
		//Debug.Log ("PARTS: " + this.currentCar.GetComponentsInChildren<CarPart>().Length);
	}

	public void setColorPanelRectSize(Rect rect){
		colorPanelRect = rect;
	}

	void OnGUI(){

		if (colorSliderR != currentColorPickerColor.r) {
			colorSliderR = currentColorPickerColor.r;
		}
		if (isCarNew) {
			allowToReset = false;

			foreach (CarPart cp in this.currentCar.GetComponentsInChildren<CarPart>()) {

				// check CarPart for Enum-type
				if (cp.carPartType.Equals (CarPartType.Body)) {

					// get parentObject this CarPart belongs to
					GameObject go = cp.transform.gameObject;


					originBodyColor = go.transform.renderer.material.color;


				}

				if (cp.carPartType.Equals (CarPartType.Wheel)) {

					// get parentObject this CarPart belongs to
					GameObject go = cp.transform.gameObject;


					originRimColor = go.transform.renderer.material.color;

				}

				if (cp.carPartType.Equals (CarPartType.Window)) {
					GameObject go = cp.transform.gameObject;


					originPaneColor = go.transform.renderer.material.color;
				}

			}
		}


		if (showColorPicker) {
			isCarNew = false;
			allowToReset = true;
			GUI.backgroundColor = Color.white;
			GUI.DrawTexture (colorPanelRect, colorTexture); 
			GUI.Box (new Rect (colorPanelRect.x * 1.4f, colorPanelRect.y * 1.02f, colorPanelRect.width * 1.1f, colorPanelRect.height / 7), "", redBackground);
			GUI.Box (new Rect (colorPanelRect.x * 1.4f, colorPanelRect.y * 1.06f, colorPanelRect.width * 1.1f, colorPanelRect.height / 7), "", greenBackground);
			GUI.Box (new Rect (colorPanelRect.x * 1.4f, colorPanelRect.y * 1.1f, colorPanelRect.width * 1.1f, colorPanelRect.height / 7), "", blueBackground);

			currentColorPickerColor.r = GUI.HorizontalSlider (new Rect (colorPanelRect.x * 1.4f, colorPanelRect.y * 1.02f, colorPanelRect.width * 1.1f, colorPanelRect.height / 4), currentColorPickerColor.r, 0, 1);
			currentColorPickerColor.g = GUI.HorizontalSlider (new Rect (colorPanelRect.x * 1.4f, colorPanelRect.y * 1.06f, colorPanelRect.width * 1.1f, colorPanelRect.height / 4), currentColorPickerColor.g, 0, 1);
			currentColorPickerColor.b = GUI.HorizontalSlider (new Rect (colorPanelRect.x * 1.4f, colorPanelRect.y * 1.1f, colorPanelRect.width * 1.1f, colorPanelRect.height / 4), currentColorPickerColor.b, 0, 1);

			if (Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseDrag) {
				Vector2 mousePos = Event.current.mousePosition;

				if (mousePos.x < colorPanelRect.x ||
				   mousePos.x > colorPanelRect.xMax ||
				   mousePos.y < colorPanelRect.y ||
				   mousePos.y > colorPanelRect.yMax) {
					return;
				}

				textureUpos = (mousePos.x - colorPanelRect.x) / colorPanelRect.width;
				textureVpos = 1.0f - (mousePos.y - colorPanelRect.y) / colorPanelRect.height;

				currentColorPickerColor = colorTexture.GetPixelBilinear (textureUpos, textureVpos);
			}

			foreach (Material carColor in carPartColor) {
				if (changeAlpha) {
					currentColorPickerColor.a = 0.5f;
				}
				carColor.color = currentColorPickerColor;
			}
		} else {
			allowToReset = false;
		}
	}   

	public Color getNewCarColor(){
		return currentColorPickerColor;
	}

	public void resetColorSettings(){
		foreach (CarPart cp in this.currentCar.GetComponentsInChildren<CarPart>()) {

			// check CarPart for Enum-type
			if(cp.carPartType.Equals(CarPartType.Body)){

				// get parentObject this CarPart belongs to
				GameObject go = cp.transform.gameObject;


				go.transform.renderer.material.color = originBodyColor;
				currentColorPickerColor = originBodyColor;

			}

			if(cp.carPartType.Equals(CarPartType.Wheel)){

				// get parentObject this CarPart belongs to
				GameObject go = cp.transform.gameObject;

				go.transform.renderer.material.color = originRimColor;
				//currentColorPickerColor = originRimColor;
			}

			if (cp.carPartType.Equals (CarPartType.Window)) {
				GameObject go = cp.transform.gameObject;


				go.transform.renderer.material.color = originPaneColor;
				//currentColorPickerColor = originPaneColor;
			}


		}
	}

	public bool allowToResetConfig(){
		return allowToReset;
	}
	
	public void setColorpicker( bool b, int id){
		//		Debug.Log (id);

		showColorPicker = b;
		switch (id) {
		case 0:
			/* Karosserie*/
			/*carPartColor.Clear ();
			carPartColor.Add (car.transform.FindChild ("Body").renderer.material); 
			currentColorPickerColor = car.transform.FindChild ("Body").renderer.material.color;
			changeAlpha = false;
			*/
			//Debug.Log ("PARTS: " + this.currentCar.GetComponentsInChildren<CarPart>().Length);

			carPartColor.Clear ();

			// iterate all CarParts on currentCar object
			foreach (CarPart cp in this.currentCar.GetComponentsInChildren<CarPart>()) {

				// check CarPart for Enum-type
				if(cp.carPartType.Equals(CarPartType.Body)){

					// get parentObject this CarPart belongs to
					GameObject go = cp.transform.gameObject;

					carPartColor.Add(go.renderer.material);
					currentColorPickerColor = go.transform.renderer.material.color;

				}
			}
	
			break;
		case 1: 
			/* Felgen*/ 

			/*carPartColor.Clear ();
			foreach (Transform cpc in currentCar.transform.FindChild("WheelFL").transform) {
				carPartColor.Add (cpc.renderer.material);
				changeAlpha = false;
			}
			currentColorPickerColor = carPartColor [0].color;
			*/

			carPartColor.Clear ();

			// iterate all CarParts on currentCar object
			foreach (CarPart cp in this.currentCar.GetComponentsInChildren<CarPart>()) {
				
				// check CarPart for Enum-type
				if(cp.carPartType.Equals(CarPartType.Wheel)){
					
					// get parentObject this CarPart belongs to
					GameObject go = cp.transform.gameObject;
					
					carPartColor.Add(go.renderer.material);
					currentColorPickerColor = go.transform.renderer.material.color;
				}
			}

			break;
		case 2: 
			/* Scheiben */
			/*carPartColor.Clear ();
			carPartColor.Add(currentCar.transform.FindChild ("Windows").renderer.material); 
			currentColorPickerColor = currentCar.transform.FindChild ("Windows").renderer.material.color;
			*/

			carPartColor.Clear ();

			// iterate all CarParts on currentCar object
			foreach (CarPart cp in this.currentCar.GetComponentsInChildren<CarPart>()) {
				
				// check CarPart for Enum-type
				if(cp.carPartType.Equals(CarPartType.Window)){
					
					// get parentObject this CarPart belongs to
					GameObject go = cp.transform.gameObject;
					
					carPartColor.Add(go.renderer.material);
					currentColorPickerColor = go.transform.renderer.material.color;
				}
			}

			changeAlpha = true;
			break;

		default:
			carPartColor.Clear ();
			changeAlpha = false;
			break;
		}
	}

	public void RGBSlider(){
	
	}
	// Update is called once per frame
	void Update () {


		//		colorR = currentColorPickerColor.r;
//		colorG = currentColor.g;
//		colorB = currentColor.b;
	}
}
