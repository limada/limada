namespace Limaki.App {
    partial class StyleEditor {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.fillColorButton = new System.Windows.Forms.Button();
            this.fillTransparency = new System.Windows.Forms.NumericUpDown();
            this.textTransparency = new System.Windows.Forms.NumericUpDown();
            this.textColorButton = new System.Windows.Forms.Button();
            this.penTransparency = new System.Windows.Forms.NumericUpDown();
            this.penColorButton = new System.Windows.Forms.Button();
            this.fontButton = new System.Windows.Forms.Button();
            this.penThicknessUpDown = new System.Windows.Forms.NumericUpDown();
            this.widthAutoSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.heightAutoSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.paintDataCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fillTransparency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTransparency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.penTransparency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.penThicknessUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthAutoSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightAutoSizeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // fillColorButton
            // 
            this.fillColorButton.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.fillColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fillColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fillColorButton.Location = new System.Drawing.Point(16, 13);
            this.fillColorButton.Name = "fillColorButton";
            this.fillColorButton.Size = new System.Drawing.Size(75, 23);
            this.fillColorButton.TabIndex = 0;
            this.fillColorButton.Text = "Fill";
            this.fillColorButton.UseVisualStyleBackColor = true;
            this.fillColorButton.Click += new System.EventHandler(this.color_Click);
            // 
            // fillTransparency
            // 
            this.fillTransparency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fillTransparency.Location = new System.Drawing.Point(109, 17);
            this.fillTransparency.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fillTransparency.Name = "fillTransparency";
            this.fillTransparency.Size = new System.Drawing.Size(37, 16);
            this.fillTransparency.TabIndex = 1;
            this.fillTransparency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fillTransparency.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.fillTransparency.ValueChanged += new System.EventHandler(this.Transparency_ValueChanged);
            this.fillTransparency.Click += new System.EventHandler(this.Transparency_ValueChanged);
            // 
            // textTransparency
            // 
            this.textTransparency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textTransparency.Location = new System.Drawing.Point(109, 87);
            this.textTransparency.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.textTransparency.Name = "textTransparency";
            this.textTransparency.Size = new System.Drawing.Size(37, 16);
            this.textTransparency.TabIndex = 3;
            this.textTransparency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textTransparency.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.textTransparency.ValueChanged += new System.EventHandler(this.Transparency_ValueChanged);
            this.textTransparency.Click += new System.EventHandler(this.Transparency_ValueChanged);
            // 
            // textColorButton
            // 
            this.textColorButton.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.textColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.textColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textColorButton.Location = new System.Drawing.Point(162, 87);
            this.textColorButton.Name = "textColorButton";
            this.textColorButton.Size = new System.Drawing.Size(50, 16);
            this.textColorButton.TabIndex = 2;
            this.textColorButton.UseVisualStyleBackColor = true;
            this.textColorButton.Click += new System.EventHandler(this.color_Click);
            // 
            // penTransparency
            // 
            this.penTransparency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.penTransparency.Location = new System.Drawing.Point(109, 46);
            this.penTransparency.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.penTransparency.Name = "penTransparency";
            this.penTransparency.Size = new System.Drawing.Size(37, 16);
            this.penTransparency.TabIndex = 5;
            this.penTransparency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.penTransparency.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.penTransparency.ValueChanged += new System.EventHandler(this.Transparency_ValueChanged);
            this.penTransparency.Click += new System.EventHandler(this.Transparency_ValueChanged);
            // 
            // penColorButton
            // 
            this.penColorButton.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.penColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.penColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.penColorButton.Location = new System.Drawing.Point(16, 42);
            this.penColorButton.Name = "penColorButton";
            this.penColorButton.Size = new System.Drawing.Size(75, 23);
            this.penColorButton.TabIndex = 4;
            this.penColorButton.Text = "Pen";
            this.penColorButton.UseVisualStyleBackColor = true;
            this.penColorButton.Click += new System.EventHandler(this.color_Click);
            // 
            // fontButton
            // 
            this.fontButton.AutoSize = true;
            this.fontButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fontButton.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.fontButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fontButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fontButton.Location = new System.Drawing.Point(16, 81);
            this.fontButton.MaximumSize = new System.Drawing.Size(75, 0);
            this.fontButton.MinimumSize = new System.Drawing.Size(75, 0);
            this.fontButton.Name = "fontButton";
            this.fontButton.Size = new System.Drawing.Size(75, 25);
            this.fontButton.TabIndex = 6;
            this.fontButton.Text = "Font";
            this.fontButton.UseVisualStyleBackColor = true;
            this.fontButton.Click += new System.EventHandler(this.fontButton_Click);
            // 
            // penThicknessUpDown
            // 
            this.penThicknessUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.penThicknessUpDown.Location = new System.Drawing.Point(175, 46);
            this.penThicknessUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.penThicknessUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.penThicknessUpDown.Name = "penThicknessUpDown";
            this.penThicknessUpDown.Size = new System.Drawing.Size(37, 16);
            this.penThicknessUpDown.TabIndex = 7;
            this.penThicknessUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.penThicknessUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.penThicknessUpDown.ValueChanged += new System.EventHandler(this.penThicknessUpDown_Click);
            this.penThicknessUpDown.Click += new System.EventHandler(this.penThicknessUpDown_Click);
            // 
            // widthAutoSizeUpDown
            // 
            this.widthAutoSizeUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.widthAutoSizeUpDown.Location = new System.Drawing.Point(109, 120);
            this.widthAutoSizeUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.widthAutoSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.widthAutoSizeUpDown.Name = "widthAutoSizeUpDown";
            this.widthAutoSizeUpDown.Size = new System.Drawing.Size(50, 16);
            this.widthAutoSizeUpDown.TabIndex = 8;
            this.widthAutoSizeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.widthAutoSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.widthAutoSizeUpDown.ValueChanged += new System.EventHandler(this.AutoSizeUpDown_Click);
            this.widthAutoSizeUpDown.Click += new System.EventHandler(this.AutoSizeUpDown_Click);
            // 
            // heightAutoSizeUpDown
            // 
            this.heightAutoSizeUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.heightAutoSizeUpDown.Location = new System.Drawing.Point(162, 121);
            this.heightAutoSizeUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.heightAutoSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.heightAutoSizeUpDown.Name = "heightAutoSizeUpDown";
            this.heightAutoSizeUpDown.Size = new System.Drawing.Size(50, 16);
            this.heightAutoSizeUpDown.TabIndex = 9;
            this.heightAutoSizeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.heightAutoSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.heightAutoSizeUpDown.ValueChanged += new System.EventHandler(this.AutoSizeUpDown_Click);
            this.heightAutoSizeUpDown.Click += new System.EventHandler(this.AutoSizeUpDown_Click);
            // 
            // paintDataCheckBox
            // 
            this.paintDataCheckBox.AutoSize = true;
            this.paintDataCheckBox.FlatAppearance.BorderSize = 0;
            this.paintDataCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paintDataCheckBox.Location = new System.Drawing.Point(16, 151);
            this.paintDataCheckBox.Name = "paintDataCheckBox";
            this.paintDataCheckBox.Size = new System.Drawing.Size(68, 17);
            this.paintDataCheckBox.TabIndex = 10;
            this.paintDataCheckBox.Text = "Paintdata";
            this.paintDataCheckBox.UseVisualStyleBackColor = true;
            this.paintDataCheckBox.CheckedChanged += new System.EventHandler(this.paintDataCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Label Size";
            // 
            // StyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.paintDataCheckBox);
            this.Controls.Add(this.heightAutoSizeUpDown);
            this.Controls.Add(this.widthAutoSizeUpDown);
            this.Controls.Add(this.penThicknessUpDown);
            this.Controls.Add(this.fontButton);
            this.Controls.Add(this.penTransparency);
            this.Controls.Add(this.penColorButton);
            this.Controls.Add(this.textTransparency);
            this.Controls.Add(this.textColorButton);
            this.Controls.Add(this.fillTransparency);
            this.Controls.Add(this.fillColorButton);
            this.Name = "StyleEditor";
            this.Size = new System.Drawing.Size(227, 180);
            ((System.ComponentModel.ISupportInitialize)(this.fillTransparency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTransparency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.penTransparency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.penThicknessUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthAutoSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightAutoSizeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Button fillColorButton;
        private System.Windows.Forms.NumericUpDown fillTransparency;
        private System.Windows.Forms.NumericUpDown textTransparency;
        private System.Windows.Forms.Button textColorButton;
        private System.Windows.Forms.NumericUpDown penTransparency;
        private System.Windows.Forms.Button penColorButton;
        private System.Windows.Forms.Button fontButton;
        private System.Windows.Forms.NumericUpDown penThicknessUpDown;
        private System.Windows.Forms.NumericUpDown widthAutoSizeUpDown;
        private System.Windows.Forms.NumericUpDown heightAutoSizeUpDown;
        private System.Windows.Forms.CheckBox paintDataCheckBox;
        private System.Windows.Forms.Label label1;
    }
}
