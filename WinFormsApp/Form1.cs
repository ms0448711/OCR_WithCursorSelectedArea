using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using OCRLibrary;
using Gma.System.MouseKeyHook;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {

        private ImageCapturer ic = new ImageCapturer();

        public Form1()
        {
            InitializeComponent();

            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }
        private IKeyboardMouseEvents m_GlobalHook;

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 's')
            {
                ic.storeCursorPos.Invoke();
                label1.Text = ic.GetCursorsString;
            }
            if (e.KeyChar == 'd')
            {
                pictureBox1.Image = Image.FromHbitmap(ic.GetImageFromCursors().GetHbitmap());
                label2.Text=ic.GetTexts();
            }
        }


    }
}
