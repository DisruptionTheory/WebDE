using System;
using System.Collections.Generic;

using WebDE.Animation;

namespace WebDE.GameObjects
{
    public partial class Item : GameEntity
    {
        public Item(string itemName)
            : base(itemName)
        {
        }

        public void SetWorldSprite(Sprite newSprite)
        {
        }

        public void SetInventorySprite(Sprite newSprite)
        {
        }

        public void SetSprite(Sprite newSprite)
        {
            this.SetWorldSprite(newSprite);
            this.SetInventorySprite(newSprite);
        }
    }
}
