﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Clock
{
    public partial class MainForm : Form
    {
        ColorDialog backgroundColorDialog;
        ColorDialog foregroundColorDialog;
        ChooseFont chooseFontDialog;
        AlarmList alarmList;
       string FontFile { get; set; }
        
        public MainForm()
        {
            InitializeComponent();
            AllocConsole(); 
            SetFontDirectory();            
            this.TransparencyKey = Color.Empty;
            backgroundColorDialog = new ColorDialog();
            foregroundColorDialog = new ColorDialog();
           
            

            chooseFontDialog = new ChooseFont();
            LoadSettings();
            alarmList = new AlarmList();
           // backgroundColorDialog.Color =Color.Black;
            //foregroundColorDialog.Color =Color.Blue;
            
            SetVisibility(false);
            this.Location = new Point
                (
                    System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - this.Width,
                    //System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - this.Height
                    20
                );
            this.Text += $"Location:{this.Location.X}x{this.Location.Y}";
                       
        }
        void SetFontDirectory()
        {
            string location = Assembly.GetEntryAssembly().Location;// получаем полный адрес исполняемого файла
            string path = Path.GetDirectoryName(location);          // из адреса извлекаем путь к файлу
            //MessageBox.Show(path);
            Directory.SetCurrentDirectory($"{path}\\..\\..\\Fonts");// переходим в каталог со шрифтами
            //MessageBox.Show(Directory.GetCurrentDirectory());
        
        }

        void LoadSettings()
        {
            StreamReader sr=new StreamReader("settings.txt");
            List<string>settings=new List<string>();
            while (!sr.EndOfStream)
            {
                settings.Add(sr.ReadLine());
            }
            sr.Close();
            backgroundColorDialog.Color = Color.FromArgb(Convert.ToInt32(settings.ToArray()[0]));
            foregroundColorDialog.Color = Color.FromArgb(Convert.ToInt32(settings.ToArray()[1]));
            FontFile = settings.ToArray()[2];
            topmostToolStripMenuItem.Checked = bool.Parse(settings.ToArray()[3]);
            showDateToolStripMenuItem.Checked = bool.Parse(settings.ToArray()[4]);
            labelTime.Font = chooseFontDialog.SetFontFile(FontFile);
            labelTime.ForeColor = foregroundColorDialog.Color;
            labelTime.BackColor = backgroundColorDialog.Color;

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            object run=rk.GetValue("Clock");
            if (run != null) loadOnWindowsStartupToolStripMenuItem.Checked = true;
            rk.Dispose();
        }

        void SaveSettings()
        {
            StreamWriter sw = new StreamWriter("settings.txt");
            sw.WriteLine(backgroundColorDialog.Color.ToArgb());//ToArgb() возвращает числовой код цвета
            sw.WriteLine(foregroundColorDialog.Color.ToArgb());
            sw.WriteLine(chooseFontDialog.FontFile.Split('\\').Last());
            sw.WriteLine(topmostToolStripMenuItem.Checked);
            sw.WriteLine(showDateToolStripMenuItem.Checked);
            sw.Close();
            Process.Start("notepad","settings.txt");
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

        private void fontsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chooseFontDialog.ShowDialog(this) == DialogResult.OK)
            { 
                labelTime.Font=chooseFontDialog.ChosenFont;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void loadOnWindowsStartupToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey rk = 
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",true);
            if (loadOnWindowsStartupToolStripMenuItem.Checked) rk.SetValue("Clock",Application.ExecutablePath);
            else rk.DeleteValue("Clock", false);// не бросать исключение, если указанная запись отсутствует
            rk.Dispose();// освобождает ресурсы, занятые объектом
        }

        private void alarmsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alarmList.ShowDialog(this);
        }
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();


        //private void notifyIconSystemTray_BalloonTipShown(object sender, EventArgs e)
        //{
        //    notifyIconSystemTray.Text="Current time" + labelTime.Text;
        //}
    }
}
