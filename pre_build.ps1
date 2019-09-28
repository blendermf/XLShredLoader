param(
    [String]$SolutionDir = $(throw "-SolutionDir is required."),
	[String]$GameAssemblyReferences,
	[String]$ModAssemblyReferences
)

$configContent = Get-Content ([io.path]::combine($SolutionDir, "config.json")) | ConvertFrom-Json;

$gameDirectory = $configContent.game_directory;

if ($ModAssemblyReferences.Length -ne 0) {
	$modAssemblyReferencesArray = $ModAssemblyReferences -split ',';
	foreach($modAssemblyReference in $modAssemblyReferencesArray) {
		$assembly = [io.path]::combine($gameDirectory, "Mods\" + $modAssemblyReference);
	
		$refDir = [io.path]::GetDirectoryName($assembly);
		$refFile = [io.path]::GetFileName($assembly);

		robocopy $refDir ([io.path]::combine($SolutionDir , "references")) $refFile /COPY:DAT /XF;
	}
}

$gameAssemblyReferencesArray = "Assembly-CSharp.dll", "Assembly-CSharp-firstpass.dll", "UnityEngine.dll", "UnityEngine.UI.dll", "UnityEngine.CoreModule.dll", "UnityEngine.IMGUIModule.dll", "UnityEngine.PhysicsModule.dll", "UnityEngine.AnimationModule.dll", "Rewired_Core.dll", "UnityEngine.AssetBundleModule.dll";

$gameAssemblyReferencesArray += $GameAssemblyReferences -split ',';

robocopy ([io.path]::combine( $gameDirectory, "SkaterXL_Data\Managed")) ([io.path]::combine($SolutionDir , "references")) @gameAssemblyReferencesArray /COPY:DAT /XF;
robocopy ([io.path]::combine( $gameDirectory, "SkaterXL_Data\Managed\UnityModManager")) ([io.path]::combine($SolutionDir , "references")) "0Harmony12.dll" "UnityModManager.dll" /COPY:DAT /XF;