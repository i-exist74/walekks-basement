using Fisobs.Core;
using UnityEngine;


namespace WalekksBasement
{
    public class ChargedLanternAbstract : AbstractPhysicalObject
    {
        public float hue;
        public float saturation;
        public float scaleX;
        public float scaleY;
        public float damage;
        public float fuel;

        public ChargedLanternAbstract(World world, WorldCoordinate pos, EntityID ID) : base(world, ChargedLanternFisob.ChargedLantern, null, pos, ID)
        {
            scaleX = 1;
            scaleY = 1;
            saturation = 0.5f;
            hue = 1f;
            fuel = -1f;
        }

        public override void Realize()
        {
            base.Realize();
            if (realizedObject == null)
                realizedObject = new ChargedLantern(this, Room.realizedRoom.MiddleOfTile(pos.Tile), Vector2.zero);
        }

        public override string ToString()
        {
            return this.SaveToString($"{hue};{saturation};{scaleX};{scaleY};{damage};{fuel}");
        }
    }
}
