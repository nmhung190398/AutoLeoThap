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

Point npcAction01 = new Point(548, 523);
Point npcAction03 = new Point(548, 574);

if (btnDangNhap != null)
{
    AutoControl.SendClickOnPosition(hWnd, btnDangNhap.Value.X + 30, btnDangNhap.Value.Y + 20);
}

enum AcctionType
{

}

enum Location
{
    BBT,
    AMT,
    IN_GAME,
    NONE
}

class Main
{
    static void checkAction()
    {

    }

    static Location getLocation()
    {
        return Location.NONE;
    }


    static bool canAttack()
    {
        return true;
    }

    static bool bossExist()
    {
        return true;
    }

    static void run()
    {

        while (false)
        {
            var location = getLocation();
            if (location == Location.BBT)
            {
                //1.close daily tab
                //2.search and click npc
                //3. vào ảo ma tháp
            }
            else if (location == Location.AMT)
            {
                //1.click oki
                if (!bossExist())
                {
                    //lên tầng
                }
                else if (canAttack())
                {
                    // click boss
                    // attack

                }
                else
                {
                    //click npc
                    // out amt
                }

            }
            else if (location == Location.IN_GAME)
            {

            }
            else
            {

            }
            Thread.Sleep(200);
        }
    }
}


