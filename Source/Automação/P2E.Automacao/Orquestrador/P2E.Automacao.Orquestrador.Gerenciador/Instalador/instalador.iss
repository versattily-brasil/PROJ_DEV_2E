[Languages]
Name: "portugues"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

#define MyAppName "Orquestrador"
#define DefaultGroupName "VersattilyBots"
#define MyAppFileName "P2E.Automacao.Orquestrador.Gerenciador"
#define MyAppVersion "1.0.17"
#define MyAppVerName "Gerenciador " + MyAppVersion
#define MyAppPublisher "C:\VersattilyBots\2E\Orquestrador"
#define MyAppIcon "ManagerIcone.ico"
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
UninstallDisplayIcon={app}\{#MyAppIcon}
UninstallDisplayName={#UninstallDisplayName}


[Files]
Source: ..\ManagerIcone.ico; DestDir: {app}; Flags: ignoreversion
Source: C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\Automação\P2E.Automacao\Orquestrador\P2E.Automacao.Orquestrador.Gerenciador\bin\Debug\*; DestDir: {app}; Flags: ignoreversion
Source: C:\ProgramData\Lerron\TFS\Projeto2E\2E-dev\Source\Automação\P2E.Automacao\Orquestrador\P2E.Automacao.Orquestrador.Gerenciador\bin\Debug\Certificado\*; DestDir: {app}\Certificado; Flags: ignoreversion

[Icons]
Name: {group}\Orquestrador_Gerenciador; Filename: {app}\P2E.Automacao.Orquestrador.Gerenciador.exe; WorkingDir: {app}

;Cria atalho no grupo de programas
Name: {group}\Orquestrador_Gerenciador; Filename: {app}\P2E.Automacao.Orquestrador.Gerenciador.exe; WorkingDir: {app}; Comment: Acesso ao Orquestrador_Gerenciador; IconIndex: 0

;Cria atalho no desktop
Name: {userdesktop}\Orquestrador_Gerenciador; Filename: {app}\P2E.Automacao.Orquestrador.Gerenciador.exe; WorkingDir: {app}; Comment: Atalho de acesso direto ao Orquestrador_Gerenciador; IconIndex: 0


[Dirs]
Name: "{app}"; Permissions: everyone-full;

[Run]
Filename: "{app}\P2E.Automacao.Orquestrador.Gerenciador.exe"; Description: "Executar o Gerenciador"; Flags: postinstall nowait skipifsilent
Filename: "{app}\P2E.Automacao.Orquestrador.Gerenciador.exe"; Flags: postinstall skipifnotsilent waituntilidle