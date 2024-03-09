using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;


public class ScriptRunner : MonoBehaviour
{
    public string scriptPath;
    private Process process;
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

    public void StartProcess(string scriptPath)
    {
        UnityEngine.Debug.Log(scriptPath);
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = scriptPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
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
        UnityEngine.Debug.Log("Pisem!!!");
        UnityEngine.Debug.Log(input);
        input = input.Replace("\n", "^");

        if (process == null || process.HasExited)
        {
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
