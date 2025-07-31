// Windows Forms implementation of Serial Grapher with Frequency Setting
// This file contains the Windows Forms UI code that would work in a Windows environment
// with the appropriate Windows Forms SDK installed.

/*
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;

// Windows Forms implementation of Serial Grapher with Frequency Setting
// This file contains the Windows Forms UI code that would work in a Windows environment
// with the appropriate Windows Forms SDK installed.

/*
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Timers;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace SerialGrapherWithFrequency
{
    public partial class MainForm : Form
    {
        private SerialPort? serialPort;
        private CartesianChart chart;
        private ObservableCollection<double> xValues;
        private ObservableCollection<double> yValues;
        private System.Timers.Timer? readTimer;
        private bool isGraphing = false;
        private int maxPlotAmount = 100;
        private double frequency = 10.0; // Default 10 Hz
        private Queue<string> dataQueue = new Queue<string>();
        private object lockObject = new object();

        // UI Controls
        private Label comLabel;
        private TextBox comTextBox;
        private Button comButton;
        
        private Label maxPlotLabel;
        private TextBox maxPlotTextBox;
        private Button maxPlotButton;
        
        private Label frequencyLabel;
        private TextBox frequencyTextBox;
        private Button frequencyButton;
        
        private Button enterButton;

        public MainForm()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeComponent()
        {
            this.Size = new System.Drawing.Size(800, 600);
            this.Text = "Serial Grapher with Frequency Setting";
            this.StartPosition = FormStartPosition.CenterScreen;

            // COM Port controls
            comLabel = new Label
            {
                Text = "COM Port:",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(70, 23)
            };
            
            comTextBox = new TextBox
            {
                Location = new System.Drawing.Point(85, 10),
                Size = new System.Drawing.Size(50, 23),
                Text = "3"
            };
            
            comButton = new Button
            {
                Text = "Set",
                Location = new System.Drawing.Point(140, 10),
                Size = new System.Drawing.Size(50, 23)
            };
            comButton.Click += OnCOMButtonClicked;

            // Max Plot controls
            maxPlotLabel = new Label
            {
                Text = "Max Plot:",
                Location = new System.Drawing.Point(200, 10),
                Size = new System.Drawing.Size(70, 23)
            };
            
            maxPlotTextBox = new TextBox
            {
                Location = new System.Drawing.Point(275, 10),
                Size = new System.Drawing.Size(50, 23),
                Text = "100"
            };
            
            maxPlotButton = new Button
            {
                Text = "Set",
                Location = new System.Drawing.Point(330, 10),
                Size = new System.Drawing.Size(50, 23)
            };
            maxPlotButton.Click += OnMaxPlotButtonClicked;

            // Frequency controls - THIS IS THE NEW FUNCTIONALITY
            frequencyLabel = new Label
            {
                Text = "Frequency (Hz):",
                Location = new System.Drawing.Point(390, 10),
                Size = new System.Drawing.Size(90, 23)
            };
            
            frequencyTextBox = new TextBox
            {
                Location = new System.Drawing.Point(485, 10),
                Size = new System.Drawing.Size(50, 23),
                Text = "10"
            };
            
            frequencyButton = new Button
            {
                Text = "Set",
                Location = new System.Drawing.Point(540, 10),
                Size = new System.Drawing.Size(50, 23)
            };
            frequencyButton.Click += OnFrequencyButtonClicked;

            // Enter button
            enterButton = new Button
            {
                Text = "Start",
                Location = new System.Drawing.Point(600, 10),
                Size = new System.Drawing.Size(60, 23)
            };
            enterButton.Click += OnEnterButtonClicked;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                comLabel, comTextBox, comButton,
                maxPlotLabel, maxPlotTextBox, maxPlotButton,
                frequencyLabel, frequencyTextBox, frequencyButton,
                enterButton
            });
        }

        private void InitializeChart()
        {
            xValues = new ObservableCollection<double>();
            yValues = new ObservableCollection<double>();

            chart = new CartesianChart
            {
                Location = new System.Drawing.Point(10, 50),
                Size = new System.Drawing.Size(760, 500),
                Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Values = yValues,
                        Fill = null,
                        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                        GeometryFill = null,
                        GeometryStroke = null
                    }
                }
            };

            this.Controls.Add(chart);
        }

        private void OnCOMButtonClicked(object? sender, EventArgs e)
        {
            if (int.TryParse(comTextBox.Text, out int comPort) && comPort > 0)
            {
                MessageBox.Show($"COM Port set to: COM{comPort}", "Success");
            }
            else
            {
                MessageBox.Show("Please enter a valid COM port number (positive integer)", "Error");
            }
        }

        private void OnMaxPlotButtonClicked(object? sender, EventArgs e)
        {
            if (int.TryParse(maxPlotTextBox.Text, out int maxPlot) && maxPlot > 0)
            {
                maxPlotAmount = maxPlot;
                MessageBox.Show($"Max plot amount set to: {maxPlot}", "Success");
            }
            else
            {
                MessageBox.Show("Please enter a valid max plot amount (positive integer)", "Error");
            }
        }

        // NEW FREQUENCY SETTING FUNCTIONALITY
        private void OnFrequencyButtonClicked(object? sender, EventArgs e)
        {
            if (double.TryParse(frequencyTextBox.Text, out double freq) && freq > 0 && freq <= 1000)
            {
                frequency = freq;
                MessageBox.Show($"Frequency set to: {freq} Hz", "Success");
                
                // If currently graphing, restart the timer with new frequency
                if (isGraphing && readTimer != null)
                {
                    readTimer.Stop();
                    readTimer.Interval = 1000.0 / frequency; // Convert Hz to milliseconds
                    readTimer.Start();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid frequency (0.1 - 1000 Hz)", "Error");
            }
        }

        private void OnEnterButtonClicked(object? sender, EventArgs e)
        {
            if (isGraphing)
            {
                StopGraphing();
            }
            else
            {
                StartGraphing();
            }
        }

        private void StartGraphing()
        {
            if (!int.TryParse(comTextBox.Text, out int comPort) || comPort <= 0)
            {
                MessageBox.Show("Please set a valid COM port first", "Error");
                return;
            }

            try
            {
                serialPort = new SerialPort($"COM{comPort}", 115200);
                serialPort.DataReceived += OnDataReceived;
                serialPort.Open();

                // Clear existing data
                xValues.Clear();
                yValues.Clear();

                // Setup timer for data processing at the specified frequency
                readTimer = new System.Timers.Timer(1000.0 / frequency);
                readTimer.Elapsed += ProcessDataQueue;
                readTimer.Start();

                isGraphing = true;
                enterButton.Text = "Stop";
                
                MessageBox.Show($"Started graphing at {frequency} Hz", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start serial communication: {ex.Message}", "Error");
            }
        }

        private void StopGraphing()
        {
            isGraphing = false;
            enterButton.Text = "Start";
            
            readTimer?.Stop();
            readTimer?.Dispose();
            readTimer = null;

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
                serialPort = null;
            }

            MessageBox.Show("Stopped graphing", "Info");
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort == null) return;

            try
            {
                string data = serialPort.ReadLine().Trim();
                
                if (data == "init")
                {
                    // Clear data on init signal
                    lock (lockObject)
                    {
                        dataQueue.Clear();
                    }
                    return;
                }

                lock (lockObject)
                {
                    dataQueue.Enqueue(data);
                }
            }
            catch (Exception ex)
            {
                // Handle timeout or other serial port exceptions silently
                Console.WriteLine($"Serial read error: {ex.Message}");
            }
        }

        private void ProcessDataQueue(object? sender, ElapsedEventArgs e)
        {
            string? dataToProcess = null;
            
            lock (lockObject)
            {
                if (dataQueue.Count > 0)
                {
                    dataToProcess = dataQueue.Dequeue();
                }
            }

            if (dataToProcess != null)
            {
                ProcessData(dataToProcess);
            }
        }

        private void ProcessData(string data)
        {
            string[] parts = data.Split(',');
            if (parts.Length == 2 && 
                double.TryParse(parts[0], out double x) && 
                double.TryParse(parts[1], out double y))
            {
                this.Invoke(new Action(() =>
                {
                    xValues.Add(x);
                    yValues.Add(y);

                    // Remove old data if we exceed max plot amount
                    while (yValues.Count > maxPlotAmount)
                    {
                        xValues.RemoveAt(0);
                        yValues.RemoveAt(0);
                    }
                }));
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            StopGraphing();
            base.OnFormClosed(e);
        }
    }
}
*/