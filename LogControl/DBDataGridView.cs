﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    internal class DBDataGridView : DataGridView
    {
        public DBDataGridView()
        {
            DoubleBuffered = true;
        }
    }
}
