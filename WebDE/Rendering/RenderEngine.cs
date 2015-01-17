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
        void Initialize();

        void Render();

        void RenderUpdate();

        Dimension GetSize();

        void Resize();

        void RenderGameEntity(GameEntity gent, View view);

        void DestroyGameEntity(GameEntity gent);

        void DestroyGUILayer(GuiLayer gent);

        void SetNeedsUpdate(GameEntity gent);

        void SetNeedsUpdate(GuiElement gelm);

        void SetNeedsUpdate(GuiLayer gelm);

        void CenterViewOn(int tileX, int tileY);

        void ClearGameBoard();

        void SetPerspective(int xOffset, int yOffset);

        void RebuildAnimationFrames();
    }

    public class DisplayOptions
    {
        private int resolutionWidth = 800;
        private int resolutionHeight = 600;

        public Dimension resolution { get; set; }

        //show outlines of objects hidden behind other objects, such as players walking behind trees
        private bool OcclusionOutlines;
    }
}