
namespace mbedSerialReceiver
{
    partial class GatePage
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.COMTextBox = new System.Windows.Forms.TextBox();
            this.COMButton = new System.Windows.Forms.Button();
            this.COMLabel = new System.Windows.Forms.Label();
            this.MaxPlotLabel = new System.Windows.Forms.Label();
            this.MaxPlotButton = new System.Windows.Forms.Button();
            this.MaxPlotTextBox = new System.Windows.Forms.TextBox();
            this.EnterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // COMTextBox
            // 
            this.COMTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.COMTextBox.Location = new System.Drawing.Point(280, 80);
            this.COMTextBox.Name = "COMTextBox";
            this.COMTextBox.Size = new System.Drawing.Size(250, 25);
            this.COMTextBox.TabIndex = 0;
            this.COMTextBox.TextChanged += new System.EventHandler(this.OnMaxPlotTextBoxChanged);
            // 
            // COMButton
            // 
            this.COMButton.Location = new System.Drawing.Point(560, 80);
            this.COMButton.Name = "COMButton";
            this.COMButton.Size = new System.Drawing.Size(75, 23);
            this.COMButton.TabIndex = 2;
            this.COMButton.Text = "確定";
            this.COMButton.UseVisualStyleBackColor = true;
            this.COMButton.Click += new System.EventHandler(this.OnCOMButtonClicked);
            // 
            // COMLabel
            // 
            this.COMLabel.AutoSize = true;
            this.COMLabel.Location = new System.Drawing.Point(180, 80);
            this.COMLabel.Name = "COMLabel";
            this.COMLabel.Size = new System.Drawing.Size(82, 18);
            this.COMLabel.TabIndex = 3;
            this.COMLabel.Text = "COM番号";
            // 
            // MaxPlotLabel
            // 
            this.MaxPlotLabel.AutoSize = true;
            this.MaxPlotLabel.Location = new System.Drawing.Point(150, 180);
            this.MaxPlotLabel.Name = "MaxPlotLabel";
            this.MaxPlotLabel.Size = new System.Drawing.Size(112, 18);
            this.MaxPlotLabel.TabIndex = 6;
            this.MaxPlotLabel.Text = "最大プロット数";
            // 
            // MaxPlotButton
            // 
            this.MaxPlotButton.Location = new System.Drawing.Point(560, 180);
            this.MaxPlotButton.Name = "MaxPlotButton";
            this.MaxPlotButton.Size = new System.Drawing.Size(75, 23);
            this.MaxPlotButton.TabIndex = 5;
            this.MaxPlotButton.Text = "確定";
            this.MaxPlotButton.UseVisualStyleBackColor = true;
            this.MaxPlotButton.Click += new System.EventHandler(this.OnMaxPlotButtonClicked);
            // 
            // MaxPlotTextBox
            // 
            this.MaxPlotTextBox.Location = new System.Drawing.Point(280, 180);
            this.MaxPlotTextBox.Name = "MaxPlotTextBox";
            this.MaxPlotTextBox.Size = new System.Drawing.Size(250, 25);
            this.MaxPlotTextBox.TabIndex = 4;
            this.MaxPlotTextBox.TextChanged += new System.EventHandler(this.OnMaxPlotTextBoxChanged);
            // 
            // EnterButton
            // 
            this.EnterButton.AccessibleName = "EnterButton";
            this.EnterButton.Location = new System.Drawing.Point(400, 300);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(120, 60);
            this.EnterButton.TabIndex = 7;
            this.EnterButton.Text = "開始";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.OnEnterButtonClicked);
            // 
            // GatePage
            // 
            this.ClientSize = new System.Drawing.Size(938, 584);
            this.Controls.Add(this.EnterButton);
            this.Controls.Add(this.MaxPlotLabel);
            this.Controls.Add(this.MaxPlotButton);
            this.Controls.Add(this.MaxPlotTextBox);
            this.Controls.Add(this.COMLabel);
            this.Controls.Add(this.COMButton);
            this.Controls.Add(this.COMTextBox);
            this.Name = "GatePage";
            this.Load += new System.EventHandler(this.GatePage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox COMTextBox;
        private System.Windows.Forms.Button COMButton;
        private System.Windows.Forms.Label COMLabel;
        private System.Windows.Forms.Label MaxPlotLabel;
        private System.Windows.Forms.Button MaxPlotButton;
        private System.Windows.Forms.TextBox MaxPlotTextBox;
        private System.Windows.Forms.Button EnterButton;
    }
}
