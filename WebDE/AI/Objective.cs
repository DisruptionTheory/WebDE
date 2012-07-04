using System;

using SharpKit.JavaScript;

using WebDE.GameObjects;

namespace WebDE.AI
{
    [JsType(JsMode.Clr, Filename = "../scripts/AI.js")]
    public partial class Objective
    {
        private GameEntity target;
    }
}
