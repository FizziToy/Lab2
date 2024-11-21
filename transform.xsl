<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" indent="yes" />
	<xsl:template match="/">
		<html>
			<head>
				<title>Список студентів</title>
			</head>
			<body>
				<h1>Список студентів</h1>
				<table border="1">
					<tr>
						<th>Факультет</th>
						<th>Кафедра</th>
						<th>Дисципліна</th>
						<th>Ім'я</th>
						<th>Оцінка</th>
					</tr>
					<xsl:for-each select="//student">
						<tr>
							<td>
								<xsl:value-of select="ancestor::faculty/@name" />
							</td>
							<td>
								<xsl:value-of select="ancestor::department/@name" />
							</td>
							<td>
								<xsl:value-of select="ancestor::discipline/@name" />
							</td>
							<td>
								<xsl:value-of select="@name" />
							</td>
							<td>
								<xsl:value-of select="@grade" />
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
