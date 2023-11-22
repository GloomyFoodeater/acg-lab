
namespace UI
{

    partial class RenderForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            PictureBox = new PictureBox();
            ScaleValue = new TextBox();
            PoligonSize = new TextBox();
            RenderButton = new Button();
            FilePath = new TextBox();
            LabComboBox = new ComboBox();
            panel1 = new Panel();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)PictureBox).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // PictureBox
            // 
            PictureBox.BackColor = SystemColors.ScrollBar;
            PictureBox.Dock = DockStyle.Fill;
            PictureBox.Location = new Point(0, 0);
            PictureBox.Name = "PictureBox";
            PictureBox.Size = new Size(884, 861);
            PictureBox.TabIndex = 1;
            PictureBox.TabStop = false;
            PictureBox.SizeChanged += PictureBox_SizeChanged;
            PictureBox.MouseDown += PictureBox_MouseDown;
            PictureBox.MouseMove += PictureBox_MouseMove;
            PictureBox.MouseUp += PictureBox_MouseUp;
            // 
            // ScaleValue
            // 
            ScaleValue.BackColor = SystemColors.InactiveCaption;
            ScaleValue.Enabled = false;
            ScaleValue.ForeColor = SystemColors.ActiveCaption;
            ScaleValue.Location = new Point(12, 44);
            ScaleValue.Name = "ScaleValue";
            ScaleValue.Size = new Size(174, 23);
            ScaleValue.TabIndex = 2;
            ScaleValue.TextChanged += StretchTextbox_OnChange;
            // 
            // PoligonSize
            // 
            PoligonSize.BackColor = SystemColors.InactiveCaption;
            PoligonSize.Enabled = false;
            PoligonSize.ForeColor = SystemColors.ActiveCaption;
            PoligonSize.HideSelection = false;
            PoligonSize.Location = new Point(12, 73);
            PoligonSize.Name = "PoligonSize";
            PoligonSize.Size = new Size(174, 23);
            PoligonSize.TabIndex = 3;
            PoligonSize.TextChanged += StretchTextbox_OnChange;
            // 
            // RenderButton
            // 
            RenderButton.BackColor = SystemColors.HotTrack;
            RenderButton.ForeColor = SystemColors.ActiveCaption;
            RenderButton.Location = new Point(208, 72);
            RenderButton.Name = "RenderButton";
            RenderButton.Size = new Size(100, 36);
            RenderButton.TabIndex = 4;
            RenderButton.Text = "Открыть";
            RenderButton.UseVisualStyleBackColor = false;
            RenderButton.Click += OpenButton_Click;
            // 
            // FilePath
            // 
            FilePath.BackColor = SystemColors.InactiveCaption;
            FilePath.Enabled = false;
            FilePath.ForeColor = SystemColors.ActiveCaption;
            FilePath.Location = new Point(12, 15);
            FilePath.Name = "FilePath";
            FilePath.Size = new Size(174, 23);
            FilePath.TabIndex = 5;
            FilePath.TextChanged += StretchTextbox_OnChange;
            // 
            // LabComboBox
            // 
            LabComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            LabComboBox.ForeColor = SystemColors.ActiveCaption;
            LabComboBox.FormattingEnabled = true;
            LabComboBox.Items.AddRange(new object[] { "Lab1", "Lab2", "Lab3", "Lab4" });
            LabComboBox.Location = new Point(362, 80);
            LabComboBox.Name = "LabComboBox";
            LabComboBox.Size = new Size(121, 23);
            LabComboBox.TabIndex = 6;
            LabComboBox.SelectedIndexChanged += StyleComboBox_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(LabComboBox);
            panel1.Controls.Add(RenderButton);
            panel1.Controls.Add(FilePath);
            panel1.Controls.Add(ScaleValue);
            panel1.Controls.Add(PoligonSize);
            panel1.Dock = DockStyle.Bottom;
            panel1.ForeColor = SystemColors.ActiveCaption;
            panel1.Location = new Point(0, 732);
            panel1.Name = "panel1";
            panel1.Size = new Size(884, 129);
            panel1.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(208, 15);
            button1.Name = "button1";
            button1.Size = new Size(183, 28);
            button1.TabIndex = 7;
            button1.Text = "Добавить источник света";
            button1.UseVisualStyleBackColor = true;
            // 
            // RenderForm
            // 
            ClientSize = new Size(884, 861);
            Controls.Add(panel1);
            Controls.Add(PictureBox);
            KeyPreview = true;
            Name = "RenderForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "3D viewer";
            WindowState = FormWindowState.Maximized;
            KeyDown += PictureBox_MoveModel;
            ((System.ComponentModel.ISupportInitialize)PictureBox).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        private PictureBox PictureBox;
        private TextBox ScaleValue;
        private TextBox PoligonSize;
        private Button RenderButton;
        private TextBox FilePath;
        private ComboBox LabComboBox;
        private Panel panel1;
        private Button button1;
    }
}