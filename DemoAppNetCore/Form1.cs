using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogControls;

namespace DemoAppNetCore
{
    public partial class Form1 : Form
    {
        private readonly LogForm _logForm;

        public Form1()
        {
            InitializeComponent();
            _logForm = new LogForm();
            _logForm.FormClosing += _logForm_FormClosing;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Debug.Assert(_logForm.InvokeRequired);
            _logForm.BeginInvoke(delegate 
            {
                if (!_logForm.Visible)
                {
                    _logForm.Show();
                }
                _logForm.Activate();
            });
        }

        private void _logForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var me = (Form)sender;
            Debug.Assert(!me.InvokeRequired);
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                me.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_logForm == null)
                return;

            _logForm.Loging.AddInformation("Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка ");
            //_logForm.Loging.AddWarning("Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка ");
            //_logForm.Loging.AddError("Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка Проверка ");
        }
    }
}
