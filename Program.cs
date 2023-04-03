// See https://aka.ms/new-console-template for more information

using AutoLeoThap;
using KAutoHelper;
using System.Drawing;
using System.Drawing.Imaging;

var hWnd = IntPtr.Zero;

hWnd = AutoControl.FindWindowHandle(null, "HS s44");

Console.WriteLine("Hello, World!");

var vptCapturer = new VptCapturer(hWnd);

vptCapturer.FullImage.Save("D:\\my-projects\\AutoLeoThap\\test.png");
var sub = (Bitmap)Bitmap.FromFile("images\\dang-nhap-btn.png");
sub.Save("D:\\my-projects\\AutoLeoThap\\test01.png");
var btnDangNhap = ImageScanOpenCV.FindOutPoint(vptCapturer.FullImage, sub);



if (btnDangNhap != null)
{
    AutoControl.SendClickOnPosition(hWnd, btnDangNhap.Value.X + 30, btnDangNhap.Value.Y + 20);
}


new Application(hWnd).run();
