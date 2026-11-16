# MODS - NFS Carbon (PS2) / NFSCVIP

Este documento e o "mapa" para quando formos adicionar mods, graficos e mais
carros. **Nada aqui esta aplicado ainda** - a estrutura esta pronta, vazia.

Regra de ouro: **nunca edite a ISO original**. Trabalhe sempre em uma copia
(o pacote aponta para a ISO original via hardlink em `iso\`).

---

## 1. Texturas (o caminho mais seguro)

O PCSX2 carrega texturas substitutas em PNG sem modificar a ISO.

- Pasta raiz configurada: `addons\texturas\`
- Subpasta do jogo: `addons\texturas\SLES-54321\`
- Dumps: `addons\texturas\SLES-54321\dumps\`
- Substitutos: `addons\texturas\SLES-54321\replacements\`

Como funciona:

1. Abra `Configurar PCSX2.exe`.
2. Settings > Graphics > Texture Replacement.
3. Marque "Dump Textures" e entre no jogo ate a textura aparecer.
4. Os PNGs aparecem em `dumps\`, com nome baseado na hash.
5. Coloque o PNG editado em `replacements\` **com o mesmo nome**.
6. Desmarque "Dump Textures" e mantenha "Load Textures" ligado
   (ja esta ligado no `PCSX2.ini`).

Packs prontos de textura normalmente ja vem com a pasta `replacements`
preenchida: basta copiar para dentro de `SLES-54321\`.

Observacao: a pasta `addons\texturas` e o `Textures` do PCSX2.ini. Se mudar
esse caminho no emulador, a pasta deixa de ser usada.

---

## 2. Carros (mods de ISO)

No PS2, o conteudo do Carbon fica em grandes arquivos dentro de `NFS\`:

```
NFS\ZDIR.BIN
NFS\ZZDATA0.BIN   (~850 MB)
NFS\ZZDATA1.BIN   (~858 MB)
NFS\ZZDATA2.BIN   (~819 MB)
NFS\ZZDATA3.BIN   (~973 MB)
```

Carros, pistas e tunagem ficam dentro desses `ZZDATA*.BIN`. Um mod de carro
de PS2 normalmente:

1. Extrai o `ZZDATA` com uma ferramenta da cena (extrator/repacker de ZZDATA).
2. Substitui os arquivos do carro.
3. Reempacota e reconstroi a ISO.

Estrutura reservada para quando implementarmos o pipeline:

```
addons\carros\<nome-do-mod>\
    conteudo\        arquivos a injetar (mesma arvore do ZZDATA)
    mod.json         metadados: nome, autor, versao, arquivos, CRC
    LEIA-ME.txt      instrucoes do autor
```

Enquanto o pipeline nao existir:

- Nao sobrescreva `iso\...iso`.
- Se for testar manualmente, faca uma **copia** da ISO em outra pasta
  (ex.: `iso\_testes\`) e aponte o launcher para ela.
- Guarde a ISO limpa para poder voltar atras.

---

## 3. Graficos

Opcoes que nao quebram o jogo:

- Resolucao interna: hoje 2x (Settings > Graphics > Internal Resolution).
  3x/4x pesa mais; em GPU integrada, teste 2x primeiro.
- Vulkan (padrao), D3D12 ou OpenGL como alternativa.
- Filtro de textura, mipmap, dithering e CAS ficam em Settings > Graphics.
- 16:9 ja configurado; o jogo tambem tem opcao de widescreen interna.
  Se a imagem ficar esticada, ative o widescreen no jogo ou volte o
  Aspect Ratio para "Auto 4:3/3:2".

ReShade / presets:

- Pasta reservada: `addons\graficos\`
- ReShade nao vem incluido. Quando formos instalar, a injecao precisa ser
  feita na pasta do PCSX2 (DX11/DX12/OpenGL/Vulkan) e o preset fica aqui.
- Alternativa sem injecao: usar os proprios filtros do PCSX2
  (CAS, FXAA, ShadeBoost) e salvar os valores em `addons\graficos\`.

---

## 4. Cheats e patches (.pnach)

- Cheats: `addons\cheats\` (precisam de "Enable Cheats" ligado no PCSX2)
- Patches: `addons\patches\` (aplicados automaticamente; `EnablePatches`
  ja esta ligado)

Formato basico de um `.pnach`:

```
gametitle=NFS Carbon (SLES-54321)
// comentario
// patch=1,EE,ENDERECO,extended,VALOR
```

O CRC do jogo e `ECCEEC09` (SLES-54321, PAL-E). O nome do arquivo `.pnach`
deve ser o CRC em maiusculas, sem `0x`, seguido de `.pnach`
(ex.: `ECCEEC09.pnach`).

O PCSX2 ja traz um patch embutido para este serial (correcao da deteccao
de save do Most Wanted Black Edition), aplicado automaticamente.

---

## 5. Perfis de controle

- Pasta: `addons\inputprofiles\`
- Em Settings > Controllers, salve um perfil; ele cai nessa pasta.
- O perfil padrao (DualShock 2 via adaptador) ja esta no `PCSX2.ini`.

---

## 6. Ordem sugerida para o futuro

1. Texturas (nao destrutivo, resultado visual imediato).
2. Graficos (presets + ReShade).
3. Cheats/patches (qualidade de vida).
4. Carros (ISO: extrair, substituir, reempacotar, testar em copia).
5. Launcher com perfis (ex.: "com mods" / "vanilla").

Cada etapa entra como um addon separado e desligavel, para nao repetir o
problema de save misturado do MC3VIP.
