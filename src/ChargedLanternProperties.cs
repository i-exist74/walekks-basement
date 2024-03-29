﻿using Fisobs.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalekksBasement
{
    public class ChargedLanternProperties : ItemProperties
    {
        public override void Throwable(Player player, ref bool throwable)
            => throwable = false;

        public override void ScavCollectScore(Scavenger scavenger, ref int score)
            => score = 3;

        public override void ScavWeaponPickupScore(Scavenger scav, ref int score)
            => score = 3;

        public override void ScavWeaponUseScore(Scavenger scav, ref int score)
            => score = 0;

        public override void Grabability(Player player, ref Player.ObjectGrabability grabability)
        {
            if (player.grasps.Any(g => g?.grabbed is ChargedLantern))
            {
                grabability = Player.ObjectGrabability.CantGrab;
            }
            else
            {
                grabability = Player.ObjectGrabability.OneHand;
            }
        }
    }
}
