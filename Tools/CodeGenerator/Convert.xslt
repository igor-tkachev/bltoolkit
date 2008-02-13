<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="text" version="1.0" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<!-- parameters passed in by the TransformCodeGenerator -->
	<xsl:param name="generator"></xsl:param>
	<xsl:param name="version"></xsl:param>
	<xsl:param name="filename"></xsl:param>
	<xsl:param name="date-created"></xsl:param>
	<xsl:param name="created-by"></xsl:param>
	<xsl:param name="namespace"></xsl:param>
	<xsl:param name="output"></xsl:param>

	<!-- support variables -->
	<xsl:variable name="lf" xml:space="preserve">&#13;</xsl:variable>
	<xsl:variable name="t1" xml:space="preserve">&#9;</xsl:variable>
	<xsl:variable name="t2" xml:space="preserve">&#9;&#9;</xsl:variable>
	<xsl:variable name="t3" xml:space="preserve">&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t4" xml:space="preserve">&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t5" xml:space="preserve">&#9;&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t6" xml:space="preserve">&#9;&#9;&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t7" xml:space="preserve">&#9;&#9;&#9;&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t8" xml:space="preserve">&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="s1" xml:space="preserve">&#32;</xsl:variable>
	<xsl:variable name="s2" xml:space="preserve">&#32;&#32;</xsl:variable>
	<xsl:variable name="s3" xml:space="preserve">&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s4" xml:space="preserve">&#32;&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s5" xml:space="preserve">&#32;&#32;&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s6" xml:space="preserve">&#32;&#32;&#32;&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s7" xml:space="preserve">&#32;&#32;&#32;&#32;&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s8" xml:space="preserve">&#32;&#32;&#32;&#32;&#32;&#32;&#32;&#32;</xsl:variable>

	<!-- customization -->
	<xsl:variable name="methodname"     select="'To'"/>

	<xsl:variable name="padding"        select="string-length('DateTimeOffset?')"/>
	<xsl:variable name="objectpad"      select="$padding - string-length('object')"/>

	<!-- the root of all evil -->
	<xsl:template match="/">
		<!--<xsl:call-template name="header-comment"/>-->
		<xsl:apply-templates select="code"/>
	</xsl:template>

	<xsl:template name="header-comment">
		<xsl:value-of select="$lf"/>
		<xsl:text>#region Generated File</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text>/*</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text> * GENERATED FILE -- DO NOT EDIT</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text> *</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text> * Generator: </xsl:text>
		<xsl:value-of select="$generator"/>
		<xsl:value-of select="$lf"/>
		<xsl:text> * Version:   </xsl:text>
		<xsl:value-of select="$version"/>
		<xsl:value-of select="$lf"/>
		<xsl:text> *</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text> *</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text> * Generated code from "</xsl:text>
		<xsl:value-of select="$filename"/>
		<xsl:text>"</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text> *</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text> * Created: </xsl:text>
		<xsl:value-of select="$date-created"/>
		<xsl:value-of select="$lf"/>
		<xsl:text> * By:      </xsl:text>
		<xsl:value-of select="$created-by"/>
		<xsl:value-of select="$lf"/>
		<xsl:text> *</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text> */</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:text>#endregion</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="code">
		<xsl:apply-templates select="using"/>
		<xsl:value-of select="$lf"/>
		<xsl:text>namespace </xsl:text>
		<xsl:value-of select="$namespace"/>
		<xsl:value-of select="$lf"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:call-template name="class"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="using">
    <xsl:if test="@condition">
      <xsl:text>#if </xsl:text>
      <xsl:value-of select="@condition"/>
      <xsl:value-of select="$lf"/>
    </xsl:if>
    <xsl:text>using </xsl:text>
		<xsl:value-of select="@namespace"/>
		<xsl:text>;</xsl:text>
    <xsl:if test="@condition">
      <xsl:value-of select="$lf"/>
      <xsl:text>#endif</xsl:text>
    </xsl:if>
    <xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- the class definition -->
	<xsl:template name="class">
		<xsl:value-of select="$t1"/>
		<xsl:text>public class Convert</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t1"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="converter|comment"/>
		<xsl:apply-templates select="region"/>

		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>private static InvalidCastException CreateInvalidCastException(Type originalType, Type conversionType)</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>return new InvalidCastException(string.Format("Invalid cast from {0} to {1}", originalType.FullName, conversionType.FullName));</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:value-of select="$t1"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- region -->
	<xsl:template match="region">
		<xsl:value-of select="$t2"/>
		<xsl:text>#region </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="converter|comment"/>
		<xsl:apply-templates select="region"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>#endregion</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- declarations -->
	<xsl:template match="converter">
		<xsl:variable name="tonullable" select="@nullable='true'"/>
		<xsl:variable name="fulltype">
			<xsl:value-of select="@type"/>
			<xsl:if test="$tonullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="typepad" select="$padding - string-length($fulltype)"/>
		<xsl:variable name="notclscompliant" select="/code/type[@name=current()/@type]/@clscompliant='false'"/>
		<xsl:variable name="condition" select="/code/type[@name=current()/@type]/@condition"/>

		<xsl:if test="$condition">
			<xsl:value-of select="$t2"/>
			<xsl:text>#if </xsl:text>
			<xsl:value-of select="$condition"/>
			<xsl:value-of select="$lf"/>
		</xsl:if>

		<xsl:value-of select="$t2"/>
		<xsl:text>#region </xsl:text>
		<xsl:value-of select="$fulltype"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>

		<xsl:apply-templates select="group|from|comment|br" mode ="body">
			<xsl:with-param name="totype" select="@type"/>
			<xsl:with-param name="toname">
				<xsl:choose>
					<xsl:when test="@name">
						<xsl:value-of select="@name"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@type"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
			<xsl:with-param name="tonullable" select="$tonullable"/>
			<xsl:with-param name="code" select="default/text()"/>
		</xsl:apply-templates>

		<xsl:if test="$notclscompliant">
			<xsl:value-of select="$t2"/>
			<xsl:text>[CLSCompliant(false)]</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

		<xsl:value-of select="$t2"/>
		<xsl:text>public static </xsl:text>
		<xsl:value-of select="$fulltype"/>
		<xsl:value-of select="$s1"/>
		<xsl:value-of select="$methodname"/>
		<xsl:if test="$tonullable">
			<xsl:text>Nullable</xsl:text>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="@name">
				<xsl:value-of select="@name"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="@type"/>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:text>(object p)</xsl:text>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$objectpad"/>
		</xsl:call-template>
		<xsl:call-template name="writecode">
			<xsl:with-param name="code">
				<xsl:apply-templates select="default">
					<xsl:with-param name="tonullable" select="$tonullable"/>
				</xsl:apply-templates>
			</xsl:with-param>
		</xsl:call-template>

		<xsl:value-of select="$lf"/>

		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>#endregion</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:if test="$condition">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t2"/>
			<xsl:text>#endif</xsl:text>
		</xsl:if>

		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- body -->
	<xsl:template match="group" mode="body">
		<xsl:param name="totype"/>
		<xsl:param name="toname"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="code"/>

