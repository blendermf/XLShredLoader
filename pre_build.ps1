param(
    [String]$SolutionDir = $(throw "-SolutionDir is required.")
)

$configContent = Get-Content ([io.path]::combine($SolutionDir, "config.json")) | ConvertFrom-Json;

$gameDirectory = $configContent.game_directory;

New-Item -ItemType directory -Path ([io.path]::combine($SolutionDir , "references\")) -Force;

robocopy ([io.path]::combine( $gameDirectory, "SkaterXL_Data\Managed")) ([io.path]::combine($SolutionDir , "references")) /MIR