using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace mbedSerialReceiver
{
    internal partial class GatePage : Form
    {
        int _comID = -1;
        int _maxPlotAmount = -1;
        int _frequency = -1;

        public GatePage()
        {
            InitializeComponent();
            PopulateCOMPorts();
        }

        private void PopulateCOMPorts()
        {
            // Get available COM ports and populate the ComboBox
            string[] availablePorts = SerialPort.GetPortNames();
            COMComboBox.Items.Clear();
            
            foreach (string port in availablePorts)
            {
                COMComboBox.Items.Add(port);
            }
            
            // Select the first port if available
            if (COMComboBox.Items.Count > 0)
            {
                COMComboBox.SelectedIndex = 0;
            }
        }

        private void OnCOMComboBoxChanged(object sender, EventArgs e) // COM combo box
        {
            if (COMComboBox.SelectedItem != null)
            {
                string selectedPort = COMComboBox.SelectedItem.ToString();
                // Extract port number from port name (e.g., "COM16" -> 16)
                if (selectedPort.StartsWith("COM") && selectedPort.Length > 3)
                {
                    string portNumberStr = selectedPort.Substring(3);
                    if (int.TryParse(portNumberStr, out int portNumber))
                    {
                        _comID = portNumber;
                        Console.WriteLine($"Selected COM port: {selectedPort} (ID: {_comID})");
                    }
                }
            }
        }

        private void OnCOMButtonClicked(object sender, EventArgs e) // COM button
        {
            Console.WriteLine("COMButton clicked.");
            // Port is already set by ComboBox selection, but we can refresh the list
            PopulateCOMPorts();
        }

        private void OnCOMTextBoxChanged(object sender, EventArgs e) // COM text box (removed - replaced with ComboBox)
        {
            // This method is no longer used since we replaced TextBox with ComboBox
        }

        private void OnMaxPlotButtonClicked(object sender, EventArgs e) // Max plot button
        {
            Console.WriteLine("MaxPlotButton clicked.");
            string plotText = MaxPlotTextBox.Text;
            if (plotText == null) return;
            if (!int.TryParse(plotText, out int plotAmount)) return;

            _maxPlotAmount = plotAmount;
        }

        private void OnMaxPlotTextBoxChanged(object sender, EventArgs e) // Max plot text box
        {

        }

        private void OnFrequencyButtonClicked(object sender, EventArgs e) // Frequency button
        {
            Console.WriteLine("FrequencyButton clicked.");
            string frequencyText = FrequencyTextBox.Text;
            if (frequencyText == null) return;
            if (!int.TryParse(frequencyText, out int frequency)) return;
            if (frequency <= 0) return; // Valid range: > 0

            _frequency = frequency;
        }

        private void OnFrequencyTextBoxChanged(object sender, EventArgs e) // Frequency text box
        {

        }

        private void OnEnterButtonClicked(object sender, EventArgs e) // Enter button
        {
            if (_comID <= 0) return;
            if (_maxPlotAmount <= 0) return;
            if (_frequency <= 0) return;

            SerialGrapher serialGrapher = new SerialGrapher(_comID, _maxPlotAmount, _frequency);
            this.Hide();
            serialGrapher.ShowDialog();
        }

        private void GatePage_Load(object sender, EventArgs e)
        {
            // Refresh COM ports when form loads
            PopulateCOMPorts();
        }
    }
}
