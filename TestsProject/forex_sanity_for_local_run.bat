@echo off
set systemType=forex
set crmUrl=https://qa-auto01-crm.airsoftltd.com
set tradingPlatformUrl=https://qa-auto01-trade.airsoftltd.com
set mongoDbConnectionString=mongodb+srv://lior:pBZpNSc9HwCLs4Qp@kube-prod01-pl-1.cwsxo.mongodb.net/qa-automation01
set sqlDbConnectionString=server=kube-prod1.cchhahczwlx4.eu-west-1.rds.amazonaws.com;database=qa-automation01;user=qa-auto01;password=F6RqUku6246hAZUW
set mongoBrandDbName=qa-automation01
set remoteDriverSource=browserstack
set apiKey=eALaphINzxvYyeYhUDsnqUaFkmIB5bkuPR84I3KVHO0OD7uGvFH1628058458053
set remoteWebDriverUrl=https://liorhalaly1:NqBHTmxyFQ7jYumRjZyi@hub-cloud.browserstack.com/wd/hub
dotnet test -v n TestsProject.csproj --configuration Release --filter TestCategory=forex-sanity -- NUnit.NumberOfTestWorkers=5
pause