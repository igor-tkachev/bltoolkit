$categoryName = "ODP.NET, Managed Driver"
try{
$categoryExists_unreg = [System.Diagnostics.PerformanceCounterCategory]::Exists($categoryName)
if($categoryExists_unreg)
{
[System.Diagnostics.PerformanceCounterCategory]::Delete($categoryName) | out-null
}
write-host("$categoryName Performance Counter was un-registered successfullly.")
}
catch{
write-host("ERROR: $categoryName Performance Counter un-registration failed.")
}
