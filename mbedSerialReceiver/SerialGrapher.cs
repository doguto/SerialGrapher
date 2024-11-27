﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;


namespace mbedSerialReceiver
{
    partial class SerialGrapher : Form
    {
        readonly SerialPort _serialPort;

        readonly int _limit = 500;
        readonly int _portID = 16;

        ChartValues<float> _rpm;
        List<string> _time;

        public SerialGrapher(int portID, int maxPlot)
        {
            Console.WriteLine("try to initialize Constructure.");
            _limit = maxPlot;
            _portID = portID;

            InitializeComponent();

            _serialPort = new SerialPort("COM" + _portID, 115200); // 適切なCOMポートに変更
            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.Open();

            //cartesianChart = cartesianChart; /*new LiveCharts.WinForms.CartesianChart();*/
            _time = new List<string>();
            _rpm = new ChartValues<float>();

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

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string line = _serialPort.ReadLine(); // 行単位でデータを受信
            if (line == "init")
            {
                _rpm.Clear();
                _time.Clear();
                return;
            }

            string[] data = line.Trim().Split(',');
            //Console.WriteLine("length : " + data.Length);

            if (data.Length != 2) return;
            if (!float.TryParse(data[0], out float time)) return;
            if (!float.TryParse(data[1], out float rpm)) return;
            
            //Console.WriteLine("get Serial data : " + data[0] + ", " + data[1]);

            this.Invoke((MethodInvoker)delegate
            {
                if (_rpm.Count > _limit) return;

                _rpm.Add(rpm);
                _time.Add(data[0]);
            });
        }
    }
}
