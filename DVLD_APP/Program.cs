using DVLD_APP;
using System;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        while (true)
        {
            frmLogin loginForm = new frmLogin();

            // فتح شاشة اللوجين كـ Dialog
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // فتح الشاشة الرئيسية بعد نجاح اللوجين
                Application.Run(new MainForm());

                // عند إغلاق MainForm (سواء ساين أوت أو إغلاق)، يتكرر الـ Loop ويفتح اللوجين مجدداً
            }
            else
            {
                // إذا ضغط المستخدم على زر الإغلاق في شاشة اللوجين، نخرج من البرنامج
                break;
            }
        }
    }
}
