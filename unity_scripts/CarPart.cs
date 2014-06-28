using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Globalization;
using System;
using System.IO;
using System.Collections.Generic;


public enum CarPartType { Body, Wheel, Interior, Window, Part, WheelLocator }

public class CarPart : MonoBehaviour {
	
	public CarPartType carPartType = CarPartType.Body;
	
	public string id;
	public float price;
	public string artNr;
	public MeshRenderer mr;

	
	// ------------------------------------------------------------------------
	
	public void init(){
		
		this.mr = this.GetComponent<MeshRenderer>();
	}
	
	// ------------------------------------------------------------------------	
	
	public void setColor(string col){

		// get color as string and return as color
		mr.material.SetColor("_Color", CarPart.convertToColor(col) );
	}
	

	// ------------------------------------------------------------------------
	
	public string getColor(){

		// get and return color as string
		return CarPart.convertFromColor (mr.material.GetColor("_Color"));
	}

	// ------------------------------------------------------------------------

	// Get Color from Color String
	public static Color convertToColor(string color) {

		try {
			Color result = new Color();
			string[] splittedVector = color.Split(new char[] { '/' });
			if (splittedVector.Length == 3) {
				result.r = Convert.ToSingle(splittedVector[0]);
				result.g = Convert.ToSingle(splittedVector[1]);
				result.b = Convert.ToSingle(splittedVector[2]);
			}
			if (splittedVector.Length == 4) {
				result.r = Convert.ToSingle(splittedVector[0]);
				result.g = Convert.ToSingle(splittedVector[1]);
				result.b = Convert.ToSingle(splittedVector[2]);
				result.a = Convert.ToSingle(splittedVector[3]);
			}
			return result;
		}
		catch (Exception) {
			return Color.white;
		}
	}

	//-------------------------------------------------------------------------

	// Get Color String from Color
	public static string convertFromColor(Color color/*, bool alpha*/) {

		bool alpha = true;

		if (alpha) {
			return color.r + "/" + color.g + "/" + color.b + "/" + color.a;
		}
		else {
			return color.r + "/" + color.g + "/" + color.b;
		}
	}

	//-------------------------------------------------------------------------

	// Get Enum from Enum String
	public static System.Object convertToEnum(string enumName, Type t) {

		try {
			return Enum.Parse(t, enumName);
		}
		catch (Exception) {
			return null;
		}
	}

	//-------------------------------------------------------------------------

	// Get Enum String from Enum
	public static string convertFromEnum(System.Object enumValue) {

		return Convert.ToString(enumValue);
	}




}
