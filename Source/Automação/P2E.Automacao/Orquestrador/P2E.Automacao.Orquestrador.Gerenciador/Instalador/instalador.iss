[Languages]
Name: "portugues"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

#define MyAppName "Orquestrador_Gerenciador"
#define DefaultGroupName "VersattilyBots"
#define MyAppFileName "P2E.Automacao.Orquestrador.Gerenciador"
#define MyAppVersion "0.7.0"
#define MyAppVerName "Gerenciador " + MyAppVersion
#define MyAppPublisher "C:\VersattilyBots\2E\Gerenciador"
#define UninstallDisplayName "Gerenciador " + MyAppVersion

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
Source: C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\Automação\P2E.Automacao\Orquestrador\P2E.Automacao.Orquestrador.Gerenciador\bin\Debug\*; DestDir: {app}; Flags: ignoreversion
Source: C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\Automação\P2E.Automacao\Orquestrador\P2E.Automacao.Orquestrador.Gerenciador\bin\Debug\Certificado\*; DestDir: {app}\Certificado; Flags: ignoreversion

[Dirs]
Name: "{app}"; Permissions: everyone-full;

[Run]
Filename: "{app}\P2E.Automacao.Orquestrador.Gerenciador.exe"; Description: "Executar o Gerenciador"; Flags: postinstall nowait skipifsilent
Filename: "{app}\P2E.Automacao.Orquestrador.Gerenciador.exe"; Flags: postinstall skipifnotsilent waituntilidle