using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbedSerialReceiver
{
    internal partial class GatePage : Form
    {
        int _comID = -1;
        int _maxPlotAmount = -1;

        public GatePage()
        {
            InitializeComponent();
        }

        private void OnCOMButtonClicked(object sender, EventArgs e) // COM button
        {
            Console.WriteLine("COMButton clicked.");
            string comText = COMTextBox.Text;
            if (comText == null) return;
            if (!int.TryParse(comText, out int comNumber)) return;

            _comID = comNumber;
        }

        private void OnCOMTextBoxChanged(object sender, EventArgs e) // COM text box
        {

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

        private void OnEnterButtonClicked(object sender, EventArgs e) // Enter button
        {
            if (_comID <= 0) return;
            if (_maxPlotAmount <= 0) return;

            SerialGrapher serialGrapher = new SerialGrapher(_comID, _maxPlotAmount);
            this.Hide();
            serialGrapher.ShowDialog();
        }

        private void GatePage_Load(object sender, EventArgs e)
        {

        }
    }
}
