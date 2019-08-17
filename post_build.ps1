param(
    [String]$TargetName = $(throw "-TargetName is required."), 
    [String]$TargetPath = $(throw "-TargetPath is required."), 
    [String]$TargetDir = $(throw "-TargetDir is required."),
    [String]$ProjectDir = $(throw "-ProjectDir is required."),
    [String]$SolutionDir = $(throw "-SolutionDir is required."),
    [String]$ConfigurationName = $(throw "-ConfigurationName is required."),
    [String]$LibNames,
    [Switch]$LoadLib #Deprecated: Use above more generic parameter with argument "XLShredLib"
)

$configContent = Get-Content ([io.path]::combine($SolutionDir, "config.json")) | ConvertFrom-Json;
$infoContent = Get-Content ([io.path]::combine($ProjectDir, 'Resources\Info.json')) | ConvertFrom-Json;

$gameDirectory = $configContent.game_directory;
$targetModName = $infoContent.Id;
$modTargetDir = [io.path]::combine( $gameDirectory, "Mods\", $targetModName);
$modTargetDirTmpParent = $modTargetDir + 'Tmp'
$modTargetDirTmp = $modTargetDirTmpParent + '\' + $targetModName;

Get-ChildItem $modTargetDir -Recurse -Exclude 'Settings.xml', 'CustomObjects', 'mainmenu' | Remove-Item;

New-Item -ItemType directory -Path $modTargetDir -Force;

copy-item $TargetPath $modTargetDir -force;
if ($ConfigurationName -eq "Debug") {
    copy-item ([io.path]::combine( $TargetDir, $TargetName + ".pdb" )) $modTargetDir -force;
}
copy-item ([io.path]::combine( $ProjectDir, "Resources\Info.json" )) $modTargetDir -force;

$libNamesArray = $libNames -split ',';
foreach ($libName in $libNamesArray) {
    copy-item ([io.path]::combine( $TargetDir, $libName + ".dll" )) $modTargetDir  -force;
    if ($ConfigurationName -eq "Debug") {
        copy-item ([io.path]::combine( $TargetDir, $libName + ".pdb" )) $modTargetDir -force;
    }
}

#Deprecated; will be deleted at some point
If ($LoadLib) {
    copy-item ([io.path]::combine( $TargetDir, "XLShredLib.dll" )) $modTargetDir  -force;
    if ($ConfigurationName -eq "Debug") {
        copy-item ([io.path]::combine( $TargetDir, "XLShredLib.pdb" )) $modTargetDir -force;
    }
}

if ($ConfigurationName -eq "Release") {
    copy-item -Recurse -Force $modTargetDir $modTargetDirTmp -Exclude Settings.xml;
    Compress-Archive -Force -Path $modTargetDirTmp -DestinationPath ([io.path]::combine($gameDirectory, "Mods\" + $TargetName + '-' + $infoContent.version + '.zip' ));
    Remove-Item -Recurse -Force $modTargetDirTmpParent; 
}