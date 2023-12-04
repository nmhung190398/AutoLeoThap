using System.Drawing;
using KAutoHelper;

namespace AutoLeoThap;

public class AutoOc
{
    public interface Data
    {
        static Bitmap Q_OC = (Bitmap)Bitmap.FromFile("images\\da-nhan-q-oc.png");
        static Bitmap DANG_BEM = (Bitmap)Bitmap.FromFile("images\\dang-bem-quai.png");
        static Bitmap DONE_Q = (Bitmap)Bitmap.FromFile("images\\done-q.png");
        static Bitmap CANCEL_Q = (Bitmap)Bitmap.FromFile("images\\cancel-q.png");
        static Bitmap CHUA_NHAN_Q = (Bitmap)Bitmap.FromFile("images\\Chua-nhan-q.png");
        static Bitmap CAN_NHAN_Q = (Bitmap)Bitmap.FromFile("images\\can-nhan-q.png");
    }


    public AutoOc(IntPtr intPtr)
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
            if (searchImage(Data.Q_OC))
            {
                var loopRp = loop(() =>
                {
                    // click npc ô chát
                    AutoControl.SendClickOnPosition(IntPtr, 254, 557);
                    reloadVptCapturer();
                    // kiểm tra xem tab npc action đã open chưa
                    if (searchImage(Data.DANG_BEM))
                    {
                        return false;
                    }

                    return true;
                });
            }
            else if (searchImage(Data.DONE_Q))
            {
                var loopRp = loop(() =>
                {
                    // click npc ô chát
                    AutoControl.SendClickOnPosition(IntPtr, 980, 649);
                    Thread.Sleep(1000);
                    reloadVptCapturer();
                    if (searchImage(Data.CANCEL_Q, click: true))
                    {
                        Thread.Sleep(1000);
                        AutoControl.SendClickOnPosition(IntPtr, 493, 373);
                        return false;
                    }

                    return true;
                });
            }
            else if (searchImage(Data.CHUA_NHAN_Q))
            {
                var loopRp = loop(() =>
                {
                    // click npc ô chát
                    AutoControl.SendClickOnPosition(IntPtr, 147, 557);
                    Thread.Sleep(1000);
                    reloadVptCapturer();
                    if (searchImage(Data.CAN_NHAN_Q, click: true))
                    {
                        Thread.Sleep(1000);
                        AutoControl.SendClickOnPosition(IntPtr, 525, 651);
                        return false;
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