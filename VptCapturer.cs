using KAutoHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLeoThap
{
    public class VptCapturer
    {
        public Bitmap FullImage { get; private set; }

        public Bitmap MapNameImage { get; private set; }


        public VptCapturer(IntPtr intPtr)
        {
            FullImage = (Bitmap)CaptureHelper.CaptureWindow(intPtr);
        }

    }
}
