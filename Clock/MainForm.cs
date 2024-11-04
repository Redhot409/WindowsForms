﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class MainForm : Form
    {
        ColorDialog backgroundColorDialog;
        ColorDialog foregroundColorDialog;
        public MainForm()
        {
            InitializeComponent();
            this.TransparencyKey = Color.Empty;
            backgroundColorDialog = new ColorDialog();
            foregroundColorDialog = new ColorDialog();
            SetVisibility(false);
            this.Location = new Point
                (
                    System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - this.Width,
                    //System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - this.Height
                    0
                );
            this.Text += $"Location:{this.Location.X}x{this.Location.Y}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss tt");
            if (cbShowDate.Checked)
            { 
                labelTime.Text+=$"\n{DateTime.Today.ToString("yyyy.MM.dd")}";
            }
            //notifyIconSystemTray.Text = "Current time " + labelTime.Text;
        }

        private void SetVisibility(bool visible)
        {
            this.TransparencyKey = visible ? Color.Empty:this.BackColor;
            this.FormBorderStyle =visible ? FormBorderStyle.Sizable : FormBorderStyle.None;
            this.ShowInTaskbar = visible;
            btnHideControls.Visible = visible;
            labelTime.BackColor = visible ?Color.Empty :backgroundColorDialog.Color;
            cbShowDate.Visible = visible;
        }
        private void btnHideControls_Click(object sender, EventArgs e)
        {
            showControlsToolStripMenuItem.Checked = false;
            //this.TransparencyKey = this.BackColor;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.ShowInTaskbar = false;
            //btnHideControls.Visible = false;
            //labelTime.BackColor = Color.Coral;
            //cbShowDate.Visible = false;
            //SetVisibility(false);
            notifyIconSystemTray.ShowBalloonTip(3, "Важная информация", "Для того, чтобы вернуть как было, нужно ткнуть 2 раза мышей по часам или по этой иконке ",ToolTipIcon.Info);
        }

        private void labelTime_DoubleClick(object sender, EventArgs e)
        {
            //this.TransparencyKey = Color.Empty;
            //this.FormBorderStyle = FormBorderStyle.Sizable;
            //this.ShowInTaskbar = true;
            //btnHideControls.Visible = true;
            //labelTime.BackColor = Color.Empty;
            //cbShowDate.Visible = true;
            //SetVisibility(true);
            showControlsToolStripMenuItem.Checked = true;
        }

        private void notifyIconSystemTray_MouseMove(object sender, MouseEventArgs e)
        {
            notifyIconSystemTray.Text = "Current time:\n " + labelTime.Text;
        }

        private void showControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void topmostToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void topmostToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = topmostToolStripMenuItem.Checked;
        }

        
        private void showControlsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(((ToolStripMenuItem)sender).Checked);
        }

        private void showDateToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            cbShowDate.Checked = ((ToolStripMenuItem)sender).Checked;
        }

        private void cbShowDate_CheckedChanged(object sender, EventArgs e)
        {
            showDateToolStripMenuItem.Checked = ((CheckBox)sender).Checked;
        }

        private void notifyIconSystemTray_DoubleClick(object sender, EventArgs e)
        {
            if (!this.TopMost)
            {
                this.TopMost = true;
                this.TopMost = false;
            }
        }

        private void foregroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (foregroundColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                labelTime.ForeColor = foregroundColorDialog.Color;
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (backgroundColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                labelTime.BackColor = backgroundColorDialog.Color;
            }
        }



        //private void notifyIconSystemTray_BalloonTipShown(object sender, EventArgs e)
        //{
        //    notifyIconSystemTray.Text="Current time" + labelTime.Text;
        //}
    }
}