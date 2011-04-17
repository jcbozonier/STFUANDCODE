using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace STFUANDCODE
{
    public class StfuAndRunCodeCommand : ICommand
    {
        private readonly CodeModel model;

        public StfuAndRunCodeCommand(CodeModel model)
        {
            this.model = model;
        }

        public void Execute(object parameter)
        {
            var codeToRun = model.Code;

            var codeToCompile = Path.GetTempFileName() + ".cs";
            var executablePath = Path.GetTempFileName() + ".exe";

            File.WriteAllLines(codeToCompile, new[] { codeToRun });

            const string compilerPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";

            using (var compilerProcess = new Process())
            {
                compilerProcess.StartInfo.FileName = compilerPath;
                compilerProcess.StartInfo.Arguments = "/out:\"" + executablePath + "\" \"" + codeToCompile + "\"";
                compilerProcess.StartInfo.UseShellExecute = false;
                compilerProcess.StartInfo.RedirectStandardOutput = true;
                compilerProcess.StartInfo.CreateNoWindow = true;

                compilerProcess.Start();
                var compilerMessages = compilerProcess.StandardOutput.ReadToEnd();
                compilerProcess.WaitForExit();
                File.WriteAllLines("build.log", new[] { compilerMessages });
                model.CompilationLog = compilerMessages;
            }

            if (File.Exists(executablePath))
            {
                using (var runProcess = new Process())
                {
                    runProcess.StartInfo.FileName = executablePath;
                    runProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                    runProcess.Start();
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}