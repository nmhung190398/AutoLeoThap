using System.Drawing;
using KAutoHelper;

namespace AutoLeoThap;

public class AutoThap
{
    public interface Data
    {
        static Bitmap BOSS_AMT = (Bitmap)Bitmap.FromFile("images\\boss-amt.png");
        static Bitmap BOSS_AMT_2 = (Bitmap)Bitmap.FromFile("images\\boss-amt-2.png");
        static Bitmap BOSS_AMT_3 = (Bitmap)Bitmap.FromFile("images\\boss-amt-3.png");
        static Bitmap BOSS_AMT_4 = (Bitmap)Bitmap.FromFile("images\\boss-amt-4.png");
        static Bitmap DAME_BOSS = (Bitmap)Bitmap.FromFile("images\\dame.png");
        static Bitmap DANG_BEM = (Bitmap)Bitmap.FromFile("images\\dang-bem-quai.png");
        static Bitmap LEN_TANG = (Bitmap)Bitmap.FromFile("images\\len-tang.png");
        static Bitmap OK_THAP = (Bitmap)Bitmap.FromFile("images\\ok-thap.png");
    }


    public AutoThap(IntPtr intPtr)
    {
        IntPtr = intPtr;
    }

    private IntPtr IntPtr { get; set; }
    private VptCapturer VptCapturer { get; set; }

    void reloadVptCapturer()
    {
        VptCapturer = new VptCapturer(IntPtr);
    }

    bool searchImage(Bitmap sub, Bitmap main = null, bool click = false, bool reload = true)
    {
        if (main == null)
        {
            main = VptCapturer.FullImage;
        }

        if (reload)
        {
            reloadVptCapturer();
        }

        var point = ImageScanOpenCV.FindOutPoint(main, sub, 0.8);

        if (click && point != null)
        {
            AutoControl.SendClickOnPosition(IntPtr, point.Value.X + sub.Width / 2, point.Value.Y - sub.Height / 2);
        }

        return point != null;
    }

    public void run()
    {
        while (true)
        {
            reloadVptCapturer();
            if (searchImage(Data.OK_THAP))
            {
                AutoControl.SendClickOnPosition(IntPtr, 524, 381);
                
            }
            else if (searchImage(Data.BOSS_AMT, click: true) || searchImage(Data.BOSS_AMT_2, click: true) || searchImage(Data.BOSS_AMT_3, click: true)|| searchImage(Data.BOSS_AMT_4, click: true))
            {
                var loopRp = loop(() =>
                {
                    // click npc ô chát
                    if (searchImage(Data.BOSS_AMT, click: true) || searchImage(Data.BOSS_AMT_2, click: true) || searchImage(Data.BOSS_AMT_3, click: true) || searchImage(Data.BOSS_AMT_4, click: true))
                    {
                        Thread.Sleep(1000);
                        reloadVptCapturer();
                        // kiểm tra xem tab npc action đã open chưa
                        if (searchImage(Data.DAME_BOSS))
                        {
                            AutoControl.SendClickOnPosition(IntPtr, 532, 522);
                            return false;
                        }

                        return true;
                    }

                    return false;
                });
                
            }
            else
            {
                var loopRp = loop(() =>
                {
                    if (searchImage(Data.DANG_BEM))
                    {
                        return false;
                    }
                    else {
                        // click npc ô chát
                        AutoControl.SendClickOnPosition(IntPtr, 183, 558);
                        Thread.Sleep(500);
                        reloadVptCapturer();
                        if (searchImage(Data.LEN_TANG))
                        {
                            Thread.Sleep(1000);
                            AutoControl.SendClickOnPosition(IntPtr, 538, 522);
                            return false;
                        }
                        return true;
                    }
                    
                    return true;
                    
                });
            }


            Thread.Sleep(1000);
        }
    }


    bool loop(Func<bool> func, long timeout = 30)
    {
        while (true)
        {
            if (!func.Invoke())
            {
                return true;
            }

            Thread.Sleep(1000);
        }

        return false;
    }
}