using UnityEngine;
using System.Collections;
//using UnityEditor;

public class GUIManager : MonoBehaviour {
	
	public static GUIManager instance;
	
    public float hSliderValue1 = 0.0F;
    private string labelSave = "Press F5 to save XML";
    private string labelLoad = "Press F6 to load XML";

	private ColorChange colorChange;
	private Rect guiArea, subMenuRect, guiLeftArea, subLeftMenuRect;
	public static Rect vehicleRect;

	public Camera guiCam, guiLeftCam;
	private static bool allowVehicleRect, showMainMenu, subMenuActivated, showExteriorMenu, showInteriorMenu, showKarosserieMenu = false;

	private int  eSubMenuInt =0;
	private float buttonWidth, buttonHeight = 0;

	private GuiXmlLoader guiXML;
	private static string guiLanguage;
	public Texture german, english, french, turkish;

	public float wheelSizeMax = 1.5f;

	GUIContent[] comboBoxListForLanguage;
	private ComboBox comboBoxLanguageControl;
	public GUIStyle listLanguageStyle = new GUIStyle();
	public GUIStyle style = new GUIStyle();
	public GUIStyle backgroundGUIstyle = new GUIStyle();
	public GUIStyle buttonStyle = new GUIStyle();
	public GUISkin sliderSkin;
	private float wheelThickness, wheelSizeY;
	private Vector3 wheelSize;

	// ------------------------------------------------------------------------ //
	
	public void Awake(){
		
		GUIManager.instance = this;
		colorChange = GameObject.Find ("ColorChanger").GetComponent<ColorChange> ();
		showMainMenu = true;

		guiXML = GameObject.Find("GUI_Camera").GetComponent<GuiXmlLoader>();
		guiXML.GetLanguage ("German");


		style.normal.textColor = Color.black;

		#region ComboBox für Sprache Vorbereitung

		/*  An dieser Stelle wird schon die ComboBox für die Sprachauswahl vorbereitet. 
			Hier wird auch gleich der Rahme bzw. die Area der GUI festgehalten, die später
			wenn die GUI gezeichnet wird, verwendet wird.  */

		// Language GUI:

		// Rahmen
		guiLeftArea = new Rect (guiLeftCam.pixelRect.xMin, guiLeftCam.pixelRect.yMax * 2.33f, guiLeftCam.pixelRect.width, guiLeftCam.pixelRect.height);

		subLeftMenuRect = new Rect (guiLeftArea.xMax *.05f, guiLeftArea.y + 25, guiLeftArea.width - 30, guiLeftArea.height - 30);

		comboBoxListForLanguage = new GUIContent[GuiXmlLoader.GetLanguageTitleForGUI().Count];

		// set entries of ComboBoxList
		for(int i = 0; i < GuiXmlLoader.GetLanguageTitleForGUI().Count; i++){
			comboBoxListForLanguage[i] = new GUIContent(GuiXmlLoader.GetLanguageTitleForGUI()[i].ToString());
		}

		listLanguageStyle.normal.textColor = Color.black; 
		listLanguageStyle.onHover.background = listLanguageStyle.hover.background = new Texture2D(2, 2);
		listLanguageStyle.padding.left = 4;
		listLanguageStyle.padding.right = 4;
		listLanguageStyle.padding.top = 4;
		listLanguageStyle.padding.bottom = 4;



		// set currentLanguage in comboBox
		foreach(string lang in GuiXmlLoader.GetLanguageTitleForGUI()){
			/*for (int i = 0; i < GuiXmlLoader.GetLanguageTitleForGUI ().Count; i++) {
				Debug.Log (GuiXmlLoader.GetLanguageTitleForGUI ()[i]);


			}
			*/
			comboBoxLanguageControl = new ComboBox(new Rect (guiLeftArea.xMax*.05f, guiLeftArea.y * .01f, guiLeftArea.width/3f, guiLeftArea.height/30), comboBoxListForLanguage[0]  ,comboBoxListForLanguage , buttonStyle, "box", listLanguageStyle);

		}
		#endregion


	}
	
	//-------------------------------------------------------------------------
	
