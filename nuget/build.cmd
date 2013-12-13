pushd %~dp0
pushd ..\src\SidebarWP8\
msbuild SidebarWP8.csproj /p:Configuration=Release
popd
nuget pack SidebarWP8.nuspec