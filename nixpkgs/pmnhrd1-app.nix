{
  lib,
  buildDotnetModule,
  dotnetCorePackages,
  copyDesktopItems,
  imagemagick,
}:
buildDotnetModule (final: {
  pname = "bsuir-PMnHRD1-app";
  version = "1.0.0";

  src = ./..;
  nugetDeps = ./deps.nix;
  mapNuGetDependencies = true;
  projectFile = "App/PMnHRD1.App.csproj";
  testProjectFile = "PMnHRD1.sln";
  dotnet-sdk = dotnetCorePackages.sdk_8_0;
  dotnet-runtime = dotnetCorePackages.runtime_8_0;

  nativeBuildInputs = [
    copyDesktopItems
    imagemagick
  ];

  meta = with lib; {
    mainProgram = "pmnhrd1-app";
    description = "Program for taking psychological tests (PMnHRD task).";
    homepage = "https://github.com/nadevko/bsuir-PMnHRD-1";
    license = licenses.gpl3Only;
    platforms = platforms.all;
  };

  postInstall = ''
    install -D -m 444 -t $out/share/applications App/io.github.nadevko.PMnHRD1.App.desktop

    icon=App/Assets/avalonia-logo.ico
    mkdir --parent $out/share/icons/hicolor/scalable/apps
    magick $icon $out/share/icons/hicolor/scalable/apps/io.github.nadevko.PMnHRD1.App.svg
    for i in 16 24 48 64 96 128 256 512; do
      size=''${i}x''${i}
      dir=$out/share/icons/hicolor/$size/apps
      mkdir -p $dir
      magick $icon -background none -resize $size $dir/io.github.nadevko.PMnHRD1.App.png
    done
  '';
})
