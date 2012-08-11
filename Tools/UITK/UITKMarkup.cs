using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    public class UITKMarkup
    {
        private string markup = string.Empty;
        internal UITKMarkup(string html) { markup = html;  }

        public string GetMarkup()
        {
            return markup;
        }

        public void SetMarkup(string html)
        {
            markup = html;
        }

    }
}