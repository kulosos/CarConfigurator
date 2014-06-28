using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRootAttribute("carConfig")]
public class SceneData {

	[XmlElement("setupId")]
	public string sceneId = "configuration1";

	[XmlElement("car")]
	public CarData carData = new CarData();
}

// #############################################################################

[XmlType("car")]
public class CarData {

	[XmlElement("carId")]
	public string carId;// = "car1";

	[XmlElement("price")]
	public float price;

	[XmlElement("artNr")]
	public string artNr;

	[XmlElement("carPart")]
	public List<CarPartData> carPartData = new List<CarPartData>();
}

// #############################################################################

[XmlType("carPart")]
public class CarPartData {

	[XmlElement("partId")]
	public string partId = "carPartId";

	[XmlElement("partType")]
	public CarPartType partType = CarPartType.Body;

	[XmlElement("partColor")]
	public string partColor = "0/1/0/1";

    [XmlElement("scale")]
	public Vector3 scale;

	[XmlElement("price")]
	public float price;

	[XmlElement("artNr")]
	public string artNr;

}
	
	