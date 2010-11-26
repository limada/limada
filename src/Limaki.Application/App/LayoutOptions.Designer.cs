namespace Limaki.App {
    partial class LayoutOptions {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.leftRigthButton = new System.Windows.Forms.RadioButton();
            this.topBottomButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DepthFirstButton = new System.Windows.Forms.RadioButton();
            this.BreathFirstButton = new System.Windows.Forms.RadioButton();
            this.centeredBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.leftRigthButton);
            this.groupBox1.Controls.Add(this.topBottomButton);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Orientation";
            // 
            // leftRigthButton
            // 
            this.leftRigthButton.AutoSize = true;
            this.leftRigthButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.leftRigthButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.leftRigthButton.Location = new System.Drawing.Point(96, 19);
            this.leftRigthButton.Name = "leftRigthButton";
            this.leftRigthButton.Size = new System.Drawing.Size(67, 17);
            this.leftRigthButton.TabIndex = 1;
            this.leftRigthButton.TabStop = true;
            this.leftRigthButton.Text = "LeftRight";
            this.leftRigthButton.UseVisualStyleBackColor = true;
            this.leftRigthButton.CheckedChanged += new System.EventHandler(this.leftRigthButton_CheckedChanged);
            // 
            // topBottomButton
            // 
            this.topBottomButton.AutoSize = true;
            this.topBottomButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.topBottomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.topBottomButton.Location = new System.Drawing.Point(6, 19);
            this.topBottomButton.Name = "topBottomButton";
            this.topBottomButton.Size = new System.Drawing.Size(71, 17);
            this.topBottomButton.TabIndex = 0;
            this.topBottomButton.TabStop = true;
            this.topBottomButton.Text = "TopDown";
            this.topBottomButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DepthFirstButton);
            this.groupBox2.Controls.Add(this.BreathFirstButton);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(3, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 51);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Algorithm";
            // 
            // DepthFirstButton
            // 
            this.DepthFirstButton.AutoSize = true;
            this.DepthFirstButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.DepthFirstButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DepthFirstButton.Location = new System.Drawing.Point(96, 19);
            this.DepthFirstButton.Name = "DepthFirstButton";
            this.DepthFirstButton.Size = new System.Drawing.Size(72, 17);
            this.DepthFirstButton.TabIndex = 1;
            this.DepthFirstButton.TabStop = true;
            this.DepthFirstButton.Text = "DepthFirst";
            this.DepthFirstButton.UseVisualStyleBackColor = true;
            // 
            // BreathFirstButton
            // 
            this.BreathFirstButton.AutoSize = true;
            this.BreathFirstButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BreathFirstButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BreathFirstButton.Location = new System.Drawing.Point(6, 19);
            this.BreathFirstButton.Name = "BreathFirstButton";
            this.BreathFirstButton.Size = new System.Drawing.Size(74, 17);
            this.BreathFirstButton.TabIndex = 0;
            this.BreathFirstButton.TabStop = true;
            this.BreathFirstButton.Text = "BreathFirst";
            this.BreathFirstButton.UseVisualStyleBackColor = true;
            this.BreathFirstButton.CheckedChanged += new System.EventHandler(this.BreathFirstButton_CheckedChanged);
            // 
            // centeredBox
            // 
            this.centeredBox.AutoSize = true;
            this.centeredBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.centeredBox.Location = new System.Drawing.Point(9, 126);
            this.centeredBox.Name = "centeredBox";
            this.centeredBox.Size = new System.Drawing.Size(54, 17);
            this.centeredBox.TabIndex = 2;
            this.centeredBox.Text = "Center";
            this.centeredBox.UseVisualStyleBackColor = true;
            this.centeredBox.CheckedChanged += new System.EventHandler(this.centeredBox_CheckedChanged);
            // 
            // LayoutOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.centeredBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "LayoutOptions";
            this.Size = new System.Drawing.Size(192, 153);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton leftRigthButton;
        private System.Windows.Forms.RadioButton topBottomButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton DepthFirstButton;
        private System.Windows.Forms.RadioButton BreathFirstButton;
        private System.Windows.Forms.CheckBox centeredBox;

    }
}