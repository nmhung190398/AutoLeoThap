using KAutoHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Stitching;

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
        static Bitmap NPC_BBT2 = (Bitmap)Bitmap.FromFile("images\\npc-bbt2.png");
        static Bitmap MAP_BBT = (Bitmap)Bitmap.FromFile("images\\bbt.png");
        static Bitmap MAP_AMT = (Bitmap)Bitmap.FromFile("images\\amt.png");
        static Bitmap MAP_AMT_2 = (Bitmap)Bitmap.FromFile("images\\amt2.png");
        static Bitmap VAO_AMT = (Bitmap)Bitmap.FromFile("images\\vao-amt.png");
        static Bitmap OUT_AMT = (Bitmap)Bitmap.FromFile("images\\out-amt.png");
        static Bitmap BOSS_AMT = (Bitmap)Bitmap.FromFile("images\\boss-amt.png");
        static Bitmap BOSS_AMT_2 = (Bitmap)Bitmap.FromFile("images\\boss-amt-2.png");
        static Bitmap BOSS_AMT_3 = (Bitmap)Bitmap.FromFile("images\\boss-amt-3.png");
        static Bitmap DAME_BOSS = (Bitmap)Bitmap.FromFile("images\\dame.png");
        static Bitmap BOSS_DIA_AMT = (Bitmap)Bitmap.FromFile("images\\dia-sat-ma-anh.png");
        static Bitmap BOSS_HOA_AMT = (Bitmap)Bitmap.FromFile("images\\hoa-da-anh-ma.png");
        static Bitmap BOSS_PHONG_AMT = (Bitmap)Bitmap.FromFile("images\\vao-amt.png");

        static Point NPC_ACTION_01 = new Point(253, 336);
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
            var main = VptCapturer.FullImage;
            if (ImageScanOpenCV.FindOutPoint(main, Data.MAP_BBT) != null)
            {
                return Location.BBT;
            }
            else if(ImageScanOpenCV.FindOutPoint(main, Data.MAP_AMT) != null || ImageScanOpenCV.FindOutPoint(main, Data.MAP_AMT_2) != null)
            {
                return Location.AMT;
            }
            else
            {
                return Location.NONE;
            }
            
           
        }


        bool canAttack()
        {
            return true;
        }

        bool bossExist()
        {
            if (searchImage(Data.BOSS_HOA_AMT) )
            {
                return false;
            } else if(searchImage(Data.BOSS_DIA_AMT))
            {
                return false;
            }
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
                var location = getLocation();
                if (location == Location.BBT)
                {
                    //1.close daily tab
                    searchImage(Data.CLOSE_DAILY_TAB, click: true);
                    //2.search and click npc
                    //3. vào ảo ma tháp
                    var loopRp = loop(() =>
                    {
                        reloadVptCapturer();
                        
                        Thread.Sleep(1000);
                        if(searchImage(Data.NPC_BBT, click: true) || searchImage(Data.NPC_BBT2, click: true))
                        {
                            Thread.Sleep(1000);
                            AutoControl.SendClickOnPosition(IntPtr, Data.NPC_ACTION_01.X , Data.NPC_ACTION_01.Y);
                            Thread.Sleep(1000);
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
                    reloadVptCapturer();
                    //1.click oki
                    AutoControl.SendClickOnPosition(IntPtr, 529 , 381);
                    if( searchImage(Data.BOSS_AMT) || searchImage(Data.BOSS_AMT_2) || searchImage(Data.BOSS_AMT_3))
                    {
                        
                        if (!bossExist())
                        {
                            //lên tầng
                            var loopRp = loop(() =>
                            {
                                // click npc ô chát
                                AutoControl.SendClickOnPosition(IntPtr, 244, 558);
                                reloadVptCapturer();
                                // kiểm tra xem tab npc action đã open chưa
                                if (searchImage(Data.OUT_AMT , click:true))
                                {
                                    var loopRp = loop(() =>
                                    {
                                        // click npc ô chát
                                        AutoControl.SendClickOnPosition(IntPtr, 244, 558);
                                        reloadVptCapturer();
                                        // kiểm tra xem tab npc action đã open chưa
                                        if (searchImage(Data.MAP_BBT))
                                        {
                                            // click lên tầng
                                            AutoControl.SendClickOnPosition(IntPtr, 876, 23);
                                            Thread.Sleep(500);
                                            AutoControl.SendClickOnPosition(IntPtr, 521, 341);
                                            Thread.Sleep(500);
                                            AutoControl.SendClickOnPosition(IntPtr, 532, 373);
                                            
                                            return false;
                                        }
                            
                                        return true;
                                    });
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
                                if (searchImage(Data.BOSS_AMT, click: true) || searchImage(Data.BOSS_AMT_2, click: true) || searchImage(Data.BOSS_AMT_3, click: true))
                                {
                                    Thread.Sleep(1000);
                                    // kiểm tra xem tab npc action đã open chưa
                                    if (searchImage(Data.DAME_BOSS, click: true))
                                    {
                                        // click tân công
                                        AutoControl.SendClickOnPosition(IntPtr, Data.NPC_ACTION_01.X, Data.NPC_ACTION_01.Y);
                                        
                                        return false;
                                    }
                                }
                                return true;
                            });
                    
                        }
                    }
                    else // lên tầng
                    {
                        var loopRp = loop(() =>
                        {
                            // click npc ô chát
                            AutoControl.SendClickOnPosition(IntPtr, 244, 558);
                            reloadVptCapturer();
                            // kiểm tra xem tab npc action đã open chưa
                            if (searchImage(Data.OUT_AMT))
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
                Thread.Sleep(1000);
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
                Thread.Sleep(1000);
            }

            return false;
        }
    }
}
