using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Tesseract;

namespace OCRLibrary
{
    public class ImageCapturer
    {


        [DllImport("user32.dll")]

        // GetCursorPos() makes everything possible

        private static extern bool GetCursorPos(ref Point lpPoint);

        private Point defPnt = new Point();

        private List<Point> _cursors;
        public string GetCursorsString {
            get{
                if (_cursors.Count==2)
                    return $"{_cursors[0].X},{_cursors[0].Y}\n{_cursors[1].X},{_cursors[1].Y}";
                else
                    return $"{_cursors[0].X},{_cursors[0].Y}";
            } }
        public Action storeCursorPos;

        public ImageCapturer()
        {
            _cursors = new List<Point>();
            storeCursorPos=() => {
                var tmp = new Point();
                GetCursorPos(ref tmp);
                _cursors.Add(tmp);
                if (_cursors.Count > 2)
                    _cursors.RemoveAt(0);
            };


        }



        private Bitmap GetSreenshot(int sourceX,int sourceY,int sizeX, int sizeY)
        {
            Bitmap bm = new Bitmap(1920, 1080);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            return bm;
        }

        public Bitmap GetImageFromCursors()
        {
            if (_cursors.Count < 2)
                return null;
            _cursors.Sort((x,y)=> { return x.X - y.X; });
            var a = _cursors[0];
            var b = _cursors[1];


            Bitmap ret = new Bitmap(b.X-a.X,b.Y-a.Y);
            Graphics g = Graphics.FromImage(ret);
            g.CopyFromScreen(a.X, a.Y, 0, 0, ret.Size);
            return ret;
        }

        public Point GetCursorPos()
        {
            GetCursorPos(ref defPnt);
            return defPnt;
        }

        public string GetTexts()
        {
            using (var engine = new TesseractEngine(@"./tessdata", "jpn", EngineMode.Default))
            {
                using (var img = PixConverter.ToPix(GetImageFromCursors()))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();

                        return text;
                    }
                }
            }
        }
        
    }
}
