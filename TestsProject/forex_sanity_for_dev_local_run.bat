@echo off
set systemType=forex
set _crmUrl=https://dev-brand01-crm.airsoftltd.com
set _tradingPlatformUrl=https://dev-brand01-trade.airsoftltd.com
set mongoDbConnectionString=mongodb+srv://lior:pBZpNSc9HwCLs4Qp@dev01-de-cwsxo.mongodb.net/dev-forex01
set sqlDbConnectionString=server=kube-dev01.chbyylohqgik.eu-west-1.rds.amazonaws.com;database=dev-forex01;user=liordev;Password=xFQ9XCseTLnvzbz6
set mongoBrandDbName=dev-forex01
set apiKey=Pi4XqBAYXapRcd0GLRyG9QwXZakWF5kWRupSlt3uaTQGskxi5pg1598508067037
set remoteWebDriverUrl=https://liorhalaly1:NqBHTmxyFQ7jYumRjZyi@hub-cloud.browserstack.com/wd/hub
dotnet test TestsProject.csproj --filter TestCategory=amit
pause
