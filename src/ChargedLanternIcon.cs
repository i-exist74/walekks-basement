using Fisobs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WalekksBasement
{
    public class ChargedLanternIcon : Icon
    {
        public override int Data(AbstractPhysicalObject apo)
        {
            return apo is ChargedLanternAbstract chargedLantern ? (int)(chargedLantern.hue * 1000f) : 0;
        }

        public override Color SpriteColor(int data)
        {
            return RWCustom.Custom.HSL2RGB(data / 1000f, 0.65f, 0.4f);
        }

        public override string SpriteName(int data)
        {
            return "icon_ChargedLantern";
        }
    }
}
