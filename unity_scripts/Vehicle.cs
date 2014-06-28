using UnityEngine;

public class Vehicle : MonoBehaviour {


	private bool initialized = false;

	GUIContent[] comboBoxList;
	private ComboBox comboBoxControl;// = new ComboBox();
	public GUIStyle listStyle = new GUIStyle();
	public GUIStyle buttonStyle = new GUIStyle();
	public bool activateComboBox = false;
	private int currentCarID = -1;

	private Rect guiLeftArea;
	public Camera guiLeftCam;



	public void init(){

		guiLeftArea = new Rect (guiLeftCam.pixelRect.xMin, guiLeftCam.pixelRect.yMax * 2.33f, guiLeftCam.pixelRect.width, guiLeftCam.pixelRect.height);

		// set size of ComboBoxList
		comboBoxList = new GUIContent[Controller.instance.carList.Count];

		// set entries of ComboBoxList
		for(int i = 0; i < Controller.instance.carList.Count; i++){
			comboBoxList[i] = new GUIContent(Controller.instance.carList[i].id.ToString());
		}

		// style ComboBox
		listStyle.normal.textColor = Color.black; 
		listStyle.onHover.background = listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left = 4;
		listStyle.padding.right = 4;
		listStyle.padding.top = 4;
		listStyle.padding.bottom = 4;



		foreach (Car car in Controller.instance.carList) {
			Rect rect = new Rect(guiLeftArea.xMax *.05f, guiLeftArea.y * .12f, guiLeftArea.width / 2 , guiLeftArea.height / 35);
			if (car.id.Equals (Controller.instance.currentCar.id)) {
				comboBoxControl = new ComboBox (rect, comboBoxList [Controller.instance.carList.IndexOf (car)], comboBoxList, buttonStyle, "box", listStyle);
			}
		}

		this.initialized = true;
	}


	// ------------------------------------------------------------------------

	public void instantiateCar(int carShowID){

		if (currentCarID != carShowID) {
			GUIManager.SetShowMainMenuBool(true);

			// set the actual carShowId to the new currentCarId
			currentCarID = carShowID;

			// destroy the old instance of car and load the new one
			Controller.instance.switchCar (Controller.instance.carList[carShowID].id);

			//set index for comboListBox
			this.comboBoxControl.SelectedItemIndex = carShowID;
		}
	}

	// ------------------------------------------------------------------------

	private void OnGUI () 
	{
		if(!initialized){
			return;
		}

		//comboBoxControl.Show ();
		if(this.activateComboBox){

			this.instantiateCar (this.comboBoxControl.Show());

		}

	}

	// ------------------------------------------------------------------------

	//returns the comboListBox index of carId as integer
	public int getComboListIndexForCarId(string carId){

		int index = -1;
		foreach(Car car in Controller.instance.carList){
			if(car.id.Equals(carId)){
				index = Controller.instance.carList.IndexOf(car);
			}
		}
		return index;
	}

	// ------------------------------------------------------------------------

	//returns the carId string for certain comboListBox index
	public string getCarIdStringForComboListIndex(int index){

		return Controller.instance.carList[index].id;
	}
}