﻿// See https://aka.ms/new-console-template for more information

using AutoLeoThap;
using KAutoHelper;
using System.Drawing;
using System.Drawing.Imaging;

class Program
{
    static void Main(string[] args)
    {
       
        Console.WriteLine("Hello, World!");
var hWnd = IntPtr.Zero;

hWnd = AutoControl.FindWindowHandle(null, "TM s44");


var vptCapturer = new VptCapturer(hWnd);

var sub = (Bitmap)Bitmap.FromFile("images\\dang-nhap-btn.png");
var btnDangNhap = ImageScanOpenCV.FindOutPoint(vptCapturer.FullImage, sub);



if (btnDangNhap != null)
{
    AutoControl.SendClickOnPosition(hWnd, btnDangNhap.Value.X + 30, btnDangNhap.Value.Y + 20);
}


new Application(hWnd).run();

    }
}

