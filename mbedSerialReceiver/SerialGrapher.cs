using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Concurrent;


namespace mbedSerialReceiver
{
    partial class SerialGrapher : Form
    {
        readonly SerialPort _serialPort;

        readonly int _limit = 500;
        readonly int _portID = 16;
        readonly int _frequency = 10; // Default frequency in Hz

        ChartValues<float> _rpm;
        List<string> _time;
        System.Windows.Forms.Timer _dataTimer;
        ConcurrentQueue<(float time, float rpm)> _dataQueue;

        public SerialGrapher(int portID, int maxPlot, int frequency)
        {
            Console.WriteLine("try to initialize Constructure.");
            _limit = maxPlot;
            _portID = portID;
            _frequency = frequency;

            InitializeComponent();

            _serialPort = new SerialPort("COM" + _portID, 115200); // 適切なCOMポートに変更
            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.Open();

            //cartesianChart = cartesianChart; /*new LiveCharts.WinForms.CartesianChart();*/
            _time = new List<string>();
            _rpm = new ChartValues<float>();
            _dataQueue = new ConcurrentQueue<(float, float)>();

            // Setup timer for controlled data display frequency
            _dataTimer = new System.Windows.Forms.Timer();
            _dataTimer.Interval = 1000 / _frequency; // Convert Hz to milliseconds
            _dataTimer.Tick += DataTimer_Tick;
            _dataTimer.Start();

            cartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "velocity [m/s]",
                    Values = _rpm
                }
            };

            cartesianChart.AxisX.Add(new Axis
            {
                Title = "Time [s]",
                Labels = _time
            });
            cartesianChart.AxisY.Add(new Axis { Title = "velocity [m/s]" });
            cartesianChart.Dock = DockStyle.Fill;

            this.Controls.Add(cartesianChart);
            this.Text = "Real-Time Graph";
            this.Width = 800;
            this.Height = 600;
        }

        private void DataTimer_Tick(object sender, EventArgs e)
        {
            if (_dataQueue.TryDequeue(out (float time, float rpm) data))
            {
                if (_rpm.Count >= _limit)
                {
                    _rpm.RemoveAt(0);
                    _time.RemoveAt(0);
                }

                _rpm.Add(data.rpm);
                _time.Add(data.time.ToString("F2"));
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string line = _serialPort.ReadLine(); // 行単位でデータを受信
            if (line == "init")
            {
                this.Invoke((MethodInvoker)delegate
                {
                    _rpm.Clear();
                    _time.Clear();
                });
                return;
            }

            string[] data = line.Trim().Split(',');
            //Console.WriteLine("length : " + data.Length);

            if (data.Length != 2) return;
            if (!float.TryParse(data[0], out float time)) return;
            if (!float.TryParse(data[1], out float rpm)) return;
            
            //Console.WriteLine("get Serial data : " + data[0] + ", " + data[1]);

            // Queue the data for processing at controlled frequency
            _dataQueue.Enqueue((time, rpm));
        }
    }
}
