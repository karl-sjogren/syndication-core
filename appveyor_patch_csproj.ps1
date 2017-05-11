if($env:appveyor_repo_branch -ne "master") {
    $xmlPath = "$env:appveyor_build_folder\src\SyndicationCore\SyndicationCore.csproj"
    $xml = [xml](get-content $xmlPath)
    $propertyGroup = $xml.Project.PropertyGroup
    $propertyGroup.Version = $env:appveyor_build_version
    $xml.Save($xmlPath)
}