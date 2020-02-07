﻿using System.Collections.Generic;
using static Randomizer.SMZ3.ItemType;

namespace Randomizer.SMZ3.Regions.Zelda {

    class PalaceOfDarkness : Z3Region, IReward {

        public override string Name => "Palace of Darkness";
        public override string Area => "Palace of Darkness";

        public RewardType Reward { get; set; } = RewardType.None;

        public PalaceOfDarkness(World world, Config config) : base(world, config) {
            RegionItems = new[] { KeyPD, BigKeyPD, MapPD, CompassPD };

            Locations = new List<Location> {
                new Location(this, 256+121, 0xEA5B, LocationType.Regular, "Palace of Darkness - Shooter Room"),
                new Location(this, 256+122, 0xEA37, LocationType.Regular, "Palace of Darkness - Big Key Chest",
                    items => items.KeyPD >= (Locations.Get("Palace of Darkness - Big Key Chest").ItemType == KeyPD ? 1 :
                        Config.Keysanity || items.Hammer && items.Bow && items.Lamp ? 6 : 5))
                    .AlwaysAllow((item, items) => item.Type == KeyPD && items.KeyPD >= 5),
                new Location(this, 256+123, 0xEA49, LocationType.Regular, "Palace of Darkness - Stalfos Basement",
                    items => items.KeyPD >= 1 || items.Bow && items.Hammer),
                new Location(this, 256+124, 0xEA3D, LocationType.Regular, "Palace of Darkness - The Arena - Bridge",
                    items => items.KeyPD >= 1 || items.Bow && items.Hammer),
                new Location(this, 256+125, 0xEA3A, LocationType.Regular, "Palace of Darkness - The Arena - Ledge",
                    items => items.Bow),
                new Location(this, 256+126, 0xEA52, LocationType.Regular, "Palace of Darkness - Map Chest",
                    items => items.Bow),
                new Location(this, 256+127, 0xEA43, LocationType.Regular, "Palace of Darkness - Compass Chest",
                    items => items.KeyPD >= (Config.Keysanity || items.Hammer && items.Bow && items.Lamp ? 4 : 3)),
                new Location(this, 256+128, 0xEA46, LocationType.Regular, "Palace of Darkness - Harmless Hellway",
                    items => items.KeyPD >= (Locations.Get("Palace of Darkness - Harmless Hellway").ItemType == KeyPD ?
                        Config.Keysanity || items.Hammer && items.Bow && items.Lamp ? 4 : 3 :
                        Config.Keysanity || items.Hammer && items.Bow && items.Lamp ? 6 : 5))
                    .AlwaysAllow((item, items) => item.Type == KeyPD && items.KeyPD >= 5),
                new Location(this, 256+129, 0xEA4C, LocationType.Regular, "Palace of Darkness - Dark Basement - Left",
                    // Todo: firerod, to be or not to be?
                    items => (items.Lamp || items.Firerod) && items.KeyPD >= (Config.Keysanity || items.Hammer && items.Bow && items.Lamp ? 4 : 3)),
                new Location(this, 256+130, 0xEA4F, LocationType.Regular, "Palace of Darkness - Dark Basement - Right",
                    // Todo: firerod, to be or not to be?
                    items => (items.Lamp || items.Firerod) && items.KeyPD >= (Config.Keysanity || items.Hammer && items.Bow && items.Lamp ? 4 : 3)),
                new Location(this, 256+131, 0xEA55, LocationType.Regular, "Palace of Darkness - Dark Maze - Top",
                    items => items.Lamp && items.KeyPD >= (Config.Keysanity || items.Hammer && items.Bow ? 6 : 5))
                    // Todo: overkill?
                    .Allow((item, items) => item.Type != KeyPD),
                new Location(this, 256+132, 0xEA58, LocationType.Regular, "Palace of Darkness - Dark Maze - Bottom",
                    items => items.Lamp && items.KeyPD >= (Config.Keysanity || items.Hammer && items.Bow ? 6 : 5))
                    // Todo: overkill?
                    .Allow((item, items) => item.Type != KeyPD),
                new Location(this, 256+133, 0xEA40, LocationType.Regular, "Palace of Darkness - Big Chest",
                    items => items.BigKeyPD && items.Lamp && items.KeyPD >= (Config.Keysanity || items.Hammer && items.Bow ? 6 : 5))
                    // Todo: overkill?
                    .Allow((item, items) => item.Type != KeyPD),
                new Location(this, 256+134, 0x180153, LocationType.Regular, "Palace of Darkness - Helmasaur King",
                    items => items.Lamp && items.Hammer && items.Bow && items.BigKeyPD && items.KeyPD >= 6),
            };
        }

        public override bool CanEnter(Progression items) {
            return (
                    items.MoonPearl ||
                    Logic.OwYba && items.Bottle ||
                    Logic.BunnyRevive && items.CanBunnyRevive()
                ) && World.CanEnter<DarkWorldNorthEast>(items) ||
                Logic.OneFrameClipUw && Logic.DungeonRevive && World.CanEnter<LightWorldDeathMountainWest>(items);
        }

        public bool CanComplete(Progression items) {
            return Locations.Get("Palace of Darkness - Helmasaur King").Available(items);
        }

    }

}