<!--
		<xsl:if test="@nullable='true'">
			<xsl:text>#if FW2</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>
-->

		<xsl:if test="@name">
			<xsl:value-of select="$t2"/>
			<xsl:text>// </xsl:text>
			<xsl:value-of select="@name"/>
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t2"/>
			<xsl:text>// </xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>
		<xsl:apply-templates select="group|from|comment|br|include" mode="body">
			<xsl:with-param name="fromnullable" select="@nullable='true'"/>
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="toname"       select="$toname"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="group"        select="."/>
			<xsl:with-param name="code">
				<xsl:choose>
					<xsl:when test ="default/text()">
						<xsl:value-of select="default/text()"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$code"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$lf"/>

<!--
		<xsl:if test="@nullable='true'">
			<xsl:text>#endif</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>
-->
	</xsl:template>

	<xsl:template match="from" mode="body">
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="toname"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="code"/>

		<xsl:call-template name="frombody">
			<xsl:with-param name="fromtype"     select="@type"/>
			<xsl:with-param name="fromnullable" select="$fromnullable"/>
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="toname"       select="$toname"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="code"         select="$code"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="include" mode="body">
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="toname"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="group"/>
		<xsl:param name="code"/>
		<xsl:call-template name="includebody">
			<xsl:with-param name="fromnullable" select="$fromnullable"/>
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="toname"       select="$toname"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="group"        select="$group"/>
			<xsl:with-param name="code"         select="$code"/>
			<xsl:with-param name="template"     select="@template"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="includebody">
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="toname"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="group"/>
		<xsl:param name="code"/>
		<xsl:param name="template"/>
		<xsl:for-each select="/code/template[@name=$template]/*">
			<xsl:choose>
				<xsl:when test="name()='type'">
					<xsl:variable name ="fromtype" select="@name"/>
					<xsl:if test="not($fromtype=$totype and $fromnullable=$tonullable) and not($group/from[@type=$fromtype])">
						<xsl:call-template name="frombody">
							<xsl:with-param name="fromtype"     select="$fromtype"/>
							<xsl:with-param name="fromnullable" select="$fromnullable"/>
							<xsl:with-param name="totype"       select="$totype"/>
							<xsl:with-param name="toname"       select="$toname"/>
							<xsl:with-param name="tonullable"   select="$tonullable"/>
							<xsl:with-param name="code"         select="$code"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:when>
				<xsl:when test="name()='include'">
					<xsl:call-template name="includebody">
						<xsl:with-param name="fromnullable" select="$fromnullable"/>
						<xsl:with-param name="totype"       select="$totype"/>
						<xsl:with-param name="toname"       select="$toname"/>
						<xsl:with-param name="tonullable"   select="$tonullable"/>
						<xsl:with-param name="group"        select="$group"/>
						<xsl:with-param name="code"         select="$code"/>
						<xsl:with-param name="template"     select="@template"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="name()='br'">
					<xsl:value-of select="$lf"/>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="frombody">
		<xsl:param name="fromtype"/>
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="toname"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="code"/>

		<xsl:variable name="notclscompliant" select="/code/type[@name=$totype]/@clscompliant='false' or /code/type[@name=$fromtype]/@clscompliant='false'"/>
		<xsl:variable name="fromcondition" select="/code/type[@name=$fromtype]/@condition"/>
		<xsl:variable name="tocondition" select="/code/type[@name=$totype]/@condition"/>

		<xsl:variable name="fromfulltype">
			<xsl:value-of select="$fromtype"/>
			<xsl:if test="$fromnullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="fromtypepad" select="$padding - string-length($fromfulltype)"/>
		<xsl:variable name="tofulltype">
			<xsl:value-of select="$totype"/>
			<xsl:if test="$tonullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>

		<xsl:if test="$fromcondition and not ($fromcondition = $tocondition)">
			<xsl:value-of select="$t2"/>
			<xsl:text>#if </xsl:text>
			<xsl:value-of select="$fromcondition"/>
			<xsl:value-of select="$lf"/>
		</xsl:if>
		<xsl:if test="$notclscompliant">
			<xsl:value-of select="$t2"/>
			<xsl:text>[CLSCompliant(false)]</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

		<xsl:value-of select="$t2"/>
		<xsl:text>public static </xsl:text>
		<xsl:value-of select="$tofulltype"/>
		<xsl:value-of select="$s1"/>

		<xsl:value-of select="$methodname"/>
		<xsl:if test="$tonullable">
			<xsl:text>Nullable</xsl:text>
		</xsl:if>
		<xsl:value-of select="$toname"/>
		<xsl:text>(</xsl:text>
		<xsl:value-of select="$fromfulltype"/>
		<xsl:text> p)</xsl:text>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$fromtypepad"/>
		</xsl:call-template>
		<xsl:call-template name="writecode">
		<xsl:with-param name="code">
			<xsl:choose>
				<xsl:when test ="text()">
					<xsl:value-of select="text()"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$code"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:with-param>
		</xsl:call-template>
		<xsl:if test="$fromcondition and not ($fromcondition = $tocondition)">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t2"/>
			<xsl:text>#endif</xsl:text>
		</xsl:if>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- runtime -->
	<xsl:template match="from" mode="runtime">
		<xsl:param name="totype"/>
		<xsl:param name="toname"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="fromnullable"/>
		<xsl:param name="default"/>
		<xsl:variable name="fromfullname">
			<xsl:value-of select="@type"/>
			<xsl:if test="$fromnullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>

		<xsl:if test="text() or $default">
			<xsl:variable name="typepad" select="$padding - string-length($fromfullname)"/>
			<xsl:variable name="fromcondition" select="/code/type[@name=current()/@type]/@condition"/>
			<xsl:variable name="tocondition" select="/code/type[@name=$totype]/@condition"/>

			<xsl:if test="$fromcondition and not ($fromcondition = $tocondition)">
				<xsl:value-of select="$t3"/>
				<xsl:text>#if </xsl:text>
				<xsl:value-of select="$fromcondition"/>
				<xsl:value-of select="$lf"/>
			</xsl:if>
			<xsl:value-of select="$t3"/>
			<xsl:text>if (p is </xsl:text>
			<xsl:value-of select="$fromfullname"/>
			<xsl:text>) </xsl:text>
			<xsl:call-template name ="writeSpaces">
				<xsl:with-param name="count" select="$typepad"/>
			</xsl:call-template>
			<xsl:text>return </xsl:text>
			<xsl:value-of select="$methodname"/>
			<xsl:if test="$tonullable">
				<xsl:text>Nullable</xsl:text>
			</xsl:if>
			<xsl:value-of select="$toname"/>
			<xsl:text>((</xsl:text>
			<xsl:value-of select="$fromfullname"/>
			<xsl:text>)p);</xsl:text>
			<xsl:if test="$fromcondition and not ($fromcondition = $tocondition)">
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
				<xsl:text>#endif</xsl:text>
			</xsl:if>
			<xsl:value-of select="$lf"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="group" mode="runtime">
		<xsl:param name="totype"/>
		<xsl:param name="toname"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="fromnullable"/>
		<xsl:param name="default"/>
		
		<xsl:variable name="fromnullablelocal" select="@nullable='true' or $fromnullable"/>
		<xsl:variable name="defaultcode">
			<xsl:choose>
				<xsl:when test="default/text()">
					<xsl:value-of select="default/text()"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$default"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:if test="not($fromnullablelocal)">

		<xsl:if test="@name">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t3"/>
			<xsl:text>// </xsl:text>
			<xsl:value-of select="@name"/>
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t3"/>
			<xsl:text>//</xsl:text>
		</xsl:if>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|from|br" mode ="runtime">
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="toname"       select="$toname"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="fromnullable" select="$fromnullablelocal"/>
			<xsl:with-param name="default"      select="$defaultcode"/>
		</xsl:apply-templates>
    </xsl:if>

	</xsl:template>

	<!-- default -->
	<xsl:template match="default">
		<xsl:param name="tonullable"/>
		<xsl:if test="@nullvalue">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t3"/>
			<xsl:text>if (p == null || p is DBNull) return </xsl:text>
			<xsl:value-of select="@nullvalue"/>
			<xsl:text>;</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>
    <xsl:value-of select="$lf"/>
    <xsl:value-of select="$t3"/>
    <xsl:text>if (p is </xsl:text>
    <xsl:value-of select="../@type"/>
    <xsl:text>) return (</xsl:text>
    <xsl:value-of select="../@type"/>
    <xsl:text>)p;</xsl:text>
    <xsl:value-of select="$lf"/>
    <xsl:if test="not(@noruntime) and @nullvalue">
			<xsl:apply-templates select="../from|../group|../br" mode="runtime">
				<xsl:with-param name="totype"     select="../@type"/>
				<xsl:with-param name="toname">
					<xsl:choose>
						<xsl:when test="../@name">
							<xsl:value-of select="../@name"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="../@type"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="tonullable" select="$tonullable"/>
			</xsl:apply-templates>
		</xsl:if>
		<xsl:value-of select="text()"/>
		<xsl:if test="not(@nothrow) and @nullvalue">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t3"/>
			<xsl:text>throw CreateInvalidCastException(p.GetType(), typeof(</xsl:text>
			<xsl:value-of select="../@type"/>
			<xsl:if test="$tonullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
			<xsl:text>));</xsl:text>
		</xsl:if>
	</xsl:template>

	<!-- comments -->
	<xsl:template name ="comment">
		<xsl:if test="text()">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t3"/>
			<xsl:text>// </xsl:text>
			<xsl:value-of select="text()"/>
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t3"/>
			<xsl:text>//</xsl:text>
		</xsl:if>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="comment">
		<xsl:call-template name="comment"/>
	</xsl:template>

	<xsl:template match="comment" mode ="partial">
		<xsl:if test="not(@tonullable) or @tonullable!='true'">
			<xsl:call-template name="comment"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="comment" mode ="nuldef">
		<xsl:if test="@tonullable='true'">
			<xsl:call-template name="comment"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="comment" mode ="body">
		<xsl:if test="not(@tonullable) or @tonullable!='true'">
			<xsl:call-template name="comment"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="comment" mode ="nulbody">
		<xsl:if test="@tonullable='true'">
			<xsl:call-template name="comment"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="br">
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="br" mode="def">
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="br" mode="body">
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="br" mode="runtime">
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- support function -->
	<xsl:template name="writecode">
		<xsl:param name="code"/>
		<xsl:choose>
			<xsl:when test="contains($code, '&#13;')">
				<!-- multi-line -->
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t2"/>
				<xsl:text>{</xsl:text>
				<xsl:value-of select="$code"/>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t2"/>
				<xsl:text>}</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<!-- single-line -->
				<xsl:text> { </xsl:text>
				<xsl:value-of select="$code"/>
				<xsl:text> }</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- support function -->
	<xsl:template name="writeSpaces">
		<xsl:param name="count"/>
		<xsl:choose>
			<xsl:when test="$count&lt;1"/>
			<xsl:when test="$count=1">
				<xsl:value-of select="$s1"/>
			</xsl:when>
			<xsl:when test="$count=2">
				<xsl:value-of select="$s2"/>
			</xsl:when>
			<xsl:when test="$count=3">
				<xsl:value-of select="$s3"/>
			</xsl:when>
			<xsl:when test="$count=4">
				<xsl:value-of select="$s4"/>
			</xsl:when>
			<xsl:when test="$count=5">
				<xsl:value-of select="$s5"/>
			</xsl:when>
			<xsl:when test="$count=6">
				<xsl:value-of select="$s6"/>
			</xsl:when>
			<xsl:when test="$count=7">
				<xsl:value-of select="$s7"/>
			</xsl:when>
			<xsl:when test="$count=8">
				<xsl:value-of select="$s8"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$s8"/>
				<xsl:call-template name ="writeSpaces">
					<xsl:with-param name="count" select="$count - 8"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
