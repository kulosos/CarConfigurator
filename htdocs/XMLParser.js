/*
 * XMLParser.js 
 *
 * @brief: Parses the XML-String, which is return by Unity WebPlayer Application
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: March 2014
 *
 */
  
 var currentXMLData = "";
  
//-----------------------------------------------------------------------------

function xmlParse(xml){
        
		  xmlDoc = $.parseXML( xml ),
		  $xml = $( xmlDoc ),
		  
		$( "#anotherElement" ).append( $xml.text());
}

//-----------------------------------------------------------------------------
	
// load xml file	
function loadXMLDoc(filename)
{
	if (window.ActiveXObject)
	  {
		 xhttp = new ActiveXObject("Msxml2.XMLHTTP");
	  }
	else 
	  {
		 xhttp = new XMLHttpRequest();
	  }
	
	xhttp.open("GET", filename, false);
	
	try {
		xhttp.responseType = "msxml-document"
		} 
	catch(err) {	} // Helping IE11
	
	xhttp.send("");
	return xhttp.responseXML;
}

//-----------------------------------------------------------------------------	

//load xml from string
function LoadXMLString(xmlString)
{
    var xDoc;
    /*
    var bType = getBrowserType();

    switch("ie")
    {
      case "ie":
        // This actually calls into a function that returns a DOMDocument 
        // on the basis of the MSXML version installed.
        // Simplified here for illustration.
        xDoc = new ActiveXObject("MSXML2.DOMDocument")
        xDoc.async = false;
        xDoc.loadXML(xmlString);
        break;
      default:
        var dp = new DOMParser();
        xDoc = dp.parseFromString(xmlString, "text/xml");
        break;
    }
	
	*/
	if (window.ActiveXObject)
	  {
		 xDoc = new ActiveXObject("MSXML2.DOMDocument")
        xDoc.async = false;
        xDoc.loadXML(xmlString);
	  }
	// code for Chrome, Firefox, Opera, etc.
	else if (document.implementation && document.implementation.createDocument)
	  {
		 var dp = new DOMParser();
        xDoc = dp.parseFromString(xmlString, "text/xml");
	  }
    return xDoc;
}

//-----------------------------------------------------------------------------
		
function displayResult(xmlstring)
{
	
	//alert("display result");
	xml = LoadXMLString(xmlstring);
	xsl = loadXMLDoc("carconfig.xsl");
	// code for IE
	if (window.ActiveXObject || xhttp.responseType == "msxml-document")
	  {
		  ex = xml.transformNode(xsl);
		  document.getElementById("configdata").innerHTML = ex;
	  }
	// code for Chrome, Firefox, Opera, etc.
	else if (document.implementation && document.implementation.createDocument)
	  {
		  xsltProcessor = new XSLTProcessor();
		  xsltProcessor.importStylesheet(xsl);
		  resultDocument = xsltProcessor.transformToFragment(xml, document);
		  if(resultDocument != null){
		  	document.getElementById("configdata").appendChild(resultDocument);
		  }
	  }
}
	
//-----------------------------------------------------------------------------
	
function applyConfigToBrowser(xml){
	
	$("#configdata").children().remove();
	displayResult(xml);
	convertColor();
	currentXMLData = xml;
	$("#debugOutputArea").val(xml);
}

//-----------------------------------------------------------------------------

function convertColor(){
	
	$( ".colorToConvert" ).each(function( index ) {
		
		var rgbaStr = $( this ).text();
		var r, g, b;
		var rgbcolors = rgbaStr.split('/', 3);
		
		r = Math.round(rgbcolors[0] * 255);
		g = Math.round(rgbcolors[1] * 255);
		b = Math.round(rgbcolors[2] * 255);
	
		var colorstring = "rgb(" + r + "," + g + "," + b + ")"; 
		
		var color = new RGBColor(colorstring);
		//console.log("converted to RGB " + color.toRGB());
		//console.log("converted to HEX " + color.toHex());

		// change color of div container and text
		$(this).css('background-color', color.toHex());
		$(this).text("");
	})
}

// -----------------------------------------------------------------------------

// auto refresh
window.setInterval(function(){
	
	if ($('#autoRefresh:checked').val() !== undefined)
	{
		//Checked
		u.getUnity().SendMessage("Admin", "saveCurrentConfig", "");
	}
	else
	{
		//Not checked
	}
		
}, 250); //in milliseconds

// -----------------------------------------------------------------------------

$(document).ready(function() {

	$("#refreshDataBtn").on("click", function(event) {
		u.getUnity().SendMessage("Admin", "saveCurrentConfig", "");	
	});
});