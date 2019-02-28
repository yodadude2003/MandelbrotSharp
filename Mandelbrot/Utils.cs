﻿using MandelbrotSharp.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Mandelbrot
{
    public class Utils {
        public static RgbaValue[] LoadPallete(string path)
        {
            List<RgbaValue> pallete = new List<RgbaValue>();
            StreamReader palleteData = new StreamReader(path);
            while (!palleteData.EndOfStream)
            {
                try
                {
                    string palleteString = palleteData.ReadLine();
                    string[] palleteTokens = palleteString.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    byte r = (byte)int.Parse(palleteTokens[0]);
                    byte g = (byte)int.Parse(palleteTokens[1]);
                    byte b = (byte)int.Parse(palleteTokens[2]);
                    RgbaValue color = new RgbaValue(r, g, b);
                    pallete.Add(color);
                }
                catch (FormatException) { }
            }
            return pallete.ToArray();
        }
    }

    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, RgbaValue colour)
        {
            int index = x + (y * Width);
            int col = (255 << 24) | (colour.red << 16) | (colour.green << 8) | (colour.blue);

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void SetBits(int[] BitsIn)
        {
            for (var i = 0; i < Bits.Length; i++)
            {
                Bits[i] = BitsIn[i];
            }
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }

    public class BigDecimalConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(BigDecimal) }));
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (type == typeof(BigDecimal))
                return BigDecimal.Parse((string)dictionary["value"]);
            else
                return null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            try
            {
                BigDecimal val = (BigDecimal)obj;
                return new Dictionary<string, object>() { { "value", val.ToString() } };
            }
            catch (InvalidCastException)
            {
                return new Dictionary<string, object>();
            }
        }
    }

}