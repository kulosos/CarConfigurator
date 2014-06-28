<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <h3 id="carConfigH3">Car Configuration</h3>
    <table border="0" cellspacing="0" cellpadding="0" id="carConfigTable" >
      <tr bgcolor="#E3E3E3">
        <th width="200" class="td_HL" >Konfiguration</th>
        <th width="200" class="td_HL">Auswahl</th>
        <th width="200" class="td_HL">Artikel-Nr.</th>
        <th width="200" class="td_HL_r">Preis</th>
      </tr>
      
      <tr>
        <td  class="td_model_label">Modell</td>
        <td  class="td_model"><xsl:value-of select="carConfig/car/carId"/></td>
        <td  class="td_model"><xsl:value-of select="carConfig/car/artNr"/></td>
        <td  class="td_model_r"><xsl:value-of select="carConfig/car/price"/> EUR</td>
      </tr>
      
      <!-- ######################################### COLORS ######################################### -->
      <tr>
        <td  class="td_midHL" >Ausstattung</td>
        <td  class="td_midHL2" colspan="3">Farbe</td>
      </tr>
      
      <xsl:for-each select="carConfig/car/carPart[partType='Body']">
      <tr>
        <td class="td_color_label">Lack</td>
        <td class="td_color"><div class="colorToConvert" > <xsl:value-of select="partColor"/> </div></td>
        <td class="td_color"> <xsl:value-of select="artNr"/> </td>
        <td class="td_color_r"> <xsl:value-of select="price"/> EUR</td>
      </tr>
      </xsl:for-each>
        
      <tr>
        <td class="td_color_label">Felgen</td>
        <td class="td_color"><div class="colorToConvert" > <xsl:value-of select="carConfig/car/carPart[partType='Wheel']/partColor"/> </div></td>
        <td class="td_color"> <xsl:value-of select="carConfig/car/carPart[partType='Wheel']/artNr"/> </td>
        <td class="td_color_r"> <xsl:value-of select="carConfig/car/carPart[partType='Wheel']/price"/> EUR</td>
      </tr>
      
      <tr>
        <td class="td_color_label">Anbauteile</td>
        <td class="td_color"><div class="colorToConvert" > <xsl:value-of select="carConfig/car/carPart[partType='Part']/partColor"/> </div></td>
        <td class="td_color"> <xsl:value-of select="carConfig/car/carPart[partType='Part']/artNr"/> </td>
        <td class="td_color_r"> <xsl:value-of select="carConfig/car/carPart[partType='Part']/price"/> EUR</td>
      </tr>
      
       <tr>
        <td class="td_color_label">Interior</td>
        <td class="td_color"><div class="colorToConvert" > <xsl:value-of select="carConfig/car/carPart[partType='Interior']/partColor"/> </div></td>
        <td class="td_color"> <xsl:value-of select="carConfig/car/carPart[partType='Interior']/artNr"/> </td>
        <td class="td_color_r"> <xsl:value-of select="carConfig/car/carPart[partType='Interior']/price"/> EUR</td>
      </tr>
      
      <xsl:for-each select="carConfig/car/carPart[partType='Window']">
      <tr>
        <td class="td_color_label">Scheibentönung</td>
        <td class="td_color"><div class="colorToConvert" > <xsl:value-of select="partColor"/> </div></td>
        <td class="td_color"> <xsl:value-of select="artNr"/> </td>
        <td class="td_color_r"> <xsl:value-of select="price"/> EUR</td>
      </tr>
      </xsl:for-each>
     
       <!-- ######################################### WHEELS ######################################### -->
      <tr>
        <td  class="td_midHL" colspan="4">Räder</td>
      </tr>
      
      <tr>
        <td class="td_color_label">Modell</td>
        <td class="td_color"> <xsl:value-of select="carConfig/car/carPart[partType='Wheel']/partId"/> </td>
        <td class="td_color"><xsl:value-of select="carConfig/car/carPart[partType='Wheel']/artNr"/></td>
        <td class="td_color_r"><xsl:value-of select="carConfig/car/carPart[partType='Wheel']/price"/> EUR</td>
      </tr>
      
      <tr>
        <td class="td_color_label">Größe</td>
        <td class="td_color"> <xsl:value-of select="carConfig/car/carPart[partType='Wheel']/scale/x"/> </td>
        <td class="td_color"> </td>
        <td class="td_color_r"> </td>
      </tr>
      
      <tr>
        <td class="td_color_label">Breite</td>
        <td class="td_color"> <xsl:value-of select="carConfig/car/carPart[partType='Wheel']/scale/z"/> </td>
        <td class="td_color"> </td>
        <td class="td_color_r"> </td>
      </tr>
      
      

    </table>
  </body>
  </html>
</xsl:template>
</xsl:stylesheet>