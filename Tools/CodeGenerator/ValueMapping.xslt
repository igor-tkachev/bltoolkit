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
	<xsl:variable name="baseclassname"  select="'MB'"/>
	<xsl:variable name="nullableprefix" select="'N'"/>
	<xsl:variable name="instancename"   select="'I'"/>
	<xsl:variable name="getmethodspec"  select="'Get(IMapDataSource s, object o, int i'"/>
	<xsl:variable name="setmethodspec"  select="'Set(IMapDataDestination d, object o, int i'"/>
	<xsl:variable name="padding"        select="string-length('DateTimeOffset?') + 1"/>

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
		<xsl:text>public static partial class ValueMapping</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t1"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:apply-templates select="group|comment|br" mode="to">
			<xsl:with-param name="mode" select="'select'"/>
		</xsl:apply-templates>
		
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- group -->
	<xsl:template match="group" mode="to">
		
		<xsl:param name="mode"/>

		<xsl:if test="@nullable='true'">
			<xsl:text>#if FW2</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

		<xsl:if test="@name">
			<xsl:value-of select="$t2"/>
			<xsl:text>#region </xsl:text>
			<xsl:value-of select="@name"/>
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$lf"/>
		</xsl:if>
		
		<xsl:apply-templates select="type|comment|br" mode="to">
			<xsl:with-param name="tonullable" select="@nullable='true'"/>
			<xsl:with-param name="mode"       select="$mode"/>
		</xsl:apply-templates>

		<xsl:if test="@name">
			<xsl:value-of select="$lf"/>
			<xsl:value-of select="$t2"/>
			<xsl:text>#endregion </xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

		<xsl:if test="@nullable='true'">
			<xsl:text>#endif</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

	</xsl:template>

	<xsl:template match="group" mode="from">
		<xsl:param name="tonick"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonullable"/>

		<xsl:param name="mode"/>

		<xsl:if test="@nullable='true'">
			<xsl:text>#if FW2</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

		<xsl:apply-templates select="type|comment|br" mode="from">
			<xsl:with-param name="fromnullable" select="@nullable='true'"/>
			<xsl:with-param name="tonick"       select="$tonick"/>
			<xsl:with-param name="totype"       select="$totype"/>
			<xsl:with-param name="tonullable"   select="$tonullable"/>
			<xsl:with-param name="mode"         select="$mode"/>
		</xsl:apply-templates>

		<xsl:if test="@nullable='true'">
			<xsl:text>#endif</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

	</xsl:template>

	<!-- type -->
	<xsl:template match="type" mode="to">
		
		<xsl:param name="tonullable"/>
		<xsl:param name="mode"/>

		<xsl:variable name="name" select="@name"/>
		<xsl:variable name="tonullablelocal" select="@nullable='true' or $tonullable"/>
		<xsl:variable name="tonick">
			<xsl:if test="$tonullablelocal">
				<xsl:value-of select="$nullableprefix"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="@nick">
					<xsl:value-of select="@nick"/>
				</xsl:when>
				<xsl:when test="/code/nick[@type=$name]">
					<xsl:value-of select="/code/nick[@type=$name]/@name"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$name"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

	<xsl:if test="$tonullablelocal">
			<xsl:text>#if FW2</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>private static IValueMapper Get</xsl:text>
		<xsl:value-of select="$tonick"/>
		<xsl:text>Mapper(Type t)</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:apply-templates select="/code/group|/code/comment|/code/br" mode="from">
			<xsl:with-param name="tonick"     select="$tonick"/>
			<xsl:with-param name="totype"     select="@name"/>
			<xsl:with-param name="tonullable" select="$tonullablelocal"/>
			<xsl:with-param name="mode"       select="$mode"/>
		</xsl:apply-templates>

		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>return null;</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:value-of select="$t2"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:if test="$tonullablelocal">
			<xsl:text>#endif</xsl:text>
			<xsl:value-of select="$lf"/>
		</xsl:if>

	</xsl:template>


	<xsl:template match="type" mode="from">

		<xsl:param name="fromnullable"/>
		<xsl:param name="tonick"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="mode"/>

		<xsl:variable name="name" select="@name"/>
		<xsl:variable name="fromnullablelocal" select="@nullable='true' or $fromnullable"/>

		<xsl:call-template name="generate">
			<xsl:with-param name="totype"     select="$totype"/>
			<xsl:with-param name="tonullable" select="$tonullable"/>
			<xsl:with-param name="tonick"     select="$tonick"/>
			<xsl:with-param name="fromnick">
				<xsl:if test="$fromnullablelocal">
					<xsl:value-of select="$nullableprefix"/>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="@nick">
						<xsl:value-of select="@nick"/>
					</xsl:when>
					<xsl:when test="/code/nick[@type=$name]">
						<xsl:value-of select="/code/nick[@type=$name]/@name"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$name"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
			<xsl:with-param name="fromtype"     select="@name"/>
			<xsl:with-param name="fromnullable" select="$fromnullablelocal"/>
			<xsl:with-param name="mode"         select="$mode"/>
		</xsl:call-template>

	</xsl:template>

	<xsl:template name="generate">
		<xsl:param name="tonick"/>
		<xsl:param name="totype"/>
		<xsl:param name="tonullable"/>
		<xsl:param name="fromnick"/>
		<xsl:param name="fromtype"/>
		<xsl:param name="fromnullable"/>
		<xsl:param name="mode"/>

		<xsl:variable name="tofulltype">
			<xsl:value-of select="$totype"/>
			<xsl:if test="$tonullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="tolongtype">
			<xsl:if test="$tonullable">
				<xsl:text>Nullable</xsl:text>
			</xsl:if>
			<xsl:value-of select="$totype"/>
		</xsl:variable>
		<xsl:variable name="totypepad" select="$padding - string-length($tofulltype)"/>

		<xsl:variable name="fromfulltype">
			<xsl:value-of select="$fromtype"/>
			<xsl:if test="$fromnullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="fromlongtype">
			<xsl:if test="$fromnullable">
				<xsl:text>Nullable</xsl:text>
			</xsl:if>
			<xsl:value-of select="$fromtype"/>
		</xsl:variable>
		<xsl:variable name="fromtypepad" select="$padding - string-length($fromfulltype)"/>

		<xsl:choose>
			<xsl:when test="$mode='mapper'">
				<xsl:value-of select="$t1"/>
				<xsl:text>internal sealed class </xsl:text>
				<xsl:value-of select="$fromnick"/>
				<xsl:text>To</xsl:text>
				<xsl:value-of select="$tonick"/>
				<xsl:text> : IValueMapper</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t1"/>
				<xsl:text>{</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t2"/>
				<xsl:text>public void Map(</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
				<xsl:text>IMapDataSource      source, object sourceObject, int sourceIndex,</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
				<xsl:text>IMapDataDestination dest,   object destObject,   int destIndex)</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t2"/>
				<xsl:text>{</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
				<xsl:text>if (source.IsNull(sourceObject, sourceIndex))</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t4"/>
				<xsl:text>dest.SetNull(destObject, destIndex);</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
				<xsl:text>else</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t4"/>
				<xsl:text>dest.Set</xsl:text>
				<xsl:value-of select="$tolongtype"/>
				<xsl:text>(destObject, destIndex,</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t5"/>

				<xsl:if test="not($tonick=$fromnick)">
					<xsl:text>Convert.To</xsl:text>
					<xsl:value-of select="$tolongtype"/>
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$lf"/>
					<xsl:value-of select="$t6"/>
				</xsl:if>
				
				<xsl:text>source.Get</xsl:text>
				<xsl:value-of select="$fromlongtype"/>
				<xsl:text>(sourceObject, sourceIndex))</xsl:text>

				<xsl:if test="not($tonick=$fromnick)">
					<xsl:text>)</xsl:text>
				</xsl:if>
				
				<xsl:text>;</xsl:text>

				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t2"/>
				<xsl:text>}</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t1"/>
				<xsl:text>}</xsl:text>
				<xsl:value-of select="$lf"/>
			</xsl:when>
			<xsl:when test="$mode='select'">
				<xsl:value-of select="$t3"/>
				<xsl:text>if (t == typeof(</xsl:text>
				<xsl:value-of select="$fromfulltype"/>
				<xsl:text>))</xsl:text>
				<xsl:call-template name ="writeSpaces">
					<xsl:with-param name="count" select="$fromtypepad"/>
				</xsl:call-template>
				<xsl:text>return new </xsl:text>
				<xsl:value-of select="$fromnick"/>
				<xsl:text>To</xsl:text>
				<xsl:value-of select="$tonick"/>
				<xsl:text>();</xsl:text>
				<xsl:value-of select="$lf"/>
			</xsl:when>
		</xsl:choose>
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

	<xsl:template match="comment" mode ="to">
		<xsl:if test="not(@tonullable) or @tonullable!='true'">
			<xsl:call-template name="comment"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="comment" mode ="from">
		<xsl:if test="not(@tonullable) or @tonullable!='true'">
			<xsl:call-template name="comment"/>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="br">
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="br" mode="to">
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="br" mode="from">
		<xsl:value-of select="$lf"/>
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