    private void OnGUI()
    {
		#region MenuLeft
		/** 

		-------------------------------  M E N U F E L D   L I N K S -----------------------------------------

		**/

		// Apply Settings to Browser (Unity webplayers sends data to browser)
		/*if (GUI.Button (new Rect (10, 50, 175, 20), "Apply Settings to Browser")){
			//Controller.instance.saveCurrentConfig();
			Controller.instance.getScreenshot();
		}*/ 

		//Debug.Log("GUILeftAreaWidth: " + guiLeftArea.width);

		// LANGUAGE COMBOBOX
		/* FLAGGENSYMBOLE */
		/*
		GUI.Label (new Rect (guiLeftArea.xMax *0.05f, guiLeftArea.y * .04f, guiLeftArea.width , guiLeftArea.height / 20), german);
		GUI.Label (new Rect (guiLeftArea.xMax *0.3f, guiLeftArea.y * .04f, guiLeftArea.width , guiLeftArea.height / 20), english);
		GUI.Label (new Rect (guiLeftArea.xMax *.55f, guiLeftArea.y * .04f, guiLeftArea.width , guiLeftArea.height / 20), french);
		GUI.Label (new Rect (guiLeftArea.xMax *.8f, guiLeftArea.y * .04f, guiLeftArea.width , guiLeftArea.height / 20), turkish);
		*/

		this.comboBoxLanguageControl.SelectedItemIndex = comboBoxLanguageControl.Show ();

		switch (this.comboBoxLanguageControl.SelectedItemIndex) {
		case 0:
			guiLanguage = "German";
			guiXML.ClearList ();
			guiXML.GetLanguage (guiLanguage);
			break;
		case 1: 
			guiLanguage = "English";
			guiXML.ClearList ();
			guiXML.GetLanguage (guiLanguage);
			break;

		case 2: 
			guiLanguage = "French";
			guiXML.ClearList ();
			guiXML.GetLanguage (guiLanguage);
			break;

		default:
			break;
		}

		// Label für Autoauswahl
		GUI.Label (new Rect (guiLeftArea.xMax *.05f, guiLeftArea.y * .1f, guiLeftArea.width , guiLeftArea.height / 15), GuiXmlLoader.GetGUIcarTypeText(), style);
		// Label für Reifenauswahl
		GUI.Label (new Rect (guiLeftArea.xMax *.05f, guiLeftArea.y * .2f, guiLeftArea.width , guiLeftArea.height / 15), GuiXmlLoader.GetGUIwheelTypeText(), style);

		//GUI.Button(new Rect (guiLeftArea.xMax *.05f, guiLeftArea.y * .22f, guiLeftArea.width /2 , guiLeftArea.height / 15), " Wheels") ;
		vehicleRect =  new Rect(guiLeftArea.xMax *.05f, guiLeftArea.y * .12f, guiLeftArea.width / 2 , guiLeftArea.height / 15);
		allowVehicleRect = true;

		/* Aktuelle Reifengröße /-breite holen */
		float currentWheelThickness = Controller.instance.wheels.getWheelWidth();
		float currentWheelSizeY =   Controller.instance.wheels.getWheelSizeY();

		// Labels für die Reifengröße / -breite 
		GUI.Label(new Rect (guiLeftArea.xMax *.6f, guiLeftArea.y * .38f, guiLeftArea.width , guiLeftArea.height / 15), GuiXmlLoader.GetGUIwheelSizeText(), style);
		GUI.Label(new Rect (guiLeftArea.xMax *.6f, guiLeftArea.y * .4f, guiLeftArea.width , guiLeftArea.height / 15), GuiXmlLoader.GetGUIwheelWidthText(), style);
		GUI.skin = sliderSkin;
		// Slider für Reifengröße /-breite
		wheelThickness = GUI.HorizontalSlider (new Rect (guiLeftArea.xMax *.05f, guiLeftArea.y * .4f, guiLeftArea.width/2 , guiLeftArea.height / 15),currentWheelThickness, .5f, 1.5f);
		Controller.instance.wheels.setWheelThickness (wheelThickness);

		wheelSizeY = GUI.HorizontalSlider (new Rect (guiLeftArea.xMax *.05f, guiLeftArea.y * .38f, guiLeftArea.width/2 , guiLeftArea.height / 15),currentWheelSizeY, .5f, wheelSizeMax);
		Controller.instance.wheels.setWheelSize (wheelSizeY);

		GUI.skin = null;

		#endregion 
		#region MenuBottom
		/** 

		-------------------------------  M E N U F E L D   U N T E N  -----------------------------------------

		**/

		/* Vorbereitung für den unteren GUI-Abschnitt */

		guiArea = new Rect (guiCam.pixelRect.xMin, guiCam.pixelRect.yMax * 2.33f, guiCam.pixelRect.width, guiCam.pixelRect.height);

		subMenuRect = new Rect (guiArea.x + 15, guiArea.y + 25, guiArea.width - 30, guiArea.height - 30);

		//Debug.Log("GUIAreaHeight: " + guiArea.height);

		/* Einstellung der Farbfeldgröße */

		colorChange.setColorPanelRectSize (new Rect (subMenuRect.x * 2.5f, subMenuRect.y * 1.08f, subMenuRect.width / 5, subMenuRect.height /2));

		/*  Buttons für das untere Menüfeld
			Angefangen mit den Menupunkten Exterior und Interior
			Gefolgt von den Menüpunkten der jeweiligen Kategorie
		 */

		// Hauptmenu
		/*
		if (showMainMenu) {
			colorChange.setColorpicker(false, 3);
			if (GUI.Button (new Rect (subMenuRect.x *1.5f , guiArea.y + 50,100,30 ), "Exterior", buttonStyle)) {
				showExteriorMenu = true;
				showMainMenu = false;
				subMenuActivated = true;
			}
			GUI.Button (new Rect (guiArea.x * 3f, guiArea.y + 50,100,30 ), "Interior", buttonStyle);


		}
		*/
		showExteriorMenu = true;
		showMainMenu = false;
		subMenuActivated = true;
		// ExteriorMenu
		if (showExteriorMenu) {
			GUI.Box (subMenuRect, "",backgroundGUIstyle );
			/*
			string[] eSubMenuString = new string[]{GuiXmlLoader.GetGUIBodyText(),GuiXmlLoader.GetGUIRimText(),GuiXmlLoader.GetGUIPaneText(), GuiXmlLoader.GetGUIBackText()};// "Karosserie", "Felgen", "Scheiben", "Back" };
			buttonWidth = subMenuRect.width / 4;
			buttonHeight = subMenuRect.height / 2;
			eSubMenuInt = GUI.SelectionGrid (new Rect (subMenuRect.x * 1f, subMenuRect.y * 1.07f, Mathf.Clamp(buttonWidth, 40, 135), Mathf.Clamp(buttonHeight, 80, 120)), eSubMenuInt, eSubMenuString, 1, buttonStyle);

			switch (eSubMenuInt) {

			case 0: 
			//Karosserie

				SubMenuGUI (eSubMenuInt);

				break;
			case 1: 
			//Felgen

				SubMenuGUI (eSubMenuInt);
				colorChange.setColorpicker(true, eSubMenuInt);
				break;
			case 2: 
			//Scheiben

				//Debug.Log ("Scheiben" + eSubMenuInt);
				colorChange.setColorpicker(true, eSubMenuInt);
				SubMenuGUI (eSubMenuInt);
				break;
			case 3: 
			//Zurück
				eSubMenuInt = 0;
				showMainMenu = true;
				showExteriorMenu = false;
				subMenuActivated = false;
				colorChange.setColorpicker(false, 3);
				break;
			default:
				break;
			}

			if (eSubMenuInt == 0) {

				//showExteriorMenu = false;

			}

			*/
			// Karosserie
			if (GUI.Button (new Rect (subMenuRect.x * 1.5f, subMenuRect.y * 1.08f, subMenuRect.width /6 ,subMenuRect.height/8), GuiXmlLoader.GetGUIBodyText(), buttonStyle)) {
				colorChange.setColorpicker(true, 0);
			}
			// Felgen
			if (GUI.Button (new Rect (subMenuRect.x * 1.5f, subMenuRect.y * 1.13f, subMenuRect.width /6 ,subMenuRect.height/8), GuiXmlLoader.GetGUIRimText(), buttonStyle)) {
				colorChange.setColorpicker(true, 1);
			}
			// Scheiben
			if (GUI.Button (new Rect (subMenuRect.x * 1.5f, subMenuRect.y * 1.18f, subMenuRect.width /6 ,subMenuRect.height/8), GuiXmlLoader.GetGUIPaneText(), buttonStyle)) {
				colorChange.setColorpicker(true, 2);
			}
			// Reset
			if(colorChange.allowToResetConfig()){
				if (GUI.Button (new Rect (subMenuRect.x * 1.5f, subMenuRect.y * 1.23f, subMenuRect.width /6 ,subMenuRect.height/8), GuiXmlLoader.GetGUIResetText(), buttonStyle)) {
					colorChange.setColorpicker(false, 0);
					colorChange.resetColorSettings();
				}
			}
			
			/*

			CODE FÜR INTERIOR MENU KOMMT HIER HIN !!!!!
			
			*/

		}
		#endregion
		if (showKarosserieMenu) {
		
			//GUI.Box (subMenuRect, "Karosserie");
		}
    }

