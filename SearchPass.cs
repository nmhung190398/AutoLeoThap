using System.Drawing;
using AutoLeoThap;
using KAutoHelper;

namespace SearchPassWord;

public class SearchPass
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

        static Bitmap KHO = (Bitmap)Bitmap.FromFile("images\\kho.png");
        static Bitmap KHO_2 = (Bitmap)Bitmap.FromFile("images\\kho-2.png");
        static Bitmap OK = (Bitmap)Bitmap.FromFile("images\\ok.png");
        static Bitmap OK_FAIL = (Bitmap)Bitmap.FromFile("images\\ok-fail.png");
    }


    public SearchPass(IntPtr intPtr)
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

    VKeys changeNumber(VKeys VKeys, int number)
    {
        switch (number)
        {
            case 0:
                VKeys = VKeys.VK_0;
                break;
            case 1:
                VKeys = VKeys.VK_1;
                break;
            case 2:
                VKeys = VKeys.VK_2;
                break;
            case 3:
                VKeys = VKeys.VK_3;
                break;
            case 4:
                VKeys = VKeys.VK_4;
                break;
            case 5:
                VKeys = VKeys.VK_5;
                break;
            case 6:
                VKeys = VKeys.VK_6;
                break;
            case 7:
                VKeys = VKeys.VK_7;
                break;
            case 8:
                VKeys = VKeys.VK_8;
                break;
            case 9:
                VKeys = VKeys.VK_9;
                break;
        }

        return VKeys;
    }

    public void run()
    {
        var number1 = VKeys.VK_0;
        var number2 = VKeys.VK_0;
        var number3 = VKeys.VK_0;
        var number4 = VKeys.VK_9;
        var number5 = VKeys.VK_2;
        var number6 = VKeys.VK_0;

        int numberCheck1 = 0;
        int numberCheck2 = 2;
        int numberCheck3 = 9;
        int numberCheck4 = 0;
        int numberCheck5 = 0;
        int numberCheck6 = 0;


        while (true)
        {
            reloadVptCapturer();
            if (searchImage(Data.KHO) || searchImage(Data.KHO_2))
            {
                // AutoControl.SendKeyBoardPress(IntPtr , VKeys.VK_0 );
                AutoControl.SendKeyChar(IntPtr, number1);
                AutoControl.SendKeyChar(IntPtr, number2);
                AutoControl.SendKeyChar(IntPtr, number3);
                AutoControl.SendKeyChar(IntPtr, number4);
                AutoControl.SendKeyChar(IntPtr, number5);
                AutoControl.SendKeyChar(IntPtr, number6);
                Thread.Sleep(500);
                AutoControl.SendClickOnPosition(IntPtr, 499, 389);
                var loopRp = loop(() =>
                {
                    Thread.Sleep(500);
                    reloadVptCapturer();


                    if (searchImage(Data.OK_FAIL))
                    {
                        AutoControl.SendClickOnPosition(IntPtr, 531, 370);
                        var loopRp = loop(() =>
                        {
                            AutoControl.SendClickOnPosition(IntPtr, 197, 557);
                            if (searchImage(Data.KHO) || searchImage(Data.KHO_2))
                            {
                                if (numberCheck1 == 9)
                                {
                                    numberCheck1 = 0;

                                    if (numberCheck2 == 9)
                                    {
                                        numberCheck2 = 0;
                                        number5 = changeNumber(number5, numberCheck2);
                                        if (numberCheck3 == 9)
                                        {
                                            numberCheck3 = 0;
                                            number4 = changeNumber(number4, numberCheck3);
                                            if (numberCheck4 == 9)
                                            {
                                                numberCheck4 = 0;
                                                number3 = changeNumber(number3, numberCheck4);
                                                if (numberCheck5 == 9)
                                                {
                                                    numberCheck5 = 0;
                                                    number2 = changeNumber(number2, numberCheck5);
                                                    if (numberCheck6 == 10)
                                                    {
                                                        return false;
                                                    }
                                                    else
                                                    {
                                                        numberCheck6 += 1;
                                                        number1 = changeNumber(number1, numberCheck6);
                                                    }
                                                }
                                                else
                                                {
                                                    numberCheck5 += 1;
                                                    number2 = changeNumber(number2, numberCheck5);
                                                }
                                            }
                                            else
                                            {
                                                numberCheck4 +=1 ;
                                                number3 = changeNumber(number3, numberCheck4);
                                            }
                                        }
                                        else
                                        {
                                            numberCheck3 += 1;
                                            number4 = changeNumber(number4, numberCheck3);
                                        }
                                    }
                                    else
                                    {
                                        numberCheck2 += 1;
                                        number5 = changeNumber(number5, numberCheck2);
                                    }

                                    number6 = changeNumber(number6, numberCheck1);
                                }
                                else
                                {
                                    numberCheck1 += 1;
                                    number6 = changeNumber(number6, numberCheck1);
                                }


                                ;
                                return false;
                            }

                            return true;
                        });
                        Console.WriteLine($"{numberCheck6}{numberCheck5}{numberCheck4}{numberCheck3}{numberCheck2}{numberCheck1}   {number1}{number2}{number3}{number4}{number5}{number6}");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine($"pass ruong la: {numberCheck6}{numberCheck5}{numberCheck4}{numberCheck3}{numberCheck2}{numberCheck1} || {number1}{number2}{number3}{number4}{number5}{number6}");
                    }

                    return true;
                });
            }
            else
            {
                AutoControl.SendClickOnPosition(IntPtr, 197, 557);
            }
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