<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Counters.aspx.cs" Inherits="Admin_Counters" Title="Counters" %>
<%@ Import Namespace="BLToolkit.Aspects" %>

<asp:Content ID="Content" ContentPlaceHolderID="cph" Runat="Server">

<table class="infoTable" cellspacing="0" cellpadding="0" border="0">
<tr>
<td style="text-align:left">GC.CollectionCount:</td><td><%= GC.CollectionCount(GC.MaxGeneration) %></td>
<td style="text-align:left;padding-left:20;">Cache Cleanup Times:</td><td><%= CacheAspect.CleanupThread.WorkTimes %></td>
<td style="text-align:left;padding-left:20;">Objects in Cache:</td><td><%= CacheAspect.CleanupThread.ObjectsInCache %></td>
</tr>

<tr>
<td style="text-align:left">GC.TotalMemory:</td><td><%= GC.GetTotalMemory(false)/(1024*1024) %>M</td>
<td style="text-align:left;padding-left:20;">Total Cleanup Time:</td><td><%= CacheAspect.CleanupThread.WorkTime %></td>
<td style="text-align:left;padding-left:20;">Objects Expired:</td><td><%= CacheAspect.CleanupThread.ObjectsExpired %></td>
</tr>
</table>
<br/>

<asp:Repeater ID="counterRepeater" runat="server"  EnableViewState="false">
<HeaderTemplate>
<table class="grid" cellspacing="0" cellpadding="0"  rules="all" border="1" style="width:100%;border-collapse:collapse">
<tr class="grid-header">
<th rowspan="2">Type</th><th rowspan="2">Method</th><th colspan="2">Calls</th><th colspan="2">Cache</th><th colspan="4">Call Time</th>
</tr>
<tr class="grid-header">
<th>Count</th><th>Ex</th><th>In</th><th>From</th><th>Min</th><th>Average</th><th>Max</th><th>Total</th>
</tr>
</HeaderTemplate>
<ItemTemplate><tr>
<td><%# GetName(((MethodCallCounter)Container.DataItem).MethodInfo.DeclaringType) %></td>
<td><%# ((MethodCallCounter)Container.DataItem).MethodInfo.Name               %></td>
<td align="right"><%# ((MethodCallCounter)Container.DataItem).TotalCount      %></td>
<td><%# ((MethodCallCounter)Container.DataItem).ExceptionCount == 0? "": ((MethodCallCounter)Container.DataItem).ExceptionCount.ToString() %></td>
<td align="right"><%# ((MethodCallCounter)Container.DataItem).CallMethodInfo.CacheAspect != null? ((MethodCallCounter)Container.DataItem).CallMethodInfo.CacheAspect.Cache.Count: 0 %></td>
<td align="right"><%# ((MethodCallCounter)Container.DataItem).CachedCount     %></td>
<td><%# GetTime(((MethodCallCounter)Container.DataItem).MinTime)              %></td>
<td><%# GetTime(((MethodCallCounter)Container.DataItem).AverageTime)          %></td>
<td><%# GetTime(((MethodCallCounter)Container.DataItem).MaxTime)              %></td>
<td><%# GetTime(((MethodCallCounter)Container.DataItem).TotalTime)            %></td>
</tr><%# GetCurrent((MethodCallCounter)Container.DataItem) %></ItemTemplate>
<SeparatorTemplate>
</SeparatorTemplate>
<FooterTemplate></table></FooterTemplate>
</asp:Repeater>

<br/>
<font size="1">
<a href="Counters.aspx?cleanup=1">cache cleanup</a>
</font>

</asp:Content>
