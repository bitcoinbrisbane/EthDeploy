cd ~/EthDeploy
dotnet restore
dotnet publish -c Release -o /var/wwwwroot

cp EthDeploy.conf /var/wwwwroot