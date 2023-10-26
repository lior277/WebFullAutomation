@echo off
set systemType=cbd
set crmUrl=https://qa-auto02-crm.greentech.software
set mongoDbConnectionString=mongodb+srv://qa-automation:KdmeG2FRvK8ScK3k@kube-cbd01-4imib.mongodb.net/qa-auto02
set shopUrl=https://qa-auto02.greentech.software
set mongoBrandDbName=qa-auto02
set apiKey=FkUtflVYHYQlPOQLZUtURpTWSccOAVSa6NL5SLAsM84dfPsdSRw1614598385223
set remoteWebDriverUrl=https://liorhalaly1:NqBHTmxyFQ7jYumRjZyi@hub-cloud.browserstack.com/wd/hub
dotnet test TestsProject.csproj --filter TestCategory=cbd-sanity
pause
