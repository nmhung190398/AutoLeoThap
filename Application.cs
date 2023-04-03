using KAutoHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLeoThap
{
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

    public interface Data
    {
        static Bitmap CLOSE_DAILY_TAB = (Bitmap)Bitmap.FromFile("images\\clone-daily-tab.png");
        static Bitmap NPC_BBT = (Bitmap)Bitmap.FromFile("images\\npc-bbt.png");

        static Point NPC_ACTION_01 = new Point(548, 523);
        static Point NPC_ACTION_02 = new Point(548, 574);
    }

    public class Application
    {
        public Application(IntPtr intPtr)
        {
            IntPtr = intPtr;
        }

        private IntPtr IntPtr { get; set; }
        private VptCapturer VptCapturer { get; set; }


        void checkAction()
        {

        }

        void reloadVptCapturer()
        {
            VptCapturer = new VptCapturer(IntPtr);
        }

        Location getLocation()
        {
            return Location.NONE;
        }


        bool canAttack()
        {
            return true;
        }

        bool bossExist()
        {
            return true;
        }

        bool searchImage(Bitmap sub,Bitmap main = null, bool click = false, bool reload = true)
        {
            if(main == null)
            {
                main = VptCapturer.FullImage;
            }
            if (reload)
            {
                reloadVptCapturer();
            }
            var point = ImageScanOpenCV.FindOutPoint(main, sub);

            if (click)
            {
                AutoControl.SendClickOnPosition(IntPtr, point.Value.X + sub.Width / 2, point.Value.Y + sub.Height / 2);
            }

            return point != null;
        }

        public void run()
        {

            while (false)
            {
                reloadVptCapturer();
                var location = getLocation();
                if (location == Location.BBT)
                {
                    //1.close daily tab
                    searchImage(Data.CLOSE_DAILY_TAB, click: true);



                    //2.search and click npc
                    //3. vào ảo ma tháp
                    var loopRp = loop(() =>
                    {
                        if(searchImage(Data.CLOSE_DAILY_TAB, click: true))
                        {
                            Thread.Sleep(100);
                            AutoControl.SendClickOnPosition(IntPtr, Data.NPC_ACTION_01.X , Data.NPC_ACTION_01.Y);
                            return false;
                        }
                        return true;
                    });

                    if (!loopRp)
                    {
                        // time out
                    }
                }
                else if (location == Location.AMT)
                {
                    //1.click oki
                    if (!bossExist())
                    {
                        //lên tầng
                        var loopRp = loop(() =>
                        {
                            // click npc ô chát
                            AutoControl.SendClickOnPosition(IntPtr, 0, 0);
                            reloadVptCapturer();
                            // kiểm tra xem tab npc action đã open chưa
                            if (searchImage(null))
                            {
                                // click lên tầng
                                AutoControl.SendClickOnPosition(IntPtr, Data.NPC_ACTION_01.X, Data.NPC_ACTION_01.Y);
                                return false;
                            }
                            
                            return true;
                        });
                    }
                    else if (canAttack())
                    {
                        // click boss
                        // attack
                        var loopRp = loop(() =>
                        {
                            // click bosss
                            if (searchImage(Data.CLOSE_DAILY_TAB, click: true))
                            {
                                Thread.Sleep(100);
                                // kiểm tra xem tab npc action đã open chưa
                                if (searchImage(null))
                                {
                                    // click tân công
                                    AutoControl.SendClickOnPosition(IntPtr, Data.NPC_ACTION_01.X, Data.NPC_ACTION_01.Y);
                                    return false;
                                }
                            }
                            return true;
                        });

                    }
                    else
                    {
                        //lên tầng
                        var loopRp = loop(() =>
                        {
                            // click npc ô chát
                            AutoControl.SendClickOnPosition(IntPtr, 0, 0);
                            reloadVptCapturer();
                            // kiểm tra xem tab npc action đã open chưa
                            if (searchImage(null))
                            {
                                // click rời ảo ma tháp
                                AutoControl.SendClickOnPosition(IntPtr, Data.NPC_ACTION_01.X, Data.NPC_ACTION_01.Y);
                                return false;
                            }

                            return true;
                        });

                        if (loopRp)
                        {
                            // đổi kênh
                        }
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



        bool loop(Func<bool> func,long timeout = 30)
        {
            while (true)
            {
                if (!func.Invoke())
                {
                    return true;
                }
                Thread.Sleep(100);
            }

            return false;
        }
    }
}
