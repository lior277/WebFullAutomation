@echo off
set systemType=Forex
set _crmUrl=https://qa-auto01-crm.airsoftltd.com
set mongoDbConnectionString=mongodb+srv://admin:asdewq123@kube-prod01-cwsxo.mongodb.net/qa-automation01?retryWrites=true&w=majority
set _tradingPlatformUrl=https://qa-auto01-trade.airsoftltd.com
set dbName=qa-automation01
set apiKey=2TCYb446wpaL3BkQI0y6EzMEn8slSfPQAwE0d768IoKxiVYCE241583752334015
set remoteWebDriverUrl=http://zalenium.airsoftltd.com/wd/hub
dotnet test TestsProject.csproj --filter TestCategory=forex-sanity
pause
