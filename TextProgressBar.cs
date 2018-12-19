using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProgressBarSample
{
    public class TextProgressBar : ProgressBar
    {
        [Description("Do you need to display any text or not"), Category("Additional Options")]
        public bool TextEnabled { get; set; } = true;

        [Description("If it's empty, % will be shown"), Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text { get; set; } = string.Empty;

        [Description("Font of the text on ProgressBar"), Category("Additional Options")]
        public Font TextFont { get; set; } = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold|FontStyle.Italic);

        private Brush _textColourBrush = Brushes.Black;
        [Category("Additional Options")]
        public Color TextColor {
            get {
                return new Pen(_textColourBrush).Color;
            }
            set
            {
                _textColourBrush = new SolidBrush(value);
            }
        }

        private Brush _progressColourBrush = Brushes.LightGreen;
        [Category("Additional Options")]
        public Color ProgressColor
        {
            get
            {
                return new Pen(_textColourBrush).Color;
            }
            set
            {
                _textColourBrush = new SolidBrush(value);
            }
        }

        public TextProgressBar()
        {
            //remove blinking/flickering
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            Graphics g = e.Graphics;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);
            rect.Inflate(-3, -3);
            if (Value > 0)
            {
                Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);

                e.Graphics.FillRectangle(_progressColourBrush, clip);
            }

            if (TextEnabled)
            {
                // Display % in case of CustomText is empty;
                string text = (Text == null || Text.Length == 0) ? $"{Value} %" : Text;
                
                SizeF len = g.MeasureString(text, TextFont);
                Point location = new Point(Convert.ToInt32((Width / 2) - len.Width / 2), Convert.ToInt32((Height / 2) - len.Height / 2));
                
                g.DrawString(text, TextFont, _textColourBrush, location);
            }
        }
    }
}

