using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


public class ScriptRunner : MonoBehaviour
{
    public string scriptPath;
    public Process process;
    public CommandParser CommandParser { get; set; }

    private void Update()
    {
        if (outputQueue.Count > 0)
        {
            lock (outputQueue)
            {
                while (outputQueue.Count > 0)
                {
                    UnityEngine.Debug.Log(outputQueue.Dequeue());
                }
            }
        }
    }

    public static string ConvertWindowsPathToWsl(string windowsPath)
    {
        if (string.IsNullOrEmpty(windowsPath)) return windowsPath;

        string wslPath = windowsPath.Replace('\\', '/');

        if (wslPath.Length >= 2 && wslPath[1] == ':')
        {
            char driveLetter = char.ToLower(wslPath[0]);
            wslPath = "/mnt/" + driveLetter + wslPath.Substring(2);
        }

        wslPath = Regex.Replace(wslPath, "/+", "/");

        return wslPath;
    }

    public void StartProcess(string scriptPath)
    {
        UnityEngine.Debug.Log(scriptPath);
        string fileName, argumentsPrefix;
        UnityEngine.Debug.Log(ConvertWindowsPathToWsl("C://Users//Pavle//mjau.sh"));
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            fileName = "wsl";
            argumentsPrefix = "";
            scriptPath = ConvertWindowsPathToWsl(scriptPath);
        }
        else
        {
            fileName = "/bin/bash";
            argumentsPrefix = "";
        }

        
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = argumentsPrefix + scriptPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        // string err = process.StandardError.ReadToEnd();
        // UnityEngine.Debug.Log($"{err}");
        UnityEngine.Debug.Log($"Script status: {process.HasExited}");
        UnityEngine.Debug.Log("Pokrenuta skripta!!!");
    }

    private Queue<string> outputQueue = new Queue<string>();

    private void EnqueueOutput(string output)
    {
        lock (outputQueue)
        {
            outputQueue.Enqueue(output);
        }
    }


    public async Task WriteToProcessAsync(string input)
    {
        UnityEngine.Debug.Log("Pisem u proces!!!");
        UnityEngine.Debug.Log(input);
        input = input.Replace("\n", "^");

        if (process == null || process.HasExited)
        {
            UnityEngine.Debug.Log("Process is not started.");
            // TODO: drugi igrac je pobedio.
            return;
        }
        process.StandardInput.WriteLine(input);
        process.StandardInput.Flush();

        var cancellationTokenSource = new CancellationTokenSource(5000);
        try
        {
            await Task.Run(() =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    string line = process.StandardOutput.ReadLine();
                    UnityEngine.Debug.Log($"Ovo je output: {line}");
                    //UnityEngine.Debug.Log($"Ovo je memorijsko: {process.PeakWorkingSet64}");
                    CommandParser.ParseCommand(line);
                    
                    break;
                }
            }, cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            UnityEngine.Debug.Log("Response time exceeded 5 seconds.");
            EnqueueOutput("Response time exceeded 5 seconds.");
        }
    }


}
