$configContent = Get-Content "./config.json" | ConvertFrom-Json;

$steamExecutable = $configContent.steam_executable;

Start-Process -FilePath $steamExecutable -ArgumentList "-applaunch 962730";