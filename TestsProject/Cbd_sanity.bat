@echo off
set systemType=Cbd
set _crmUrl=https://qa-auto01-crm.greentech.software
set mongoDbConnectionString=mongodb+srv://qa-automation:KdmeG2FRvK8ScK3k@kube-cbd01-4imib.mongodb.net
set shopUrl=https://qa-auto01.greentech.software
set dbName=qa-auto01
set apiKey=kiTxpeOJWy9UuNkaYu47vVGau9N9kuJQixVU86bxMwFRWEmz4XK1584698831249
set remoteWebDriverUrl=http://zalenium.airsoftltd.com/wd/hub
dotnet test TestsProject.csproj --filter TestCategory=cbd-sanity
pause