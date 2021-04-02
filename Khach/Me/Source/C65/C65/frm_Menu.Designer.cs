namespace C65
{
    partial class frm_Menu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Menu));
            this.buttonTaoToa = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTaoToa
            // 
            this.buttonTaoToa.BackColor = System.Drawing.Color.Firebrick;
            this.buttonTaoToa.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonTaoToa.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.buttonTaoToa.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonTaoToa.Location = new System.Drawing.Point(12, 21);
            this.buttonTaoToa.Name = "buttonTaoToa";
            this.buttonTaoToa.Size = new System.Drawing.Size(266, 61);
            this.buttonTaoToa.TabIndex = 167;
            this.buttonTaoToa.Text = "TẠO TOA";
            this.buttonTaoToa.UseVisualStyleBackColor = false;
            // 
            // frm_Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(290, 343);
            this.Controls.Add(this.buttonTaoToa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTaoToa;
    }
}