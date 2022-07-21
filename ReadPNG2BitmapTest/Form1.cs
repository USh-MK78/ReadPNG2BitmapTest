using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ReadPNG2BitmapTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        public ImageBit.BitmapInfo SWQ { get; set; }
        public ImageBit.BitmapInfo SWQ2 { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Test",
                InitialDirectory = @"C:\Users\user\Desktop",
                Filter = "png file|*.png"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            Bitmap bitmap = new Bitmap(openFileDialog.FileName);

            var d = ImageBit.RunTask<ImageBit.BitmapInfo>(ImageBit.GetBitmapXY(bitmap));
            d.Wait();
            SWQ = d.Result;

            var d2 = ImageBit.RunTask<ImageBit.BitmapInfo>(ImageBit.GetBitmapYX(bitmap));
            d2.Wait();
            SWQ2 = d2.Result;

            //SWQ = ImageBit.RunT(bitmap);

            if (SWQ != null)
            {
                label1.Text = "SWQ";
            }

            if (SWQ2 != null)
            {
                label1.Text = "SWQ2";
            }

            pictureBox1.Image = ImageBit.ToBitmap(SWQ, ImageBit.ReadType.XY);

            pictureBox2.Image = ImageBit.ToBitmap(SWQ2, ImageBit.ReadType.YX);
        }
    }
}
