using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class GuiXmlLoader: MonoBehaviour
{

	public  TextAsset GameAsset;

	static string gTbody = "";
	static string gTrims = "";
	static string gTpane = "";
	static string gTreset = "";

	static string gTload = "";
	static string gTsave = ""; 
	static string gTcarSelection = "";
	static string gTpdf = ""; 
	static string gTwheelSelection = "";
	static string gTwheelSize = "";
	static string gTwheelWidth = ""; 

	static List<string> subBtnText = new List<string> ();
	static List<string> LanguageText = new List<string> ();

	private string language = "German";

	List<Dictionary<string,string>> languages = new List<Dictionary<string,string>>();
	Dictionary<string,string> obj;

	void Start()
	{ //Timeline of the Level creator
		GetLanguage(language);
	}

	public static List<string> GetLanguageTitleForGUI(){
		return LanguageText;
	}

	public void GetLanguage(string language)
	{
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(GameAsset.text); // load the file.
		XmlNodeList languageList = xmlDoc.GetElementsByTagName("languages"); // array of the level nodes.

		//Debug.Log (languageList.Count + " " + languageList.Item (0).Attributes["id"].Value);

		foreach (XmlNode langInfo in languageList)
		{
			//Debug.Log (langInfo.ChildNodes.Item(0).Name);

			XmlNodeList langcontent = langInfo.ChildNodes;


			/* AUSLESEN DER DATEN AUS DER XML DATEI */

			foreach (XmlNode langItems in langcontent) // levels itens nodes.
			{
				LanguageText.Add (langItems.Attributes["id"].InnerText);

				if(langItems.Attributes ["id"].Value == language){
			
					XmlNodeList langText = langItems.ChildNodes;
				
					foreach (XmlNode text in langText) {

						XmlNodeList guiTextList = text.ChildNodes;

						foreach(XmlNode guiText in guiTextList){

							if (guiText.Name == "buttonLeft") {
								switch (guiText.Attributes ["id"].Value) {
								case "load":
									gTload = guiText.InnerText;
									//Debug.Log (gTbody);
									break;
								case "save":
									gTsave = guiText.InnerText;
									break;
								case "pdf":
									gTpdf = guiText.InnerText;
									break;
								case "carType":
									gTcarSelection = guiText.InnerText;
									break;
								case "wheelType":
									gTwheelSelection = guiText.InnerText;
									break;
								case "wheelSize":
									gTwheelSize = guiText.InnerText;
									break;
								case "wheelWidth":
									gTwheelWidth = guiText.InnerText;
									break;
								default:
									break;
								}

							}

							//	Debug.Log (guiText.InnerText);

							if (guiText.Name== "mainButton") {
								switch (text.Attributes ["id"].Value) {
								case "0":
										gTbody = guiText.InnerText;
									//Debug.Log (gTbody);
									break;
								case "1":
										gTrims = guiText.InnerText;
									break;
								case "2":
										gTpane = guiText.InnerText;
									break;
								case "3":
										gTreset = guiText.InnerText;
									break;
								default:
									break;
								}

							}

							if (guiText.Name == "subButton") {
								subBtnText.Add (guiText.InnerText);

							}
						
						}

					}


				}

			}
			//languages.Add(obj); // add whole obj dictionary to gui[].

		}
	}

	public static string GetGUIBodyText(){
		return gTbody;
	}

	public static string GetGUIRimText(){
		return gTrims;
	}

	public static string GetGUIPaneText(){
		return gTpane;
	}

	public static string GetGUIResetText(){
		return gTreset;
	}
	public static List<string> GetGUISubButtonText(){
		return subBtnText;

	}
	public void ClearList(){
		subBtnText.Clear ();
	}

	public static string GetGUILoadText(){
		return gTload;
	}

	public static string GetGUISaveText(){
		return gTsave;
	}

	public static string GetGUIpdfText(){
		return gTpdf;
	}

	public static string GetGUIcarTypeText(){
		return gTcarSelection;
	}

	public static string GetGUIwheelTypeText(){
		return gTwheelSelection;
	}

	public static string GetGUIwheelSizeText(){
		return gTwheelSize;
	}

	public static string GetGUIwheelWidthText(){
		return gTwheelWidth;
	}


}

