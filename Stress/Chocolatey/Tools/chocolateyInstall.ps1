$packageName = 'Stress.portable' # name for the package, used in messages
$url ="https://github.com/jquintus/Stress/releases/download/v1.0.1/bin.zip"

$installDir = Join-Path $env:AllUsersProfile "$packageName"
Write-Host "Adding `'$installDir`' to the path and the current shell path"
Install-ChocolateyPath "$installDir"
$env:Path = "$($env:Path);$installDir"

Install-ChocolateyZipPackage "$packageName" "$url" "$installDir"
