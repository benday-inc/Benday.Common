$currentDir = Get-Location

$sourceTemplateFile = "IEnumerableAssertionExtensions.cs"
$sourceReplacementToken = "IEnumerable"

$sourceCodeDir = Join-Path $currentDir "Benday.Common.Testing"

# Ensure the source code directory exists
if (-not (Test-Path $sourceCodeDir)) {
    Write-Host "Source code directory $sourceCodeDir does not exist. Exiting."
    exit 1
}

# verify that source template file exists in source dir
$sourceTemplateFilePath = Join-Path $sourceCodeDir $sourceTemplateFile
if (-not (Test-Path $sourceTemplateFilePath)) {
    Write-Host "Source template file $sourceTemplateFilePath does not exist. Exiting."
    exit 1
}


function CopyAndReplaceToken {
    param(
        [string]$WorkingDir,
        [string]$SourceFile,
        [string]$TargetFile,
        [string]$TargetReplacementToken,
        [string]$SourceReplacementToken,
        [switch]$skipCopy = $false
    )
    # write skipcopy value to console
    Write-Host "Skip copy: $skipCopy"
    
    $targetFilePath = Join-Path $WorkingDir $TargetFile

    if ($true -eq $skipCopy) {
        # if target file doesn't exist, throw an error
        if (-not (Test-Path $targetFilePath)) {
            Write-Host "Target file $targetFilePath does not exist. Exiting."
            exit 1
        }
        
        Write-Host "Skipping copy for $TargetFile..."
    }
    else {
        Write-Host "Copying $SourceFile to $TargetFile..."
        Copy-Item -Path $SourceFile -Destination $targetFilePath -Force
    }
    
    # verify that the target file was copied
    if (-not (Test-Path $targetFilePath)) {
        Write-Host "Target file $targetFilePath could not be created. Exiting."
        exit 1
    }

    # Replace the token in the copied file
    slnutil replacetoken `
        /filename:$targetFilePath `
        /token:"$SourceReplacementToken" `
        /value:"$TargetReplacementToken"
    
        Write-Host "Replaced token '$SourceReplacementToken' with '$TargetReplacementToken' in $TargetFile"
    Write-Host "Updated $TargetFile successfully."
    Write-Host "--------------------------------------------------"
    Write-Host ""
}


CopyAndReplaceToken `
    -WorkingDir $sourceCodeDir `
    -SourceFile $sourceTemplateFilePath `
    -TargetFile "ArrayAssertionExtensions.cs" `
    -SourceReplacementToken "<IEnumerable<T>>" `
    -TargetReplacementToken "<T[]>" `
    
CopyAndReplaceToken `
    -WorkingDir $sourceCodeDir `
    -SourceFile $sourceTemplateFilePath `
    -TargetFile "ArrayAssertionExtensions.cs" `
    -SourceReplacementToken "IEnumerableAssertionExtensions" `
    -TargetReplacementToken "ArrayAssertionExtensions" `
    -skipCopy

CopyAndReplaceToken `
    -WorkingDir $sourceCodeDir `
    -SourceFile $sourceTemplateFilePath `
    -TargetFile "ArrayAssertionExtensions.cs" `
    -SourceReplacementToken "ICheckCollectionAssertion" `
    -TargetReplacementToken "ICheckArrayAssertion" `
    -skipCopy

CopyAndReplaceToken `
    -WorkingDir $sourceCodeDir `
    -SourceFile $sourceTemplateFilePath `
    -TargetFile "ArrayAssertionExtensions.cs" `
    -SourceReplacementToken "ICheckArrayAssertion<IEnumerable<T?>>" `
    -TargetReplacementToken "ICheckArrayAssertion<T[]>" `
    -skipCopy
   

CopyAndReplaceToken `
    -WorkingDir $sourceCodeDir `
    -SourceFile $sourceTemplateFilePath `
    -TargetFile "IListAssertionExtensions.cs" `
    -TargetReplacementToken "IList" `
    -SourceReplacementToken "IEnumerable"

CopyAndReplaceToken `
    -WorkingDir $sourceCodeDir `
    -SourceFile $sourceTemplateFilePath `
    -TargetFile "ListAssertionExtensions.cs" `
    -TargetReplacementToken "List" `
    -SourceReplacementToken "IEnumerable"



