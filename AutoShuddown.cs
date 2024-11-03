// Bu proje MIT Lisansı altında lisanslanmıştır.
// Daha fazla bilgi için LICENSE dosyasını kontrol edin.

using System;
using System.Diagnostics;
using System.Windows.Forms;

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
            for (int hour = 0; hour < 24; hour++)
            {
                comboBox1.Items.Add(hour.ToString("D2"));
            }

            for (int minute = 0; minute < 60; minute++)
            {
                comboBox2.Items.Add(minute.ToString("D2"));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateRemainingTimeDisplay();
            CalculateRemainingTime();
        }

        private void CalculateRemainingTime()
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

                remainingTime = selectedTime - currentTime;
                countdownTimer.Start();
            }
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (remainingTime.TotalSeconds > 0)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                textBox2.Text = $"Kalan süre: {remainingTime:hh\\:mm\\:ss}";
            }
            else
            {
                countdownTimer.Stop();
                ShutdownComputer();
            }
        }

        private void ShutdownComputer()
        {
            Process.Start("shutdown", "/s /f /t 0");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
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
                textBox2.Text = $"Kalan süre: {tempRemainingTime:hh\\:mm\\:ss}";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            countdownTimer.Stop(); // Geri sayımı durdurur
        }
    }
}
