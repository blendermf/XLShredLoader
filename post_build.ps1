param(
    [String]$TargetName = $(throw "-TargetName is required."), 
    [String]$TargetPath = $(throw "-TargetPath is required."), 
    [String]$TargetDir = $(throw "-TargetDir is required."),
    [String]$ProjectDir = $(throw "-ProjectDir is required."),
    [String]$SolutionDir = $(throw "-SolutionDir is required."),
    [Switch]$LoadLib
)

$configContent = Get-Content ([io.path]::combine($SolutionDir, "config.json")) | ConvertFrom-Json;

$gameDirectory = $configContent.game_directory;

New-Item -ItemType directory -Path ([io.path]::combine( $gameDirectory, "Mods\", $TargetName + "\" )) -Force;

copy-item $TargetPath ([io.path]::combine( $gameDirectory, "Mods\", $TargetName)) -force;
copy-item ([io.path]::combine( $TargetDir, $TargetName + ".pdb" )) ([io.path]::combine( $gameDirectory, "Mods\", $TargetName )) -force;
copy-item ([io.path]::combine( $ProjectDir, "Resources\Info.json" )) ([io.path]::combine( $gameDirectory, "Mods\", $TargetName )) -force;
If ($LoadLib) {
    copy-item ([io.path]::combine( $TargetDir, "XLShredLib.dll" )) ([io.path]::combine( $gameDirectory, "Mods\", $TargetName )) -force;
    copy-item ([io.path]::combine( $TargetDir, "XLShredLib.pdb" )) ([io.path]::combine( $gameDirectory, "Mods\", $TargetName )) -force;
}