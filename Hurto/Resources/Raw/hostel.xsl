<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <xsl:param name="attr" select="''" />
  <xsl:param name="value" select="''" />

  <xsl:template match="hostel">
    <html>
      <head>
        <meta charset="utf-8" />
        <style>
          html, body { margin: 0; background: #FFFFFF; color: #000000; }
          body { font-family: Arial, sans-serif; padding: 8px; }
          table { border-collapse: collapse; }
          td, th { border: 1px solid #555; padding: 4px 8px; }
          th { background: #ddd; }
          @media (prefers-color-scheme: dark) {
            html, body { background: #1E1E1E; color: #EEEEEE; }
            th { background: #3A3A3A; }
            td, th { border-color: #888; }
          }
        </style>
      </head>
      <body>
        <h2><xsl:value-of select="@name" /></h2>
        <xsl:if test="$attr != ''">
          <p>Фільтр: <b><xsl:value-of select="$attr" /></b> = <b><xsl:value-of select="$value" /></b></p>
        </xsl:if>
        <table>
          <tr>
            <th>ПІБ</th>
            <th>Факультет</th>
            <th>Кафедра</th>
            <th>Курс</th>
            <th>Проживання (кімната, з - по)</th>
          </tr>
          <xsl:for-each select="student[$attr = '' or @*[name() = $attr][. = $value]]">
            <xsl:sort select="@name" />
            <tr>
              <td><xsl:value-of select="@name" /></td>
              <td><xsl:value-of select="@faculty" /></td>
              <td><xsl:value-of select="@department" /></td>
              <td><xsl:value-of select="@course" /></td>
              <td>
                <xsl:for-each select="residence">
                  <div>
                    кімн. <xsl:value-of select="@room" />,
                    <xsl:value-of select="@checkIn" /> - <xsl:value-of select="@checkOut" />
                  </div>
                </xsl:for-each>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
