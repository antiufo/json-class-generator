// Copyright © 2010 Xamasoft

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace Xamasoft.JsonCSharpClassGenerator
{
    public partial class frmCSharpClassGeneration : Form
    {






        public frmCSharpClassGeneration()
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            Program.InitAppServices();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var b = new FolderBrowserDialog())
            {
                b.ShowNewFolderButton = true;
                b.SelectedPath = edtTargetFolder.Text;
                b.Description = "Please select a folder where to save the generated files.";
                if (b.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    edtTargetFolder.Text = b.SelectedPath;
                }

            }
        }

        private void frmCSharpClassGeneration_FormClosing(object sender, FormClosingEventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.UseProperties = radProperties.Checked;
            settings.InternalVisibility = radInternal.Checked;
            settings.SecondaryNamespace = edtSecondaryNamespace.Text;

            if (radNestedClasses.Checked) settings.NamespaceStrategy = 0;
            else if (radSameNamespace.Checked) settings.NamespaceStrategy = 1;
            else settings.NamespaceStrategy = 2;

            if (!settings.UseSeparateNamespace)
            {
                settings.SecondaryNamespace = string.Empty;
            }
            settings.Save();
        }

        private void frmCSharpClassGeneration_Load(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;
            (settings.UseProperties ? radProperties : radFields).Checked = true;
            (settings.InternalVisibility ? radInternal : radPublic).Checked = true;
            edtSecondaryNamespace.Text = settings.SecondaryNamespace;
            if (settings.NamespaceStrategy == 0) radNestedClasses.Checked = true;
            else if (settings.NamespaceStrategy == 1) radSameNamespace.Checked = true;
            else radDifferentNamespace.Checked = true;
            UpdateStatus();
        }

        private void edtNamespace_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            if (edtJson.Text == string.Empty)
            {
                MessageBox.Show(this, "Please insert some sample JSON.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                edtJson.Focus();
                return;
            }
            if (edtTargetFolder.Text == string.Empty)
            {
                MessageBox.Show(this, "Please specify an output directory.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (edtNamespace.Text == string.Empty)
            {
                MessageBox.Show(this, "Please specify a namespace.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (edtMainClass.Text == string.Empty)
            {
                MessageBox.Show(this, "Please specify a main class name.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var gen = new JsonClassGenerator();
            gen.Example = edtJson.Text;
            gen.InternalVisibility = radInternal.Checked;
            gen.ExplicitDeserialization = chkExplicitDeserialization.Checked;
            gen.Namespace = edtNamespace.Text;
            gen.NoHelperClass = chkNoHelper.Checked;
            gen.SecondaryNamespace = radDifferentNamespace.Checked ? edtSecondaryNamespace.Text : null;
            gen.TargetFolder = edtTargetFolder.Text;
            gen.UseProperties = radProperties.Checked;
            gen.MainClass = edtMainClass.Text;
            gen.UsePascalCase = chkPascalCase.Checked;
            gen.UseNestedClasses = radNestedClasses.Checked;
            gen.ApplyObfuscationAttributes = chkApplyObfuscationAttributes.Checked;
            try
            {
                gen.GenerateClasses();
                MessageBox.Show(this, "The code has been generated successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to generate the code: " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            using (var w = new frmAbout())
            {
                w.ShowDialog(this);
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            edtJson.Text = Clipboard.GetText();
        }

        private void chkExplicitDeserialization_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            chkNoHelper.Enabled = chkExplicitDeserialization.Checked;


            if (edtSecondaryNamespace.Text.Contains("JsonTypes") || edtSecondaryNamespace.Text == string.Empty)
            {
                edtSecondaryNamespace.Text = edtNamespace.Text == string.Empty ? string.Empty : edtNamespace.Text + "." + edtMainClass.Text + "JsonTypes";
            }

            edtSecondaryNamespace.Enabled = radDifferentNamespace.Checked;
        }

        private void radNestedClasses_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void radSameNamespace_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void radDifferentNamespace_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void edtMainClass_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        //private void edtMainClass_Enter(object sender, EventArgs e)
        //{

        //    edtMainClass.Focus();
        //    edtMainClass.SelectAll();
        //}

        //private void edtTargetFolder_Enter(object sender, EventArgs e)
        //{
        //    edtTargetFolder.SelectAll();
        //}

        //private void edtNamespace_Enter(object sender, EventArgs e)
        //{
        //    edtNamespace.SelectAll();
        //}

        //private void edtJson_Enter(object sender, EventArgs e)
        //{
        //    edtJson.SelectAll();
        //}








    }
}
