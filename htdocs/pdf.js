// You'll need to make your image into a Data URL
// Use http://dataurl.net/#dataurlmaker

// ------------------------------------------------------------------------------------

function savePdf(imgData){
	
	var addData = "data:image/jpeg;base64,"
	var finalData = addData + imgData;
	
    var doc = new jsPDF();
    var specialElementHandlers = {
        '#ignorePdf': function (element, renderer) {
            return true;
        }
    };

	$("#debugOutputArea").val("");
        var table = tableToJson($('#carConfigTable').get(0))
        var doc = new jsPDF('p','pt', 'a4', true);

		 var xPos = 50;
		 var cellWidth = 120;
		 var cellHeight = 30;
		 var upHeight =  300;//((600-180)*0.7)+70;
		 var upWidth = 480;//(960-192)*0.7;
		 var currentYPosition = upHeight + (3*cellHeight);
		
        doc.cellInitialize();
		
        $.each(table, function (i, row){
			
			//currentYPosition = currentYPosition + cellHeight;
            console.debug(row);
            $.each(row, function (j, cell){

				if(cell.indexOf('colorToConvert') == -1){ 
					doc.setFont("helvetica");
					doc.setFontType("normal");
					doc.setFontSize(12);
					doc.setTextColor(0,0,0);
					
					// headlines bold
					if(i==0 || i== 2 || i==8){
						doc.setFontType("bold");
						doc.setFontSize(14);	
						doc.setTextColor(0,0,0);
					}
					
					doc.cell(xPos, upHeight, cellWidth, cellHeight, cell, i);  // 2nd parameter=top margin,1st=left margin 3rd=row cell width 4th=Row height
				} 
				else {

					// ingore ColorStrings and draw colored Rectangles
					var rgbcolors = convertColorForPdf(cell);
					doc.setDrawColor(0);
					doc.setFillColor( Math.round(rgbcolors[0] ),  Math.round(rgbcolors[1] ),  Math.round(rgbcolors[2] ));
					//doc.setFillColor(255,255,0);
					doc.rect(cellWidth+60, currentYPosition+2, 26, 26, 'FD'); // filled red square with black borders
					
					doc.setFont("helvetica");
					doc.setFontType("normal");
					doc.setFontSize(12);
					doc.setTextColor(255,0,0);
					doc.cell(xPos, upHeight, cellWidth, cellHeight, "", i);  // 2nd parameter=top margin,1st=left margin 3rd=row cell width 4th=Row height
					currentYPosition = currentYPosition + cellHeight;
				}
            })
        })
		
	doc.setFont("helvetica");
	doc.setFontType("bold");
	doc.setFontSize(16);
	doc.text(xPos, 50, 'Car Configuration');
	
	doc.addImage(finalData, 'JPEG', xPos, 60, upWidth, upHeight-70);
	
	doc.save('CarConfig.pdf');

}

// ------------------------------------------------------------------------------------

function tableToJson(table) {
    var data = [];

    // first row needs to be headers
    var headers = [];
    for (var i=0; i<table.rows[0].cells.length; i++) {
        headers[i] = table.rows[0].cells[i].innerHTML.toLowerCase().replace(/ /gi,'');
    }

    // go through cells
    for (var i=0; i<table.rows.length; i++) {

        var tableRow = table.rows[i];
        var rowData = {};

        for (var j=0; j<tableRow.cells.length; j++) {
            rowData[ headers[j] ] = tableRow.cells[j].innerHTML;
        }
        data.push(rowData);
    }       

    return data;
}

// ------------------------------------------------------------------------------------

function convertColorForPdf(cell){

		var rgbStr = cell.replace('<div class="colorToConvert" style="background-color: rgb(',"");//.slice(0,57);
		var rgbStr2 = rgbStr.replace(');\"></div>', ""); 
		
		var r, g, b;
		var rgbcolors = rgbStr2.split(',', 3);

		r = Math.round(rgbcolors[0] * 255);
		g = Math.round(rgbcolors[1] * 255);
		b = Math.round(rgbcolors[2] * 255);

		return rgbcolors;
}

//-------------------------------------------------------------------------------------

$(document).ready(function() {

	$("#printPdfBtn").on("click", function(event) {
		u.getUnity().SendMessage("Admin", "getScreenshot", "");	
	});
});