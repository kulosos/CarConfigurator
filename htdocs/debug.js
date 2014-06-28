/*
 * Debug Console Script
 *
 * @brief: Used for debugging outputs directly on the website (e.g. XML)
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: March 2014
 *
 */
  
//-----------------------------------------------------------------------------

var enable = false;


function debuglog(outputstring){
	$(".debug").append("------------------------------------------------------------------------------------------------------------------------<br><br>"+ getTimestamp() + " >> " + outputstring);
}

//-----------------------------------------------------------------------------

function getTimestamp(){
	var dateObj = new Date();
	var hour = dateObj.getHours();
	var minute = dateObj.getMinutes();
	var seconds = dateObj.getSeconds();
	var hourOut = ((hour < 10) ? "0" + hour : hour);
	var minOut = ((minute < 10) ? "0" + minute : minute);
	var secOut = ((seconds < 10) ? "0" + seconds : seconds);
	var timestamp = hourOut + ":" + minOut + ":" + secOut;
	return timestamp;
};

//-----------------------------------------------------------------------------

$(document).ready(function() {
  

	// clear debug console
	/*$("#clearDebug").on("click", function(event) {
		$(".debug").text("");
	});*/

	$("#clearDebug").on("click", function(event) {
		$("#debugOutputArea").val("");
	});

	var enable = true;
	
	// activate or deactivate the debug console
	$("#activateDebug").on("click", function(event) {
		
		event.preventDefault();
		if(enable){
			$(".debug2").fadeOut('fast');
			$(".debugclear").fadeOut('fast');
			enable = false;
			$("#activateDebug").text("Activate Debug Console");
			//$("#activateDebug").value("Activate Debug Console");
		}
		else{
			$(".debug2").fadeIn('fast');
			$(".debugclear").fadeIn('fast');
			enable = true;
			$("#activateDebug").text("Deactivate Debug Console");
			//$("#activateDebug").value("Deactivate Debug Console");
		}
	});
  

});
