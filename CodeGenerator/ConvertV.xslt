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
	<xsl:variable name="lf">&#13;</xsl:variable>
	<xsl:variable name="t1">&#9;</xsl:variable>
	<xsl:variable name="t2">&#9;&#9;</xsl:variable>
	<xsl:variable name="t3">&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t4">&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t5">&#9;&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t6">&#9;&#9;&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t7">&#9;&#9;&#9;&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="t8">&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;</xsl:variable>
	<xsl:variable name="s1">&#32;</xsl:variable>
	<xsl:variable name="s2">&#32;&#32;</xsl:variable>
	<xsl:variable name="s3">&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s4">&#32;&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s5">&#32;&#32;&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s6">&#32;&#32;&#32;&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s7">&#32;&#32;&#32;&#32;&#32;&#32;&#32;</xsl:variable>
	<xsl:variable name="s8">&#32;&#32;&#32;&#32;&#32;&#32;&#32;&#32;</xsl:variable>

	<!-- customization -->
	<xsl:variable name="baseclassname"  select="'CB'"/>
	<xsl:variable name="nullableprefix" select="'N'"/>
	<xsl:variable name="instancename"   select="'I'"/>
	<xsl:variable name="methodname"     select="'C'"/>
	<xsl:variable name="padding"        select="string-length('SqlDateTime')"/>

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
		<xsl:text>using </xsl:text>
		<xsl:value-of select="@namespace"/>
		<xsl:text>;</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- the class definition -->
	<xsl:template name="class">
		<xsl:value-of select="$t1"/>
		<xsl:text>public static partial class Convert&lt;T,P&gt;</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t1"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>static readonly </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T, P&gt; </xsl:text>
		<xsl:value-of select="$instancename"/>
		<xsl:text> = GetConverter();</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>static </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T, P&gt; GetConverter()</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>Type t = typeof(T);</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>// Convert to the same type.</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>//</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>if (t.IsAssignableFrom(typeof(P)))</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t4"/>
		<xsl:text>return (</xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T, P&gt;)(object)(new Assignable&lt;T&gt;());</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="converter|comment" mode="def"/>
		<xsl:apply-templates select="region" mode="def"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>return new Default&lt;T,P&gt;();</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="converter" mode="body"/>
		<xsl:apply-templates select="region" mode="body"/>
		<xsl:value-of select="$t1"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- region (method declarations) -->
	<xsl:template match="region" mode="def">
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>// </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>//</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="converter|comment|br" mode="def"/>
		<xsl:apply-templates select="region" mode="def"/>
	</xsl:template>

	<!-- region (method bodies) -->
	<xsl:template match="region" mode="body">
		<xsl:value-of select="$t2"/>
		<xsl:text>#region </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="converter|comment" mode="body"/>
		<xsl:apply-templates select="region" mode="body"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>#endregion</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- declarations -->
	<xsl:template name="def">
		<xsl:param name="tonullable"/>
		<xsl:variable name="typepad" select="$padding - string-length(@type)"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>if (t == typeof(</xsl:text>
		<xsl:value-of select="@type"/>
		<xsl:if test="$tonullable">
			<xsl:text>?</xsl:text>
		</xsl:if>
		<xsl:text>))</xsl:text>
		<xsl:if test="not($tonullable)">
			<xsl:value-of select="$s1"/>
		</xsl:if>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$typepad"/>
		</xsl:call-template>
		<xsl:text>return Get</xsl:text>
		<xsl:if test="$tonullable">
			<xsl:text>Nullable</xsl:text>
		</xsl:if>
		<xsl:value-of select="@type"/>
		<xsl:text>Converter();</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="converter" mode="def">
		<xsl:call-template name="def">
			<xsl:with-param name="tonullable" select="@nullable='true'"/>
		</xsl:call-template>
	</xsl:template>

	<!-- bodies -->
	<xsl:template name="body">
		<xsl:param name="tonullable"/>
		<xsl:variable name="totype" select="@type"/>
		<xsl:variable name="tonick">
			<xsl:if test="$tonullable">
				<xsl:value-of select="$nullableprefix"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="/code/nick[@type=$totype]">
					<xsl:value-of select="/code/nick[@type=$totype]/@name"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$totype"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tofulltype">
			<xsl:value-of select="@type"/>
			<xsl:if test="$tonullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>
		<xsl:value-of select="$t2"/>
		<xsl:text>#region </xsl:text>
		<xsl:value-of select="$tofulltype"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|from|comment|br" mode ="def">
			<xsl:with-param name="totype" select="@type"/>
			<xsl:with-param name="tonick" select="$tonick"/>
			<xsl:with-param name="tonullable" select="$tonullable"/>
			<xsl:with-param name="default" select="default/text()"/>
		</xsl:apply-templates>

		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>sealed class </xsl:text>
		<xsl:value-of select="$tonick"/>
		<xsl:text>_O         : </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;</xsl:text>
		<xsl:value-of select="$tofulltype"/>
		<xsl:text> ,object>    </xsl:text>
		<xsl:text>{ public override </xsl:text>
		<xsl:value-of select="$tofulltype"/>
		<xsl:value-of select="$s1"/>
		<xsl:value-of select="$methodname"/>
			<xsl:text>(object p)  </xsl:text>
		<xsl:call-template name="writeCode">
			<xsl:with-param name="code">
				<xsl:apply-templates select="default"/>
			</xsl:with-param>
		</xsl:call-template>
		<xsl:text> }</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>static </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T, P&gt; Get</xsl:text>
		<xsl:if test="$tonullable">
			<xsl:text>Nullable</xsl:text>
		</xsl:if>
		<xsl:value-of select="@type"/>
		<xsl:text>Converter()</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>Type t = typeof(P);</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|from|comment|br" mode="body">
			<xsl:with-param name="totype" select="@type"/>
			<xsl:with-param name="tonick" select="$tonick"/>
			<xsl:with-param name="tonullable" select="$tonullable"/>
			<xsl:with-param name="default" select="default/text()"/>
		</xsl:apply-templates>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>if (t == typeof(object))      return (</xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T, P&gt;)(object)(new </xsl:text>
		<xsl:value-of select="$tonick"/>
		<xsl:text>_O</xsl:text>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$padding - string-length($tonick) - 1"/>
		</xsl:call-template>
		<xsl:text>());</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>return (</xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T, P&gt;)(object)Convert&lt;</xsl:text>
		<xsl:value-of select="$tofulltype"/>
		<xsl:text>, object&gt;.</xsl:text>
		<xsl:value-of select="$instancename"/>
		<xsl:text>;</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>#endregion</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="converter" mode="body">
		<xsl:call-template name="body">
			<xsl:with-param name="tonullable" select="@nullable='true'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="includedef">
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonick"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="default"/>
		<xsl:param name="group"/>
		<xsl:param name="template"/>
		<xsl:for-each select="/code/template[@name=$template]/*">
			<xsl:choose>
				<xsl:when test="name()='type'">
					<xsl:variable name ="fromtype" select="@name"/>
					<xsl:if test="not($fromtype=$totype and $fromnullable=$tonullable) and not($group/from[@type=$fromtype])">
						<xsl:call-template name="fromdef">
							<xsl:with-param name="fromnick">
								<xsl:if test="$fromnullable">
									<xsl:value-of select="$nullableprefix"/>
								</xsl:if>
								<xsl:choose>
									<xsl:when test="/code/nick[@type=$fromtype]">
										<xsl:value-of select="/code/nick[@type=$fromtype]/@name"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$fromtype"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:with-param>
							<xsl:with-param name="fromtype"     select="$fromtype"/>
							<xsl:with-param name="fromnullable" select="$fromnullable"/>
							<xsl:with-param name="totype"       select="$totype"/>
							<xsl:with-param name="tonick"       select="$tonick"/>
							<xsl:with-param name="tonullable"   select="$tonullable"/>
							<xsl:with-param name="code"         select="$default"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:when>
				<xsl:when test="name()='include'">
					<xsl:call-template name="includedef">
						<xsl:with-param name="fromnullable" select="$fromnullable"/>
						<xsl:with-param name="totype"       select="$totype"/>
						<xsl:with-param name="tonick"       select="$tonick"/>
						<xsl:with-param name="tonullable"   select="$tonullable"/>
						<xsl:with-param name="default"      select="$default"/>
						<xsl:with-param name="group"        select="$group"/>
						<xsl:with-param name="template"     select="@template"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="name()='br'">
					<xsl:value-of select="$lf"/>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="include" mode="def">
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonick"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="default"/>
		<xsl:param name="group"/>
		<xsl:call-template name="includedef">
			<xsl:with-param name="fromnullable" select="$fromnullable"/>
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="tonick"       select="$tonick"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="default"      select="$default"/>
			<xsl:with-param name="group"        select="$group"/>
			<xsl:with-param name="template"     select="@template"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="includebody">
		<xsl:param name="fromnullable"/>
		<xsl:param name="tonick"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="group"/>
		<xsl:param name="template"/>
		<xsl:for-each select="/code/template[@name=$template]/*">
			<xsl:choose>
				<xsl:when test="name()='type'">
					<xsl:variable name ="fromtype" select="@name"/>
					<xsl:if test="not($fromtype=$totype and $fromnullable=$tonullable) and not($group/from[@type=$fromtype])">
						<xsl:call-template name="frombody">
							<xsl:with-param name="fromnick">
								<xsl:if test="$fromnullable">
									<xsl:value-of select="$nullableprefix"/>
								</xsl:if>
								<xsl:choose>
									<xsl:when test="/code/nick[@type=$fromtype]">
										<xsl:value-of select="/code/nick[@type=$fromtype]/@name"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$fromtype"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:with-param>
							<xsl:with-param name="fromtype"     select="$fromtype"/>
							<xsl:with-param name="fromnullable" select="$fromnullable"/>
							<xsl:with-param name="totype"       select="$totype"/>
							<xsl:with-param name="tonick"       select="$tonick"/>
							<xsl:with-param name="tonullable"   select="$tonullable"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:when>
				<xsl:when test="name()='include'">
					<xsl:call-template name="includebody">
						<xsl:with-param name="fromnullable" select="$fromnullable"/>
						<xsl:with-param name="totype"       select="$totype"/>
						<xsl:with-param name="tonick"       select="$tonick"/>
						<xsl:with-param name="tonullable"   select="$tonullable"/>
						<xsl:with-param name="group"        select="$group"/>
						<xsl:with-param name="template"     select="@template"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="name()='br'">
					<xsl:value-of select="$lf"/>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="include" mode="body">
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonick"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="group"/>
		<xsl:call-template name="includebody">
			<xsl:with-param name="fromnullable" select="$fromnullable"/>
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="tonick"       select="$tonick"/>
			<xsl:with-param name="group"        select="$group"/>
			<xsl:with-param name="template"     select="@template"/>
		</xsl:call-template>
	</xsl:template>

	<!-- group -->
	<xsl:template match="group" mode="def">
		<xsl:param name="totype"/>
		<xsl:param name="tonick"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="default"/>
		<xsl:variable name="name" select="@name"/>
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
		<xsl:if test="@name">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t2"/>
			<xsl:text>// </xsl:text>
			<xsl:value-of select="@name"/>
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t2"/>
			<xsl:text>//</xsl:text>
		</xsl:if>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|from|comment|br|include" mode="def">
			<xsl:with-param name="fromnullable" select="@nullable='true'"/>
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="tonick"       select="$tonick"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="default"      select="$defaultcode"/>
			<xsl:with-param name="group"        select="."/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="group" mode="body">
		<xsl:param name="totype"/>
		<xsl:param name="tonick"/>
		<xsl:param name="tonullable"/>
		<xsl:variable name="name" select="@name"/>
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
		<xsl:apply-templates select="group|from|comment|br|include" mode="body">
			<xsl:with-param name="fromnullable" select="@nullable='true'"/>
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="tonick"       select="$tonick"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="group"        select="."/>
		</xsl:apply-templates>
	</xsl:template>

	<!-- from -->
	<xsl:template match="from" mode="def">
		<xsl:param name="fromnullable"/>
		<xsl:param name="tonick"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="default"/>
		<xsl:variable name="fromtype" select="@type"/>
		<xsl:call-template name="fromdef">
			<xsl:with-param name="fromnick">
				<xsl:if test="$fromnullable">
					<xsl:value-of select="$nullableprefix"/>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="@nick">
						<xsl:value-of select="@nick"/>
					</xsl:when>
					<xsl:when test="/code/nick[@type=$fromtype]">
						<xsl:value-of select="/code/nick[@type=$fromtype]/@name"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$fromtype"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
			<xsl:with-param name="fromtype" select="$fromtype"/>
			<xsl:with-param name="fromnullable" select="@nullable='true' or $fromnullable"/>
			<xsl:with-param name="totype"   select="$totype"/>
			<xsl:with-param name="tonick"   select="$tonick"/>
			<xsl:with-param name="tonullable" select="$tonullable"/>
			<xsl:with-param name="code">
				<xsl:choose>
					<xsl:when test="text()">
						<xsl:value-of select="text()"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$default"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="fromdef">
		<xsl:param name="fromnick"/>
		<xsl:param name="fromtype"/>
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonick"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="code"/>
		<xsl:variable name="fromfulltype">
			<xsl:value-of select="$fromtype"/>
			<xsl:if test="$fromnullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="tofulltype">
			<xsl:value-of select="$totype"/>
			<xsl:if test="$tonullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="typepad" select="$padding - string-length($fromfulltype)"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>sealed class </xsl:text>
		<xsl:value-of select="$tonick"/>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="$fromnick"/>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$padding - string-length($fromnick) - string-length($tonick)"/>
		</xsl:call-template>
		<xsl:text>: </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;</xsl:text>
		<xsl:value-of select="$tofulltype"/>
		<xsl:text>,</xsl:text>
		<xsl:value-of select="$fromfulltype"/>
		<xsl:text>&gt;</xsl:text>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$typepad"/>
		</xsl:call-template>
		<xsl:text>{ public override </xsl:text>
		<xsl:value-of select="$tofulltype"/>
		<xsl:value-of select="$s1"/>
		<xsl:value-of select="$methodname"/>
			<xsl:text>(</xsl:text>
		<xsl:value-of select="$fromfulltype"/>
		<xsl:text> p)</xsl:text>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$typepad"/>
		</xsl:call-template>
		<xsl:call-template name="writeCode">
			<xsl:with-param name="code" select="$code"/>
		</xsl:call-template>
		<xsl:text> }</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="from" mode="body">
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonick"/>
		<xsl:param name="tonullable"/>

		<xsl:variable name ="fromtype" select="@type"/>
		
		<xsl:call-template name="frombody">
			<xsl:with-param name="fromnick">
				<xsl:if test="$fromnullable">
					<xsl:value-of select="$nullableprefix"/>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="@nick">
						<xsl:value-of select="@nick"/>
					</xsl:when>
					<xsl:when test="/code/nick[@type=$fromtype]">
						<xsl:value-of select="/code/nick[@type=$fromtype]/@name"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$fromtype"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
			<xsl:with-param name="fromtype" select="$fromtype"/>
			<xsl:with-param name="fromnullable" select="@nullable='true' or $fromnullable"/>
			<xsl:with-param name="totype"   select="$totype"/>
			<xsl:with-param name="tonick"   select="$tonick"/>
			<xsl:with-param name="tonullable" select="$tonullable"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="frombody">
		<xsl:param name="fromnick"/>
		<xsl:param name="fromtype"/>
		<xsl:param name="fromnullable"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonick"/>
		<xsl:param name="tonullable"/>
		<xsl:variable name="fromfulltype">
			<xsl:value-of select="$fromtype"/>
			<xsl:if test="$fromnullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>

		<xsl:value-of select="$t3"/>
		<xsl:text>if (t == typeof(</xsl:text>
		<xsl:value-of select="$fromfulltype"/>
		<xsl:text>)) </xsl:text>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$padding - string-length($fromfulltype)"/>
		</xsl:call-template>
		<xsl:text>return (</xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T, P&gt;)(object)(new </xsl:text>
		<xsl:value-of select="$tonick"/>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="$fromnick"/>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$padding - string-length($fromnick) - string-length($tonick)"/>
		</xsl:call-template>
		<xsl:text>());</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="from" mode="runtime">
		<xsl:param name="totype"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="default"/>
		<xsl:if test="text() or $default">
			<xsl:variable name="typepad" select="$padding - string-length(@type)"/>
			<xsl:value-of select="$t4"/>
			<xsl:text>if (p is </xsl:text>
			<xsl:value-of select="@type"/>
			<xsl:text>) </xsl:text>
			<xsl:call-template name ="writeSpaces">
				<xsl:with-param name="count" select="$typepad"/>
			</xsl:call-template>
			<xsl:text>return Convert</xsl:text>
			<xsl:text>&lt;</xsl:text>
			<xsl:value-of select="$totype"/>
			<xsl:if test="$tonullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
			<xsl:text>,</xsl:text>
			<xsl:value-of select="@type"/>
			<xsl:text>&gt;</xsl:text>
			<xsl:call-template name ="writeSpaces">
				<xsl:with-param name="count" select="$typepad"/>
			</xsl:call-template>
			<xsl:text>.</xsl:text>
			<xsl:value-of select="$instancename"/>
			<xsl:text>.</xsl:text>
			<xsl:value-of select="$methodname"/>
			<xsl:text>((</xsl:text>
			<xsl:value-of select="@type"/>
			<xsl:text>)p);</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>
	</xsl:template>

	<!-- group -->
	<xsl:template match="group" mode="runtime">
		<xsl:param name="totype"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="default"/>
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
		<xsl:if test="@name">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t4"/>
			<xsl:text>// </xsl:text>
			<xsl:value-of select="@name"/>
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t4"/>
			<xsl:text>//</xsl:text>
		</xsl:if>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|from|br" mode ="runtime">
			<xsl:with-param name="totype" select="$totype"/>
			<xsl:with-param name="tonullable" select="$tonullable"/>
			<xsl:with-param name="default" select="$defaultcode"/>
		</xsl:apply-templates>
	</xsl:template>

	<!-- default -->
	<xsl:template match="default">
		<xsl:if test="@nullvalue">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t4"/>
			<xsl:text>if (p == null) return </xsl:text>
			<xsl:value-of select="@nullvalue"/>
			<xsl:text>;</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>
		<xsl:if test="not(@noruntime) and @nullvalue">
			<xsl:apply-templates select="../from|../group|../br" mode="runtime">
				<xsl:with-param name="totype" select="../@type"/>
				<xsl:with-param name="tonullable" select="../@tonullable='true'"/>
			</xsl:apply-templates>
		</xsl:if>
		<xsl:value-of select="text()"/>
		<xsl:if test="not(@nothrow) and @nullvalue">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t4"/>
			<xsl:text>throw new InvalidCastException(string.Format(</xsl:text>
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t5"/>
			<xsl:text>"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));</xsl:text>
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

	<xsl:template match="comment" mode ="def">
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
	<xsl:template name="writeCode">
		<xsl:param name="code"/>
		<xsl:choose>
			<xsl:when test="contains($code, '&#13;')">
				<!-- multi-line -->
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
				<xsl:text>{</xsl:text>
				<xsl:value-of select="$code"/>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
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
