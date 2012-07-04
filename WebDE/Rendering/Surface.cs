using System;

using SharpKit.JavaScript;

using WebDE.Timekeeper;

namespace WebDE.Rendering
{
    [JsType(JsMode.Clr, Filename = "../scripts/Rendering.js")]
    public static class Surface
    {
        private static IRenderEngine renderer;

        public static void Initialize(IRenderEngine renderer)
        {
            Surface.renderer = renderer;

            Clock.AddRender(Surface.renderer.Render);
        }
    }
}