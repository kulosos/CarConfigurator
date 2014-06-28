/*
 * FileStream.js
 *
 * @brief: File Save / Load
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: March 2014
 *
 */
  
//-----------------------------------------------------------------------------


function saveXMLAsFile()
{
	var textToWrite = document.getElementById("debugOutputArea").value;
	var textFileAsBlob = new Blob([textToWrite], {type:'text/plain'});
	var fileNameToSaveAs = "config.xml" // document.getElementById("inputFileNameToSaveAs").value;

	var downloadLink = document.createElement("a");
	downloadLink.download = fileNameToSaveAs;
	downloadLink.innerHTML = "Download File";
	if (window.webkitURL != null)
	{
		// Chrome allows the link to be clicked
		// without actually adding it to the DOM.
		downloadLink.href = window.webkitURL.createObjectURL(textFileAsBlob);
	}
	else
	{
		// Firefox requires the link to be added to the DOM
		// before it can be clicked.
		downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
		downloadLink.onclick = destroyClickedElement;
		downloadLink.style.display = "none";
		document.body.appendChild(downloadLink);
	}

	downloadLink.click();
	
	document.getElementById("debugOutputArea").value = "File saved. \n\n" + textToWrite;
}

// --------------------------------------------------------------------------------------

function destroyClickedElement(event)
{
	document.body.removeChild(event.target);
}

// --------------------------------------------------------------------------------------

function loadFileAsText()
{
	var fileToLoad = document.getElementById("fileToLoad").files[0];

	var fileReader = new FileReader();
	fileReader.onload = function(fileLoadedEvent) 
	{
		var textFromFileLoaded = fileLoadedEvent.target.result;
		document.getElementById("debugOutputArea").value = "File loaded. \n\n" + textFromFileLoaded;
		u.getUnity().SendMessage("Admin", "loadCurrentConfig", textFromFileLoaded);
	};
	fileReader.readAsText(fileToLoad, "UTF-8");
}

// --------------------------------------------------------------------------------------

$(document).ready(function() {
  
$("#saveToFileBtn").on("click", function(event) {
		saveXMLAsFile();
	});
	
});

