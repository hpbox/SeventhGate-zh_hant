namespace SeventhGate
{
	partial class DebugWindow
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
			if(disposing && (components != null))
			{
				components.Dispose();
				this.FormClosing -= this.DebugWindowClosing;
				updateStringCallBack -= _OnUpdateDebugBox;
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated acm_code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the acm_code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugWindow));
			this.textBoxDebug = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBoxDebug
			// 
			this.textBoxDebug.BackColor = System.Drawing.Color.White;
			this.textBoxDebug.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.textBoxDebug.ForeColor = System.Drawing.Color.Purple;
			this.textBoxDebug.HideSelection = false;
			this.textBoxDebug.Location = new System.Drawing.Point(0, 0);
			this.textBoxDebug.MaxLength = 0;
			this.textBoxDebug.Multiline = true;
			this.textBoxDebug.Name = "textBoxDebug";
			this.textBoxDebug.ReadOnly = true;
			this.textBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxDebug.Size = new System.Drawing.Size(892, 370);
			this.textBoxDebug.TabIndex = 0;
			this.textBoxDebug.WordWrap = false;
			// 
			// DebugWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 370);
			this.Controls.Add(this.textBoxDebug);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DebugWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DebugWindow";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxDebug;
	}
}