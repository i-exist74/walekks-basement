using System;
using BepInEx;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WalekksBasement
{
    //[BepInDependency("author.some_other_mods_guid", BepInDependency.DependencyFlags.HardDependency)]
    // to anyone looking at this, please fix the dependencies. i forgot how to set them up properly lmao

    [BepInPlugin(modID, pluginName, version)]
    public class PluginMain : BaseUnityPlugin
    {
        public const string modID = "walekks.basement";
        public const string pluginName = "Walekk's Basement";
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
            //obsolete currently
        }

        private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld rainWorld)
        {
            orig(rainWorld);
            options = new RemixOptions(this); //remix options is just placeholder for if we need them
            OptionInterface oi = options;
            try
            {
                MachineConnector.SetRegisteredOI(modID, oi);

                //put hooks or whatever here
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
