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
	<xsl:variable name="baseclassname"  select="'MB'"/>
	<xsl:variable name="nullableprefix" select="'N'"/>
	<xsl:variable name="instancename"   select="'I'"/>
	<xsl:variable name="getmethodspec"  select="'Get(IMapDataSource s, object o, int i'"/>
	<xsl:variable name="setmethodspec"  select="'Set(IMapDataDestination d, object o, int i'"/>
	<xsl:variable name="padding"        select="string-length('SqlDateTime') + 1"/>

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

		<xsl:value-of select="$t2"/>
		<xsl:text>static ValueMapping()</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:apply-templates select="group|comment|br" mode="def">
			<xsl:with-param name="mode" select="'reg'"/>
		</xsl:apply-templates>
		<xsl:value-of select="$t2"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>

		<xsl:apply-templates select="group|comment|br" mode="def">
			<xsl:with-param name="mode" select="'mapper'"/>
		</xsl:apply-templates>
		<xsl:call-template name="getmappers"/>
		<xsl:value-of select="$lf"/>
		<xsl:call-template name="setmappers"/>
		<xsl:value-of select="$t1"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template name="getmappers">
		<xsl:value-of select="$t2"/>
		<xsl:text>static class GetData&lt;T&gt;</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>public abstract class </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;Q&gt;</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t4"/>
		<xsl:text>public abstract Q </xsl:text>
		<xsl:value-of select="$getmethodspec"/>
		<xsl:text>);</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>static readonly </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T&gt; </xsl:text>
		<xsl:value-of select="$instancename"/>
		<xsl:text> = GetGetter();</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>static </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T&gt; GetGetter()</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t4"/>
		<xsl:text>Type t = typeof(T);</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|comment|br" mode="body"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t4"/>
		<xsl:text>return null;</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|comment|br" mode="def">
			<xsl:with-param name="mode" select="'getter'"/>
		</xsl:apply-templates>
		<xsl:value-of select="$t2"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template name="setmappers">
		<xsl:value-of select="$t2"/>
		<xsl:text>static class SetData&lt;T&gt;</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t2"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>public abstract class </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;Q&gt;</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t4"/>
		<xsl:text>public abstract void </xsl:text>
		<xsl:value-of select="$setmethodspec"/>
		<xsl:text>, Q v);</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>static readonly </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T&gt; </xsl:text>
		<xsl:value-of select="$instancename"/>
		<xsl:text> = GetSetter();</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>static </xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T&gt; GetSetter()</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>{</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t4"/>
		<xsl:text>Type t = typeof(T);</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|comment|br" mode="body"/>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t4"/>
		<xsl:text>return null;</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$t3"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
		<xsl:value-of select="$lf"/>
		<xsl:apply-templates select="group|comment|br" mode="def">
			<xsl:with-param name="mode" select="'setter'"/>
		</xsl:apply-templates>
		<xsl:value-of select="$t2"/>
		<xsl:text>}</xsl:text>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<!-- group -->
	<xsl:template match="group" mode="body">
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
		<xsl:apply-templates select="group|type|comment|br|include" mode="body">
			<xsl:with-param name="nullable" select="@nullable='true'"/>
			<xsl:with-param name="group"    select="."/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="group" mode="def">
		<xsl:param name="mode"/>

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
		<xsl:apply-templates select="group|type|comment|br|include" mode="def">
			<xsl:with-param name="nullable" select="@nullable='true'"/>
			<xsl:with-param name="group"    select="."/>
			<xsl:with-param name="mode"     select="$mode"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template name="includedef">
		<xsl:param name="nullable"/>
		<xsl:param name="group"/>
		<xsl:param name="template"/>
		<xsl:param name="mode"/>

		<xsl:for-each select="/code/template[@name=$template]/*">
			<xsl:choose>
				<xsl:when test="name()='type'">
					<xsl:variable name ="name" select="@name"/>
					<xsl:if test="not($group/type[@name=$name])">
						<xsl:call-template name="typedef">
							<xsl:with-param name="nick">
								<xsl:if test="$nullable">
									<xsl:value-of select="$nullableprefix"/>
								</xsl:if>
								<xsl:choose>
									<xsl:when test="/code/nick[@type=$name]">
										<xsl:value-of select="/code/nick[@type=$name]/@name"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$name"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:with-param>
							<xsl:with-param name="type"     select="$name"/>
							<xsl:with-param name="nullable" select="$nullable"/>
							<xsl:with-param name="mode"     select="$mode"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:when>
				<xsl:when test="name()='include'">
					<xsl:call-template name="includedef">
						<xsl:with-param name="nullable" select="$nullable"/>
						<xsl:with-param name="group"    select="$group"/>
						<xsl:with-param name="template" select="@template"/>
						<xsl:with-param name="mode"     select="$mode"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="name()='br'">
					<xsl:value-of select="$lf"/>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="include" mode="def">
		<xsl:param name="nullable"/>
		<xsl:param name="group"/>
		<xsl:param name="mode"/>
		
		<xsl:call-template name="includedef">
			<xsl:with-param name="nullable" select="$nullable"/>
			<xsl:with-param name="group"    select="$group"/>
			<xsl:with-param name="template" select="@template"/>
			<xsl:with-param name="mode"     select="$mode"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="includebody">
		<xsl:param name="nullable"/>
		<xsl:param name="group"/>
		<xsl:param name="template"/>
		<xsl:for-each select="/code/template[@name=$template]/*">
			<xsl:choose>
				<xsl:when test="name()='type'">
					<xsl:variable name ="name" select="@name"/>
					<xsl:if test="not($group/type[@name=$name])">
						<xsl:call-template name="typebody">
							<xsl:with-param name="nick">
								<xsl:if test="$nullable">
									<xsl:value-of select="$nullableprefix"/>
								</xsl:if>
								<xsl:choose>
									<xsl:when test="/code/nick[@type=$name]">
										<xsl:value-of select="/code/nick[@type=$name]/@name"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$name"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:with-param>
							<xsl:with-param name="type"     select="$name"/>
							<xsl:with-param name="nullable" select="$nullable"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:when>
				<xsl:when test="name()='include'">
					<xsl:call-template name="includebody">
						<xsl:with-param name="nullable" select="$nullable"/>
						<xsl:with-param name="group"    select="$group"/>
						<xsl:with-param name="template" select="@template"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="name()='br'">
					<xsl:value-of select="$lf"/>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="include" mode="body">
		<xsl:param name="nullable"/>
		<xsl:param name="group"/>
		<xsl:call-template name="includebody">
			<xsl:with-param name="nullable" select="$nullable"/>
			<xsl:with-param name="group"    select="$group"/>
			<xsl:with-param name="template" select="@template"/>
		</xsl:call-template>
	</xsl:template>

	<!-- type -->
	<xsl:template match="type" mode="def">
		<xsl:param name="nullable"/>
		<xsl:param name="mode"/>

		<xsl:variable name="name" select="@name"/>
		<xsl:call-template name="typedef">
			<xsl:with-param name="nick">
				<xsl:if test="@nullable">
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
			<xsl:with-param name="type"     select="@name"/>
			<xsl:with-param name="nullable" select="@nullable='true' or $nullable"/>
			<xsl:with-param name="mode"     select="$mode"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="typedef">
		<xsl:param name="nick"/>
		<xsl:param name="type"/>
		<xsl:param name="nullable"/>
		<xsl:param name="mode"/>

		<xsl:variable name="fulltype">
			<xsl:value-of select="$type"/>
			<xsl:if test="$nullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="longtype">
			<xsl:if test="$nullable">
				<xsl:text>Nullable</xsl:text>
			</xsl:if>
			<xsl:value-of select="$type"/>
		</xsl:variable>

		<xsl:variable name="typepad" select="$padding - string-length($fulltype)"/>

		<xsl:value-of select="$t3"/>
		<xsl:choose>
			<xsl:when test="$mode='getter' or $mode='setter'">
				<xsl:text>sealed class </xsl:text>
				<xsl:value-of select="$nick"/>
				<xsl:call-template name ="writeSpaces">
					<xsl:with-param name="count" select="$padding - string-length($nick)"/>
				</xsl:call-template>
				<xsl:text>: </xsl:text>
				<xsl:value-of select="$baseclassname"/>
				<xsl:text>&lt;</xsl:text>
				<xsl:value-of select="$fulltype"/>
				<xsl:text>&gt;</xsl:text>
				<xsl:call-template name ="writeSpaces">
					<xsl:with-param name="count" select="$typepad"/>
				</xsl:call-template>
				<xsl:text>{ public override </xsl:text>
				<xsl:choose>
					<xsl:when test="$mode='getter'">
						<xsl:value-of select="$fulltype"/>
						<xsl:call-template name ="writeSpaces">
							<xsl:with-param name="count" select="$typepad"/>
						</xsl:call-template>
						<xsl:value-of select="$getmethodspec"/>
						<xsl:text>) { return s.Get</xsl:text>
						<xsl:value-of select="$longtype"/>
						<xsl:call-template name ="writeSpaces">
							<xsl:with-param name="count" select="$typepad"/>
						</xsl:call-template>
						<xsl:text>(o, i); }</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text> void </xsl:text>
						<xsl:value-of select="$setmethodspec"/>
						<xsl:text>, </xsl:text>
						<xsl:value-of select="$fulltype"/>
						<xsl:call-template name ="writeSpaces">
							<xsl:with-param name="count" select="$typepad"/>
						</xsl:call-template>
						<xsl:text>v) { d.Set</xsl:text>
						<xsl:value-of select="$longtype"/>
						<xsl:call-template name ="writeSpaces">
							<xsl:with-param name="count" select="$typepad"/>
						</xsl:call-template>
						<xsl:text>(o, i, v); }</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text> }</xsl:text>
			</xsl:when>
			<xsl:when test="$mode='mapper'">
				<xsl:text>sealed class </xsl:text>
				<xsl:value-of select="$nick"/>
				<xsl:text>To</xsl:text>
				<xsl:value-of select="$nick"/>
				<xsl:text> : IValueMapper</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
				<xsl:text>{</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t4"/>
				<xsl:text>public void Map(</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t5"/>
				<xsl:text>IMapDataSource      source, object sourceObject, int sourceIndex,</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t5"/>
				<xsl:text>IMapDataDestination dest,   object destObject,   int destIndex)</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t4"/>
				<xsl:text>{</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t5"/>
				<xsl:text>if (source.IsNull(sourceObject, sourceIndex))</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t6"/>
				<xsl:text>dest.SetNull(destObject, destIndex);</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t5"/>
				<xsl:text>else</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t6"/>
				<xsl:text>dest.Set</xsl:text>
				<xsl:value-of select="$longtype"/>
				<xsl:text>(destObject, destIndex,</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t7"/>
				<xsl:text>source.Get</xsl:text>
				<xsl:value-of select="$longtype"/>
				<xsl:text>(sourceObject, sourceIndex));</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t4"/>
				<xsl:text>}</xsl:text>
				<xsl:value-of select="$lf"/>
				<xsl:value-of select="$t3"/>
				<xsl:text>}</xsl:text>
				<xsl:value-of select="$lf"/>
			</xsl:when>
			<xsl:when test="$mode='reg'">
				<xsl:text>AddSameType(typeof(</xsl:text>
				<xsl:value-of select="$fulltype"/>
				<xsl:text>),</xsl:text>
				<xsl:call-template name ="writeSpaces">
					<xsl:with-param name="count" select="$typepad"/>
				</xsl:call-template>
				<xsl:text>new </xsl:text>
				<xsl:value-of select="$nick"/>
				<xsl:text>To</xsl:text>
				<xsl:value-of select="$nick"/>
				<xsl:text>());</xsl:text>

			</xsl:when>
		</xsl:choose>
		<xsl:value-of select="$lf"/>
	</xsl:template>

	<xsl:template match="type" mode="body">
		<xsl:param name="nullable"/>

		<xsl:variable name ="name" select="@name"/>
		
		<xsl:call-template name="typebody">
			<xsl:with-param name="nick">
				<xsl:if test="$nullable">
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
			<xsl:with-param name="nullable" select="@nullable='true' or $nullable"/>
			<xsl:with-param name="type"   select="$name"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="typebody">
		<xsl:param name="nick"/>
		<xsl:param name="type"/>
		<xsl:param name="nullable"/>
		<xsl:variable name="fulltype">
			<xsl:value-of select="$type"/>
			<xsl:if test="$nullable">
				<xsl:text>?</xsl:text>
			</xsl:if>
		</xsl:variable>

		<xsl:value-of select="$t4"/>
		<xsl:text>if (t == typeof(</xsl:text>
		<xsl:value-of select="$fulltype"/>
		<xsl:text>)) </xsl:text>
		<xsl:call-template name ="writeSpaces">
			<xsl:with-param name="count" select="$padding - string-length($fulltype)"/>
		</xsl:call-template>
		<xsl:text>return (</xsl:text>
		<xsl:value-of select="$baseclassname"/>
		<xsl:text>&lt;T&gt;)(object)(new </xsl:text>
		<xsl:value-of select="$nick"/>
		<xsl:text>());</xsl:text>
		<xsl:value-of select="$lf"/>
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
