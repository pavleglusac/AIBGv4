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

    public void StartProcess(string scriptPath)
    {
        UnityEngine.Debug.Log("Starting process for script: " + scriptPath);
        string fileName, argumentsPrefix;
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
            scriptPath = $"\"{scriptPath}\"";;
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


    public async Task WriteToProcessAsync(string input, string playerName)
    {

        input = input.Replace("\n", "");
        UnityEngine.Debug.Log($"Writing to process: {input}");
        EnqueueOutput($"Writing to process: {input}");

        if (process == null || process.HasExited)
        {
            EnqueueOutput("Process is not started.");

            string err = process?.StandardError.ReadToEnd();
            EnqueueOutput($"Standard Error: {err}");
            string output = process?.StandardOutput.ReadToEnd();
            EnqueueOutput($"Standard Output: {output}");

            string combined = $"Process died! \nStandard Error: {err}\nStandard Output: {output}";
            CommandParser.FinishGame(combined);
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

                UnityEngine.Debug.Log($"Response time exceeded {TIMEOUT} milliseconds.");
                EnqueueOutput($"Response time exceeded {TIMEOUT} milliseconds.");
                throw new Exception($"Response time exceeded {TIMEOUT} milliseconds.");
            }
        }
        catch (TaskCanceledException)
        {
            UnityEngine.Debug.Log("Operation was canceled.");
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
