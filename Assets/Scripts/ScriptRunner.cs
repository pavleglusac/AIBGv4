using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScriptRunner : MonoBehaviour
{
    public string scriptPath;
    
    private void Update()
    {
        if (outputQueue.Count > 0)
        {
            UnityEngine.Debug.Log("apdejttt");
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
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/python3",
                Arguments = "-u " + scriptPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        new Thread(() =>
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                stopwatch.Stop();

                EnqueueOutput($"Time taken: {stopwatch.Elapsed.TotalMilliseconds} ms - Output from {scriptPath}: {line}");

                stopwatch.Restart();
            }

            string stderr = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(stderr))
            {
                EnqueueOutput($"Error: {stderr}");
            }

            process.WaitForExit();
        }).Start();
    }

    private Queue<string> outputQueue = new Queue<string>();

    private void EnqueueOutput(string output)
    {
        lock (outputQueue)
        {
            outputQueue.Enqueue(output);
        }
    }
}
