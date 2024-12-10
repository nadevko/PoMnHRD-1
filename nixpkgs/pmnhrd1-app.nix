{
  lib,
  buildDotnetModule,
  dotnetCorePackages,
}:
buildDotnetModule (
  final:
  let
    appId = "io.github.nadevko.pmnhrd1.app";
  in
  {
    pname = "bsuir-PMnHRD1-app";
    version = "1.0";

    src = ./..;
    nugetDeps = ./deps.nix;
    projectFile = "App/PMnHRD1.App.csproj";
    testProjectFile = "PMnHRD1.sln";
    dotnet-sdk = dotnetCorePackages.sdk_8_0;
    dotnet-runtime = dotnetCorePackages.runtime_8_0;

    meta = with lib; {
      mainProgram = "pmnhrd1-app";
      description = "Program for taking psychological tests (PMnHRD task).";
      longDescription = ''
        This application is designed to help users complete various psychological
        tests about management. It provides a user-friendly interface, twenty one
        test separated into seven topics. It includes various test modules such as
        personality assessment, stress management, and leadership skills evaluation.

        The application was developed as BSUIR task for "Psychology of Management
        and Development of Human Resources" subject.
      '';
      homepage = "https://github.com/nadevko/bsuir-PMnHRD-1";
      license = licenses.gpl3Only;
      platforms = platforms.all;
    };

    postInstall = ''
      install -D -m 644 App/Deploy/app.desktop $out/share/applications/${appId}.desktop
      icons=$out/share/icons/hicolor
      install -D -m 644 App/Assets/icon.svg $icons/scalable/apps/${appId}.svg
      for size in 16 24 32 48 64 96 128 256 512; do
        install -D -m 644 App/Assets/icon.$size.png $icons/''${size}x''${size}/apps/${appId}.png
      done
    '';
  }
)
