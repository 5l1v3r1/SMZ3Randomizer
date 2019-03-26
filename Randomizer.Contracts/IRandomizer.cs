﻿using System;
using System.Collections.Generic;

namespace Randomizer.Contracts
{
    public interface IPatchData
    {
        IDictionary<int, byte[]> Patches { get; }
        string Seed { get; }
        string Name { get; }
        List<Dictionary<string, string>> Playthrough { get; }
    }

    public interface IRandomizer
    {
        IPatchData GenerateSeed(IDictionary<string, string> options, string seed);
    }
}