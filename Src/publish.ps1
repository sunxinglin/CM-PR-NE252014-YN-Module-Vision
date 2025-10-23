$ErrorActionPreference = "Stop"
$PSDefaultParameterValues['*:Encoding'] = 'utf8'


$dateFormatted = (Get-Date).ToString("yyyyMMddHHmmss")

$DIST_Client = "../dist/${dateFormatted}/TApp"

$Release_Client = "../release//${dateFormatted}/TApp"

$Release_FILE = "../release/${dateFormatted}.zip"


Get-ChildItem -Path . -Directory -Filter "bin" -Recurse | ForEach-Object { 
    Remove-Item -Path $_.FullName -Recurse -Force 
}

Get-ChildItem -Path . -Directory -Filter "obj" -Recurse | ForEach-Object { 
    Remove-Item -Path $_.FullName -Recurse -Force 
}

Get-ChildItem -Path . -Directory -Filter "paket-files" | ForEach-Object { 
    Remove-Item -Path $_.FullName -Recurse -Force 
}

dotnet restore
if ($LASTEXITCODE -ne 0) {
    throw ;
}


dotnet build 
if ($LASTEXITCODE -ne 0) {
    throw ;
}

dotnet publish .\TApp\TApp.csproj -c Release -f net472 --runtime win-x64 --self-contained false -o $DIST_Client
if ($LASTEXITCODE -ne 0) {
    throw ;
}

Copy-Item -Path $DIST_Client -Destination $Release_Client -Recurse

Remove-Item -Path "../dist" -Recurse
Remove-Item -Path "$Release_Client/appsettings.json"

Compress-Archive -Path $Release_Client  -DestinationPath $Release_FILE

Write-Host "app is published: " -NoNewline -ForegroundColor Green
Write-Host $Release_FILE -ForegroundColor Blue

