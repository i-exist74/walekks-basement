using System;
using BepInEx;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Fisobs.Core;
using Smoke;

namespace WalekksBasement
{
    //[BepInDependency("author.some_other_mods_guid", BepInDependency.DependencyFlags.HardDependency)]
    // to anyone looking at this, please fix the dependencies. i forgot how to set them up properly lmao

    [BepInPlugin(modID, pluginName, version)]
    public class PluginMain : BaseUnityPlugin
    {
        public const string modID = "walekks.basement.code";
        public const string pluginName = "Walekk's Basement Code Stuff";
        public const string version = "1.0.0";

        public static RemixOptions options;

        private void OnEnable()
        {
            try
            {
                Logger.LogInfo("loading plugin " + pluginName);

                On.RainWorld.OnModsInit += Extras.WrapInit(LoadResources);
                On.RainWorld.OnModsInit += RainWorld_OnModsInit;

                Logger.LogInfo("plugin " + pluginName + " is loaded!");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private void LoadResources(RainWorld rainWorld)
        {
            Futile.atlasManager.LoadImage("icon_ChargedLantern");
        }

        private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld rainWorld)
        {
            orig(rainWorld);
            options = new RemixOptions(this); //remix options is just placeholder for if we need them
            OptionInterface oi = options;
            try
            {
                MachineConnector.SetRegisteredOI(modID, oi);
                Content.Register(new ChargedLanternFisob());

                On.Spear.LodgeInCreature += Spear_LodgeInCreature;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void Spear_LodgeInCreature(On.Spear.orig_LodgeInCreature orig, Spear spear, SharedPhysics.CollisionResult result, bool eu)
        {
            orig(spear, result, eu);
            var targetCreature = result.obj as Creature;
            if (spear.thrownBy.Template.type == CreatureTemplate.Type.Slugcat && targetCreature.dead == false)
            {
                if (spear.thrownBy.grasps[0].grabbed is ChargedLantern)
                {
                    spear.thrownBy.room.AddObject(new ShockWave(spear.thrownBy.firstChunk.pos, 100, 5, 10));

                    var chargedLanternInstance = spear.thrownBy.grasps[0].grabbed.abstractPhysicalObject as ChargedLanternAbstract;

                    chargedLanternInstance.fuel = 5f;
                }
                if (spear.thrownBy.grasps[1].grabbed is ChargedLantern)
                {
                    spear.thrownBy.room.AddObject(new ShockWave(spear.thrownBy.firstChunk.pos, 100, 5, 10));

                    var chargedLanternInstance = spear.thrownBy.grasps[1].grabbed.abstractPhysicalObject as ChargedLanternAbstract;

                    chargedLanternInstance.fuel = 5f;
                }
            }
        }
    }
}
