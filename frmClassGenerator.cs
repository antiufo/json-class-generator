// Copyright (c) 2010 Andrea Martinelli
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace JsonCSharpClassGenerator
{
    public partial class frmCSharpClassGeneration : Form
    {






        public frmCSharpClassGeneration()
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            Program.InitAppServices();
        }

        private void chkSeparateNamespace_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
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

            Properties.Settings.Default.UseProperties = radProperties.Checked;
            Properties.Settings.Default.InternalVisibility = radInternal.Checked;
            Properties.Settings.Default.SecondaryNamespace = edtSecondaryNamespace.Text;
            if (!Properties.Settings.Default.UseSeparateNamespace)
            {
                Properties.Settings.Default.SecondaryNamespace = string.Empty;
            }
            Properties.Settings.Default.Save();
        }

        private void frmCSharpClassGeneration_Load(object sender, EventArgs e)
        {
            (Properties.Settings.Default.UseProperties ? radProperties : radFields).Checked = true;
            (Properties.Settings.Default.InternalVisibility ? radInternal : radPublic).Checked = true;
            edtSecondaryNamespace.Text = Properties.Settings.Default.SecondaryNamespace;
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

            var gen = new JsonClassGenerator();
            gen.Example = edtJson.Text;
            gen.InternalVisibility = radInternal.Checked;
            gen.ExplicitDeserialization = chkExplicitDeserialization.Checked;
            gen.Namespace = edtNamespace.Text;
            gen.NoHelperClass = chkNoHelper.Checked;
            gen.SecondaryNamespace = chkSeparateNamespace.Checked ? edtSecondaryNamespace.Text : null;
            gen.TargetFolder = edtTargetFolder.Text;
            gen.UseProperties = radProperties.Checked;
            gen.MainClass = edtMainClass.Text;
            gen.UsePascalCase = chkPascalCase.Checked;
            /*   try
               {*/
            gen.GenerateClasses();
            MessageBox.Show(this, "The code has been generated successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(this, "Unable to generate the code: " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
              }*/
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


            if (edtSecondaryNamespace.Text.EndsWith(".JsonTypes") || edtSecondaryNamespace.Text == string.Empty)
            {
                edtSecondaryNamespace.Text = edtNamespace.Text == string.Empty ? string.Empty : edtNamespace.Text + ".JsonTypes";
            }

            if (chkSeparateNamespace.Checked)
            {
                if (string.IsNullOrEmpty(edtSecondaryNamespace.Text)) edtSecondaryNamespace.Text = "MyProject.JsonTypes";
                edtSecondaryNamespace.Enabled = true;
            }
            else
            {
                edtSecondaryNamespace.Enabled = false;
            }
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
