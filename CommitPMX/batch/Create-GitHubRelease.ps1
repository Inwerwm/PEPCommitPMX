if (!(Test-Path Licenses)) {
    New-Item -Path Licenses -ItemType Directory
}
Copy-Item -Path ..\..\..\..\LICENSE  -Destination Licenses/CommitPMX
Copy-Item -Path ..\..\..\batch\Licenses\SevenZipSharp  -Destination Licenses/SevenZipSharp
Copy-Item -Path ..\..\..\batch\Licenses\7-Zip  -Destination Licenses/7-Zip
Compress-Archive -Path 7z.cdll, CommitPMX.dll, CommitPMX.dll.config, Newtonsoft.Json.dll, PEPExtensions.dll, SevenZipSharp.dll, ..\..\..\..\README.html, ..\..\..\batch\ê‡ñæèë.html, Licenses -DestinationPath release.zip -Force