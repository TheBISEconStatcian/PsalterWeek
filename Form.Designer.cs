using System.Diagnostics;

namespace WhichPsalmWeek
{
    partial class frm_CalculateWeekDay
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        static readonly Size initial_window_size = new Size(341, 450);

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        /// 
        private void InitializeComponent()
        {
            lbl_Instruction = new Label();
            calendar_SelectDay = new MonthCalendar();
            btn_BeginCalulation = new Button();
            txt_SelectDay = new MaskedTextBox();
            lbl_Output = new Label();
            SuspendLayout();
            // 
            // lbl_Instruction
            // 
            lbl_Instruction.AutoSize = true;
            lbl_Instruction.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_Instruction.Location = new Point(25, 9);
            lbl_Instruction.Name = "lbl_Instruction";
            lbl_Instruction.Size = new Size(270, 38);
            lbl_Instruction.TabIndex = 4;
            lbl_Instruction.Text = "Please give in a date";
            // 
            // calendar_SelectDay
            // 
            calendar_SelectDay.Location = new Point(59, 47);
            calendar_SelectDay.Name = "calendar_SelectDay";
            calendar_SelectDay.TabIndex = 3;
            calendar_SelectDay.DateChanged += calendar_SelectDay_DateChanged;
            // 
            // btn_BeginCalulation
            // 
            btn_BeginCalulation.Location = new Point(118, 299);
            btn_BeginCalulation.Name = "btn_BeginCalulation";
            btn_BeginCalulation.Size = new Size(94, 29);
            btn_BeginCalulation.TabIndex = 2;
            btn_BeginCalulation.Text = "Calculate";
            btn_BeginCalulation.UseVisualStyleBackColor = true;
            btn_BeginCalulation.Click += btn_BeginCalulation_Click;
            // 
            // txt_SelectDay
            // 
            txt_SelectDay.Location = new Point(126, 266);
            txt_SelectDay.Mask = "00/00/0000";
            txt_SelectDay.Name = "txt_SelectDay";
            txt_SelectDay.Size = new Size(79, 27);
            txt_SelectDay.TabIndex = 5;
            txt_SelectDay.Text = "26122023";
            txt_SelectDay.MaskInputRejected += txt_SelectDay_MaskInputRejected;
            txt_SelectDay.Validating += txt_SelectDay_MaskInputValidate;
            // 
            // lbl_Output
            // 
            lbl_Output.AutoSize = true;
            lbl_Output.Location = new Point(164, 388);
            lbl_Output.Name = "lbl_Output";
            lbl_Output.Size = new Size(0, 20);
            lbl_Output.TabIndex = 0;
            // 
            // frm_CalculateWeekDay
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(316, 391);
            Controls.Add(txt_SelectDay);
            Controls.Add(lbl_Output);
            Controls.Add(btn_BeginCalulation);
            Controls.Add(calendar_SelectDay);
            Controls.Add(lbl_Instruction);
            Name = "frm_CalculateWeekDay";
            Text = "Calculate Week Day";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_Instruction;
        private MonthCalendar calendar_SelectDay;
        private Button btn_BeginCalulation;
        private Label lbl_Output;
        private MaskedTextBox txt_SelectDay;
    }
    internal static class ControlExtensions
    {
        public static void UpdateLocBelowControl(this Control below, in Control above, int v_space = 7)
        {
            int x_shift = (above.ClientSize.Width - below.ClientSize.Width) / 2; //If below is wider, then negative, otherwise positive
            int y_shift = above.Height + v_space;
            Debug.Print((above.Location.X + x_shift).ToString());
            Debug.Print((above.Location.Y + y_shift).ToString());
            below.Location = new Point(above.Location.X + x_shift, above.Location.Y + y_shift);
        }
    }
}