	/* Menüpunkte für das Untermenu der jeweiligen Kategorie */
	void SubMenuGUI(int carPartNr){
		// Einstellung der Größe für die Buttons
		buttonWidth = subMenuRect.width / 8.5f;
		buttonHeight = subMenuRect.height / 4;

		// Button für Farbauswahl
		colorChange.setColorpicker(true, carPartNr);
		/*
		if (GUI.Button (new Rect (subMenuRect.x * 1.7f, subMenuRect.y * 1.07f,  buttonWidth, buttonHeight), GuiXmlLoader.GetGUISubButtonText()[0], buttonStyle)) {

			
		}
		// Button für Materialauswahl
		if (GUI.Button (new Rect (subMenuRect.x * 1.7f, subMenuRect.y * 1.16f,  buttonWidth, Mathf.Clamp(buttonHeight, 20,70)), "Material", buttonStyle)) {

		}
		*/
	}

	public bool getSubMenuBool(){
	
		return subMenuActivated;
	}
		
	public static void SetShowMainMenuBool(bool b){
		showMainMenu = b;
		showExteriorMenu = false;
		subMenuActivated = false;
		//		

	}

	public static bool getVehicleRectBool(){
		return allowVehicleRect;
	}
	public static Rect getVehicleRect(){

			return vehicleRect;
	}
}
