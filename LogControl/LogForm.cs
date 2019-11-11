using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogControls
{
    public partial class LogForm : Form
    {
        private readonly Thread _uiThread;
        public LogFormApi Loging { get; }
        private readonly Action<string> _addInformationDelegate;
        private readonly Action<string> _addErrorDelegate;
        private readonly Action<string> _addWarningDelegate;

        /// <summary>
        /// Создаёт новый UI поток и дальнейшее обращение к форме 
        /// или её дочерним элементам требуется через Invoke.
        /// </summary>
        public LogForm()
        {
            _uiThread = new Thread(UiThread);
            _uiThread.IsBackground = true;
            _uiThread.SetApartmentState(ApartmentState.STA);
            _uiThread.Name = "LogForm";
            lock (_uiThread)
            {
                _uiThread.Start(this);
                Monitor.Wait(_uiThread);
            }
            Loging = new LogFormApi(this);

            _addInformationDelegate = new Action<string>(InnerAddInformation);
            _addErrorDelegate = new Action<string>(InnerAddError);
            _addWarningDelegate = new Action<string>(InnerAddWarning);
        }

        [STAThread]
        private static void UiThread(object state)
        {
            var self = (LogForm)state;

            self.InitializeComponent();

            // Привязать форму к текущему потоку.
            self.CreateHandle();

            Application.Run();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            lock(_uiThread)
            {
                Monitor.Pulse(_uiThread);
            }
        }

        private void AddInformation(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(_addInformationDelegate, message);
            }
            else
            {
                InnerAddInformation(message);
            }
        }

        private void AddError(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(_addErrorDelegate, message);
            }
            else
            {
                InnerAddError(message);
            }
        }

        private void AddWarning(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(_addWarningDelegate, message);
            }
            else
            {
                InnerAddWarning(message);
            }
        }

        private void InnerAddInformation(string message)
        {
            logControl1.AddInformation(message);
        }

        private void InnerAddError(string message)
        {
            logControl1.AddError(message);
        }

        private void InnerAddWarning(string message)
        {
            logControl1.AddWarning(message);
        }

        public sealed class LogFormApi
        {
            private readonly LogForm _form;

            internal LogFormApi(LogForm form)
            {
                _form = form;
            }

            public void AddInformation(string message) =>
                _form.AddInformation(message);

            public void AddWarning(string message) =>
                _form.AddWarning(message);

            public void AddError(string message) =>
                _form.AddError(message);
        }
    }
}
