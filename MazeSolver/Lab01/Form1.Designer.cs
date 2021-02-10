namespace Lab01
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.UI_btn_Load = new System.Windows.Forms.Button();
            this.UI_BTN_SColor = new System.Windows.Forms.Button();
            this.UI_btn_DColor = new System.Windows.Forms.Button();
            this.UI_btn_Solve = new System.Windows.Forms.Button();
            this.UI_NUD_Speed = new System.Windows.Forms.NumericUpDown();
            this.LB_speed = new System.Windows.Forms.Label();
            this.UI_LB_MessageViewer = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.UI_NUD_Speed)).BeginInit();
            this.SuspendLayout();
            // 
            // UI_btn_Load
            // 
            this.UI_btn_Load.Location = new System.Drawing.Point(13, 13);
            this.UI_btn_Load.Name = "UI_btn_Load";
            this.UI_btn_Load.Size = new System.Drawing.Size(286, 23);
            this.UI_btn_Load.TabIndex = 0;
            this.UI_btn_Load.Text = "Load Maze";
            this.UI_btn_Load.UseVisualStyleBackColor = true;
            // 
            // UI_BTN_SColor
            // 
            this.UI_BTN_SColor.BackColor = System.Drawing.Color.Fuchsia;
            this.UI_BTN_SColor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.UI_BTN_SColor.Location = new System.Drawing.Point(13, 42);
            this.UI_BTN_SColor.Name = "UI_BTN_SColor";
            this.UI_BTN_SColor.Size = new System.Drawing.Size(99, 23);
            this.UI_BTN_SColor.TabIndex = 1;
            this.UI_BTN_SColor.Text = "Solve Color";
            this.UI_BTN_SColor.UseVisualStyleBackColor = false;
            // 
            // UI_btn_DColor
            // 
            this.UI_btn_DColor.BackColor = System.Drawing.Color.LimeGreen;
            this.UI_btn_DColor.Location = new System.Drawing.Point(13, 72);
            this.UI_btn_DColor.Name = "UI_btn_DColor";
            this.UI_btn_DColor.Size = new System.Drawing.Size(99, 23);
            this.UI_btn_DColor.TabIndex = 2;
            this.UI_btn_DColor.Text = "Dead Color";
            this.UI_btn_DColor.UseVisualStyleBackColor = false;
            // 
            // UI_btn_Solve
            // 
            this.UI_btn_Solve.Enabled = false;
            this.UI_btn_Solve.Location = new System.Drawing.Point(118, 42);
            this.UI_btn_Solve.Name = "UI_btn_Solve";
            this.UI_btn_Solve.Size = new System.Drawing.Size(181, 94);
            this.UI_btn_Solve.TabIndex = 3;
            this.UI_btn_Solve.Text = "Solve";
            this.UI_btn_Solve.UseVisualStyleBackColor = true;
            // 
            // UI_NUD_Speed
            // 
            this.UI_NUD_Speed.Location = new System.Drawing.Point(62, 102);
            this.UI_NUD_Speed.Name = "UI_NUD_Speed";
            this.UI_NUD_Speed.Size = new System.Drawing.Size(50, 20);
            this.UI_NUD_Speed.TabIndex = 4;
            // 
            // LB_speed
            // 
            this.LB_speed.AutoSize = true;
            this.LB_speed.Location = new System.Drawing.Point(21, 102);
            this.LB_speed.Name = "LB_speed";
            this.LB_speed.Size = new System.Drawing.Size(38, 13);
            this.LB_speed.TabIndex = 5;
            this.LB_speed.Text = "Speed";
            // 
            // UI_LB_MessageViewer
            // 
            this.UI_LB_MessageViewer.FormattingEnabled = true;
            this.UI_LB_MessageViewer.Location = new System.Drawing.Point(12, 142);
            this.UI_LB_MessageViewer.Name = "UI_LB_MessageViewer";
            this.UI_LB_MessageViewer.Size = new System.Drawing.Size(286, 173);
            this.UI_LB_MessageViewer.TabIndex = 6;
            // 
            // Form1
            // 
            this.AcceptButton = this.UI_btn_Load;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 324);
            this.Controls.Add(this.UI_LB_MessageViewer);
            this.Controls.Add(this.LB_speed);
            this.Controls.Add(this.UI_NUD_Speed);
            this.Controls.Add(this.UI_btn_Solve);
            this.Controls.Add(this.UI_btn_DColor);
            this.Controls.Add(this.UI_BTN_SColor);
            this.Controls.Add(this.UI_btn_Load);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lab 01";
            ((System.ComponentModel.ISupportInitialize)(this.UI_NUD_Speed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UI_btn_Load;
        private System.Windows.Forms.Button UI_BTN_SColor;
        private System.Windows.Forms.Button UI_btn_DColor;
        private System.Windows.Forms.Button UI_btn_Solve;
        private System.Windows.Forms.NumericUpDown UI_NUD_Speed;
        private System.Windows.Forms.Label LB_speed;
        private System.Windows.Forms.ListBox UI_LB_MessageViewer;
    }
}

