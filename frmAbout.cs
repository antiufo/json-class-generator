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
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
#if !APPSERVICES
            btnSendFeedback.Visible = false;
            btnCheckUpdates.Visible = false;
#endif
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://at-my-window.blogspot.com/?page=json-class-generator");
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            lblVersion.Text=string.Format(lblVersion.Text, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() );
        }

        private void btnSendFeedback_Click(object sender, EventArgs e)
        {
#if APPSERVICES
            Program.appServices.ShowFeedbackForm(this);
#endif
        }

        private void btnCheckUpdates_Click(object sender, EventArgs e)
        {
#if APPSERVICES
            Program.appServices.UpdateChecker.ManualUpdatesCheck(this);
#endif
        }


    }
}
