call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"

cd %~dp0

set sln="C:\Develop\CrossPlatformMR\App\UWP\CrossPlatformMR.sln"
set configuration=Release
set platform=x64
set appxDir="C:\Develop\CrossPlatformMR\App\UWP\dist"
set packageCertificateKeyFile="C:\Develop\CrossPlatformMR\App\PackageCertificate.pfx"
set packageCertificatePassword="p@ssword"

MSBuild %sln% ^
    /t:clean;rebuild ^
    /p:Platform=%Platform% ^
    /p:Configuration=%configuration% ^
    /p:AppxBundlePlatforms=%platform% ^
    /p:AppxPackageDir=%appxDir% ^
    /p:AppxBundle=Always ^
    /p:UapAppxPackageBuildMode=SideloadOnly ^
    /p:AppInstallerUri=https://not/used ^
    /p:AppxPackageSigningEnabled=true ^
    /p:PackageCertificateKeyFile=%packageCertificateKeyFile% ^
    /p:PackageCertificatePassword=%packageCertificatePassword%

if %ERRORLEVEL% neq 0 ( 
    echo ErrorLevel:%ERRORLEVEL%
    echo "Build Error."
)
