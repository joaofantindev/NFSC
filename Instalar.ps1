# Instalar.ps1 - NFSCVIP
# Prepara o pacote após clone/cópia no Windows (verificações, desbloqueio, pastas).
# Execute: powershell -ExecutionPolicy Bypass -File .\Instalar.ps1

$ErrorActionPreference = 'Stop'
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

function Write-Ok($msg)   { Write-Host "[OK]    $msg" -ForegroundColor Green }
function Write-Warn($msg) { Write-Host "[AVISO] $msg" -ForegroundColor Yellow }
function Write-Fail($msg) { Write-Host "[FALTA] $msg" -ForegroundColor Red; $script:fail++ }
function Write-Info($msg) { Write-Host "[INFO]  $msg" -ForegroundColor Cyan }

$script:fail = 0
$base = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $base

Write-Host "============================================================"
Write-Host " INSTALADOR - NFSCVIP (NFS Carbon PS2 no PCSX2)"
Write-Host "============================================================"
Write-Host ""
Write-Info "Pasta base: $base"
Write-Host ""

# 1) Informações de ambiente (git opcional)
$gitDir = Join-Path $base ".git"
if (Test-Path $gitDir) {
    $git = Get-Command git -ErrorAction SilentlyContinue
    if ($git) {
        Write-Ok "Git encontrado: $($git.Source)"
    } else {
        Write-Warn "Pasta .git encontrada, mas o Git não está no PATH."
    }
} else {
    Write-Info "Pasta não é um repositório git (ok se foi copiada/extraída)."
}

Write-Host ""

# 2) Criar pastas esperadas
$dirs = @(
    "addons\carros",
    "addons\cheats",
    "addons\graficos",
    "addons\inputprofiles",
    "addons\patches",
    "addons\texturas\SLES-54321\dumps",
    "addons\texturas\SLES-54321\replacements",
    "SaveData",
    "SaveData\Savestates",
    "Capturas",
    "iso",
    "PCSX2\bios"
)
foreach ($d in $dirs) {
    $p = Join-Path $base $d
    if (-not (Test-Path $p)) {
        New-Item -ItemType Directory -Path $p -Force | Out-Null
        Write-Info "Pasta criada: $d"
    }
}
Write-Ok "Estrutura de pastas verificada"

# 3) Desbloquear arquivos baixados da internet (Zone.Identifier)
Write-Info "Removendo bloqueio do Windows (Mark of the Web) de executáveis e DLLs..."
try {
    $toUnblock = Get-ChildItem -Path $base -Recurse -Include *.exe,*.dll -File -ErrorAction SilentlyContinue
    $count = 0
    foreach ($f in $toUnblock) {
        try { Unblock-File -Path $f.FullName -ErrorAction SilentlyContinue; $count++ } catch {}
    }
    Write-Ok "Arquivos processados para desbloqueio: $count"
} catch {
    Write-Warn "Não foi possível desbloquear alguns arquivos: $($_.Exception.Message)"
}

Write-Host ""
Write-Host "--- Verificação dos componentes ---"

# 4) Verificações principais
$pcsx2 = Join-Path $base "PCSX2\pcsx2-qt.exe"
$portable = Join-Path $base "PCSX2\portable.txt"
$biosDir = Join-Path $base "PCSX2\bios"
$iso = Join-Path $base "iso\Need for Speed - Carbon (Europe, Australia).iso"
$ini = Join-Path $base "PCSX2\inis\PCSX2.ini"

if (Test-Path $pcsx2) { Write-Ok "PCSX2 encontrado (PCSX2\\pcsx2-qt.exe)" } else { Write-Fail "PCSX2 ausente: PCSX2\\pcsx2-qt.exe" }
if (Test-Path $portable) { Write-Ok "Modo portátil ativo (PCSX2\\portable.txt)" } else { Write-Fail "portable.txt ausente em PCSX2\\" }

$biosBins = @()
if (Test-Path $biosDir) { $biosBins = Get-ChildItem -Path $biosDir -Filter *.bin -File -ErrorAction SilentlyContinue }
if ($biosBins -and $biosBins.Count -gt 0) {
    Write-Ok "BIOS encontrada em PCSX2\\bios\\ ($($biosBins[0].Name))"
} else {
    Write-Fail "BIOS de PS2 ausente em PCSX2\\bios\\ (ex.: scph70004.bin). Copie o dump da BIOS do seu console."
}

if (Test-Path $iso) {
    $len = (Get-Item $iso).Length
    if ($len -gt 100MB) {
        Write-Ok ("ISO encontrada ({0:N2} GB)" -f ($len / 1GB))
    } else {
        Write-Warn "ISO existe mas parece pequena ($len bytes). Substitua pela ISO real do jogo."
        $script:fail++
    }
} else {
    Write-Fail "ISO ausente: iso\\Need for Speed - Carbon (Europe, Australia).iso. Coloque a ISO nessa pasta (veja iso\\LEIA-ME.txt)."
}

if (Test-Path $ini) { Write-Ok "Configuracao presente (PCSX2\\inis\\PCSX2.ini)" } else { Write-Fail "PCSX2.ini ausente em PCSX2\\inis\\" }

$verificador = Join-Path $base "Ferramentas\Verificar instalacao.exe"
if (Test-Path $verificador) { Write-Ok "Verificador encontrado (Ferramentas\\Verificar instalacao.exe)" } else { Write-Warn "Verificador nao encontrado em Ferramentas\\Verificar instalacao.exe" }

Write-Host ""
Write-Host "============================================================"
if ($script:fail -eq 0) {
    Write-Host " Instalacao/verificacao concluida: TUDO PRONTO." -ForegroundColor Green
    Write-Host ' Execute: "Iniciar NFS Carbon com controle PS2.exe"' -ForegroundColor Green
} else {
    Write-Host " Instalacao/verificacao concluida com $($script:fail) pendencia(s)." -ForegroundColor Yellow
    Write-Host " Corrija os itens [FALTA]/[AVISO] acima e rode o instalador novamente." -ForegroundColor Yellow
    Write-Host " Dica: execute Ferramentas\\Verificar instalacao.exe para mais detalhes." -ForegroundColor Yellow
}
Write-Host "============================================================"
Write-Host ""

try {
    if (-not [Console]::IsInputRedirected) {
        Write-Host "Pressione qualquer tecla para sair..."
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    }
} catch {}

exit $(if ($script:fail -eq 0) { 0 } else { 1 })
