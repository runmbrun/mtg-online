namespace MTGClient
{
    partial class MTGClientForm
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageLogin = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageRegisteredUser = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.tabPageNewUser = new System.Windows.Forms.TabPage();
            this.tabPageCollection = new System.Windows.Forms.TabPage();
            this.pictureBoxCardImage = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridViewDeck = new System.Windows.Forms.DataGridView();
            this.dataGridViewCollection = new System.Windows.Forms.DataGridView();
            this.tabPageStore = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.buttonBuyThemeDecks = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonBuyFoils = new System.Windows.Forms.Button();
            this.tabPageLobby = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonChat = new System.Windows.Forms.Button();
            this.textBoxChat = new System.Windows.Forms.TextBox();
            this.tabPageGame = new System.Windows.Forms.TabPage();
            this.tabPageAdmin = new System.Windows.Forms.TabPage();
            this.listBoxResults = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageLogin.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageRegisteredUser.SuspendLayout();
            this.tabPageCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCardImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCollection)).BeginInit();
            this.tabPageStore.SuspendLayout();
            this.tabPageLobby.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageLogin);
            this.tabControl1.Controls.Add(this.tabPageCollection);
            this.tabControl1.Controls.Add(this.tabPageStore);
            this.tabControl1.Controls.Add(this.tabPageLobby);
            this.tabControl1.Controls.Add(this.tabPageGame);
            this.tabControl1.Controls.Add(this.tabPageAdmin);
            this.tabControl1.Location = new System.Drawing.Point(0, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(857, 476);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tabControl1_KeyPress);
            // 
            // tabPageLogin
            // 
            this.tabPageLogin.Controls.Add(this.tabControl2);
            this.tabPageLogin.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogin.Name = "tabPageLogin";
            this.tabPageLogin.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogin.Size = new System.Drawing.Size(849, 450);
            this.tabPageLogin.TabIndex = 0;
            this.tabPageLogin.Text = "Login";
            this.tabPageLogin.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageRegisteredUser);
            this.tabControl2.Controls.Add(this.tabPageNewUser);
            this.tabControl2.Location = new System.Drawing.Point(6, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(245, 156);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPageRegisteredUser
            // 
            this.tabPageRegisteredUser.Controls.Add(this.label2);
            this.tabPageRegisteredUser.Controls.Add(this.buttonLogin);
            this.tabPageRegisteredUser.Controls.Add(this.textBoxUser);
            this.tabPageRegisteredUser.Controls.Add(this.label1);
            this.tabPageRegisteredUser.Controls.Add(this.textBoxPassword);
            this.tabPageRegisteredUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageRegisteredUser.Name = "tabPageRegisteredUser";
            this.tabPageRegisteredUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegisteredUser.Size = new System.Drawing.Size(237, 130);
            this.tabPageRegisteredUser.TabIndex = 0;
            this.tabPageRegisteredUser.Text = "Registered User";
            this.tabPageRegisteredUser.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(57, 84);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(63, 22);
            this.buttonLogin.TabIndex = 0;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(84, 13);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(100, 20);
            this.textBoxUser.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "User";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(84, 49);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(100, 20);
            this.textBoxPassword.TabIndex = 1;
            // 
            // tabPageNewUser
            // 
            this.tabPageNewUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageNewUser.Name = "tabPageNewUser";
            this.tabPageNewUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNewUser.Size = new System.Drawing.Size(237, 130);
            this.tabPageNewUser.TabIndex = 1;
            this.tabPageNewUser.Text = "New User";
            this.tabPageNewUser.UseVisualStyleBackColor = true;
            // 
            // tabPageCollection
            // 
            this.tabPageCollection.Controls.Add(this.pictureBoxCardImage);
            this.tabPageCollection.Controls.Add(this.label4);
            this.tabPageCollection.Controls.Add(this.label3);
            this.tabPageCollection.Controls.Add(this.dataGridViewDeck);
            this.tabPageCollection.Controls.Add(this.dataGridViewCollection);
            this.tabPageCollection.Location = new System.Drawing.Point(4, 22);
            this.tabPageCollection.Name = "tabPageCollection";
            this.tabPageCollection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCollection.Size = new System.Drawing.Size(849, 450);
            this.tabPageCollection.TabIndex = 1;
            this.tabPageCollection.Text = "Collection";
            this.tabPageCollection.UseVisualStyleBackColor = true;
            // 
            // pictureBoxCardImage
            // 
            this.pictureBoxCardImage.Location = new System.Drawing.Point(622, 19);
            this.pictureBoxCardImage.Name = "pictureBoxCardImage";
            this.pictureBoxCardImage.Size = new System.Drawing.Size(200, 285);
            this.pictureBoxCardImage.TabIndex = 2;
            this.pictureBoxCardImage.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 307);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Decks";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Collection";
            // 
            // dataGridViewDeck
            // 
            this.dataGridViewDeck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDeck.Location = new System.Drawing.Point(6, 323);
            this.dataGridViewDeck.Name = "dataGridViewDeck";
            this.dataGridViewDeck.Size = new System.Drawing.Size(816, 132);
            this.dataGridViewDeck.TabIndex = 0;
            // 
            // dataGridViewCollection
            // 
            this.dataGridViewCollection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCollection.Location = new System.Drawing.Point(6, 19);
            this.dataGridViewCollection.Name = "dataGridViewCollection";
            this.dataGridViewCollection.Size = new System.Drawing.Size(610, 285);
            this.dataGridViewCollection.TabIndex = 0;
            this.dataGridViewCollection.SelectionChanged += new System.EventHandler(this.dataGridViewCollection_SelectionChanged);
            // 
            // tabPageStore
            // 
            this.tabPageStore.Controls.Add(this.comboBox1);
            this.tabPageStore.Controls.Add(this.buttonBuyThemeDecks);
            this.tabPageStore.Controls.Add(this.label7);
            this.tabPageStore.Controls.Add(this.label6);
            this.tabPageStore.Controls.Add(this.buttonBuyFoils);
            this.tabPageStore.Location = new System.Drawing.Point(4, 22);
            this.tabPageStore.Name = "tabPageStore";
            this.tabPageStore.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStore.Size = new System.Drawing.Size(849, 450);
            this.tabPageStore.TabIndex = 2;
            this.tabPageStore.Text = "Store";
            this.tabPageStore.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(205, 94);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // buttonBuyThemeDecks
            // 
            this.buttonBuyThemeDecks.Location = new System.Drawing.Point(205, 134);
            this.buttonBuyThemeDecks.Name = "buttonBuyThemeDecks";
            this.buttonBuyThemeDecks.Size = new System.Drawing.Size(75, 23);
            this.buttonBuyThemeDecks.TabIndex = 2;
            this.buttonBuyThemeDecks.Text = "Buy";
            this.buttonBuyThemeDecks.UseVisualStyleBackColor = true;
            this.buttonBuyThemeDecks.Click += new System.EventHandler(this.buttonBuyThemeDecks_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(202, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Theme Packs";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(59, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Foil Packs";
            // 
            // buttonBuyFoils
            // 
            this.buttonBuyFoils.Location = new System.Drawing.Point(62, 94);
            this.buttonBuyFoils.Name = "buttonBuyFoils";
            this.buttonBuyFoils.Size = new System.Drawing.Size(75, 23);
            this.buttonBuyFoils.TabIndex = 0;
            this.buttonBuyFoils.Text = "Buy";
            this.buttonBuyFoils.UseVisualStyleBackColor = true;
            this.buttonBuyFoils.Click += new System.EventHandler(this.buttonBuy_Click);
            // 
            // tabPageLobby
            // 
            this.tabPageLobby.Controls.Add(this.label5);
            this.tabPageLobby.Controls.Add(this.buttonChat);
            this.tabPageLobby.Controls.Add(this.textBoxChat);
            this.tabPageLobby.Location = new System.Drawing.Point(4, 22);
            this.tabPageLobby.Name = "tabPageLobby";
            this.tabPageLobby.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLobby.Size = new System.Drawing.Size(849, 450);
            this.tabPageLobby.TabIndex = 3;
            this.tabPageLobby.Text = "Lobby";
            this.tabPageLobby.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 428);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Chat";
            // 
            // buttonChat
            // 
            this.buttonChat.Location = new System.Drawing.Point(585, 422);
            this.buttonChat.Name = "buttonChat";
            this.buttonChat.Size = new System.Drawing.Size(75, 23);
            this.buttonChat.TabIndex = 1;
            this.buttonChat.Text = "Send";
            this.buttonChat.UseVisualStyleBackColor = true;
            this.buttonChat.Click += new System.EventHandler(this.buttonChat_Click);
            // 
            // textBoxChat
            // 
            this.textBoxChat.Location = new System.Drawing.Point(54, 425);
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.Size = new System.Drawing.Size(511, 20);
            this.textBoxChat.TabIndex = 0;
            // 
            // tabPageGame
            // 
            this.tabPageGame.Location = new System.Drawing.Point(4, 22);
            this.tabPageGame.Name = "tabPageGame";
            this.tabPageGame.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGame.Size = new System.Drawing.Size(849, 450);
            this.tabPageGame.TabIndex = 4;
            this.tabPageGame.Text = "Game";
            this.tabPageGame.UseVisualStyleBackColor = true;
            // 
            // tabPageAdmin
            // 
            this.tabPageAdmin.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdmin.Name = "tabPageAdmin";
            this.tabPageAdmin.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdmin.Size = new System.Drawing.Size(849, 450);
            this.tabPageAdmin.TabIndex = 5;
            this.tabPageAdmin.Text = "Admin";
            this.tabPageAdmin.UseVisualStyleBackColor = true;
            // 
            // listBoxResults
            // 
            this.listBoxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxResults.FormattingEnabled = true;
            this.listBoxResults.Location = new System.Drawing.Point(0, 494);
            this.listBoxResults.Name = "listBoxResults";
            this.listBoxResults.Size = new System.Drawing.Size(857, 134);
            this.listBoxResults.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 628);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(857, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // MTGClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 650);
            this.Controls.Add(this.listBoxResults);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "MTGClientForm";
            this.Text = "Magic the Gathering";
            this.tabControl1.ResumeLayout(false);
            this.tabPageLogin.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageRegisteredUser.ResumeLayout(false);
            this.tabPageRegisteredUser.PerformLayout();
            this.tabPageCollection.ResumeLayout(false);
            this.tabPageCollection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCardImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCollection)).EndInit();
            this.tabPageStore.ResumeLayout(false);
            this.tabPageStore.PerformLayout();
            this.tabPageLobby.ResumeLayout(false);
            this.tabPageLobby.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageLogin;
        private System.Windows.Forms.TabPage tabPageCollection;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridViewDeck;
        private System.Windows.Forms.DataGridView dataGridViewCollection;
        private System.Windows.Forms.TabPage tabPageStore;
        private System.Windows.Forms.TabPage tabPageLobby;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageRegisteredUser;
        private System.Windows.Forms.TabPage tabPageNewUser;
        private System.Windows.Forms.ListBox listBoxResults;
        private System.Windows.Forms.Button buttonBuyFoils;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.PictureBox pictureBoxCardImage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonChat;
        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button buttonBuyThemeDecks;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPageGame;
        private System.Windows.Forms.TabPage tabPageAdmin;
    }
}

