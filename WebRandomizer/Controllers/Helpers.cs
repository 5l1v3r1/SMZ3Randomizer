﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebRandomizer.Controllers {
    public class Helpers {
        public static string SerializeEnumAsString(object ob) {
            return JsonConvert.SerializeObject(
                ob,
                new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented,
                    Converters = new List<JsonConverter> { new StringEnumConverter() }
                }
            );
        }

        public static string GetSeedHashNames(Dictionary<int, byte[]> patch, string gameId) {
            string[] names = {
                "GEEMER", "RIPPER", "ATOMIC", "POWAMP", "SCISER", "NAMIHE", "PUROMI", "ALCOON",
                "BEETOM","OWTCH","ZEBBO","ZEELA","HOLTZ","VIOLA","WAVER","RINKA",
                "BOYON","CHOOT","KAGO","SKREE","COVERN","EVIR","TATORI","OUM",
                "PUYO","YARD","ZOA","FUNE","GAMET","GERUTA","SOVA","BULL",
                "ARRGI","BABUSU","BORU","HACHI","BABURU","TAINON","GERUDO","GIBO",
                "KOPPI","PON","HOBA","HYU","KISU","KYUNE","RIBA","MEDUSA",
                "TERU","FANGIN","PIKKU","POPO","NOMOSU","GUZU","AIGORU","ROPA",
                "GAPURA","HEISHI","SUTARU","TOZOKU","TOPPO","WAINDA","KURIPI","ZORA"};

            if (gameId == "smz3") {
                var hashData = patch[0x420000];
                return $"{names[hashData[0] & 0x3F]} {names[hashData[1] & 0x3F]} {names[hashData[2] & 0x3F]} {names[hashData[3] & 0x3F]}";
            } else {
                var hashData = patch[0x2FFF00];
                return $"{names[hashData[0] & 0x1F]} {names[hashData[1] & 0x1F]} {names[hashData[2] & 0x1F]} {names[hashData[3] & 0x1F]}";
            }
        }
    }
}
