namespace Limaki.UseCases.Winform.Viewers {
    partial class LayoutEditor {
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
            this.label1 = new System.Windows.Forms.Label();
            this.heightDistanceDown = new System.Windows.Forms.NumericUpDown();
            this.widthDistanceUpDown = new System.Windows.Forms.NumericUpDown();
            this.centeredCheckBox = new System.Windows.Forms.CheckBox();
            this.leftRightButton = new System.Windows.Forms.RadioButton();
            this.topBottomButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.backColorTransparency = new System.Windows.Forms.NumericUpDown();
            this.backColorButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.heightDistanceDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthDistanceUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backColorTransparency)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Distance";
            // 
            // heightDistanceDown
            // 
            this.heightDistanceDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.heightDistanceDown.Location = new System.Drawing.Point(153, 14);
            this.heightDistanceDown.Maximum = new decimal(new int[] {
                                                                        10000,
                                                                        0,
                                                                        0,
                                                                        0});
            this.heightDistanceDown.Minimum = new decimal(new int[] {
                                                                        1,
                                                                        0,
                                                                        0,
                                                                        -2147483648});
            this.heightDistanceDown.Name = "heightDistanceDown";
            this.heightDistanceDown.Size = new System.Drawing.Size(60, 16);
            this.heightDistanceDown.TabIndex = 13;
            this.heightDistanceDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.heightDistanceDown.Value = new decimal(new int[] {
                                                                      1,
                                                                      0,
                                                                      0,
                                                                      -2147483648});
            this.heightDistanceDown.ValueChanged += new System.EventHandler(this.distanceUpDown_Click);
            this.heightDistanceDown.Click += new System.EventHandler(this.distanceUpDown_Click);
            // 
            // widthDistanceUpDown
            // 
            this.widthDistanceUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.widthDistanceUpDown.Location = new System.Drawing.Point(92, 14);
            this.widthDistanceUpDown.Maximum = new decimal(new int[] {
                                                                         10000,
                                                                         0,
                                                                         0,
                                                                         0});
            this.widthDistanceUpDown.Minimum = new decimal(new int[] {
                                                                         1,
                                                                         0,
                                                                         0,
                                                                         -2147483648});
            this.widthDistanceUpDown.Name = "widthDistanceUpDown";
            this.widthDistanceUpDown.Size = new System.Drawing.Size(60, 16);
            this.widthDistanceUpDown.TabIndex = 12;
            this.widthDistanceUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.widthDistanceUpDown.Value = new decimal(new int[] {
                                                                       1,
                                                                       0,
                                                                       0,
                                                                       -2147483648});
            this.widthDistanceUpDown.ValueChanged += new System.EventHandler(this.distanceUpDown_Click);
            this.widthDistanceUpDown.Click += new System.EventHandler(this.distanceUpDown_Click);
            // 
            // centeredCheckBox
            // 
            this.centeredCheckBox.AutoSize = true;
            this.centeredCheckBox.FlatAppearance.BorderSize = 0;
            this.centeredCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.centeredCheckBox.Location = new System.Drawing.Point(13, 97);
            this.centeredCheckBox.Name = "centeredCheckBox";
            this.centeredCheckBox.Size = new System.Drawing.Size(66, 17);
            this.centeredCheckBox.TabIndex = 15;
            this.centeredCheckBox.Text = "Centered";
            this.centeredCheckBox.UseVisualStyleBackColor = true;
            this.centeredCheckBox.CheckedChanged += new System.EventHandler(this.centeredCheckBox_CheckedChanged);
            // 
            // leftRightButton
            // 
            this.leftRightButton.AutoSize = true;
            this.leftRightButton.Checked = true;
            this.leftRightButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.leftRightButton.Location = new System.Drawing.Point(18, 19);
            this.leftRightButton.Name = "leftRightButton";
            this.leftRightButton.Size = new System.Drawing.Size(67, 17);
            this.leftRightButton.TabIndex = 16;
            this.leftRightButton.TabStop = true;
            this.leftRightButton.Text = "LeftRight";
            this.leftRightButton.UseVisualStyleBackColor = true;
            this.leftRightButton.Click += new System.EventHandler(this.orientationButton_Click);
            // 
            // topBottomButton
            // 
            this.topBottomButton.AutoSize = true;
            this.topBottomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.topBottomButton.Location = new System.Drawing.Point(100, 19);
            this.topBottomButton.Name = "topBottomButton";
            this.topBottomButton.Size = new System.Drawing.Size(76, 17);
            this.topBottomButton.TabIndex = 17;
            this.topBottomButton.Text = "TopBottom";
            this.topBottomButton.UseVisualStyleBackColor = true;
            this.topBottomButton.Click += new System.EventHandler(this.orientationButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.leftRightButton);
            this.groupBox1.Controls.Add(this.topBottomButton);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(13, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 55);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Orientation";
            // 
            // backColorTransparency
            // 
            this.backColorTransparency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.backColorTransparency.Location = new System.Drawing.Point(106, 124);
            this.backColorTransparency.Maximum = new decimal(new int[] {
                                                                           255,
                                                                           0,
                                                                           0,
                                                                           0});
            this.backColorTransparency.Name = "backColorTransparency";
            this.backColorTransparency.Size = new System.Drawing.Size(37, 16);
            this.backColorTransparency.TabIndex = 20;
            this.backColorTransparency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.backColorTransparency.Value = new decimal(new int[] {
                                                                         255,
                                                                         0,
                                                                         0,
                                                                         0});
            this.backColorTransparency.Click += new System.EventHandler(this.Transparency_ValueChanged);
            // 
            // backColorButton
            // 
            this.backColorButton.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.backColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.backColorButton.Location = new System.Drawing.Point(13, 120);
            this.backColorButton.Name = "backColorButton";
            this.backColorButton.Size = new System.Drawing.Size(75, 23);
            this.backColorButton.TabIndex = 19;
            this.backColorButton.Text = "BackColor";
            this.backColorButton.UseVisualStyleBackColor = true;
            this.backColorButton.Click += new System.EventHandler(this.color_Click);
            // 
            // LayoutEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backColorTransparency);
            this.Controls.Add(this.backColorButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.centeredCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.heightDistanceDown);
            this.Controls.Add(this.widthDistanceUpDown);
            this.Name = "LayoutEditor";
            this.Size = new System.Drawing.Size(227, 161);
            ((System.ComponentModel.ISupportInitialize)(this.heightDistanceDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthDistanceUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backColorTransparency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown heightDistanceDown;
        private System.Windows.Forms.NumericUpDown widthDistanceUpDown;
        private System.Windows.Forms.CheckBox centeredCheckBox;
        private System.Windows.Forms.RadioButton leftRightButton;
        private System.Windows.Forms.RadioButton topBottomButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown backColorTransparency;
        private System.Windows.Forms.Button backColorButton;
    }
}