Push-Location
cd "C:\Users\c-secrej\Desktop\Dungeon\source\ReadableThings\ReadableThings\bin\debug"
Import-Module .\ReadableThings.dll
Pop-Location

function look
{
    Write-Host "There is nothing to see here." 
}

$defaultRoom = New-Object Ogresoft.Thing("obsure place")

if ($you -eq $null)  {
    $you = New-Object Ogresoft.Thing($env:USERNAME); 
}

$you.Move($defaultRoom)

function create
{
    $description = ""
    $args | foreach { $description = $description + " " + $_ }

    if ([string]::IsNullOrEmpty($description)) { 
        Write-Host "What is it that you wish to create?"
        return $null;
    }

    $thing = New-Object Ogresoft.Thing($description)

    if ($you -eq $null)  {
        $you = New-Object Ogresoft.Thing($env:USERNAME);     
    }

    $thing.Move($you); 

    return $thing 
}

function inventory {
    Write-Host $you.Inventory.Description
}