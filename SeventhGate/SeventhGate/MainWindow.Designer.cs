namespace SeventhGate
{
	partial class MainWindow
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
			//wlanWrapper.Dispose();

			if(disposing && (components != null))
			{
				components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMaxPeersNum = new System.Windows.Forms.TextBox();
            this.buttonPasswordChar = new System.Windows.Forms.Button();
            this.labelSharingDescription = new System.Windows.Forms.Label();
            this.textBoxNetworkPass = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNetworkName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxClients = new System.Windows.Forms.ListBox();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            this.checkBoxAnimateTray = new System.Windows.Forms.CheckBox();
            this.checkBoxCheckUpdatesAtStart = new System.Windows.Forms.CheckBox();
            this.checkBoxDebugWindow = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageBasic = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonConnectionsAll = new System.Windows.Forms.RadioButton();
            this.radioButtonConnectionsActive = new System.Windows.Forms.RadioButton();
            this.listBoxNetworkConnections = new System.Windows.Forms.ListBox();
            this.tabAdvanced1 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.tabAdvanced2 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.panelTwitter = new System.Windows.Forms.Panel();
            this.labelProduct = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelAssemblyTitle = new System.Windows.Forms.Label();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.toolTipTwitter = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipOnline = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageBasic.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabAdvanced1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabAdvanced2.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.BackColor = System.Drawing.Color.Silver;
            this.buttonStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.buttonStartStop.Location = new System.Drawing.Point(282, 66);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(63, 21);
            this.buttonStartStop.TabIndex = 7;
            this.buttonStartStop.Text = "啟動";
            this.buttonStartStop.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxMaxPeersNum);
            this.groupBox2.Controls.Add(this.buttonPasswordChar);
            this.groupBox2.Controls.Add(this.labelSharingDescription);
            this.groupBox2.Controls.Add(this.textBoxNetworkPass);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxNetworkName);
            this.groupBox2.Controls.Add(this.buttonStartStop);
            this.groupBox2.Location = new System.Drawing.Point(0, 123);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(351, 115);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "無線網路設定";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(282, 94);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(63, 15);
            this.progressBar.TabIndex = 9;
            this.progressBar.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(3, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "用戶數上限";
            // 
            // textBoxMaxPeersNum
            // 
            this.textBoxMaxPeersNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMaxPeersNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxMaxPeersNum.Location = new System.Drawing.Point(81, 66);
            this.textBoxMaxPeersNum.MaxLength = 2;
            this.textBoxMaxPeersNum.Name = "textBoxMaxPeersNum";
            this.textBoxMaxPeersNum.Size = new System.Drawing.Size(35, 21);
            this.textBoxMaxPeersNum.TabIndex = 6;
            this.textBoxMaxPeersNum.Text = "5";
            // 
            // buttonPasswordChar
            // 
            this.buttonPasswordChar.BackColor = System.Drawing.Color.Silver;
            this.buttonPasswordChar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonPasswordChar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.buttonPasswordChar.Location = new System.Drawing.Point(282, 39);
            this.buttonPasswordChar.Name = "buttonPasswordChar";
            this.buttonPasswordChar.Size = new System.Drawing.Size(63, 21);
            this.buttonPasswordChar.TabIndex = 4;
            this.buttonPasswordChar.Text = "隱藏";
            this.buttonPasswordChar.UseVisualStyleBackColor = false;
            // 
            // labelSharingDescription
            // 
            this.labelSharingDescription.AutoSize = true;
            this.labelSharingDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSharingDescription.ForeColor = System.Drawing.Color.Purple;
            this.labelSharingDescription.Location = new System.Drawing.Point(3, 94);
            this.labelSharingDescription.Name = "labelSharingDescription";
            this.labelSharingDescription.Size = new System.Drawing.Size(0, 13);
            this.labelSharingDescription.TabIndex = 8;
            // 
            // textBoxNetworkPass
            // 
            this.textBoxNetworkPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNetworkPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxNetworkPass.Location = new System.Drawing.Point(81, 39);
            this.textBoxNetworkPass.MaxLength = 80;
            this.textBoxNetworkPass.Name = "textBoxNetworkPass";
            this.textBoxNetworkPass.Size = new System.Drawing.Size(195, 21);
            this.textBoxNetworkPass.TabIndex = 3;
            this.textBoxNetworkPass.Text = "myp1a2s3s4w5o6r7d8";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(3, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "密碼";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "網路名稱";
            // 
            // textBoxNetworkName
            // 
            this.textBoxNetworkName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNetworkName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxNetworkName.Location = new System.Drawing.Point(81, 15);
            this.textBoxNetworkName.MaxLength = 40;
            this.textBoxNetworkName.Name = "textBoxNetworkName";
            this.textBoxNetworkName.Size = new System.Drawing.Size(264, 21);
            this.textBoxNetworkName.TabIndex = 1;
            this.textBoxNetworkName.Text = "MyWirelessNetwork";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxClients);
            this.groupBox3.Location = new System.Drawing.Point(0, 308);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(351, 106);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "連線的使用者";
            // 
            // listBoxClients
            // 
            this.listBoxClients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxClients.FormattingEnabled = true;
            this.listBoxClients.Location = new System.Drawing.Point(6, 18);
            this.listBoxClients.Name = "listBoxClients";
            this.listBoxClients.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBoxClients.Size = new System.Drawing.Size(339, 80);
            this.listBoxClients.TabIndex = 0;
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.BackColor = System.Drawing.Color.White;
            this.textBoxStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxStatus.ForeColor = System.Drawing.Color.Purple;
            this.textBoxStatus.Location = new System.Drawing.Point(0, 443);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(359, 134);
            this.textBoxStatus.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxStartMinimized);
            this.groupBox4.Controls.Add(this.checkBoxAnimateTray);
            this.groupBox4.Controls.Add(this.checkBoxCheckUpdatesAtStart);
            this.groupBox4.Controls.Add(this.checkBoxDebugWindow);
            this.groupBox4.Location = new System.Drawing.Point(0, 241);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(351, 64);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "程式選項";
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(157, 18);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(120, 16);
            this.checkBoxStartMinimized.TabIndex = 1;
            this.checkBoxStartMinimized.Text = "啟動程式時最小化";
            this.checkBoxStartMinimized.UseVisualStyleBackColor = true;
            // 
            // checkBoxAnimateTray
            // 
            this.checkBoxAnimateTray.AutoSize = true;
            this.checkBoxAnimateTray.Checked = true;
            this.checkBoxAnimateTray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAnimateTray.Enabled = false;
            this.checkBoxAnimateTray.Location = new System.Drawing.Point(6, 39);
            this.checkBoxAnimateTray.Name = "checkBoxAnimateTray";
            this.checkBoxAnimateTray.Size = new System.Drawing.Size(108, 16);
            this.checkBoxAnimateTray.TabIndex = 2;
            this.checkBoxAnimateTray.Text = "系統列圖示動畫";
            this.checkBoxAnimateTray.UseVisualStyleBackColor = true;
            // 
            // checkBoxCheckUpdatesAtStart
            // 
            this.checkBoxCheckUpdatesAtStart.AutoSize = true;
            this.checkBoxCheckUpdatesAtStart.Checked = true;
            this.checkBoxCheckUpdatesAtStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCheckUpdatesAtStart.Location = new System.Drawing.Point(6, 18);
            this.checkBoxCheckUpdatesAtStart.Name = "checkBoxCheckUpdatesAtStart";
            this.checkBoxCheckUpdatesAtStart.Size = new System.Drawing.Size(132, 16);
            this.checkBoxCheckUpdatesAtStart.TabIndex = 0;
            this.checkBoxCheckUpdatesAtStart.Text = "啟動程式時檢查更新";
            this.checkBoxCheckUpdatesAtStart.UseVisualStyleBackColor = true;
            // 
            // checkBoxDebugWindow
            // 
            this.checkBoxDebugWindow.AutoSize = true;
            this.checkBoxDebugWindow.Checked = true;
            this.checkBoxDebugWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDebugWindow.Location = new System.Drawing.Point(157, 39);
            this.checkBoxDebugWindow.Name = "checkBoxDebugWindow";
            this.checkBoxDebugWindow.Size = new System.Drawing.Size(72, 16);
            this.checkBoxDebugWindow.TabIndex = 3;
            this.checkBoxDebugWindow.Text = "訊息輸出";
            this.checkBoxDebugWindow.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.CausesValidation = false;
            this.tabControl.Controls.Add(this.tabPageBasic);
            this.tabControl.Controls.Add(this.tabAdvanced1);
            this.tabControl.Controls.Add(this.tabAdvanced2);
            this.tabControl.Controls.Add(this.tabAbout);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(359, 440);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageBasic
            // 
            this.tabPageBasic.Controls.Add(this.groupBox1);
            this.tabPageBasic.Controls.Add(this.groupBox2);
            this.tabPageBasic.Controls.Add(this.groupBox4);
            this.tabPageBasic.Controls.Add(this.groupBox3);
            this.tabPageBasic.Location = new System.Drawing.Point(4, 22);
            this.tabPageBasic.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageBasic.Name = "tabPageBasic";
            this.tabPageBasic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBasic.Size = new System.Drawing.Size(351, 414);
            this.tabPageBasic.TabIndex = 0;
            this.tabPageBasic.Text = "基本";
            this.tabPageBasic.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.listBoxNetworkConnections);
            this.groupBox1.Location = new System.Drawing.Point(0, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 116);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "網路連線";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonConnectionsAll);
            this.panel1.Controls.Add(this.radioButtonConnectionsActive);
            this.panel1.Location = new System.Drawing.Point(6, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 19);
            this.panel1.TabIndex = 3;
            // 
            // radioButtonConnectionsAll
            // 
            this.radioButtonConnectionsAll.AutoSize = true;
            this.radioButtonConnectionsAll.Location = new System.Drawing.Point(88, 3);
            this.radioButtonConnectionsAll.Name = "radioButtonConnectionsAll";
            this.radioButtonConnectionsAll.Size = new System.Drawing.Size(47, 16);
            this.radioButtonConnectionsAll.TabIndex = 1;
            this.radioButtonConnectionsAll.Text = "全部";
            this.radioButtonConnectionsAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonConnectionsActive
            // 
            this.radioButtonConnectionsActive.AutoSize = true;
            this.radioButtonConnectionsActive.Checked = true;
            this.radioButtonConnectionsActive.Location = new System.Drawing.Point(3, 3);
            this.radioButtonConnectionsActive.Name = "radioButtonConnectionsActive";
            this.radioButtonConnectionsActive.Size = new System.Drawing.Size(59, 16);
            this.radioButtonConnectionsActive.TabIndex = 0;
            this.radioButtonConnectionsActive.TabStop = true;
            this.radioButtonConnectionsActive.Text = "作用中";
            this.radioButtonConnectionsActive.UseVisualStyleBackColor = true;
            // 
            // listBoxNetworkConnections
            // 
            this.listBoxNetworkConnections.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxNetworkConnections.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxNetworkConnections.FormattingEnabled = true;
            this.listBoxNetworkConnections.ItemHeight = 15;
            this.listBoxNetworkConnections.Location = new System.Drawing.Point(6, 32);
            this.listBoxNetworkConnections.Name = "listBoxNetworkConnections";
            this.listBoxNetworkConnections.Size = new System.Drawing.Size(339, 77);
            this.listBoxNetworkConnections.TabIndex = 0;
            // 
            // tabAdvanced1
            // 
            this.tabAdvanced1.Controls.Add(this.groupBox7);
            this.tabAdvanced1.Controls.Add(this.groupBox5);
            this.tabAdvanced1.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced1.Margin = new System.Windows.Forms.Padding(0);
            this.tabAdvanced1.Name = "tabAdvanced1";
            this.tabAdvanced1.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdvanced1.Size = new System.Drawing.Size(351, 414);
            this.tabAdvanced1.TabIndex = 1;
            this.tabAdvanced1.Text = "進階 1";
            this.tabAdvanced1.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Enabled = false;
            this.groupBox7.Location = new System.Drawing.Point(0, 159);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(351, 258);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "自動啟動/停止";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxInfo);
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(351, 159);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "無線網路詳細資料";
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.BackColor = System.Drawing.Color.White;
            this.textBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxInfo.ForeColor = System.Drawing.Color.Purple;
            this.textBoxInfo.Location = new System.Drawing.Point(6, 15);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.Size = new System.Drawing.Size(339, 139);
            this.textBoxInfo.TabIndex = 1;
            // 
            // tabAdvanced2
            // 
            this.tabAdvanced2.Controls.Add(this.groupBox8);
            this.tabAdvanced2.Controls.Add(this.groupBox6);
            this.tabAdvanced2.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced2.Margin = new System.Windows.Forms.Padding(0);
            this.tabAdvanced2.Name = "tabAdvanced2";
            this.tabAdvanced2.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdvanced2.Size = new System.Drawing.Size(351, 414);
            this.tabAdvanced2.TabIndex = 3;
            this.tabAdvanced2.Text = "進階 2";
            this.tabAdvanced2.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Enabled = false;
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(351, 192);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "連接埠轉送";
            // 
            // groupBox6
            // 
            this.groupBox6.Enabled = false;
            this.groupBox6.Location = new System.Drawing.Point(0, 198);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(351, 219);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "MAC 位址過濾";
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.panelTwitter);
            this.tabAbout.Controls.Add(this.labelProduct);
            this.tabAbout.Controls.Add(this.panel2);
            this.tabAbout.Controls.Add(this.textBoxDescription);
            this.tabAbout.Controls.Add(this.labelCopyright);
            this.tabAbout.Controls.Add(this.labelAssemblyTitle);
            this.tabAbout.Controls.Add(this.labelCompanyName);
            this.tabAbout.Controls.Add(this.labelVersion);
            this.tabAbout.Location = new System.Drawing.Point(4, 22);
            this.tabAbout.Margin = new System.Windows.Forms.Padding(0);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(351, 414);
            this.tabAbout.TabIndex = 2;
            this.tabAbout.Text = "關於";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // panelTwitter
            // 
            this.panelTwitter.BackgroundImage = global::SeventhGate.Properties.Resources.twitter;
            this.panelTwitter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelTwitter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelTwitter.Location = new System.Drawing.Point(283, 49);
            this.panelTwitter.Name = "panelTwitter";
            this.panelTwitter.Size = new System.Drawing.Size(35, 31);
            this.panelTwitter.TabIndex = 29;
            this.panelTwitter.Tag = "";
            this.toolTipTwitter.SetToolTip(this.panelTwitter, "Share SeventhGate on Twitter");
            // 
            // labelProduct
            // 
            this.labelProduct.AutoSize = true;
            this.labelProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelProduct.ForeColor = System.Drawing.Color.Purple;
            this.labelProduct.Location = new System.Drawing.Point(3, 105);
            this.labelProduct.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelProduct.MaximumSize = new System.Drawing.Size(0, 16);
            this.labelProduct.Name = "labelProduct";
            this.labelProduct.Size = new System.Drawing.Size(54, 16);
            this.labelProduct.TabIndex = 28;
            this.labelProduct.Text = "Product";
            this.labelProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::SeventhGate.Properties.Resources.SeventhGate;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(283, 83);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(62, 59);
            this.panel2.TabIndex = 27;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BackColor = System.Drawing.Color.White;
            this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxDescription.Location = new System.Drawing.Point(0, 152);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.Size = new System.Drawing.Size(351, 114);
            this.textBoxDescription.TabIndex = 23;
            this.textBoxDescription.TabStop = false;
            this.textBoxDescription.Text = "Description";
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCopyright.ForeColor = System.Drawing.Color.Purple;
            this.labelCopyright.Location = new System.Drawing.Point(3, 83);
            this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 16);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(65, 16);
            this.labelCopyright.TabIndex = 26;
            this.labelCopyright.Text = "Copyright";
            this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAssemblyTitle
            // 
            this.labelAssemblyTitle.AutoSize = true;
            this.labelAssemblyTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.12727F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAssemblyTitle.ForeColor = System.Drawing.Color.Purple;
            this.labelAssemblyTitle.Location = new System.Drawing.Point(2, 3);
            this.labelAssemblyTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelAssemblyTitle.Name = "labelAssemblyTitle";
            this.labelAssemblyTitle.Size = new System.Drawing.Size(117, 18);
            this.labelAssemblyTitle.TabIndex = 25;
            this.labelAssemblyTitle.Text = "Assembly Title";
            this.labelAssemblyTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCompanyName
            // 
            this.labelCompanyName.AutoSize = true;
            this.labelCompanyName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelCompanyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCompanyName.ForeColor = System.Drawing.Color.Purple;
            this.labelCompanyName.Location = new System.Drawing.Point(3, 126);
            this.labelCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 16);
            this.labelCompanyName.Name = "labelCompanyName";
            this.labelCompanyName.Size = new System.Drawing.Size(173, 16);
            this.labelCompanyName.TabIndex = 26;
            this.labelCompanyName.Text = "Company ConnectionName";
            this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipOnline.SetToolTip(this.labelCompanyName, "Visit SeventhGate project site");
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelVersion.ForeColor = System.Drawing.Color.Purple;
            this.labelVersion.Location = new System.Drawing.Point(3, 49);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelVersion.MaximumSize = new System.Drawing.Size(0, 16);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(38, 16);
            this.labelVersion.TabIndex = 5;
            this.labelVersion.Text = "版本";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolTipTwitter
            // 
            this.toolTipTwitter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolTipTwitter.IsBalloon = true;
            this.toolTipTwitter.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipTwitter.ToolTipTitle = "Twitter";
            // 
            // toolTipOnline
            // 
            this.toolTipOnline.ForeColor = System.Drawing.Color.Purple;
            this.toolTipOnline.IsBalloon = true;
            this.toolTipOnline.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipOnline.ToolTipTitle = "Online";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(359, 578);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.textBoxStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SeventhGate";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageBasic.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabAdvanced1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabAdvanced2.ResumeLayout(false);
            this.tabAbout.ResumeLayout(false);
            this.tabAbout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonStartStop;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox textBoxStatus;
		private System.Windows.Forms.TextBox textBoxNetworkPass;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxNetworkName;
		private System.Windows.Forms.Label labelSharingDescription;
		private System.Windows.Forms.Button buttonPasswordChar;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxMaxPeersNum;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox checkBoxDebugWindow;
		private System.Windows.Forms.CheckBox checkBoxCheckUpdatesAtStart;
		private System.Windows.Forms.ListBox listBoxClients;
		private System.Windows.Forms.CheckBox checkBoxAnimateTray;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageBasic;
		private System.Windows.Forms.TabPage tabAdvanced1;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.TextBox textBoxInfo;
		private System.Windows.Forms.TabPage tabAbout;
		private System.Windows.Forms.CheckBox checkBoxStartMinimized;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton radioButtonConnectionsAll;
		private System.Windows.Forms.RadioButton radioButtonConnectionsActive;
		private System.Windows.Forms.ListBox listBoxNetworkConnections;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Label labelProduct;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.Label labelAssemblyTitle;
		private System.Windows.Forms.Label labelCompanyName;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.TabPage tabAdvanced2;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Panel panelTwitter;
		private System.Windows.Forms.ToolTip toolTipTwitter;
		private System.Windows.Forms.ToolTip toolTipOnline;
	}
}

