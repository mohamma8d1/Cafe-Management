using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagemnt.Controls
{
    [DefaultEvent("_TextChanged")]
    public partial class MainTextBox : UserControl
    {
        //Fields
        private Color bordercolor = Color.MediumSeaGreen;
        private int bordersize = 2;
        private bool underlinedstyle = false;
        private Color borderFocusColor = Color.Gray;
        private bool isFocused = false;

        //Constructor
        public MainTextBox()
        {
            InitializeComponent();
        }
        //Default Event
        public event EventHandler _TextChanged;
        //TextBox-> TextChanged event
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_TextChanged != null)
                _TextChanged.Invoke(sender, e);
        }
        //Properties
        public Color Bordercolor
        {
            get
            {
                return bordercolor;
            }
            set
            {
                bordercolor = value;
                this.Invalidate();
            }
        }
        public int Bordersize
        {
            get
            {
                return bordersize;
            }
            set
            {
                bordersize = value;
                this.Invalidate();
            }
        }
        public bool Underlinedstyle
        {
            get
            {
                return underlinedstyle;
            }
            set
            {
                underlinedstyle = value;
                this.Invalidate();
            }
        }

        public bool PasswordChar
        {
            get { return textBox1.UseSystemPasswordChar; }
            set { textBox1.UseSystemPasswordChar = value; }
        }
        public bool Multiline
        {
            get { return textBox1.Multiline; }
            set { textBox1.Multiline = value; }
        }
        public string Texts { get => textBox1.Text; set => textBox1.Text = value; }

        //Override Methods

        public override Color BackColor 
        { 
            get => base.BackColor;
            set 
            { 
                base.BackColor = value;
                textBox1.BackColor = value;
            } 
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                textBox1.ForeColor = value;
            }
        }
        public override Font Font 
        { 
            get => base.Font;
            set
            {
                base.Font = value;
                textBox1.Font = value;
                if (DesignMode)
                    UpdateControlHeight();
            }

        }

        public Color BorderFocusColor { get => borderFocusColor; set => borderFocusColor = value; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            //Draw Border
            using (Pen penBorder = new Pen(bordercolor, bordersize))
            {
                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                
                if (!isFocused) //Normal
                {
                    //Line Style & Normal Line
                    if (underlinedstyle)
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }
                else //Focused
                {
                    penBorder.Color = borderFocusColor;

                    //Line Style & Normal Line
                    if (underlinedstyle)
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }


                
            }

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if(this.DesignMode)
                UpdateControlHeight();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }

        private void UpdateControlHeight()
        {
            if(textBox1.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new Size(0, txtHeight);
                textBox1.Multiline = false;

                this.Height = textBox1.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (_TextChanged != null)
                _TextChanged.Invoke(sender, e);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
        }
    }
}
