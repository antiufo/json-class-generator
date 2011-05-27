namespace JsonCSharpClassGenerator
{
    partial class frmCSharpClassGeneration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCSharpClassGeneration));
            this.btnGenerate = new System.Windows.Forms.Button();
            this.edtJson = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNamespace = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.radFields = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.radPublic = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radProperties = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.radInternal = new System.Windows.Forms.RadioButton();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.edtSecondaryNamespace = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAbout = new System.Windows.Forms.Button();
            this.chkPascalCase = new System.Windows.Forms.CheckBox();
            this.edtMainClass = new System.Windows.Forms.TextBox();
            this.edtTargetFolder = new System.Windows.Forms.TextBox();
            this.chkSeparateNamespace = new System.Windows.Forms.CheckBox();
            this.chkNoHelper = new System.Windows.Forms.CheckBox();
            this.edtNamespace = new System.Windows.Forms.TextBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(496, 476);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 10;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // edtJson
            // 
            this.edtJson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtJson.Location = new System.Drawing.Point(15, 191);
            this.edtJson.MaxLength = 10000000;
            this.edtJson.Multiline = true;
            this.edtJson.Name = "edtJson";
            this.edtJson.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edtJson.Size = new System.Drawing.Size(637, 279);
            this.edtJson.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Generate C# classes from sample JSON:";
            // 
            // lblNamespace
            // 
            this.lblNamespace.AutoSize = true;
            this.lblNamespace.Location = new System.Drawing.Point(12, 12);
            this.lblNamespace.Name = "lblNamespace";
            this.lblNamespace.Size = new System.Drawing.Size(68, 13);
            this.lblNamespace.TabIndex = 4;
            this.lblNamespace.Text = "Namespace:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(577, 476);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(346, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Members generation:";
            // 
            // radFields
            // 
            this.radFields.AutoSize = true;
            this.radFields.Location = new System.Drawing.Point(3, 26);
            this.radFields.Name = "radFields";
            this.radFields.Size = new System.Drawing.Size(218, 17);
            this.radFields.TabIndex = 13;
            this.radFields.Text = "Create pre-populated read only fields";
            this.radFields.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(346, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Visibility:";
            // 
            // radPublic
            // 
            this.radPublic.AutoSize = true;
            this.radPublic.Location = new System.Drawing.Point(3, 26);
            this.radPublic.Name = "radPublic";
            this.radPublic.Size = new System.Drawing.Size(56, 17);
            this.radPublic.TabIndex = 17;
            this.radPublic.Text = "Public";
            this.radPublic.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.radProperties);
            this.flowLayoutPanel1.Controls.Add(this.radFields);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(363, 25);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(224, 46);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // radProperties
            // 
            this.radProperties.AutoSize = true;
            this.radProperties.Checked = true;
            this.radProperties.Location = new System.Drawing.Point(3, 3);
            this.radProperties.Name = "radProperties";
            this.radProperties.Size = new System.Drawing.Size(114, 17);
            this.radProperties.TabIndex = 0;
            this.radProperties.TabStop = true;
            this.radProperties.Text = "Create properties";
            this.radProperties.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.radInternal);
            this.flowLayoutPanel2.Controls.Add(this.radPublic);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(363, 90);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(71, 46);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // radInternal
            // 
            this.radInternal.AutoSize = true;
            this.radInternal.Checked = true;
            this.radInternal.Location = new System.Drawing.Point(3, 3);
            this.radInternal.Name = "radInternal";
            this.radInternal.Size = new System.Drawing.Size(65, 17);
            this.radInternal.TabIndex = 0;
            this.radInternal.TabStop = true;
            this.radInternal.Text = "Internal";
            this.radInternal.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(276, 63);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(24, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Target folder:";
            // 
            // edtSecondaryNamespace
            // 
            this.edtSecondaryNamespace.Location = new System.Drawing.Point(35, 118);
            this.edtSecondaryNamespace.Name = "edtSecondaryNamespace";
            this.edtSecondaryNamespace.Size = new System.Drawing.Size(151, 22);
            this.edtSecondaryNamespace.TabIndex = 5;
            this.edtSecondaryNamespace.Text = "MyProject.JsonTypes";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Main class name:";
            // 
            // btnAbout
            // 
            this.btnAbout.Image = global::JsonCSharpClassGenerator.Properties.Resources.help;
            this.btnAbout.Location = new System.Drawing.Point(626, 7);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(26, 24);
            this.btnAbout.TabIndex = 26;
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // chkPascalCase
            // 
            this.chkPascalCase.AutoSize = true;
            this.chkPascalCase.Checked = global::JsonCSharpClassGenerator.Properties.Settings.Default.UsePascalCase;
            this.chkPascalCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPascalCase.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::JsonCSharpClassGenerator.Properties.Settings.Default, "UsePascalCase", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkPascalCase.Location = new System.Drawing.Point(349, 165);
            this.chkPascalCase.Name = "chkPascalCase";
            this.chkPascalCase.Size = new System.Drawing.Size(138, 17);
            this.chkPascalCase.TabIndex = 27;
            this.chkPascalCase.Text = "Convert to PascalCase";
            this.chkPascalCase.UseVisualStyleBackColor = true;
            // 
            // edtMainClass
            // 
            this.edtMainClass.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::JsonCSharpClassGenerator.Properties.Settings.Default, "MainClassName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.edtMainClass.Location = new System.Drawing.Point(115, 37);
            this.edtMainClass.Name = "edtMainClass";
            this.edtMainClass.Size = new System.Drawing.Size(185, 22);
            this.edtMainClass.TabIndex = 1;
            this.edtMainClass.Text = global::JsonCSharpClassGenerator.Properties.Settings.Default.MainClassName;
            // 
            // edtTargetFolder
            // 
            this.edtTargetFolder.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.edtTargetFolder.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.edtTargetFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::JsonCSharpClassGenerator.Properties.Settings.Default, "TargetFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.edtTargetFolder.Location = new System.Drawing.Point(115, 65);
            this.edtTargetFolder.Name = "edtTargetFolder";
            this.edtTargetFolder.Size = new System.Drawing.Size(155, 22);
            this.edtTargetFolder.TabIndex = 2;
            this.edtTargetFolder.Text = global::JsonCSharpClassGenerator.Properties.Settings.Default.TargetFolder;
            // 
            // chkSeparateNamespace
            // 
            this.chkSeparateNamespace.AutoSize = true;
            this.chkSeparateNamespace.Checked = global::JsonCSharpClassGenerator.Properties.Settings.Default.UseSeparateNamespace;
            this.chkSeparateNamespace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparateNamespace.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::JsonCSharpClassGenerator.Properties.Settings.Default, "UseSeparateNamespace", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkSeparateNamespace.Location = new System.Drawing.Point(12, 101);
            this.chkSeparateNamespace.Name = "chkSeparateNamespace";
            this.chkSeparateNamespace.Size = new System.Drawing.Size(275, 17);
            this.chkSeparateNamespace.TabIndex = 4;
            this.chkSeparateNamespace.Text = "Use a separate namespace for secondary classes:";
            this.chkSeparateNamespace.UseVisualStyleBackColor = true;
            this.chkSeparateNamespace.CheckedChanged += new System.EventHandler(this.chkSeparateNamespace_CheckedChanged);
            // 
            // chkNoHelper
            // 
            this.chkNoHelper.AutoSize = true;
            this.chkNoHelper.Checked = global::JsonCSharpClassGenerator.Properties.Settings.Default.NoHelper;
            this.chkNoHelper.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::JsonCSharpClassGenerator.Properties.Settings.Default, "NoHelper", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkNoHelper.Location = new System.Drawing.Point(349, 142);
            this.chkNoHelper.Name = "chkNoHelper";
            this.chkNoHelper.Size = new System.Drawing.Size(174, 17);
            this.chkNoHelper.TabIndex = 8;
            this.chkNoHelper.Text = "Do not generate helper class";
            this.chkNoHelper.UseVisualStyleBackColor = true;
            // 
            // edtNamespace
            // 
            this.edtNamespace.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::JsonCSharpClassGenerator.Properties.Settings.Default, "Namespace", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.edtNamespace.Location = new System.Drawing.Point(115, 9);
            this.edtNamespace.Name = "edtNamespace";
            this.edtNamespace.Size = new System.Drawing.Size(185, 22);
            this.edtNamespace.TabIndex = 0;
            this.edtNamespace.Text = global::JsonCSharpClassGenerator.Properties.Settings.Default.Namespace;
            this.edtNamespace.TextChanged += new System.EventHandler(this.edtNamespace_TextChanged);
            // 
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPaste.Location = new System.Drawing.Point(347, 476);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(75, 23);
            this.btnPaste.TabIndex = 28;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // frmCSharpClassGeneration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(664, 511);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.chkPascalCase);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.edtMainClass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.edtTargetFolder);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.edtSecondaryNamespace);
            this.Controls.Add(this.chkSeparateNamespace);
            this.Controls.Add(this.chkNoHelper);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblNamespace);
            this.Controls.Add(this.edtNamespace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edtJson);
            this.Controls.Add(this.btnGenerate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCSharpClassGeneration";
            this.Text = "JSON C# Class Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCSharpClassGeneration_FormClosing);
            this.Load += new System.EventHandler(this.frmCSharpClassGeneration_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox edtJson;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edtNamespace;
        private System.Windows.Forms.Label lblNamespace;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkNoHelper;
        private System.Windows.Forms.CheckBox chkSeparateNamespace;
        private System.Windows.Forms.TextBox edtSecondaryNamespace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radFields;
        private System.Windows.Forms.RadioButton radProperties;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radInternal;
        private System.Windows.Forms.RadioButton radPublic;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TextBox edtTargetFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edtMainClass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.CheckBox chkPascalCase;
        private System.Windows.Forms.Button btnPaste;
    }
}

