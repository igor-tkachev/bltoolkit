$categoryName = "ODP.NET, Managed Driver"
try{
$categoryHelp = "$categoryName Performance Counter"
$categoryType = [System.Diagnostics.PerformanceCounterCategoryType]::MultiInstance
$categoryExists_reg = [System.Diagnostics.PerformanceCounterCategory]::Exists($categoryName)
if($categoryExists_reg)
{
[System.Diagnostics.PerformanceCounterCategory]::Delete($categoryName)
}
$counterCreationDataList = New-Object -TypeName System.Diagnostics.CounterCreationDataCollection
$counterCreationDataList.Clear()
$RateOfCountsPerSecond64 = [System.Diagnostics.PerformanceCounterType]::RateOfCountsPerSecond64
$NumberOfItems64 = [System.Diagnostics.PerformanceCounterType]::NumberOfItems64
$counterCreationData1 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'HardConnectsPerSecond', [string]::Empty, $RateOfCountsPerSecond64
$counterCreationData2 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'HardDisconnectsPerSecond', [string]::Empty, $RateOfCountsPerSecond64
$counterCreationData3 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'SoftConnectsPerSecond', [string]::Empty, $RateOfCountsPerSecond64
$counterCreationData4 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'SoftDisconnectsPerSecond', [string]::Empty, $RateOfCountsPerSecond64
$counterCreationData5 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'NumberOfActiveConnectionPools', [string]::Empty, $NumberOfItems64
$counterCreationData6 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'NumberOfInactiveConnectionPools',[string]::Empty, $NumberOfItems64
$counterCreationData7 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'NumberOfActiveConnections', [string]::Empty, $NumberOfItems64
$counterCreationData8 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'NumberOfFreeConnections', [string]::Empty, $NumberOfItems64
$counterCreationData9 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'NumberOfPooledConnections', [string]::Empty, $NumberOfItems64
$counterCreationData10 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'NumberOfNonPooledConnections', [string]::Empty, $NumberOfItems64
$counterCreationData11 = New-Object -TypeName System.Diagnostics.CounterCreationData -ArgumentList 'NumberOfReclaimedConnections', [string]::Empty, $NumberOfItems64
$counterCreationDataList.Add($counterCreationData1) | out-null
$counterCreationDataList.Add($counterCreationData2) | out-null
$counterCreationDataList.Add($counterCreationData3) | out-null
$counterCreationDataList.Add($counterCreationData4) | out-null
$counterCreationDataList.Add($counterCreationData5) | out-null
$counterCreationDataList.Add($counterCreationData6) | out-null
$counterCreationDataList.Add($counterCreationData7) | out-null
$counterCreationDataList.Add($counterCreationData8) | out-null
$counterCreationDataList.Add($counterCreationData9) | out-null
$counterCreationDataList.Add($counterCreationData10) | out-null
$counterCreationDataList.Add($counterCreationData11) | out-null
[System.Diagnostics.PerformanceCounterCategory]::Create($categoryName, $categoryHelp, $categoryType, $counterCreationDataList) | out-null
write-host("$categoryHelp was registered successfullly.")
}
catch{
write-host("ERROR: $categoryHelp registration failed.")
}