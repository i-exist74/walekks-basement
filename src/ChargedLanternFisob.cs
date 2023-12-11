using Fisobs.Core;
using Fisobs.Items;
using Fisobs.Properties;
using Fisobs.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalekksBasement
{
    internal class ChargedLanternFisob : Fisob
    {
        public static readonly AbstractPhysicalObject.AbstractObjectType ChargedLantern = new AbstractPhysicalObject.AbstractObjectType("ChargedLantern", true);

        public static readonly MultiplayerUnlocks.SandboxUnlockID ChargedLanternSandbox = new MultiplayerUnlocks.SandboxUnlockID("ChargedLanternSandbox", true);

        public ChargedLanternFisob() : base(ChargedLantern)
        {
            // Fisobs auto-loads the `icon_CentiShield` embedded resource as a texture.
            // See `CentiShields.csproj` for how you can add embedded resources to your project.

            // If you want a simple grayscale icon, you can omit the following line.
            Icon = new ChargedLanternIcon();

            SandboxPerformanceCost = new SandboxPerformanceCost(linear: 0.35f, exponential: 0f);

            RegisterUnlock(ChargedLanternSandbox, parent: MultiplayerUnlocks.SandboxUnlockID.BigCentipede, data: 70);
        }

        public override AbstractPhysicalObject Parse(World world, EntitySaveData saveData, SandboxUnlock unlock)
        {
            // Centi shield data is just floats separated by ; characters.
            string[] p = saveData.CustomData.Split(';');

            if (p.Length < 5)
            {
                p = new string[5];
            }

            var result = new ChargedLanternAbstract(world, saveData.Pos, saveData.ID)
            {
                scaleX = float.TryParse(p[2], out var x) ? x : 1,
                scaleY = float.TryParse(p[3], out var y) ? y : 1,
            };

            // If this is coming from a sandbox unlock, the hue and size should depend on the data value (see CentiShieldIcon below).
            if (unlock is SandboxUnlock u)
            {
                if (u.Data == 0)
                {
                    result.scaleX += 0.2f;
                    result.scaleY += 0.2f;
                }
            }

            return result;
        }

        private static readonly ChargedLanternProperties properties = new ChargedLanternProperties();

        public override ItemProperties Properties(PhysicalObject forObject)
        {
            // If you need to use the forObject parameter, pass it to your ItemProperties class's constructor.
            // The Mosquitoes example demonstrates this.
            return properties;
        }
    }
}
