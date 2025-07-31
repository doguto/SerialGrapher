using System;
using System.IO.Ports;
using System.Timers;
using System.Collections.Generic;

namespace SerialGrapherWithFrequency
{
    /// <summary>
    /// Console-based Serial Grapher with Frequency Setting functionality
    /// Demonstrates the core logic for frequency-controlled data acquisition
    /// </summary>
    internal static class Program
    {
        private static SerialPort? serialPort;
        private static System.Timers.Timer? dataTimer;
        private static Queue<(double x, double y)> dataQueue = new Queue<(double, double)>();
        private static List<(double x, double y)> plotData = new List<(double, double)>();
        
        private static double frequency = 10.0; // Default 10 Hz
        private static int maxPlotPoints = 100;
        private static int comPort = 3;
        private static bool isRunning = false;
        private static readonly object lockObject = new object();

        static void Main()
        {
            Console.WriteLine("=== Serial Grapher with Frequency Setting ===");
            Console.WriteLine("This demonstrates the frequency setting functionality for controlling data acquisition rate.");
            Console.WriteLine();

            ShowMenu();
            
            string? input;
            while ((input = Console.ReadLine()) != "q")
            {
                ProcessCommand(input?.Trim() ?? "");
                Console.WriteLine();
                ShowMenu();
            }

            Cleanup();
        }

        private static void ShowMenu()
        {
            Console.WriteLine($"Current Settings:");
            Console.WriteLine($"  COM Port: COM{comPort}");
            Console.WriteLine($"  Max Plot Points: {maxPlotPoints}");
            Console.WriteLine($"  Frequency: {frequency} Hz");
            Console.WriteLine($"  Status: {(isRunning ? "Running" : "Stopped")}");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  c <port>     - Set COM port (e.g., 'c 3' for COM3)");
            Console.WriteLine("  m <max>      - Set max plot points (e.g., 'm 200')");
            Console.WriteLine("  f <freq>     - Set frequency in Hz (e.g., 'f 50' for 50Hz)");
            Console.WriteLine("  s            - Start/Stop data acquisition");
            Console.WriteLine("  d            - Show current data");
            Console.WriteLine("  t            - Test frequency setting (simulate data)");
            Console.WriteLine("  q            - Quit");
            Console.Write("Enter command: ");
        }

        private static void ProcessCommand(string command)
        {
            if (string.IsNullOrEmpty(command)) return;

            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "c":
                    if (parts.Length > 1 && int.TryParse(parts[1], out int port) && port > 0)
                    {
                        if (isRunning)
                        {
                            Console.WriteLine("Cannot change COM port while running. Stop first.");
                        }
                        else
                        {
                            comPort = port;
                            Console.WriteLine($"COM port set to COM{comPort}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid COM port. Use 'c <port>' (e.g., 'c 3')");
                    }
                    break;

                case "m":
                    if (parts.Length > 1 && int.TryParse(parts[1], out int maxPoints) && maxPoints > 0)
                    {
                        maxPlotPoints = maxPoints;
                        Console.WriteLine($"Max plot points set to {maxPlotPoints}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid max points. Use 'm <max>' (e.g., 'm 200')");
                    }
                    break;

                case "f":
                    if (parts.Length > 1 && double.TryParse(parts[1], out double freq) && freq > 0 && freq <= 1000)
                    {
                        SetFrequency(freq);
                    }
                    else
                    {
                        Console.WriteLine("Invalid frequency. Use 'f <freq>' (0.1 - 1000 Hz, e.g., 'f 50')");
                    }
                    break;

                case "s":
                    if (isRunning)
                        StopAcquisition();
                    else
                        StartAcquisition();
                    break;

                case "d":
                    ShowData();
                    break;

                case "t":
                    if (parts.Length > 1 && int.TryParse(parts[1], out int duration))
                        TestFrequency(duration);
                    else
                        TestFrequency(5); // Default 5 seconds
                    break;

                default:
                    Console.WriteLine("Unknown command. Type 'q' to quit.");
                    break;
            }
        }

        private static void SetFrequency(double newFreq)
        {
            frequency = newFreq;
            Console.WriteLine($"Frequency set to {frequency} Hz");
            
            if (isRunning && dataTimer != null)
            {
                // Update timer interval while running
                dataTimer.Stop();
                dataTimer.Interval = 1000.0 / frequency;
                dataTimer.Start();
                Console.WriteLine($"Updated acquisition rate to {frequency} Hz (interval: {1000.0/frequency:F1} ms)");
            }
        }

