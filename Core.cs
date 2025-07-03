using MelonLoader;
using HarmonyLib;
using Il2CppPipistrello;

[assembly: MelonInfo(typeof(More_Cheats.Core), "More Cheats", "1.0.0", "idea", null)]
[assembly: MelonGame("Pocket Trap", "Pipistrello")]

namespace More_Cheats
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }

    [HarmonyPatch(typeof(Director), "InitDebugMenu")]
    public static class Cheats_Patch
    {
        private static void Postfix(Il2CppPipistrello.Director __instance)
        {
            // Load Save State
            Action loadSaveState = () => {
                Game.Record record = __instance.DebugSaveLoad();
                if (record != null)
                {
                    __instance.InitFromRecord(record);
                }
            };
            __instance.debugMenu.AddCheat("Save State: Load", loadSaveState);

            // Spawn Battery
            Action spawnBattery = () =>
            {
                if (__instance.player == null)
                {
                    return;
                }

                ObjectBattery battery = new ObjectBattery(__instance);
                battery.position = __instance.player.position;
                battery.objectDefName = "battery";
                __instance.CreateObject(battery);
            };
            __instance.debugMenu.AddCheat("Spawn Battery", spawnBattery);

            // Spawn Key
            Action spawnKey = () =>
            {
                if (__instance.player == null)
                {
                    return;
                }

                ObjectKey key = new ObjectKey(__instance);
                key.position = __instance.player.position;
                key.objectDefName = "key";
                __instance.CreateObject(key);
            };
            __instance.debugMenu.AddCheat("Spawn Key", spawnKey);

            // Spawn Laser
            Action spawnLaser = () =>
            {
                if (__instance.player == null)
                {
                    return;
                }

                LaserCannon laser = new LaserCannon(__instance);
                laser.position = __instance.player.position;
                laser.objectDefName = "laserCannon";
                laser.laserWarmedUp = false;
                laser.laserBeam.bombCheckCountdownTimer = -1;
                laser.fallingFromSkyTimer = -1;
                __instance.CreateObject(laser);
            };
            __instance.debugMenu.AddCheat("Spawn Laser", spawnLaser);

            // List Objects
            Action listObjects = () =>
            {
                foreach (var obj in __instance.objects)
                {
                    MelonLogger.Msg(obj.DebugId() + ": " + obj.globalObjectId.objectId);
                }
            };
            __instance.debugMenu.AddCheat("List Objects", listObjects);

            // Change Equips
            Action changeEquips = () =>
            {
                UIDialog equipMenu = Menu.MakeWorkbenchMenu(__instance);
                __instance.ShowDialog(equipMenu);
            };
            __instance.debugMenu.AddCheat("Change Equips", changeEquips);
        }
    }
}