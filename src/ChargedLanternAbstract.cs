using Fisobs.Core;
using UnityEngine;


namespace WalekksBasement
{
    public class ChargedLanternAbstract : AbstractPhysicalObject
    {
        public float scaleX;
        public float scaleY;
        public float damage;
        public float fuel;

        public ChargedLanternAbstract(World world, WorldCoordinate pos, EntityID ID) : base(world, ChargedLanternFisob.ChargedLantern, null, pos, ID)
        {
            scaleX = 1;
            scaleY = 1;
            fuel = 5f;
        }

        public override void Realize()
        {
            base.Realize();
            realizedObject ??= new ChargedLantern(this, Room.realizedRoom.MiddleOfTile(pos.Tile), Vector2.zero);
        }

        public override string ToString()
        {
            return this.SaveToString($"{scaleX};{scaleY};{damage};{fuel}");
        }
    }
}
