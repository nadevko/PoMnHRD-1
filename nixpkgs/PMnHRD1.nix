{
  lib,
  buildDotnetModule,
  dotnetCorePackages,
}:
buildDotnetModule {
  pname = "PMnHRD1";
  version = "1.0.0";

  src = ../App;
  nugetDeps = ./deps.nix;
  projectFile = "PMnHRD1.App.csproj";
  dotnet-sdk = dotnetCorePackages.sdk_8_0;
  dotnet-runtime = dotnetCorePackages.runtime_8_0;

  meta = with lib; {
    description = "Program for taking psychological tests (PMnHRD task).";
    homepage = "https://github.com/nadevko/bsuir-PMnHRD-1";
    license = licenses.gpl3Only;
    platforms = platforms.all;
  };
}
