using System;
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

    public partial class AddAlarm : Form
    {
        public Alarm Alarm { get; set; }
        public AddAlarm()
        {
            InitializeComponent();
            Alarm = new Alarm();
            labelFilename.MaximumSize=new Size(this.Width-50,25);
            openFileDialogSound.Filter = "MP-3 (*.mp3) |*.mp3| Flac (*.flac)|*.flac|All Audio|*.mp3;*.flac";
        }
        void InitAlarm()
        {
            Alarm.Date = dateTimePickerDate.Enabled ? dateTimePickerDate.Value : DateTime.MinValue;
            Alarm.Time = dateTimePickerDate.Value;
           
            Alarm.Filename=labelFilename.Text;
            for (int i = 0; i < checkedListBoxWeek.CheckedIndices.Count; i++)
            {
                //Свойство CheckedIndices -это коллекция, кот содержит индексы выбранных галочек в checkedListBox
                //Alarm.Weekdays[i]= (checkedListBoxWeek.Items[i]  as CheckBox).Checked;
                Alarm.Weekdays[checkedListBoxWeek.CheckedIndices[i]] = true;
                Console.Write(checkedListBoxWeek.CheckedIndices[i] + "\t");
            }
            Console.WriteLine();
        }
       

        private void checkBoxExactDate_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerDate.Enabled =((CheckBox)sender).Checked;
        }

        private void labelFilename_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;
        }

        private void buttonChooseFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogSound.ShowDialog(this) == DialogResult.OK)
            { 
                Alarm.Filename=labelFilename.Text = openFileDialogSound.FileName;
            }
        }
    }
}
