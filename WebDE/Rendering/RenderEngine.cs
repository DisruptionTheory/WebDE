using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.GameObjects;
using WebDE.GUI;

namespace WebDE.Rendering
{
    [JsType(JsMode.Clr, Filename = "../scripts/Rendering.js")]
    public interface IRenderEngine
    {
        void InitialRender();

        void Render();

        Dimension GetSize();

        void Resize();

        void RenderGameEntity(GameEntity gent);

        void DestroyGameEntity(GameEntity gent);

        void SetNeedsUpdate(GameEntity gent);

        void SetNeedsUpdate(GuiElement gelm);

        void SetNeedsUpdate(GuiLayer gelm);
    }
}