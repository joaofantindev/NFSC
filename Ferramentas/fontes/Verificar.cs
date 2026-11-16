using System;
using System.IO;

static class Program
{
    static int falhas = 0;

    static void Check(bool ok, string okMessage, string failMessage)
    {
        if (ok)
        {
            Console.WriteLine("[OK]    " + okMessage);
        }
        else
        {
            Console.WriteLine("[FALTA] " + failMessage);
            falhas++;
        }
    }

    static int Main()
    {
        string exeDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
        string baseDir = Path.GetFullPath(Path.Combine(exeDir, ".."));

        Console.WriteLine("============================================================");
        Console.WriteLine(" VERIFICACAO - NFSCVIP (NFS Carbon PS2)");
        Console.WriteLine("============================================================");
        Console.WriteLine();

        string pcsx2 = Path.Combine(baseDir, "PCSX2", "pcsx2-qt.exe");
        string portable = Path.Combine(baseDir, "PCSX2", "portable.txt");
        string biosDir = Path.Combine(baseDir, "PCSX2", "bios");
        string iso = Path.Combine(baseDir, "iso", "Need for Speed - Carbon (Europe, Australia).iso");
        string ini = Path.Combine(baseDir, "PCSX2", "inis", "PCSX2.ini");
        string padDb = Path.Combine(baseDir, "PCSX2", "game_controller_db.txt");

        Check(File.Exists(pcsx2), "PCSX2 encontrado", "PCSX2\\pcsx2-qt.exe");
        Check(File.Exists(portable), "Modo portatil ativo", "PCSX2\\portable.txt");
        Check(Directory.Exists(biosDir) && Directory.GetFiles(biosDir, "*.bin").Length > 0,
            "BIOS do PS2 presente", "BIOS do PS2 em PCSX2\\bios\\");
        Check(File.Exists(iso), "ISO do NFS Carbon presente", "ISO em iso\\");
        Check(File.Exists(ini), "Configuracao PCSX2.ini presente", "PCSX2\\inis\\PCSX2.ini");
        Check(Directory.Exists(Path.Combine(baseDir, "SaveData")), "Pasta SaveData presente", "Pasta SaveData");
        Check(Directory.Exists(Path.Combine(baseDir, "Capturas")), "Pasta Capturas presente", "Pasta Capturas");
        Check(Directory.Exists(Path.Combine(baseDir, "addons")), "Pasta addons presente", "Pasta addons");
        Check(File.Exists(Path.Combine(baseDir, "addons", "inputprofiles", "Teclado.ini")),
            "Perfil de teclado presente", "Perfil addons\\inputprofiles\\Teclado.ini");

        string iniText = File.Exists(ini) ? File.ReadAllText(ini) : "";
        Check(iniText.IndexOf("BIOS = scph70004.bin", StringComparison.OrdinalIgnoreCase) >= 0,
            "BIOS PAL configurada no PCSX2.ini", "BIOS nao aponta para scph70004.bin em PCSX2.ini");

        string padText = File.Exists(padDb) ? File.ReadAllText(padDb) : "";
        Check(padText.IndexOf("03000000790000000600000000000000", StringComparison.Ordinal) >= 0,
            "Mapeamento do controle PS2 no banco SDL", "Mapeamento do controle PS2 no banco SDL");

        Check(iniText.IndexOf("SDL-0/FaceSouth", StringComparison.OrdinalIgnoreCase) >= 0,
            "Binds do controle configurados para SDL-0", "Binds do controle nao estao em SDL-0");

        Console.WriteLine();
        if (falhas == 0)
            Console.WriteLine("Tudo pronto. Execute \"Iniciar NFS Carbon com controle PS2.exe\".");
        else
            Console.WriteLine("Existem " + falhas + " pendencias. Leia o Leia-me.txt.");
        Console.WriteLine();

        if (!Console.IsInputRedirected)
        {
            Console.Write("Pressione qualquer tecla para continuar . . .");
            Console.ReadKey(true);
        }

        return falhas == 0 ? 0 : 1;
    }
}
