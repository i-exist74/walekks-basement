using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menu.Remix;
using Menu.Remix.MixedUI;
using RWCustom;
using UnityEngine;

namespace WalekksBasement
{
    public class RemixOptions : OptionInterface
    {
        public RemixOptions(PluginMain pluginMain) 
        {
            //put binds here
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Tabs = new OpTab[]
            {
                new OpTab(this, "optab")
            };
            OpLabel opLabel = new OpLabel(new Vector2(150f, 560f), new Vector2(300f, 30f), "placeholder", FLabelAlignment.Center, true, null);
            this.Tabs[0].AddItems(new UIelement[]
            {
                opLabel
            });
        }
    }
}
