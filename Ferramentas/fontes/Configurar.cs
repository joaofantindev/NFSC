using System;
using System.Diagnostics;
using System.IO;
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
    static int Main()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
        string pcsx2 = Path.Combine(baseDir, "PCSX2", "pcsx2-qt.exe");

        if (!File.Exists(pcsx2))
        {
            MessageBox.Show("PCSX2 nao encontrado em:\n" + pcsx2,
                "NFSCVIP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }

        Environment.SetEnvironmentVariable("SDL_GAMECONTROLLERCONFIG", PadMapping);
        Environment.SetEnvironmentVariable("SDL_GAMECONTROLLERCONFIG_FILE",
            Path.Combine(baseDir, "PCSX2", "game_controller_db.txt"));

        try
        {
            ProcessStartInfo psi = new ProcessStartInfo(pcsx2, "-portable");
            psi.WorkingDirectory = baseDir;
            psi.UseShellExecute = false;
            Process.Start(psi);
            return 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Falha ao abrir o PCSX2:\n" + ex.Message,
                "NFSCVIP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }
    }
}
