using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Animation;
using WebDE.GUI;
using WebDE.GameObjects;

//http://www.colorzilla.com/gradient-editor/
//http://www.colorzilla.com/gradient-editor/#d3cf54+1,000000+100&1+0,0+100;Custom
namespace WebDE.Rendering
{
    //visual gradient area when rendering
    //really (for now) it's kind of just a series of stuff to be applied to an element
    //and can also be returned as a string...
    [JsType(JsMode.Clr, Filename = "../scripts/Rendering.js")]
    public class Gradient
    {
        /*
        public static string ApplyToElement(HTMLelement elementToGradient, Color gradientColor)
        {
            elementToGradient.Style.Background = "-webkit-radial-gradient(center, ellipse cover, " +
                "rgba(" + gradientColor.red + "," + gradientColor.green + "," + gradientColor.blue + ",1) 0%, " +
                "rgba(" + gradientColor.red + "," + gradientColor.green + "," + gradientColor.blue + ",0.99) 1%, rgba(0,0,0,0) 100%)";
        }
        */

        public static string ToString(Color gradientColor)
        {
            return "-webkit-radial-gradient(center, ellipse cover, " +
                "rgba(" + gradientColor.red + "," + gradientColor.green + "," + gradientColor.blue + ",1) 0%, " +
                "rgba(" + gradientColor.red + "," + gradientColor.green + "," + gradientColor.blue + ",0.99) 1%, rgba(0,0,0,0) 100%)"; 
                //"background: -webkit-gradient(radial, center center, 0px, center center, 100%, color-stop(0%,rgba(211,207,84,1)), color-stop(1%,rgba(211,207,84,0.99)), color-stop(100%,rgba(0,0,0,0))); /* Chrome,Safari4+ */" + 
                //"-webkit-radial-gradient(center, ellipse cover, rgba(211,207,84,1) 0%,rgba(211,207,84,0.99) 1%,rgba(0,0,0,0) 100%); /* Chrome10+,Safari5.1+ */";
                //"background: radial-gradient(center, ellipse cover, rgba(211,207,84,1) 0%,rgba(211,207,84,0.99) 1%,rgba(0,0,0,0) 100%); /* W3C */";
        }

        public static string LightStone(Color gradientColor)
        {
            return
                //" url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPHJhZGlhbEdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgY3g9IjUwJSIgY3k9IjUwJSIgcj0iNzUlIj4KICAgIDxzdG9wIG9mZnNldD0iMTUlIiBzdG9wLWNvbG9yPSIjMDA1MGM4IiBzdG9wLW9wYWNpdHk9IjAuMjUiLz4KICAgIDxzdG9wIG9mZnNldD0iMjYlIiBzdG9wLWNvbG9yPSIjMDA1MGM4IiBzdG9wLW9wYWNpdHk9IjAuMzQiLz4KICAgIDxzdG9wIG9mZnNldD0iNTklIiBzdG9wLWNvbG9yPSIjMDA1MGM4IiBzdG9wLW9wYWNpdHk9IjAuNiIvPgogICAgPHN0b3Agb2Zmc2V0PSI2NiUiIHN0b3AtY29sb3I9IiMwMDUwYzgiIHN0b3Atb3BhY2l0eT0iMC42NSIvPgogICAgPHN0b3Agb2Zmc2V0PSI4NSUiIHN0b3AtY29sb3I9IiMwMDUwYzgiIHN0b3Atb3BhY2l0eT0iMCIvPgogICAgPHN0b3Agb2Zmc2V0PSIxMDAlIiBzdG9wLWNvbG9yPSIjMDA1MGM4IiBzdG9wLW9wYWNpdHk9IjAiLz4KICA8L3JhZGlhbEdyYWRpZW50PgogIDxyZWN0IHg9Ii01MCIgeT0iLTUwIiB3aWR0aD0iMTAxIiBoZWlnaHQ9IjEwMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+); " +
                //" -moz-radial-gradient(center, ellipse cover,  rgba(0,80,200,0.25) 15%, rgba(0,80,200,0.34) 26%, rgba(0,80,200,0.6) 59%, rgba(0,80,200,0.65) 66%, rgba(0,80,200,0) 85%, rgba(0,80,200,0) 100%); " +
                " -webkit-gradient(radial, center center, 0px, center center, 100%, color-stop(15%,rgba(0,80,200,0.25)), color-stop(26%,rgba(0,80,200,0.34)), color-stop(59%,rgba(0,80,200,0.6)), color-stop(66%,rgba(0,80,200,0.65)), color-stop(85%,rgba(0,80,200,0)), color-stop(100%,rgba(0,80,200,0))); " +
                " -webkit-radial-gradient(center, ellipse cover,  rgba(0,80,200,0.25) 15%,rgba(0,80,200,0.34) 26%,rgba(0,80,200,0.6) 59%,rgba(0,80,200,0.65) 66%,rgba(0,80,200,0) 85%,rgba(0,80,200,0) 100%); ";
                //" -o-radial-gradient(center, ellipse cover,  rgba(0,80,200,0.25) 15%,rgba(0,80,200,0.34) 26%,rgba(0,80,200,0.6) 59%,rgba(0,80,200,0.65) 66%,rgba(0,80,200,0) 85%,rgba(0,80,200,0) 100%); " +
                //" -ms-radial-gradient(center, ellipse cover,  rgba(0,80,200,0.25) 15%,rgba(0,80,200,0.34) 26%,rgba(0,80,200,0.6) 59%,rgba(0,80,200,0.65) 66%,rgba(0,80,200,0) 85%,rgba(0,80,200,0) 100%); " +
                //" radial-gradient(ellipse at center,  rgba(0,80,200,0.25) 15%,rgba(0,80,200,0.34) 26%,rgba(0,80,200,0.6) 59%,rgba(0,80,200,0.65) 66%,rgba(0,80,200,0) 85%,rgba(0,80,200,0) 100%); " +
                //"filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#400050c8', endColorstr='#000050c8',GradientType=1 );";
        }
    }
}
