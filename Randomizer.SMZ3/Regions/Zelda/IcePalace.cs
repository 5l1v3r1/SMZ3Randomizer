﻿using System.Collections.Generic;
using static Randomizer.SMZ3.ItemType;

namespace Randomizer.SMZ3.Regions.Zelda {

    class IcePalace : Z3Region, IReward {

        public override string Name => "Ice Palace";
        public override string Area => "Ice Palace";

        public RewardType Reward { get; set; } = RewardType.None;

        public IcePalace(World world, Config config) : base(world, config) {
            RegionItems = new[] { KeyIP, BigKeyIP, MapIP, CompassIP };

            Locations = new List<Location> {
                new Location(this, 256+161, 0xE9D4, LocationType.Regular, "Ice Palace - Compass Chest"),
                new Location(this, 256+162, 0xE9E0, LocationType.Regular, "Ice Palace - Spike Room",
                    items => items.Hookshot || !items.BigKeyIP && items.KeyIP >= 1),
                new Location(this, 256+163, 0xE9DD, LocationType.Regular, "Ice Palace - Map Chest",
                    items => items.Hammer && items.CanLiftLight() && Location("Ice Palace - Spike Room").Available(items)),
                new Location(this, 256+164, 0xE9A4, LocationType.Regular, "Ice Palace - Big Key Chest",
                    items => items.Hammer && items.CanLiftLight() && Location("Ice Palace - Spike Room").Available(items)),
                new Location(this, 256+165, 0xE9E3, LocationType.Regular, "Ice Palace - Iced T Room"),
                new Location(this, 256+166, 0xE995, LocationType.Regular, "Ice Palace - Freezor Chest",
                    items => items.CanMeltIceEnemies()),
                new Location(this, 256+167, 0xE9AA, LocationType.Regular, "Ice Palace - Big Chest",
                    items => items.BigKeyIP),
                new Location(this, 256+168, 0x180157, LocationType.Regular, "Ice Palace - Kholdstare",
                    items => items.BigKeyIP && items.KeyIP >= (items.Somaria ? 1 : 2) &&
                        items.CanLiftLight() && items.Hammer && items.CanMeltIceEnemies()),
            };
        }

        public override bool CanEnter(Progression items) {
            return (items.CanMeltIceEnemies() || Logic.OneFrameClipUw) && (
                (items.MoonPearl || Logic.DungeonRevive) &&
                    (items.Flippers || Logic.FakeFlipper) &&
                    items.CanLiftHeavy() ||
                World.CanEnter<DarkWorldSouth>(items) && (
                    Logic.OneFrameClipOw && Logic.MirrorWrap && items.Mirror || (
                        items.MoonPearl ||
                        Logic.OwYba && items.Bottle ||
                        Logic.BunnyRevive && items.CanBunnyRevive()
                    ) && (
                        Logic.MirrorWrap && items.Mirror && (
                            Logic.BootsClip && items.Boots ||
                            Logic.SuperSpeed && items.CanSpinSpeed()
                        ) ||
                        items.Flippers && (
                            Logic.OneFrameClipOw ||
                            Logic.BootsClip && items.Boots
                        )
                    )
                )
            );
        }

        public bool CanComplete(Progression items) {
            return Location("Ice Palace - Kholdstare").Available(items);
        }

    }

}
