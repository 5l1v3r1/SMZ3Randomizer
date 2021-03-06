﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Randomizer.Contracts;
using static Randomizer.Contracts.RandomizerOptionType;

namespace Randomizer.SuperMetroid {

    public class Randomizer : IRandomizer {
        
        public static readonly Version version = new Version(3, 0);

        public string Id => "sm";
        public string Name => "Super Metroid Item Randomizer";

        public string Version => version.ToString();

        static Regex alphaNumeric = new Regex(@"[A-Z\d]", RegexOptions.IgnoreCase);

        public List<IRandomizerOption> Options => new List<IRandomizerOption> {
            Config.GetRandomizerOption<Logic>("Logic"),
            Config.GetRandomizerOption<Goal>("Goal"),
            Config.GetRandomizerOption<Placement>("Item Placement"),
            new RandomizerOption {
                Key = "seed", Description = "Seed", Type = Seed
            },
            Config.GetRandomizerOption("Race", "Race ROM (no spoilers)", false),
            Config.GetRandomizerOption<GameMode>("Game mode"),
            new RandomizerOption {
                Key = "players", Description = "Players", Type = Players, Default = "2"
            },
        };

        public ISeedData GenerateSeed(IDictionary<string, string> options, string seed) {
            int randoSeed;
            if (string.IsNullOrEmpty(seed)) {
                randoSeed = System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, int.MaxValue);
                seed = randoSeed.ToString();
            } else {
                randoSeed = int.Parse(seed);
                /* The Random ctor takes the absolute value of a negative seed.
                 * This is an non-obvious behavior so we treat a negative value
                 * as out of range. */
                if (randoSeed < 0)
                    throw new ArgumentOutOfRangeException("Expected the seed option value to be an integer value in the range [0, 2147483647]");
            }

            var rnd = new Random(randoSeed);
            var config = new Config(options);

            if (config.Race) {
                rnd = new Random(rnd.Next());
            }

            int players = options.ContainsKey("players") ? int.Parse(options["players"]) : 1;
            var worlds = new List<World>();

            if (config.GameMode == GameMode.Normal || players == 1) {
                worlds.Add(new World(config, "Player", 0, new HexGuid()));
            }
            else {
                for (int p = 0; p < players; p++) {
                    var found = options.TryGetValue($"player-{p}", out var player);
                    if (!found || !alphaNumeric.IsMatch(player))
                        throw new ArgumentException($"Name for player {p + 1} not provided, or contains no alphanumeric characters");
                    worlds.Add(new World(config, player, p, new HexGuid()));
                }
            }

            var filler = new Filler(worlds, config, rnd);
            filler.Fill();

            var playthrough = new Playthrough(worlds, config);
            var spheres = playthrough.Generate();

            var seedData = new SeedData {
                Guid = new HexGuid(),
                Seed = seed,
                Game = Name,
                Logic = config.Logic.ToLString(),
                Playthrough = config.Race ? new List<Dictionary<string, string>>() : spheres,
                Mode = config.GameMode.ToLString(),
                Worlds = new List<IWorldData>()
            };

            int patchSeed = rnd.Next();
            foreach(var world in worlds) {
                var patchRnd = new Random(patchSeed);
                var patch = new Patch(world, worlds, seedData.Guid, config.Race ? 0 : randoSeed, patchRnd);
                var worldData = new WorldData {
                    Id = world.Id,
                    Guid = world.Guid,
                    Player = world.Player,
                    Patches = patch.Create(),
                    Locations = world.Locations.Select(l => new LocationData() { LocationId = l.Id, ItemId = (int)l.Item.Type, ItemWorldId = l.Item.World.Id }).ToList<ILocationData>()
                };

                seedData.Worlds.Add(worldData);
            }

            return seedData;
        }
    }

    public class RandomizerOption : IRandomizerOption {
        public string Key { get; set; }
        public string Description { get; set; }
        public RandomizerOptionType Type { get; set; }
        public Dictionary<string, string> Values { get; set; }
        public string Default { get; set; }
    }

    public class SeedData : ISeedData {
        public string Guid { get; set; }
        public string Seed { get; set; }
        public string Game { get; set; }
        public string Logic { get; set; }
        public string Mode { get; set; }
        public List<IWorldData> Worlds { get; set; }
        public List<Dictionary<string, string>> Playthrough { get; set; }
    }

    public class WorldData : IWorldData {

        public int Id { get; set; }
        public string Guid { get; set; }
        public string Player { get; set; }
        public Dictionary<int, byte[]> Patches { get; set; }
        public List<ILocationData> Locations { get; set; }
    }

    public class LocationData : ILocationData {
        public int LocationId { get; set; }
        public int ItemId { get; set; }
        public int ItemWorldId { get; set; }
    }
}
