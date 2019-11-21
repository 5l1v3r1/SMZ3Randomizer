﻿using System.Collections.Generic;
using System.Linq;

namespace Randomizer.SMZ3 {

    enum LocationType {
        Regular,
        HeraStandingKey,
        Pedestal,
        Ether,
        Bombos,
        NotInDungeon,

        Visible,
        Chozo,
        Hidden
    }

    delegate bool Requirement(Progression items);
    delegate bool Verification(Item item, Progression items);

    class Location {

        public int Id { get; set; }
        public string Name { get; set; }
        public LocationType Type { get; set; }
        public int Address { get; set; }
        public Item Item { get; set; }
        public Region Region { get; set; }

        public int Weight {
            get { return weight ?? Region.Weight; }
        }

        readonly Requirement canAccess;
        Verification alwaysAllow;
        Verification allow;
        int? weight;

        public ItemType ItemType {
            get { return Item?.Type ?? ItemType.Nothing; }
        }

        public Location(Region region, int id, int address, LocationType type, string name)
            : this(region, id, address, type, name, items => true) {
        }

        public Location(Region region, int id, int address, LocationType type, string name, Requirement access) {
            Region = region;
            Id = id;
            Name = name;
            Type = type;
            Address = address;
            canAccess = access;
            alwaysAllow = (item, items) => false;
            allow = (item, items) => true;
        }

        public Location Weighted(int? weight) {
            this.weight = weight;
            return this;
        }

        public Location AlwaysAllow(Verification allow) {
            alwaysAllow = allow;
            return this;
        }

        public Location Allow(Verification allow) {
            this.allow = allow;
            return this;
        }

        public bool Available(Progression items) {
            return Region.CanEnter(items) && canAccess(items);
        }

        public bool CanFill(Item item, Progression items) {
            var oldItem = Item;
            Item = item;
            bool fillable = alwaysAllow(item, items) || (Region.CanFill(item) && allow(item, items) && Available(items));
            Item = oldItem;
            return fillable;
        }
    }

    static class LocationsExtensions {

        public static IEnumerable<Location> Empty(this IEnumerable<Location> locations) {
            return locations.Where(l => l.Item == null);
        }

        public static IEnumerable<Location> Filled(this IEnumerable<Location> locations) {
            return locations.Where(l => l.Item != null);
        }

        public static IEnumerable<Location> AvailableWithinWorld(this IEnumerable<Location> locations, IEnumerable<Item> items) {
            return locations.Select(x => x.Region.World).Distinct().SelectMany(world =>
                locations.Where(l => l.Region.World == world).Available(items.Where(i => i.World == world)));
        }

        public static IEnumerable<Location> Available(this IEnumerable<Location> locations, IEnumerable<Item> items) {
            var progression = new Progression(items);
            return locations.Where(l => l.Available(progression));
        }

        public static IEnumerable<Location> CanFillWithinWorld(this IEnumerable<Location> locations, Item item, IEnumerable<Item> items) {
            var itemWorldProgression = new Progression(items.Where(i => i.World == item.World).Append(item));
            return locations.Select(x => x.Region.World).Distinct().SelectMany(world => {
                var progression = new Progression(items.Where(i => i.World == world));
                return locations.Where(l =>
                    l.Region.World == world &&
                    l.CanFill(item, progression) &&
                    item.World.Locations.Find(ll => ll.Id == l.Id).Available(itemWorldProgression));
            });
        }

    }

}
