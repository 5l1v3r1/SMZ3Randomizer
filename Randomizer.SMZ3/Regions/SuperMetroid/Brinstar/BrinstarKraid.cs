﻿using System.Collections.Generic;

namespace Randomizer.SMZ3.Regions.SuperMetroid {

    class BrinstarKraid : SMRegion, IReward {

        public override string Name => "Brinstar Kraid";
        public override string Area => "Brinstar";

        public RewardType Reward { get; set; } = RewardType.GoldenFourBoss;

        public BrinstarKraid(World world, Config config) : base(world, config) {
            Locations = new List<Location> {
                new Location(this, 43, 0xC7899C, LocationType.Hidden, "Energy Tank, Kraid",
                    items => !Config.Keysanity || items.KraidKey),
                new Location(this, 48, 0xC78ACA, LocationType.Chozo, "Varia Suit",
                    items => !Config.Keysanity || items.KraidKey),
                new Location(this, 44, 0xC789EC, LocationType.Hidden, "Missile (Kraid)", Logic switch {
                    _ => new Requirement(items => items.CanUsePowerBombs())
                }),
            };
        }

        public override bool CanEnter(Progression items) {
            return (items.CanDestroyBombWalls() || items.SpeedBooster || items.CanAccessNorfairUpperPortal()) &&
                items.Super && items.CanPassBombPassages();
        }

        public bool CanComplete(Progression items) {
            return Location("Varia Suit").Available(items);
        }

    }

}
