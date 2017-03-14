namespace DecompilerInterface {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvAssemblyViewer = new System.Windows.Forms.TreeView();
            this.tcOutput = new System.Windows.Forms.TabControl();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdAssembly = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvAssemblyViewer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tcOutput);
            this.splitContainer1.Size = new System.Drawing.Size(1009, 608);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvAssemblyViewer
            // 
            this.tvAssemblyViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAssemblyViewer.Location = new System.Drawing.Point(0, 0);
            this.tvAssemblyViewer.Name = "tvAssemblyViewer";
            this.tvAssemblyViewer.Size = new System.Drawing.Size(257, 608);
            this.tvAssemblyViewer.TabIndex = 0;
            // 
            // tcOutput
            // 
            this.tcOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcOutput.Location = new System.Drawing.Point(0, 0);
            this.tcOutput.Name = "tcOutput";
            this.tcOutput.SelectedIndex = 0;
            this.tcOutput.Size = new System.Drawing.Size(748, 608);
            this.tcOutput.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1009, 28);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openAssemblyToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openAssemblyToolStripMenuItem
            // 
            this.openAssemblyToolStripMenuItem.Name = "openAssemblyToolStripMenuItem";
            this.openAssemblyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openAssemblyToolStripMenuItem.Size = new System.Drawing.Size(240, 26);
            this.openAssemblyToolStripMenuItem.Text = "Open Assembly";
            this.openAssemblyToolStripMenuItem.Click += new System.EventHandler(this.OpenAssemblyToolStripMenuItem_Click);
            // 
            // ofdAssembly
            // 
            this.ofdAssembly.Filter = "Assemblies|*.exe;*.dll|All Files|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 636);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "C# Decompiler GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tcOutput;
        private System.Windows.Forms.TreeView tvAssemblyViewer;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openAssemblyToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofdAssembly;
    }
}

