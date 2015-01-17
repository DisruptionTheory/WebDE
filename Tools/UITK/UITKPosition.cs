﻿using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class UITKPosition
    {
        internal UITKComponent Owner
        {
            get;
            private set;
        }

        public int X
        {
            get;// {if(AlignCenterHorizontal)return
            internal set;// { internalx = value; }
        }
        private int internalx;

        public int Y
        {
            get;
            internal set;
        }
        private int internaly;

        public UITKPosition(int x, int y, UITKComponent owner)
        {
            X = x;
            Y = y;
            Owner = owner;
            //AlignCenterHorizontal = false;
            //AlignCenterVertical = false;
        }

        private void AlignCenterHorizontal()
        {
            int surfaceCenter = Surface.Width / 2;
            int componentCenter = Owner.Width / 2;
            X = surfaceCenter - componentCenter;
        }

        private void AlignCenterVertical()
        {
            int surfaceCenter = Surface.Height / 2;
            int componentCenter = Owner.Height / 2;
            Y = surfaceCenter - componentCenter;
        }
    }
}