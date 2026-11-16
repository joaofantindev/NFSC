using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

static class Program
{
    const string PadMapping =
        "03000000790000000600000000000000,PS2 DualShock via adaptador USB (DragonRise)," +
        "a:b2,b:b1,back:b8,dpdown:h0.4,dpleft:h0.8,dpright:h0.2,dpup:h0.1," +
        "leftshoulder:b4,leftstick:b10,lefttrigger:b6,leftx:a0,lefty:a1," +
        "rightshoulder:b5,rightstick:b11,righttrigger:b7,rightx:a2,righty:a3," +
        "start:b9,x:b3,y:b0,platform:Windows";

    [STAThread]
    static int Main(string[] args)
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
        string pcsx2 = Path.Combine(baseDir, "PCSX2", "pcsx2-qt.exe");
        string biosDir = Path.Combine(baseDir, "PCSX2", "bios");
        string iso = Path.Combine(baseDir, "iso", "Need for Speed - Carbon (Europe, Australia).iso");

        if (!File.Exists(pcsx2))
        {
            Fail("PCSX2 nao encontrado em:\n" + pcsx2);
            return 1;
        }

        if (!Directory.Exists(biosDir) || Directory.GetFiles(biosDir, "*.bin").Length == 0)
        {
            MessageBox.Show(
                "A BIOS do PS2 nao foi encontrada em:\n" + biosDir +
                "\n\nRecoloque os arquivos scph70004.* nessa pasta.",
                "NFSCVIP - BIOS ausente", MessageBoxButtons.OK, MessageBoxIcon.Error);
            try { Process.Start("explorer.exe", "\"" + biosDir + "\""); } catch { }
            return 1;
        }

        if (!File.Exists(iso))
        {
            Fail("ISO nao encontrada:\n" + iso);
            return 1;
        }

        Environment.SetEnvironmentVariable("SDL_GAMECONTROLLERCONFIG", PadMapping);
        Environment.SetEnvironmentVariable("SDL_GAMECONTROLLERCONFIG_FILE",
            Path.Combine(baseDir, "PCSX2", "game_controller_db.txt"));

        string arguments;
        if (args.Length > 0)
        {
            arguments = JoinArgs(args);
        }
        else
        {
            arguments = "-portable -fullscreen -fastboot " + Quote(iso);
        }

        try
        {
            ProcessStartInfo psi = new ProcessStartInfo(pcsx2, arguments);
            psi.WorkingDirectory = baseDir;
            psi.UseShellExecute = false;
            Process.Start(psi);
            return 0;
        }
        catch (Exception ex)
        {
            Fail("Falha ao iniciar o PCSX2:\n" + ex.Message);
            return 1;
        }
    }

    static void Fail(string message)
    {
        MessageBox.Show(message, "NFSCVIP", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    static string JoinArgs(string[] args)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < args.Length; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(Quote(args[i]));
        }
        return sb.ToString();
    }

    static string Quote(string value)
    {
        return "\"" + value.Replace("\"", "\\\"") + "\"";
    }
}
