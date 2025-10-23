$ErrorActionPreference = "Stop"
$PSDefaultParameterValues['*:Encoding'] = 'utf8'

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
