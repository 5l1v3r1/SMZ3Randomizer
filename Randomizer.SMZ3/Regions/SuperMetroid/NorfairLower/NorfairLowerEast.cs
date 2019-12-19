﻿using System.Collections.Generic;
using static Randomizer.SMZ3.SMLogic;

namespace Randomizer.SMZ3.Regions.SuperMetroid {

    class NorfairLowerEast : SMRegion, IReward {

        public override string Name => "Norfair Lower East";
        public override string Area => "Norfair Lower";

        public RewardType Reward { get; set; } = RewardType.GoldenFourBoss;

        public NorfairLowerEast(World world, Config config) : base(world, config) {
            Locations = new List<Location> {
                new Location(this, 73, 0xC78F30, LocationType.Visible, "Missile (Mickey Mouse room)", Logic switch {
                    Casual => items => true,
                    _ => items => items.Morph && items.CanDestroyBombWalls()
                }),
                new Location(this, 74, 0xC78FCA, LocationType.Visible, "Missile (lower Norfair above fire flea room)"),
                new Location(this, 75, 0xC78FD2, LocationType.Visible, "Power Bomb (lower Norfair above fire flea room)", Logic switch {
                    Casual => items => true,
                    _ => items => items.CanPassBombPassages()
                }),
                new Location(this, 76, 0xC790C0, LocationType.Visible, "Power Bomb (Power Bombs of shame)", Logic switch {
                    _ => items => items.CanUsePowerBombs()
                }),
                new Location(this, 77, 0xC79100, LocationType.Visible, "Missile (lower Norfair near Wave Beam)", Logic switch {
                    Casual => items => true,
                    _ => items => items.Morph && items.CanDestroyBombWalls()
                }),
                new Location(this, 78, 0xC79108, LocationType.Hidden, "Energy Tank, Ridley", Logic switch {
                    _ => items => (!Config.Keysanity || items.RidleyKey) && items.CanUsePowerBombs() && items.Super
                }),
                new Location(this, 80, 0xC79184, LocationType.Visible, "Energy Tank, Firefleas")
            };
        }

        public override bool CanEnter(Progression items) {
            return Logic switch {
                Casual =>
                    items.Varia && (
                        World.CanEnter<NorfairUpperEast>(items) && items.CanUsePowerBombs() && items.SpaceJump && items.Gravity ||
                        items.CanAccessNorfairLowerPortal() && items.CanDestroyBombWalls() && items.Super && items.CanUsePowerBombs() && items.CanFly()),
                _ =>
                    items.Varia && (
                        World.CanEnter<NorfairUpperEast>(items) && items.CanUsePowerBombs() && (items.HiJump || items.Gravity) ||
                        items.CanAccessNorfairLowerPortal() && items.CanDestroyBombWalls() && items.Super && (items.CanFly() || items.CanSpringBallJump() || items.SpeedBooster)
                    ) &&
                    (items.CanFly() || items.HiJump || items.CanSpringBallJump() || items.Ice && items.Charge) &&
                    (items.CanPassBombPassages() || items.ScrewAttack && items.SpaceJump) &&
                    (items.Morph || items.HasEnergyCapacity(5))
            };
        }

        public bool CanComplete(Progression items) {
            return Locations.Get("Energy Tank, Ridley").Available(items);
        }

    }

}
