[Languages]
Name: "portugues"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

#define MyAppName "Orquestrador_Console"
#define DefaultGroupName "VersattilyBots"
#define MyAppFileName "P2E.Automacao.Orquestrador.Console"
#define MyAppVersion "1.0.0"
#define MyAppVerName "Orquestrador " + MyAppVersion
;#define MyAppPublisher "C:\VersattilyBots\2E\Console"
#define MyAppPublisher "C:\VersattilyBots\2E\Gerenciador"
#define MyAppIcon "ConsoleIcone.ico"
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
UninstallDisplayIcon={app}\{#MyAppIcon}
UninstallDisplayName={#UninstallDisplayName}


[Files]
Source: ..\ConsoleIcone.ico; DestDir: {app}; Flags: ignoreversion
Source: C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\Automação\P2E.Automacao\Orquestrador\P2E.Automacao.Orquestrador.Console\bin\Debug\*; DestDir: {app}; Flags: ignoreversion
Source: C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\Automação\P2E.Automacao\Orquestrador\P2E.Automacao.Orquestrador.Console\bin\Debug\Certificado\*; DestDir: {app}\Certificado; Flags: ignoreversion

[Icons]
Name: {group}\Orquestrador_Console; Filename: {app}\P2E.Automacao.Orquestrador.Console.exe; WorkingDir: {app}

;Cria atalho no grupo de programas
Name: {group}\Orquestrador_Console; Filename: {app}\P2E.Automacao.Orquestrador.Console.exe; WorkingDir: {app}; Comment: Acesso ao Orquestrador_Console; IconIndex: 0

;Cria atalho no desktop
Name: {userdesktop}\Orquestrador_Console; Filename: {app}\P2E.Automacao.Orquestrador.Console.exe; WorkingDir: {app}; Comment: Atalho de acesso direto ao Orquestrador_Console; IconIndex: 0


[Dirs]
Name: "{app}"; Permissions: everyone-full;

[Run]
Filename: "{app}\P2E.Automacao.Orquestrador.Console.exe"; Description: "Executar o Console"; Flags: postinstall nowait skipifsilent
Filename: "{app}\P2E.Automacao.Orquestrador.Console.exe"; Flags: postinstall skipifnotsilent waituntilidle
