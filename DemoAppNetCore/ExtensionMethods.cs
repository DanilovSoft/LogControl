using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoAppNetCore
{
    internal static class ExtensionMethods
    {
        public static object Invoke(this Control control, Action action)
        {
            return control.Invoke(method: action);
        }

        public static IAsyncResult BeginInvoke(this Control control, Action action)
        {
            return control.BeginInvoke(method: action);
        }
    }
}
