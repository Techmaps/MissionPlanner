﻿using MissionPlanner.Drawing;
using System;
using System.Drawing;


namespace MissionPlanner.Drawing
{
    public class Form
    {
        public static Point MousePosition { get; set; }
        public static Keys ModifierKeys { get; set; }
    }

    public class PaintEventArgs : EventArgs
    {
        private Rectangle clientRectangle;
        private IGraphics gg;
        public PaintEventArgs(IGraphics gg, Rectangle clientRectangle)
        {
            this.gg = gg;
            this.clientRectangle = clientRectangle;
        }

        public IGraphics Graphics => gg;
    }
}