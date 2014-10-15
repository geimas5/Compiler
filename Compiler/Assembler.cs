namespace Compiler
{
    using System;
    using System.Diagnostics;

    public static class Assembler
    {
        //%comspec% /k ""C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat"" x86_amd64
        public static void ExecutAssemble()
        {
            var procStartInfo = new ProcessStartInfo("cmd", "/c Assemble.bat")
                                    {
                                        RedirectStandardOutput = true,
                                        UseShellExecute = false,
                                        CreateNoWindow = true
                                    };


            var proc = new Process { StartInfo = procStartInfo };
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();
            Console.WriteLine(result);
        }
    }
}