        private static void StartAcquisition()
        {
            try
            {
                // Try to open serial port (this will fail in this environment, but shows the logic)
                Console.WriteLine($"Attempting to open COM{comPort}...");
                
                // In a real Windows environment, this would work:
                // serialPort = new SerialPort($"COM{comPort}", 115200);
                // serialPort.DataReceived += OnDataReceived;
                // serialPort.Open();
                
                // For demonstration, we'll simulate the serial port
                Console.WriteLine("Serial port simulation mode (no actual hardware connection)");
                
                // Clear existing data
                lock (lockObject)
                {
                    dataQueue.Clear();
                    plotData.Clear();
                }

                // Setup timer for data processing at the specified frequency
                dataTimer = new System.Timers.Timer(1000.0 / frequency);
                dataTimer.Elapsed += OnTimerElapsed;
                dataTimer.Start();

                isRunning = true;
                Console.WriteLine($"Started data acquisition at {frequency} Hz");
                Console.WriteLine($"Data will be processed every {1000.0/frequency:F1} milliseconds");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start acquisition: {ex.Message}");
            }
        }

        private static void StopAcquisition()
        {
            isRunning = false;
            
            dataTimer?.Stop();
            dataTimer?.Dispose();
            dataTimer = null;

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
                serialPort = null;
            }

            Console.WriteLine("Stopped data acquisition");
        }

        private static void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            // Simulate receiving data from microcontroller
            // In real implementation, this would process data from the serial port queue
            double time = DateTime.Now.Subtract(DateTime.Today).TotalSeconds;
            double value = Math.Sin(time * 0.5) + 0.1 * Math.Sin(time * 5); // Simulated data
            
            lock (lockObject)
            {
                plotData.Add((time, value));
                
                // Remove old data if we exceed max plot points
                while (plotData.Count > maxPlotPoints)
                {
                    plotData.RemoveAt(0);
                }
            }
        }

        private static void ShowData()
        {
            lock (lockObject)
            {
                Console.WriteLine($"Current data points: {plotData.Count}/{maxPlotPoints}");
                if (plotData.Count > 0)
                {
                    Console.WriteLine("Last 5 data points:");
                    int start = Math.Max(0, plotData.Count - 5);
                    for (int i = start; i < plotData.Count; i++)
                    {
                        var (x, y) = plotData[i];
                        Console.WriteLine($"  [{i}] X: {x:F3}, Y: {y:F3}");
                    }
                }
            }
        }

        private static void TestFrequency(int durationSeconds)
        {
            Console.WriteLine($"Testing frequency setting for {durationSeconds} seconds...");
            Console.WriteLine("This will show how frequency affects data acquisition timing.");
            
            var testTimer = new System.Timers.Timer(1000.0 / frequency);
            int counter = 0;
            DateTime startTime = DateTime.Now;
            
            testTimer.Elapsed += (sender, e) =>
            {
                counter++;
                TimeSpan elapsed = DateTime.Now - startTime;
                Console.WriteLine($"Data point {counter} acquired at {elapsed.TotalMilliseconds:F0}ms (target: {counter * (1000.0/frequency):F0}ms)");
            };
            
            testTimer.Start();
            
            // Run for specified duration
            System.Threading.Thread.Sleep(durationSeconds * 1000);
            
            testTimer.Stop();
            testTimer.Dispose();
            
            TimeSpan totalElapsed = DateTime.Now - startTime;
            double actualFreq = counter / totalElapsed.TotalSeconds;
            
            Console.WriteLine($"Test completed:");
            Console.WriteLine($"  Target frequency: {frequency} Hz");
            Console.WriteLine($"  Actual frequency: {actualFreq:F2} Hz");
            Console.WriteLine($"  Data points: {counter}");
            Console.WriteLine($"  Duration: {totalElapsed.TotalSeconds:F2} seconds");
        }

        private static void Cleanup()
        {
            if (isRunning)
            {
                StopAcquisition();
            }
            Console.WriteLine("Cleanup completed. Goodbye!");
        }
    }
}
