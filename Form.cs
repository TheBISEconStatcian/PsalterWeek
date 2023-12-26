using System.ComponentModel;
using System.Globalization;

namespace WhichPsalmWeek
{
    public partial class frm_CalculateWeekDay : Form
    {
        public frm_CalculateWeekDay()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void btn_BeginCalulation_Click(object sender, EventArgs e)
        {
            DateTime date_for_calculating = calendar_SelectDay.SelectionStart;

            lbl_Output.Text = "Do something very cool";
            lbl_Output.UpdateLocBelowControl(btn_BeginCalulation, 10);
        }

        private void txt_SelectDay_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
        private void txt_SelectDay_MaskInputValidate(object sender, CancelEventArgs e)
        {
            DateTime date;
            if (DateTime.TryParseExact(txt_SelectDay.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out date))
            {
                // If the input is a valid date, set the date in the MonthCalendar.
                e.Cancel = false;
                calendar_SelectDay.SelectionStart = date;
                calendar_SelectDay.SelectionEnd = date;
            }
            else
            {
                // If the input is not a valid date, cancel the event and select the text.
                e.Cancel = true;
                txt_SelectDay.Select(0, txt_SelectDay.Text.Length);

                // Optionally, display a message to the user.
                MessageBox.Show("Please enter a valid date in the format dd/MM/yyyy.");
            }
        }

        private void calendar_SelectDay_DateChanged(object sender, DateRangeEventArgs e)
        {
            txt_SelectDay.Text = e.Start.ToString("dd/MM/yyyy");
        }
    }
}