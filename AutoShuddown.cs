// Bu proje MIT Lisansı altında lisanslanmıştır.
// Daha fazla bilgi için LICENSE dosyasını kontrol edin.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Management;

namespace AutoShuddown
{
    public partial class AutoShuddown : Form
    {
        private Timer countdownTimer;
        private TimeSpan remainingTime;

        public AutoShuddown()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            countdownTimer = new Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void InitializeComboBoxes()
        {
        int currentHour = DateTime.Now.Hour;
        int currentMinute = DateTime.Now.Minute;

        for (int hour = 0; hour < 24; hour++)
        {
            comboBox1.Items.Add(hour.ToString("D2"));
        }
        comboBox1.SelectedItem = currentHour.ToString("D2");

        // Dakika ComboBox'ı için
        for (int minute = 0; minute < 60; minute++)
        {
            comboBox2.Items.Add(minute.ToString("D2"));
        }
        comboBox2.SelectedItem = currentMinute.ToString("D2");
        eventtype.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {}

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {}

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateRemainingTimeDisplay();
            CalculateRemainingTime();
        }

        private void CalculateRemainingTime()
        {
            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
            {
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                eventtype.Enabled = false;
                int selectedHour = int.Parse(comboBox1.SelectedItem.ToString());
                int selectedMinute = int.Parse(comboBox2.SelectedItem.ToString());

                DateTime currentTime = DateTime.Now;
                DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, selectedHour, selectedMinute, 0);

                if (selectedTime < currentTime)
                {
                    selectedTime = selectedTime.AddDays(1);
                }

                remainingTime = selectedTime - currentTime;
                countdownTimer.Start();
            }
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (remainingTime.TotalSeconds > 0)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                timer.Text = $"Kalan süre: {remainingTime:hh\\:mm\\:ss}";
            }
            else
            {
                countdownTimer.Stop();
                ShutdownComputer();
            }
            int hours = remainingTime.Hours;
            int minutes = remainingTime.Minutes;
            int seconds = remainingTime.Seconds;
            if (hours == 2 && minutes == 00 && seconds == 00|| hours == 1 && minutes == 00 && seconds == 00 || hours == 0 && minutes == 30 && seconds == 00 || hours == 0 && minutes == 10 && seconds == 00 || hours == 0 && minutes == 1 && seconds == 00)
            {
                pushNotify($"Kalan süre: {remainingTime:hh\\:mm}");
            }
        }

        private void ShutdownComputer()
        {
            countdownTimer.Stop();
            timer.Text = "Sayaç Duraklatıldı.";
            pushNotify("Sayaç Duraklatıldı");
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            eventtype.Enabled = true;
            int selectedIndex = eventtype.SelectedIndex;
            if (selectedIndex == 0) {
                Process.Start("shutdown", "/s /f /t 0");
            } 
            else if (selectedIndex == 1) 
            {
                Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
            } else
            {
                Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState Sleep");
            }
        }

        private void UpdateRemainingTimeDisplay()
        {
            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
            {
                int selectedHour = int.Parse(comboBox1.SelectedItem.ToString());
                int selectedMinute = int.Parse(comboBox2.SelectedItem.ToString());

                DateTime currentTime = DateTime.Now;
                DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, selectedHour, selectedMinute, 0);

                if (selectedTime < currentTime)
                {
                    selectedTime = selectedTime.AddDays(1);
                }

                TimeSpan tempRemainingTime = selectedTime - currentTime;
                timer.Text = $"Kalan süre: {tempRemainingTime:hh\\:mm\\:ss}";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (remainingTime.TotalSeconds > 0)
            {
                countdownTimer.Stop();
                timer.Text = "Sayaç Duraklatıldı.";
                pushNotify("Sayaç Duraklatıldı");
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                eventtype.Enabled = true;
            }
        }

        private void pushNotify(string message)
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(3000, "Bilgi", message, ToolTipIcon.Info);
            notifyIcon.Dispose();
        }

        private void timer_Click(object sender, EventArgs e)
        {

        }
    }
}
