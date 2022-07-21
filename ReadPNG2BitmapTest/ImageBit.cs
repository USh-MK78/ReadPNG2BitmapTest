using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ReadPNG2BitmapTest
{
    public class ImageBit
    {
        public class ColorRGBA
        {
            public int R { get; set; }
            public int G { get; set; }
            public int B { get; set; }
            public int A { get; set; }

            public ColorRGBA()
            {
                R = 0;
                G = 0;
                B = 0;
                A = 0;
            }
        }

        public class ColorRGBAByte
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public ColorRGBAByte()
            {
                R = 0;
                G = 0;
                B = 0;
                A = 0;
            }

            public byte[] ToByteArray()
            {
                return new byte[] { R, G, B, A };
            }
        }

        public class BitmapColor
        {
            public Color BitColor { get; set; }

            public ColorRGBA ToColorRGBA()
            {
                ColorRGBA NewRGBA = new ColorRGBA();
                NewRGBA.R = BitColor.R;
                NewRGBA.G = BitColor.G;
                NewRGBA.B = BitColor.B;
                NewRGBA.A = BitColor.A;
                return NewRGBA;
            }

            public ColorRGBAByte ToColorRGBAByte()
            {
                ColorRGBAByte colorRGBAByte = new ColorRGBAByte();
                colorRGBAByte.R = BitColor.R;
                colorRGBAByte.G = BitColor.G;
                colorRGBAByte.B = BitColor.B;
                colorRGBAByte.A = BitColor.A;
                return colorRGBAByte;
            }

            public byte[] ToByteArray()
            {
                ColorRGBAByte colorRGBAByte = new ColorRGBAByte();
                colorRGBAByte.R = BitColor.R;
                colorRGBAByte.G = BitColor.G;
                colorRGBAByte.B = BitColor.B;
                colorRGBAByte.A = BitColor.A;

                return colorRGBAByte.ToByteArray();
            }
        }

        public class BitmapInfo
        {
            public ImageSize imageSize { get; set; }
            public class ImageSize
            {
                public int X { get; set; }
                public int Y { get; set; }
            }

            public List<List<BitmapColor>> bitmapColorList_List { get; set; }
        }

        public static Bitmap OpenImage(string Path)
        {
            return new Bitmap(Path);
        }

        public static BitmapInfo GetBitmapYX(Bitmap bitmap)
        {
            List<List<BitmapColor>> BitmapColorList_List = new List<List<BitmapColor>>();

            for (int Pixel_Y = 0; Pixel_Y < bitmap.Height; Pixel_Y++)
            {

                List<BitmapColor> BitmapColor_List = new List<BitmapColor>();

                for (int Pixel_X = 0; Pixel_X < bitmap.Width; Pixel_X++)
                {
                    BitmapColor bitmapColor = new BitmapColor();
                    bitmapColor.BitColor = bitmap.GetPixel(Pixel_X, Pixel_Y);
                    BitmapColor_List.Add(bitmapColor);
                }

                BitmapColorList_List.Add(BitmapColor_List);
            }

            BitmapInfo bitmapInfo = new BitmapInfo
            {
                bitmapColorList_List = BitmapColorList_List,
                imageSize = new BitmapInfo.ImageSize
                {
                    X = bitmap.Width,
                    Y = bitmap.Height
                }
            };

            return bitmapInfo;
        }

        public static BitmapInfo GetBitmapXY(Bitmap bitmap)
        {
            List<List<BitmapColor>> BitmapColorList_List = new List<List<BitmapColor>>();

            for (int Pixel_X = 0; Pixel_X < bitmap.Width; Pixel_X++)
            {

                List<BitmapColor> BitmapColor_List = new List<BitmapColor>();

                for (int Pixel_Y = 0; Pixel_Y < bitmap.Height; Pixel_Y++)
                {
                    BitmapColor bitmapColor = new BitmapColor();
                    bitmapColor.BitColor = bitmap.GetPixel(Pixel_X, Pixel_Y);
                    BitmapColor_List.Add(bitmapColor);
                }

                BitmapColorList_List.Add(BitmapColor_List);
            }

            BitmapInfo bitmapInfo = new BitmapInfo
            {
                bitmapColorList_List = BitmapColorList_List,
                imageSize = new BitmapInfo.ImageSize
                {
                    X = bitmap.Width,
                    Y = bitmap.Height
                }
            };

            return bitmapInfo;
        }

        public static Task<T> RunTask<T>(object obj)
        {
            Task<T> r = Task.Run(() => { return (T)obj; });
            return r;
        }

        public enum ReadType
        {
            XY,
            YX
        }

        public static Bitmap ToBitmap(BitmapInfo bitmapInfo, ReadType readType)
        {
            Bitmap bitmap = null;
            if (readType == ReadType.XY)
            {
                Task<Bitmap> b_Task = RunTask<Bitmap>(ToBitmapXY(bitmapInfo));
                b_Task.Wait();
                bitmap = b_Task.Result;
            }
            if (readType == ReadType.YX)
            {
                Task<Bitmap> b_Task = RunTask<Bitmap>(ToBitmapYX(bitmapInfo));
                b_Task.Wait();
                bitmap = b_Task.Result;
            }

            return bitmap;
        }


        public static Bitmap ToBitmapYX(BitmapInfo bitmapInfo)
        {
            Bitmap bitmap = new Bitmap(bitmapInfo.imageSize.X, bitmapInfo.imageSize.Y);

            for (int Pixel_Y = 0; Pixel_Y < bitmapInfo.bitmapColorList_List.Count; Pixel_Y++)
            {
                for (int Pixel_X = 0; Pixel_X < bitmap.Width; Pixel_X++)
                {
                    bitmap.SetPixel(Pixel_X, Pixel_Y, bitmapInfo.bitmapColorList_List[Pixel_Y][Pixel_X].BitColor);
                }
            }

            return bitmap;
        }

        public static Bitmap ToBitmapXY(BitmapInfo bitmapInfo)
        {
            Bitmap bitmap = new Bitmap(bitmapInfo.imageSize.X, bitmapInfo.imageSize.Y);

            for (int Pixel_X = 0; Pixel_X < bitmap.Width; Pixel_X++)
            {
                for (int Pixel_Y = 0; Pixel_Y < bitmap.Height; Pixel_Y++)
                {
                    bitmap.SetPixel(Pixel_X, Pixel_Y, bitmapInfo.bitmapColorList_List[Pixel_X][Pixel_Y].BitColor);
                }
            }

            return bitmap;
        }
    }
}
