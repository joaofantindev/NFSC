using System;
using System.Diagnostics;
using System.IO;
using System.Text;

static class Program
{
    static int Main()
    {
        try
        {
            // Base = pasta do pacote (raiz). O exe pode ficar na raiz ou em Ferramentas.
            string exeDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            string baseDir = exeDir;
            string script = Path.Combine(baseDir, "Instalar.ps1");
            if (!File.Exists(script))
            {
                string alt = Path.GetFullPath(Path.Combine(exeDir, ".."));
                string altScript = Path.Combine(alt, "Instalar.ps1");
                if (File.Exists(altScript))
                {
                    baseDir = alt;
                    script = altScript;
                }
            }

            if (!File.Exists(script))
            {
                Console.WriteLine("[ERRO] Instalar.ps1 nao encontrado em: " + exeDir);
                Console.WriteLine("Coloque Instalar.exe na pasta raiz do pacote (junto de Instalar.ps1).");
                Pause();
                return 1;
            }

            string ps = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "WindowsPowerShell", "v1.0", "powershell.exe");
            if (!File.Exists(ps)) ps = "powershell.exe";

            string args = "-NoProfile -ExecutionPolicy Bypass -File " + Quote(script);

            ProcessStartInfo psi = new ProcessStartInfo(ps, args);
            psi.WorkingDirectory = baseDir;
            psi.UseShellExecute = false;

            Process p = Process.Start(psi);
            if (p == null)
            {
                Console.WriteLine("[ERRO] Nao foi possivel iniciar o PowerShell.");
                Pause();
                return 1;
            }
            p.WaitForExit();
            return p.ExitCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ERRO] Falha ao executar o instalador: " + ex.Message);
            Pause();
            return 1;
        }
    }

    static string Quote(string value)
    {
        return "\"" + value.Replace("\"", "\\\"") + "\"";
    }

    static void Pause()
    {
        try
        {
            if (!Console.IsInputRedirected)
            {
                Console.Write("Pressione qualquer tecla para sair...");
                Console.ReadKey(true);
            }
        }
        catch { }
    }
}
