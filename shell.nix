{
  pkgs ? import <nixpkgs> {
    overlays = [ (import ./nixpkgs) ];
  },
}:
with pkgs;
let
  dotnetPkg = with dotnetCorePackages; combinePackages [ sdk_8_0 ];
  dotnetDeps = [
    xorg.libX11
    xorg.libICE
    xorg.libSM
    fontconfig
  ];
in
mkShell rec {
  name = "PMnHRD-1";

  vscode-settings = writeText "settings.json" (
    builtins.toJSON {
      "dotnetAcquisitionExtension.sharedExistingDotnetPath" = DOTNET_ROOT;
    }
  );

  packages = [
    dotnetPkg
    appstream
    dpkg
    flatpak
    flatpak-builder
    rpm
  ] ++ dotnetDeps;
  LD_LIBRARY_PATH = with pkgs; lib.makeLibraryPath dotnetDeps;
  DOTNET_ROOT = "${dotnetPkg}";

  shellHook = ''
    install -D ${vscode-settings} .vscode/settings.json
  '';
}
