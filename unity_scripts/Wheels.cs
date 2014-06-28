using UnityEngine;
using System.Collections.Generic;


public enum wheelScaleType { larger, smaller, wider, thinner }

public class Wheels : MonoBehaviour {

	private bool initialized = false;
	GUIContent[] comboBoxList;
	private ComboBox comboBoxControl;// = new ComboBox();
	public GUIStyle listStyle = new GUIStyle();

	public List<CarPart> selectableWheels = new List<CarPart>();
	//public List<Car> matchingCars = new List<Car>();
    public bool activateComboBox = true;

	public CarPart currentWheelSelection;
	public List<CarPart> currentInstantiatedWheels = new List<CarPart>();
	private CarPart wheel;

	private GameObject wheelsNode;

	private Rect guiLeftArea;
	public Camera guiLeftCam;

	public GUIStyle buttonStyle = new GUIStyle();

	//public wheelScaleType scaleType = CarPartType.Body;


    // ----------------------------------------------------------------------

	public void init(){
		guiLeftArea = new Rect (guiLeftCam.pixelRect.xMin, guiLeftCam.pixelRect.yMax * 2.33f, guiLeftCam.pixelRect.width, guiLeftCam.pixelRect.height);
		// set size of ComboBoxList
		comboBoxList = new GUIContent[this.selectableWheels.Count];

		// set entries of ComboBoxList
		for(int i = 0; i < this.selectableWheels.Count; i++){
			this.comboBoxList[i] = new GUIContent(this.selectableWheels[i].id.ToString());
		}

		// style ComboBox
		this.listStyle.normal.textColor = Color.black; 
		this.listStyle.onHover.background = this.listStyle.hover.background = new Texture2D(2, 2);
		this.listStyle.padding.left = 4;
		this.listStyle.padding.right = 4;
		this.listStyle.padding.top = 4;
		this.listStyle.padding.bottom = 4;

		// set currentCar in comboBox
		//foreach(CarPart cp in this.SelectableWheels){
		Rect rect = new Rect (guiLeftArea.xMax *.05f, guiLeftArea.y * .22f, guiLeftArea.width /1.6f , guiLeftArea.height / 35);
		this.comboBoxControl = new ComboBox(rect, this.comboBoxList[0], this.comboBoxList, buttonStyle, "box", this.listStyle);

		this.initialized = true;

		// set first item to init currentWheel
		this.currentWheelSelection = this.selectableWheels[0];

		this.setWheels(this.currentWheelSelection);
	}

	// ------------------------------------------------------------------------

	private void OnGUI () 
	{
		if(!initialized){
			return;
		}

		//this.comboBoxControl.Show ();

		if(this.activateComboBox){
			// gives the current selected id to the instantiate method
			this.instantiateCarPart (this.comboBoxControl.Show ());
		}
	}

	// -----------------------------------------------------------------------

	public void instantiateCarPart(int id){

		if (this.selectableWheels[id] != this.currentWheelSelection) {
			GUIManager.SetShowMainMenuBool(true);

			// destroy the old instance of carPart (wheels) and load the new parts
			this.setWheels(this.selectableWheels[id]);

			// set the actual wheelId to the new currentWheelSelection
			this.currentWheelSelection = this.selectableWheels[id];

			//set index for comboListBox
			this.comboBoxControl.SelectedItemIndex = id;
		}
	}

	// -----------------------------------------------------------------------


	public void setWheels(CarPart currentCarPart){

		// destroy all already instantiated Wheel-GOs and clear List
		this.destroyWheels();

		//Get wheel locator of car
		List<CarPart> wheelLocators = new List<CarPart> ();
		foreach(CarPart cp in Controller.instance.currentCar.GetComponentsInChildren<CarPart>()){
			if(cp.carPartType.Equals(CarPartType.WheelLocator)){

				// add appropriate cp to list
				wheelLocators.Add(cp);
			}
		}

		// instantiate a wheel for any wheelLocator
		for(int i = 0; i < wheelLocators.Count; i++){ 

			// instantiate
			CarPart instantiatedCarPart = (CarPart)GameObject.Instantiate (currentCarPart);

			// add instantiated cp to list
			this.currentInstantiatedWheels.Add(instantiatedCarPart);

			// set position
			Transform wheelLocatorTransform = wheelLocators[i].transform;
			instantiatedCarPart.transform.position = wheelLocatorTransform.position;
			instantiatedCarPart.transform.localEulerAngles = wheelLocatorTransform.localEulerAngles;
			instantiatedCarPart.transform.parent = wheelLocatorTransform;
			//this.setCarPartParent();
		}

	}

	// -----------------------------------------------------------------------------

	public void destroyWheels(){

		foreach(CarPart go in this.currentInstantiatedWheels){
			GameObject.DestroyImmediate(go.gameObject);		
		}
		this.currentInstantiatedWheels.Clear();
	}

	// -----------------------------------------------------------------------------
	public float getWheelSizeY(){
		return currentInstantiatedWheels [0].transform.localScale.y;
	}

	public float getWheelWidth(){
		return currentInstantiatedWheels [0].transform.localScale.x;
	}

	public void setWheelSize(float newSize){

		foreach(CarPart cp in this.currentInstantiatedWheels){
			cp.transform.localScale = new Vector3 (cp.transform.localScale.x, newSize, newSize);
		}


	}

	public void setWheelThickness(float newSize){
		foreach(CarPart cp in this.currentInstantiatedWheels){
			cp.transform.localScale = new Vector3 (newSize,  cp.transform.localScale.y, cp.transform.localScale.z);
		}

	}


	public void setWheelScale(wheelScaleType scaleType){

		Debug.Log (currentInstantiatedWheels[0].transform.localScale);

		if(scaleType.Equals(wheelScaleType.larger)){
			foreach(CarPart cp in this.currentInstantiatedWheels){
				cp.transform.localScale = new Vector3 (cp.transform.localScale.x, cp.transform.localScale.y + 0.1f, cp.transform.localScale.z + 0.1f);
			}
		}
		else if(scaleType.Equals(wheelScaleType.smaller)){
			foreach(CarPart cp in this.currentInstantiatedWheels){
				cp.transform.localScale = new Vector3 (cp.transform.localScale.x, cp.transform.localScale.y - 0.1f, cp.transform.localScale.z - 0.1f);
			}
		}
		else if(scaleType.Equals(wheelScaleType.wider)){
			foreach(CarPart cp in this.currentInstantiatedWheels){
				cp.transform.localScale = new Vector3 (cp.transform.localScale.x + 0.1f, cp.transform.localScale.y, cp.transform.localScale.z);
			}
		}
		else if(scaleType.Equals(wheelScaleType.thinner)){
			foreach(CarPart cp in this.currentInstantiatedWheels){
				cp.transform.localScale = new Vector3 (cp.transform.localScale.x - 0.1f, cp.transform.localScale.y, cp.transform.localScale.z);
			}
		}
	}

	// -----------------------------------------------------------------------------

	public int getComboListIndexForCarPartId(string cpId){

		int index = -1;

		foreach(CarPart cp in this.selectableWheels){

			if(cp.id.Equals(cpId)){
				index = this.selectableWheels.IndexOf(cp);
			}

		}
		return index;
	}
}