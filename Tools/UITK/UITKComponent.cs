using System;
using System.Collections.Generic;
using SharpKit.JavaScript;
using SharpKit.Html4;
using SharpKit.jQuery;

namespace UITK
{
    [JsType(JsMode.Clr, Filename = "scripts/UITK.js")]
    internal interface UITKComponent
    {
        int Height { get; }
        int Width { get; }
        UITKStyles Styles { get; }
        UITKMarkup Markup { get; }
        string Id { get; }
    }
}