using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

public class ScriptRunner : MonoBehaviour
{
    public const int TIMEOUT = 3000;

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
        wslPath = Regex.Replace(wslPath, @"([\s()])", @"\$1");
        return wslPath;
    }
    bool IsRunningOnWine()
    {
        string wineVar = Environment.GetEnvironmentVariable("WINELOADERNOEXEC");
        string winePath = Environment.GetEnvironmentVariable("WINEPREFIX");
        return !string.IsNullOrEmpty(wineVar) || !string.IsNullOrEmpty(winePath);
    }

    public void StartProcess(string scriptPath)
    {
        UnityEngine.Debug.Log("Starting process for script: " + scriptPath);
        string fileName, argumentsPrefix;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !IsRunningOnWine())
        {
            fileName = "wsl";
            argumentsPrefix = "-e ";
            scriptPath = ConvertWindowsPathToWsl(scriptPath);
            argumentsPrefix = argumentsPrefix + $"bash -c \"{scriptPath} || exit\"" + " -or exit";
        }
        else
        {
            fileName = "/bin/bash";
            argumentsPrefix = "";
            scriptPath = $"\"{scriptPath}\""; ;
        }
        UnityEngine.Debug.Log($"Converted path: {scriptPath}");

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
                CreateNoWindow = true,
            }
        };

        process.Start();
        UnityEngine.Debug.Log("Process started.");
        UnityEngine.Debug.Log($"Script is alive: {process.HasExited == false}");
    }

    private readonly Queue<string> outputQueue = new Queue<string>();

    private void EnqueueOutput(string output)
    {
        outputQueue.Enqueue(output);
    }

    async void ShowError()
    {

        var output = new List<string>();
        output.Add("Standard Error: ");
        // CustomReadLineAsync read the output of the process, so we can't use it here. Read for 500 ms
        var readTask = process.StandardError.ReadToEndAsync();
        if (await Task.WhenAny(readTask, Task.Delay(500)) == readTask)
        {
            output.Add(readTask.Result);
        }
        else
        {
            output.Add("No output from the process.");
        }

        var readTask2 = process.StandardOutput.ReadToEndAsync();
        output.Add("Standard Output: ");
        if (await Task.WhenAny(readTask2, Task.Delay(500)) == readTask2)
        {
            output.Add(readTask2.Result);
        }
        else
        {
            output.Add("No output from the process.");
        }

        string combined = string.Join("\n", output);

        CommandParser.FinishGame(combined);
    }


    public async Task WriteToProcessAsync(string input, string playerName)
    {

        input = input.Replace("\n", "");
        EnqueueOutput($"Writing to process: {input}");
        bool invalidTurn = false;

        if (process == null || process.HasExited)
        {
            EnqueueOutput("Process is not started.");
            ShowError();
            return;
        }

        process.StandardInput.WriteLine(input);
        process.StandardInput.Flush();

        var cancellationTokenSource = new CancellationTokenSource();
        var streamToken = new CancellationTokenSource();
        try
        {
            var delayTask = Task.Delay(TIMEOUT, cancellationTokenSource.Token);
            var readTask = Task.Run(() => CustomReadLineAsync(process.StandardOutput, streamToken.Token));

            var completedTask = await Task.WhenAny(readTask, delayTask);

            if (completedTask == readTask && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                string line = await readTask;
                CommandParser.ParseCommand(line, playerName);
            }
            else
            {
                streamToken.Cancel();
                cancellationTokenSource.Cancel();
                CommandParser.ParseCommand("script_timeout", playerName);
                invalidTurn = true;

                EnqueueOutput($"Response time exceeded {TIMEOUT} milliseconds.");
                throw new Exception($"Response time exceeded {TIMEOUT} milliseconds.");
            }
        }
        catch (TaskCanceledException)
        {
            if (!invalidTurn)
            {
                CommandParser.ParseCommand(null, playerName);
            }

            EnqueueOutput("Operation was canceled.");
            throw new Exception("Operation was canceled.");
        }
        finally
        {
            streamToken.Cancel();
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }

    public async Task<string> CustomReadLineAsync(StreamReader streamReader, CancellationToken cancellationToken)
    {
        Memory<char> buffer = new Memory<char>(new char[1024]);
        StringBuilder stringBuilder = new StringBuilder();

        while (!cancellationToken.IsCancellationRequested)
        {
            int readCount = await streamReader.ReadAsync(buffer, cancellationToken);

            if (readCount == 0)
                break;

            string tempString = new string(buffer.Span.Slice(0, readCount).ToArray());
            int lineEnd = tempString.IndexOf('\n');
            if (lineEnd >= 0)
            {
                stringBuilder.Append(tempString, 0, lineEnd + 1);
                break;
            }
            else
            {
                stringBuilder.Append(tempString);
            }
        }

        if (stringBuilder.Length == 0)
            return null;

        return stringBuilder.ToString();
    }
}
