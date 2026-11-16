# NFSCVIP — Need for Speed: Carbon (PS2) no PC

Pacote portátil para jogar **Need for Speed: Carbon** (PlayStation 2, PAL `SLES-54321`) no Windows usando o **PCSX2 2.x** em modo portátil, com mapeamento para controle de PS2 (DualShock 2 via adaptador USB), estrutura de addons/mods e saves isolados — no mesmo espírito do MC3VIP.

> O Carbon de PS2 não possui porte nativo recompilado, então este pacote usa emulação via PCSX2. Tudo fica dentro da pasta: emulador, configuração e addons. Nada é instalado no Windows.

## Requisitos

- **Windows 10/11 64 bits**
- **Git** (para clonar o repositório)
- **GPU com suporte a Vulkan** (recomendado; o pacote vem configurado em Vulkan 2x)
- **BIOS de PS2** (não incluída por questões legais — veja abaixo)
- **ISO do jogo** (não incluída — veja abaixo)

## Instalação (do zero, após clonar)

1. Instale o [Git for Windows](https://git-scm.com/download/win) (se ainda não tiver).
2. Clone o repositório:
   ```powershell
   git clone https://github.com/joaofantindev/NFSC.git NFSCVIP
   cd NFSCVIP
   ```
3. Execute o instalador:
   - clique duas vezes em **`Instalar.exe`** (recomendado) — ou em `Instalar.cmd` / `powershell -ExecutionPolicy Bypass -File .\Instalar.ps1`
4. Adicione a **ISO do jogo** (veja a seção abaixo).
5. Forneça a **BIOS de PS2 PAL** (`scph70004.bin` e arquivos associados) em `PCSX2\bios\` caso o verificador indique que está faltando.  
   Você deve usar apenas um dump da BIOS de um console que você possui.
6. Execute **`Ferramentas\Verificar instalacao.exe`** (ou rode o instalador novamente) para confirmar que está tudo OK.
7. Execute **`Iniciar NFS Carbon com controle PS2.exe`**.

## ISO do jogo (não incluída)

A ISO **não faz parte** deste repositório (está ignorada no `.gitignore`). Você precisa colocar o arquivo manualmente na pasta `iso\` com o **nome exato**:

```
iso\Need for Speed - Carbon (Europe, Australia).iso
```

**Onde baixar (exemplo):** https://romsfun.com/roms/playstation-2/need-for-speed-carbon.html

- Use a versão **Europe/Australia (PAL)**, serial `SLES-54321`, para compatibilidade com a configuração do pacote.
- Após o download/extração, renomeie (se necessário) e mova a ISO para `iso\` com o nome acima.
- Use apenas uma cópia do jogo que você possua legalmente.

## BIOS (não incluída)

Arquivos de BIOS do PlayStation 2 são propriedade da Sony e **não são distribuídos** neste repositório. O pacote espera, por padrão:

- `PCSX2\bios\scph70004.bin` (PAL v12, configurada em `PCSX2\inis\PCSX2.ini`)

Coloque o(s) arquivo(s) do seu dump nessa pasta e rode `Ferramentas\Verificar instalacao.exe`.

## Como jogar

- **Jogar:** `Iniciar NFS Carbon com controle PS2.exe`
- **Configurar vídeo/áudio/controle:** `Configurar PCSX2.exe`
- **Verificar instalação:** `Ferramentas\Verificar instalacao.exe`

Controles, saves, gráficos e mods estão documentados em:
- [`Leia-me.txt`](Leia-me.txt)
- [`MODS.md`](MODS.md)

## Estrutura

- `PCSX2/` — emulador portátil e configuração
- `iso/` — ISO do jogo
- `addons/` — texturas, cheats, patches, perfis de input, carros, gráficos
- `SaveData/` — memory card e savestates
- `Capturas/` — screenshots e vídeos
- `Ferramentas/` — verificador e fontes

## Aviso legal

- Este repositório **não** concede direitos sobre o jogo nem sobre a BIOS. Use apenas com dumps/BIOS que você possui.
- **PCSX2** é software livre (GPL-3.0). Créditos aos desenvolvedores do PCSX2.
- Estrutura inspirada no MC3VIP, adaptada para emulação via PCSX2.

## Solução de problemas

- **Tela esticada:** ative widescreen nas opções do jogo ou ajuste Aspect Ratio em `Configurar PCSX2.exe > Settings > Graphics`.
- **Sem controle:** carregue `addons\inputprofiles\Teclado.ini` em Settings > Controllers > Load Profile.
- **Arquivos bloqueados pelo Windows (Mark of the Web):** rode `Instalar.ps1` ou `Unblock-File` nos `.exe`/`.dll` se necessário.
- **ISO/BIOS faltando:** execute `Ferramentas\Verificar instalacao.exe` para ver o que falta.
