[Languages]
Name: "portugues"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

#define MyAppName "Orquestrador_Console"
#define DefaultGroupName "VersattilyBots"
#define MyAppFileName "P2E.Automacao.Orquestrador.Console"
#define MyAppVersion "0.9.0"
#define MyAppVerName "Orquestrador " + MyAppVersion
#define MyAppPublisher "C:\VersattilyBots\2E\Console"
#define UninstallDisplayName "Orquestrador " + MyAppVersion

[Setup]
AppName={#MyAppName}
UsePreviousAppDir=no
DefaultDirName={#MyAppPublisher}
DefaultGroupName={#DefaultGroupName}
AppCopyright={#MyAppPublisher}
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoCopyright={#MyAppPublisher}
VersionInfoDescription={#MyAppName}
DisableProgramGroupPage=true
SolidCompression=true
OutputBaseFilename={#MyAppName}-{#MyAppVersion}
VersionInfoTextVersion={#MyAppVersion}
VersionInfoProductName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
UninstallDisplayName={#UninstallDisplayName}

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\Automação\P2E.Automacao\Orquestrador\P2E.Automacao.Orquestrador.Console\bin\Debug\*; DestDir: {app}; Flags: ignoreversion
Source: C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\Automação\P2E.Automacao\Orquestrador\P2E.Automacao.Orquestrador.Console\bin\Debug\Certificado\*; DestDir: {app}\Certificado; Flags: ignoreversion

[Dirs]
Name: "{app}"; Permissions: everyone-full;

[Run]
Filename: "{app}\P2E.Automacao.Orquestrador.Console.exe"; Description: "Executar o Console"; Flags: postinstall nowait skipifsilent
Filename: "{app}\P2E.Automacao.Orquestrador.Console.exe"; Flags: postinstall skipifnotsilent waituntilidle
