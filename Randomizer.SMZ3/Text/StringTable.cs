﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer.SMZ3.Text {

    class StringTable {

        internal readonly IList<(string, byte[])> entries;

        public StringTable() {
            entries = CreateEntries();
        }

        public void SetPedestalText(string text) {
            SetText("mastersword_pedestal_translated", text);
        }

        public void SetEtherText(string text) {
            SetText("tablet_ether_book", text);
        }

        public void SetBombosText(string text) {
            SetText("tablet_bombos_book", text);
        }

        void SetText(string name, string text) {
            var index = entries.IndexOf(entries.First(x => x.Item1 == name));
            entries[index] = (name, Dialog.Compiled(text, false));
        }

        public byte[] GetPaddedBytes() {
            return GetBytes(true);
        }

        public byte[] GetBytes(bool pad = false) {
            const int maxBytes = 0x7355;
            var data = entries.SelectMany(x => x.Item2).ToList();

            if (data.Count > maxBytes)
                throw new InvalidOperationException($"String Table exceeds 0x{maxBytes.ToString("X")} bytes");

            if (pad && data.Count < maxBytes)
                return data.Concat(Enumerable.Repeat<byte>(0xFF, maxBytes - data.Count)).ToArray();
            return data.ToArray();
        }

        class NoPause {
            public string Text { get; }
            public NoPause(string text) => Text = text;
        }

        static IList<(string, byte[])> PrepareData(IEnumerable<(string, object)> list) {
            return list.Select(x => (x.Item1, x.Item2 switch {
                byte[] bytes => bytes,
                string text => Dialog.Compiled(text, true),
                NoPause y => Dialog.Compiled(y.Text, false),
                var o => throw new InvalidOperationException($"Did not expect an object of type {o.GetType()}"),
            })).ToList();
        }

        /* The order of the dialog entries is significant */
        static IList<(string, byte[])> CreateEntries() {
            /* Numbers in comments refer to US text numbers. */
            /* Except for the first few entries, JP1.0 text numbers are smaller by 2 */
            return PrepareData(new (string, object)[] {
                ("set_cursor", new byte[] { 0xFB, 0xFC, 0x00, 0xF9, 0xFF, 0xFF, 0xFF, 0xF8, 0xFF, 0xFF, 0xE4, 0xFE, 0x68 }),
                ("set_cursor2", new byte[] { 0xFB, 0xFC, 0x00, 0xF8, 0xFF, 0xFF, 0xFF, 0xF9, 0xFF, 0xFF, 0xE4, 0xFE, 0x68 }),
                ("game_over_menu", new NoPause("{SPEED0}\nSave-Continue\nSave-Quit\nContinue")),
                ("var_test", new NoPause("0= ᚋ, 1= ᚌ\n2= ᚍ, 3= ᚎ")),
                ("follower_no_enter", "Can't you take me some place nice."),
                ("choice_1_3", new byte[] { 0xFB, 0xFC, 0x00, 0xF7, 0xE4, 0xF8, 0xFF, 0xF9, 0xFF, 0xFE, 0x71 }),
                ("choice_2_3", new byte[] { 0xFB, 0xFC, 0x00, 0xF7, 0xFF, 0xF8, 0xE4, 0xF9, 0xFF, 0xFE, 0x71 }),
                ("choice_3_3", new byte[] { 0xFB, 0xFC, 0x00, 0xF7, 0xFF, 0xF8, 0xFF, 0xF9, 0xE4, 0xFE, 0x71 }),
                ("choice_1_2", new byte[] { 0xFB, 0xFC, 0x00, 0xF7, 0xE4, 0xF8, 0xFF, 0xFE, 0x72 }),
                ("choice_2_2", new byte[] { 0xFB, 0xFC, 0x00, 0xF7, 0xFF, 0xF8, 0xE4, 0xFE, 0x72 }),
                ("uncle_leaving_text", "I'm just going out for a pack of smokes."),
                ("uncle_dying_sewer", "I've fallen and I can't get up, take this."),
                // 0x10
                ("tutorial_guard_1", "Only adults should travel at night."),
                ("tutorial_guard_2", "You can press X to see the Map."),
                ("tutorial_guard_3", "Press the A button to lift things by you."),
                ("tutorial_guard_4", "When you has a sword, press B to slash it."),
                ("tutorial_guard_5", "このメッセージはニホンゴでそのまま"), // on purpose
                ("tutorial_guard_6", "Are we really still reading these?"),
                ("tutorial_guard_7", "Jeeze! There really are a lot of things."),
                ("priest_sanctuary_before_leave", "Go be a hero!"),
                ("sanctuary_enter", "YAY!\nYou saved Zelda!"),
                ("zelda_sanctuary_story", "Do you want to hear me say this again?\n{HARP}\n  ≥ no\n    yes\n{CHOICE}"),
                ("priest_sanctuary_before_pendants", "Go'on and get them pendants so you can beat up Agahnim."),
                ("priest_sanctuary_after_pendants_before_master_sword", "Kudos! But seriously, you should be getting the master sword, not having a kegger in here."),
                ("priest_sanctuary_dying", "They took her to the castle! Take your sword and save her!"),
                ("zelda_save_sewers", "You saved me!"),
                ("priest_info", "So, I'm the dude that will protect Zelda. Don't worry, I got this covered."),
                ("zelda_sanctuary_before_leave", "Be careful!"),
                ("telepathic_intro", "{NOBORDER}\n{SPEED6}\nHey, come find me and help me!"),
                // 0x20
                ("telepathic_reminder", "{NOBORDER}\n{SPEED6}\nI'm in the castle basement."),
                ("zelda_go_to_throne", "Go north to the throne."),
                ("zelda_push_throne", "Let's push it from the left!"),
                ("zelda_switch_room_pull", "Pull this lever using A."),
                ("zelda_save_lets_go", "Let's get out of here!"),
                ("zelda_save_repeat", "I like talking, do you?\n  ≥ no\n    yes\n{CHOICE}"),
                ("zelda_before_pendants", "You need to find all the pendants…\n\n\nNumpty."),
                ("zelda_after_pendants_before_master_sword", "Very pretty pendants, but really you should be getting that sword in the forest!"),
                ("telepathic_zelda_right_after_master_sword", "{NOBORDER}\n{SPEED6}\nHi @,\nHave you been thinking about me?\narrrrrgghh…\n… … …"),
                ("zelda_sewers", "Just a little further to the Sanctuary."),
                ("zelda_switch_room", "The Sanctuary!\n\nPull my finger"),
                ("kakariko_sahasrahla_wife", "Heya, @!\nLong time no see.\nYou want a master sword?\n\nWell good luck with that."),
                ("kakariko_sahasrahla_wife_sword_story", "It occurs to me that I like toast and jam, but cheese and crackers is better.\nYou like?\n  ≥ cheese\n    jam\n{CHOICE}"),
                ("kakariko_sahasrahla_wife_closing", "Anywho, I have things to do. You see those 2 ovens?\n\nYeah, 2!\nWho has 2 ovens nowadays?!"),
                ("kakariko_sahasrahla_after_master_sword", "Cool sword!\n\n\n…\n\n\n…\n\n\nPlease save us"),
                ("kakariko_alert_guards", "GUARDS! HELP!\nThe creeper\n@ is here!"),
                // 0x30
                ("sahasrahla_quest_have_pendants", "{BOTTOM}\nCool beans, but I think you should mosey on over to the lost woods."),
                ("sahasrahla_quest_have_master_sword", "{BOTTOM}\nThat's a pretty sword, but I'm old, forgetful, and old. Why don't you go do all the hard work while I hang out in this hut."),
                ("sahasrahla_quest_information", "{BOTTOM}\nSahasrahla, I am. You would do well to find the 3 pendants from the 3 dungeons in the Light World.\nUnderstand?\n  ≥ yes\n    no\n{CHOICE}"),
                ("sahasrahla_bring_courage", "{BOTTOM}\nWhile you're here, could you do me a solid and get the green pendant from that dungeon?\n{HARP}\nI'll give you a present if you do."),
                ("sahasrahla_have_ice_rod", "{BOTTOM}\nLike, I sit here, and tell you what to do?\n\n\nAlright, go and find all the maidens, there are, like, maybe 7 of them. I dunno anymore. I'm old."),
                ("telepathic_sahasrahla_beat_agahnim", "{NOBORDER}\n{SPEED6}\nNice, so you beat Agahnim. Now you must beat Ganon. Good Luck!"),
                ("telepathic_sahasrahla_beat_agahnim_no_pearl", "{NOBORDER}\n{SPEED6}\nOh, also you forgot the Moon Pearl, dingus. Go back and find it!"),
                ("sahasrahla_have_boots_no_icerod", "{BOTTOM}\nCave in South East has a cool item."),
                ("sahasrahla_have_courage", "{BOTTOM}\nLook, you have the green pendant! I'll give you something. Go kill the other two bosses for more pendant fun!"),
                ("sahasrahla_found", "{BOTTOM}\nYup!\n\nI'm the old man you are looking for. I'll keep it short and sweet: Go into that dungeon, then bring me the green pendant and talk to me again."),
                ("sign_rain_north_of_links_house", "↑ Dying Uncle\n  This way…"),
                ("sign_north_of_links_house", "> Randomizer Don't read me, go beat Ganon!"),
                ("sign_path_to_death_mountain", "Cave to lost, old man.\nGood luck."),
                ("sign_lost_woods", "\n↑ Lost Woods"),
                ("sign_zoras", "Danger!\nDeep water!\nZoras!"),
                ("sign_outside_magic_shop", "Welcome to the Magic Shoppe"),
                // 0x40
                ("sign_death_mountain_cave_back", "Cave away from sky cabbages"),
                ("sign_east_of_links_house", "↓ Lake Hylia\n\n Also, a shop"),
                ("sign_south_of_lumberjacks", "← Kakariko\n  Village"),
                ("sign_east_of_desert", "← Desert\n\n     It's hot."),
                ("sign_east_of_sanctuary", "↑→ Potions!\n\nWish Waterfall"),
                ("sign_east_of_castle", "→ East Palace\n\n← Castle"),
                ("sign_north_of_lake", "\n Lake  Hiriah"),
                ("sign_desert_thief", "Don't talk to me or touch my sign!"),
                ("sign_lumberjacks_house", "Lumberjacks, Inc.\nYou see 'em, we saw 'em."),
                ("sign_north_kakariko", "↓ Kakariko\n  Village"),
                ("witch_bring_mushroom", "Double, double toil and trouble!\nBring me a mushroom!"),
                ("witch_brewing_the_item", "This mushroom is busy brewing. Come back later."),
                ("witch_assistant_no_bottle", "You got to give me the mushroom, Numpty."),
                ("witch_assistant_no_empty_bottle", "Gotta use your stuff before you can get more."),
                ("witch_assistant_informational", "Red is life\nGreen is magic\nBlue is both\nI'll heal you for free, though."),
                ("witch_assistant_no_bottle_buying", "If only you had something to put that in, like a bottle…"),
                // 0x50
                ("potion_shop_no_empty_bottles", "Whoa, bucko!\nNo empty bottles."),
                ("item_get_lamp", "Lamp! You can see in the dark, and light torches."),
                ("item_get_boomerang", "Boomerang! Press START to select it."),
                ("item_get_bow", "You're in bow mode now!"),
                ("item_get_shovel", "This is my new mop. My friend George, he gave me this mop. It's a pretty good mop. It's not as good as my old mop. I miss my old mop. But it's still a good mop."),
                ("item_get_magic_cape", "Finally! We get to play Invisible Man!"),
                ("item_get_powder", "It's the powder. Let's cause some mischief!"),
                ("item_get_flippers", "Splish! Splash! Let's go take a bath!"),
                ("item_get_power_gloves", "Feel the power! You can now lift light rocks! Rock on!"),
                ("item_get_pendant_courage", "We have the Pendant of Courage! How brave!"),
                ("item_get_pendant_power", "We have the Pendant of Power! How robust!"),
                ("item_get_pendant_wisdom", "We have the Pendant of Wisdom! How astute!"),
                ("item_get_mushroom", "A Mushroom! Don't eat it. Find a witch."),
                ("item_get_book", "It book! U R now litterit!"),
                ("item_get_moonpearl", "I found a shiny marble! No more hops!"),
                ("item_get_compass", "A compass! I can now find the boss."),
                // 0x60
                ("item_get_map", "Yo! You found a MAP! Press X to see it."),
                ("item_get_ice_rod", "It's the Ice Rod! Freeze Ray time."),
                ("item_get_fire_rod", "A Rod that shoots fire? Let's burn all the things!"),
                ("item_get_ether", "We can chill out with this!"),
                ("item_get_bombos", "Let's set everything on fire, and melt things!"),
                ("item_get_quake", "Time to make the earth shake, rattle, and roll!"),
                ("item_get_hammer", "STOP!\n\nHammer Time!"), // 66
                ("item_get_ocarina", "Finally! We can play the Song of Time!"),
                ("item_get_cane_of_somaria", "Make blocks!\nThrow blocks!\nSplode Blocks!"),
                ("item_get_hookshot", "BOING!!!\nBOING!!!\nSay no more…"),
                ("item_get_bombs", "BOMBS! Use A to pick 'em up, throw 'em, get hurt!"),
                ("item_get_bottle", "It's a terrarium. I hope we find a lizard!"),
                ("item_get_big_key", "Yo! You got a Big Key!"),
                ("item_get_titans_mitts", "So, like, you can now lift anything.\nANYTHING!"),
                ("item_get_magic_mirror", "We could stare at this all day or, you know, beat Ganon…"),
                ("item_get_fake_mastersword", "It's the Master Sword! …or not…\n\n         FOOL!"),
                // 0x70
                ("post_item_get_mastersword", "{NOBORDER}\n{SPEED6}\n@, you got the sword!\n{CHANGEMUSIC}\nNow let's go beat up Agahnim!"),
                ("item_get_red_potion", "Red goo to go! Nice!"),
                ("item_get_green_potion", "Green goo to go! Nice!"),
                ("item_get_blue_potion", "Blue goo to go! Nice!"),
                ("item_get_bug_net", "Surprise Net! Let's catch stuff!"),
                ("item_get_blue_mail", "Blue threads? Less damage activated!"),
                ("item_get_red_mail", "You feel the power of the eggplant on your head."),
                ("item_get_temperedsword", "Nice… I now have a craving for Cheetos."),
                ("item_get_mirror_shield", "Pit would be proud!"),
                ("item_get_cane_of_byrna", "It's the Blue Cane. You can now protect yourself with lag!"),
                ("missing_big_key", "Something is missing…\nThe Big Key?"),
                ("missing_magic", "Something is missing…\nMagic meter?"),
                ("item_get_pegasus_boots", "Finally, it's bonking time!\nHold A to dash"),
                ("talking_tree_info_start", "Whoa! I can talk again!"),
                ("talking_tree_info_1", "Yank on the pitchfork in the center of town, ya heard it here."),
                ("talking_tree_info_2", "Ganon is such a dingus, no one likes him, ya heard it here."),
                // 0x80
                ("talking_tree_info_3", "There is a portal near the Lost Woods, ya heard it here."),
                ("talking_tree_info_4", "Use bombs to quickly kill the Hinox, ya heard it here."),
                ("talking_tree_other", "I can breathe!"),
                ("item_get_pendant_power_alt", "We have the Pendant of Power! How robust!"),
                ("item_get_pendant_wisdom_alt", "We have the Pendant of Wisdom! How astute!"),
                ("game_shooting_choice", "20 rupees.\n5 arrows.\nWin rupees!\nWant to play?\n  ≥ yes\n    no\n{CHOICE}"),
                ("game_shooting_yes", "Let's do this!"),
                ("game_shooting_no", "Where are you going? Straight up!"),
                ("game_shooting_continue", "Keep playing?\n  ≥ yes\n    no\n{CHOICE}"),
                ("pond_of_wishing", "-Wishing Pond-\n\n On Vacation"),
                ("pond_item_select", "Pick something\nto throw in.\n{ITEMSELECT}"),
                ("pond_item_test", "You toss this?\n  ≥ yup\n    wrong\n{CHOICE}"),
                ("pond_will_upgrade", "You're honest, so I'll give you a present."),
                ("pond_item_test_no", "You sure?\n  ≥ oh yeah\n    um\n{CHOICE}"),
                ("pond_item_test_no_no", "Well, I don't want it, so take it back."),
                ("pond_item_boomerang", "I don't much like you, so have this worse Boomerang."),
                // 0x90
                ("pond_item_shield", "I grant you the ability to block fireballs. Don't lose this to a Pikit!"),
                ("pond_item_silvers", "So, wouldn't it be nice to kill Ganon? These should help in the final phase."),
                ("pond_item_bottle_filled", "Bottle Filled!\nMoney Saved!"),
                ("pond_item_sword", "Thank you for the sword, here is a stick of butter."),
                ("pond_of_wishing_happiness", "Happiness up!\nYou are now\nᚌᚋ happy!"),
                ("pond_of_wishing_choice", "Your wish?\n  ≥more bombs\n   more arrows\n{CHOICE}"),
                ("pond_of_wishing_bombs", "Woo-hoo!\nYou can now\ncarry ᚌᚋ bombs"),
                ("pond_of_wishing_arrows", "Woo-hoo!\nYou can now\nhold ᚌᚋ arrows"),
                ("pond_of_wishing_full_upgrades", "You have all I can give you, here are your rupees back."),
                ("mountain_old_man_first", "Look out for holes, and monsters."),
                ("mountain_old_man_deadend", "Oh, goody, hearts in jars! This place is creepy."),
                ("mountain_old_man_turn_right", "Turn right. Let's get out of this place."),
                ("mountain_old_man_lost_and_alone", "Hello. I can't see anything. Take me with you."),
                ("mountain_old_man_drop_off", "Here's a thing to help you, good luck!"),
                ("mountain_old_man_in_his_cave_pre_agahnim", "You need to beat the tower at the top of the mountain."),
                ("mountain_old_man_in_his_cave", "You can find stuff in the tower at the top of this mountain.\nCome see me if you'd like to be healed."),
                // 0xA0
                ("mountain_old_man_in_his_cave_post_agahnim", "You should be heading to the castle… you have a portal there now.\nSay hi anytime you like."),
                ("tavern_old_man_awake", "Life? Love? Happiness? The question you should really ask is: Was this generated by Stoops Alu or Stoops Jet?"),
                ("tavern_old_man_unactivated_flute", "You should play that flute for the weathervane, cause reasons."),
                ("tavern_old_man_know_tree_unactivated_flute", "You should play that flute for the weathervane, cause reasons."),
                ("tavern_old_man_have_flute", "Life? Love? Happiness? The question you should really ask is: Was this generated by Stoops Alu or Stoops Jet?"),
                ("chicken_hut_lady", "This is\nChristos' hut.\n\nHe's out, searching for a bow."),
                ("running_man", "Hi, Do you\nknow Veetorp?\n\nYou really\nshould. And\nall the other great guys who made this possible.\nGo thank them.\n\n\nIf you can catch them…"),
                ("game_race_sign", "Why are you reading this sign? Run!!!"),
                ("sign_bumper_cave", "You need Cape, but not Hookshot"),
                ("sign_catfish", "toss rocks\ntoss items\ntoss cookies"),
                ("sign_north_village_of_outcasts", "↑ Skull Woods\n\n↓ Steve's Town"),
                ("sign_south_of_bumper_cave", "\n→ Karkats cave"),
                ("sign_east_of_pyramid", "\n→ Dark Palace"),
                ("sign_east_of_bomb_shop", "\n← Bomb Shoppe"),
                ("sign_east_of_mire", "\n← Misery Mire\n no way in.\n no way out."),
                ("sign_village_of_outcasts", "Have a Trulie Awesome Day!"),
                // 0xB0
                ("sign_before_wishing_pond", "waterfall\nup ahead\nmake wishes"),
                ("sign_before_catfish_area", "→↑ Have you met Woeful Ike?"),
                ("castle_wall_guard", "Looking for a Princess? Look downstairs."),
                ("gate_guard", "No Lonks Allowed!"),
                ("telepathic_tile_eastern_palace", "{NOBORDER}\nYou need a Bow to get past the red Eyegore. derpy"),
                ("telepathic_tile_tower_of_hera_floor_4", "{NOBORDER}\nIf you find a shiny ball, you can be you in the Dark World."),
                ("hylian_text_1", "%== %== %==\n ^ %==% ^\n%== ^%%^ ==^"),
                ("mastersword_pedestal_translated", "A test of strength: If you have 3 pendants, I'm yours."),
                ("telepathic_tile_spectacle_rock", "{NOBORDER}\nUse the Mirror, or the Hookshot and Hammer, to get to Tower of Hera!"),
                ("telepathic_tile_swamp_entrance", "{NOBORDER}\nDrain the floodgate to raise the water here!"),
                ("telepathic_tile_thieves_town_upstairs", "{NOBORDER}\nBlind hates bright light."),
                ("telepathic_tile_misery_mire", "{NOBORDER}\nLighting 4 torches will open your way forward!"),
                ("hylian_text_2", "%%^= %==%\n ^ =%^=\n==%= ^^%^"),
                ("desert_entry_translated", "Kneel before this stone, and magic will move around you."),
                ("telepathic_tile_under_ganon", "{NOBORDER}\nOnly Silver Arrows will finish off a blue Ganon, or really well timed spins in phase 4."),
                ("telepathic_tile_palace_of_darkness", "{NOBORDER}\nThis is a funny looking Enemizer"),
                // 0xC0
                ("telepathic_tile_desert_bonk_torch_room", "{NOBORDER}\nThings can be knocked down, if you fancy yourself a dashing dude."),
                ("telepathic_tile_castle_tower", "{NOBORDER}\nYou can reflect Agahnim's energy with Sword, Bug-net or Hammer."),
                ("telepathic_tile_ice_large_room", "{NOBORDER}\nAll right, stop collaborate and listen\nIce is back with my brand new invention."),
                ("telepathic_tile_turtle_rock", "{NOBORDER}\nYou Shall not pass… without the red cane."),
                ("telepathic_tile_ice_entrace", "{NOBORDER}\nYou can use Fire Rod or Bombos to pass."),
                ("telepathic_tile_ice_stalfos_knights_room", "{NOBORDER}\nKnock 'em down and then bomb them dead."),
                ("telepathic_tile_tower_of_hera_entrance", "{NOBORDER}\nThis is a bad place, with a guy who will make you fall…\n\n\na lot."),
                ("houlihan_room", "My name is Chris Houlihan, but I'm sure you only care about the money. Take it, it's not like I can stop you."),
                ("caught_a_bee", "Caught a Bee\n  ≥ keep\n    release\n{CHOICE}"),
                ("caught_a_fairy", "Caught Fairy!\n  ≥ keep\n    release\n{CHOICE}"),
                ("no_empty_bottles", "Whoa, bucko!\nNo empty bottles."),
                ("game_race_boy_time", "Your time was\nᚎᚍ min ᚌᚋ sec."),
                ("game_race_girl", "You have 15 seconds,\nGo… Go… Go…"),
                ("game_race_boy_success", "Nice!\nYou can have this trash!"),
                ("game_race_boy_failure", "Too slow!\nI keep my\nprecious!"),
                ("game_race_boy_already_won", "You already have your prize, dingus!"),
                // 0xD0
                ("game_race_boy_sneaky", "Thought you could sneak in, eh?"),
                ("bottle_vendor_choice", "I gots bottles.\nYous gots 100 rupees?\n  ≥ I want\n    no way!"),
                ("bottle_vendor_get", "Nice! Hold it up son! Show the world what you got!"),
                ("bottle_vendor_no", "Fine! I didn't want your money anyway."),
                ("bottle_vendor_already_collected", "Dude! You already have it."),
                ("bottle_vendor_bee", "Cool! A bee! Here's 100 rupees."),
                ("bottle_vendor_fish", "Whoa! A fish! You walked this all the way here?"),
                ("hobo_item_get_bottle", "You think life is rough? I guess you can take my last item. Except this tent. That's MY tent!"),
                ("blacksmiths_what_you_want", "Nice of you to come back!\nWould you like us mess with your sword?\n  ≥ Temper\n    It's fine\n{CHOICE}"),
                ("blacksmiths_paywall", "It's 10 rupees\n  ≥ Easy\n    Hang on…\n{CHOICE}"),
                ("blacksmiths_extra_okay", "Are you sure you're sure?\n  ≥ Ah, yup\n    Hang on…\n{CHOICE}"),
                ("blacksmiths_tempered_already", "Whelp… We can't make this any better."),
                ("blacksmiths_temper_no", "Oh, come by any time!"),
                ("blacksmiths_bogart_sword", "We're going to have to take it to work on it."),
                ("blacksmiths_get_sword", "Sword is done. Now, back to our bread!"),
                ("blacksmiths_shop_before_saving", "I lost my friend. Help me find him!"),
                // 0xE0
                ("blacksmiths_shop_saving", "You found him! Colour me happy! Come back right away and we will bang on your sword."),
                ("blacksmiths_collect_frog", "Ribbit! Ribbit! Let's find my partner. To the shop!"),
                ("blacksmiths_still_working", "Something this precious takes time… Come back later."),
                ("blacksmiths_saving_bows", "Thanks!\n\nThanks!"),
                ("blacksmiths_hammer_anvil", "Dernt Take Er Jerbs!"),
                ("dark_flute_boy_storytime", "Hi!\nI'm Stumpy!\nI've been chillin' in this world for a while now, but I miss my flute. If I gave you a shovel, would you go digging for it?\n  ≥ sure\n    nahh\n{CHOICE}"),
                ("dark_flute_boy_get_shovel", "Schaweet! Here you go. Happy digging!"),
                ("dark_flute_boy_no_get_shovel", "Oh I see, not good enough for you… FINE!"),
                ("dark_flute_boy_flute_not_found", "Still haven't found the item? Dig in the Light World around here, dingus!"),
                ("dark_flute_boy_after_shovel_get", "So I gave you an item, and you're still here.\n\n\n\n\n\nI mean, we can sit here and stare at each other, if you like…\n\n\n\n\n\n\n\nFine, I guess you should just go."),
                ("shop_fortune_teller_lw_hint_0", "{BOTTOM}\nBy the black cats, the book opens the desert"),
                ("shop_fortune_teller_lw_hint_1", "{BOTTOM}\nBy the black cats, nothing doing"),
                ("shop_fortune_teller_lw_hint_2", "{BOTTOM}\nBy the black cats, I'm cheap"),
                ("shop_fortune_teller_lw_hint_3", "{BOTTOM}\nBy the black cats, am I cheap?"),
                ("shop_fortune_teller_lw_hint_4", "{BOTTOM}\nBy the black cats, Zora lives at the end of the river"),
                ("shop_fortune_teller_lw_hint_5", "{BOTTOM}\nBy the black cats, The Cape can pass through the barrier"),
                // 0xF0
                ("shop_fortune_teller_lw_hint_6", "{BOTTOM}\nBy the black cats, Spin, Hammer, or Net to hurt Agahnim"),
                ("shop_fortune_teller_lw_hint_7", "{BOTTOM}\nBy the black cats, You can jump in the well by the blacksmiths"),
                ("shop_fortune_teller_lw_no_rupees", "{BOTTOM}\nThe black cats are hungry, come back with rupees"),
                ("shop_fortune_teller_lw", "{BOTTOM}\nWelcome to the Fortune Shoppe!\nFancy a read?\n  ≥I must know\n   negative\n{CHOICE}"),
                ("shop_fortune_teller_lw_post_hint", "{BOTTOM}\nFor ᚋᚌ rupees\nIt is done.\nBe gone!"),
                ("shop_fortune_teller_lw_no", "{BOTTOM}\nWell then, why did you even come in here?"),
                ("shop_fortune_teller_lw_hint_8", "{BOTTOM}\nBy the black cats, why you do?"),
                ("shop_fortune_teller_lw_hint_9", "{BOTTOM}\nBy the black cats, panda crackers"),
                ("shop_fortune_teller_lw_hint_10", "{BOTTOM}\nBy the black cats, the missing blacksmith is south of the Village of Outcasts"),
                ("shop_fortune_teller_lw_hint_11", "{BOTTOM}\nBy the black cats, open chests to get stuff"),
                ("shop_fortune_teller_lw_hint_12", "{BOTTOM}\nBy the black cats, you can buy a new bomb at the Bomb Shoppe"),
                ("shop_fortune_teller_lw_hint_13", "{BOTTOM}\nBy the black cats, big bombs blow up cracked walls in pyramids"),
                ("shop_fortune_teller_lw_hint_14", "{BOTTOM}\nBy the black cats, you need all the crystals to open Ganon's Tower"),
                ("shop_fortune_teller_lw_hint_15", "{BOTTOM}\nBy the black cats, Silver Arrows will defeat Ganon in his final phase"),
                ("dark_sanctuary", "For 20 rupees I'll tell you something?\nHow about it?\n  ≥ yes\n    no\n{CHOICE}"),
                ("dark_sanctuary_hint_0", "I once was a tea kettle, but then I moved up in the world, and now you can see me as this. Makes you wonder. What I could be next time."),
                // 0x100
                ("dark_sanctuary_no", "Then go away!"),
                ("dark_sanctuary_hint_1", "There is a thief in the desert, he can open creepy chests that follow you. But now that we have that out of the way, do you like my hair? I've spent eons getting it this way."),
                ("dark_sanctuary_yes", "With Crystals 5&6, you can find a great fairy in the pyramid.\n\nFlomp Flomp, Whizzle Whomp"),
                ("dark_sanctuary_hint_2", "All I can say is that my life is pretty plain,\nI like watchin' the puddles gather rain,\nAnd all I can do is just pour some tea for two,\nAnd speak my point of view but it's not sane,\nIt's not sane"),
                ("sick_kid_no_bottle", "{BOTTOM}\nI'm sick! Show me a bottle, get something!"),
                ("sick_kid_trade", "{BOTTOM}\nCool Bottle! Here's something for you."),
                ("sick_kid_post_trade", "{BOTTOM}\nLeave me alone\nI'm sick. You have my item."),
                ("desert_thief_sitting", "………………………"),
                ("desert_thief_following", "why……………"),
                ("desert_thief_question", "I was a thief. I open purple chests!\nKeep secret?\n  ≥ sure thing\n    never!\n{CHOICE}"),
                ("desert_thief_question_yes", "Cool, bring me any purple chests you find."),
                ("desert_thief_after_item_get", "You tell anyone and I will give you such a pinch!"),
                ("desert_thief_reassure", "Bring chests. It's a secret to everyone."),
                ("hylian_text_3", "^^ ^%=^= =%=\n=%% =%%=^\n==%^= %=^^%"),
                ("tablet_ether_book", "Can you make things fall out of the sky? With the Master Sword, you can!"),
                ("tablet_bombos_book", "Can you make things fall out of the sky? With the Master Sword, you can!"),
                // 0x110
                ("magic_bat_wake", "You bum! I was sleeping! Where's my magic bolts?"),
                ("magic_bat_give_half_magic", "How you like me now?"),
                ("intro_main", new NoPause("{INTRO}\n Episode  III\n{PAUSE3}\n A Link to\n   the Past\n{PAUSE3}\n  Randomizer\n{PAUSE3}\nAfter mostly disregarding what happened in the first two games.\n{PAUSE3}\nLink awakens to his uncle leaving the house.\n{PAUSE3}\nHe just runs out the door,\n{PAUSE3}\ninto the rainy night.\n{PAUSE3}\n{CHANGEPIC}\nGanon has moved around all the items in Hyrule.\n{PAUSE7}\nYou will have to find all the items necessary to beat Ganon.\n{PAUSE7}\nThis is your chance to be a hero.\n{PAUSE3}\n{CHANGEPIC}\nYou must get the 7 crystals to beat Ganon.\n{PAUSE9}\n{CHANGEPIC}")),
                ("intro_throne_room", new NoPause("{IBOX}\nLook at this Stalfos on the throne.")),
                ("intro_zelda_cell", new NoPause("{IBOX}\nIt is your time to shine!")),
                ("intro_agahnim", new NoPause("{IBOX}\nAlso, you need to defeat this guy!")),
                ("pickup_purple_chest", "A curious box. Let's take it with us!"),
                ("bomb_shop", "30 bombs for 100 rupees. Good deals all day!"),
                ("bomb_shop_big_bomb", "30 bombs for 100 rupees, 100 rupees 1 BIG bomb. Good deals all day!"),
                ("bomb_shop_big_bomb_buy", "Thanks!\nBoom goes the dynamite!"),
                ("item_get_big_bomb", "YAY! press A to splode it!"),
                ("kiki_second_extortion", "For 100 more, I'll open this place.\nHow about it?\n  ≥ open\n    nah\n{CHOICE}"),
                ("kiki_second_extortion_no", "Heh, good luck getting in."),
                ("kiki_second_extortion_yes", "Yay! Rupees!\nOkay, let's do this!"),
                ("kiki_first_extortion", "I'm Kiki. I like rupees, may I have 10?\nHow about it?\n  ≥ yes\n    no\n{CHOICE}"),
                ("kiki_first_extortion_yes", "Nice. I'll tag along with you for a bit."),
                // 0x120
                ("kiki_first_extortion_no", "Pfft. I have no reason to hang. See ya!"),
                ("kiki_leaving_screen", "No no no no no! We should play by my rules! Goodbye…"),
                ("blind_in_the_cell", "You saved me!\nPlease get me out of here!"),
                ("blind_by_the_light", "Aaaahhhh~!\nS-so bright~!"),
                ("blind_not_that_way", "No! Don't go that way!"),
                ("aginah_l1sword_no_book", "I once had a fish dinner. I still remember it to this day."),
                ("aginah_l1sword_with_pendants", "Do you remember when I was young?\n\nI sure don't."),
                ("aginah", "So, I've been living in this cave for years, and you think you can just come along and bomb open walls?"),
                ("aginah_need_better_sword", "Once, I farted in this cave so bad all the jazz hands guys ran away and hid in the sand."),
                ("aginah_have_better_sword", "Pandas are very vicious animals. Never forget…\n\n\n\n\nI never will…"),
                ("catfish", "You woke me from my nap! Take this, and get out!"),
                ("catfish_after_item", "I don't have anything else for you!\nTake this!"),
                // 12C
                ("lumberjack_right", "One of us always lies."),
                ("lumberjack_left", "One of us always tells the truth."),
                ("lumberjack_left_post_agahnim", "One of us likes peanut butter."),
                ("fighting_brothers_right", "I walled off my brother Leo\n\nWhat a dingus."),
                // 0x130
                ("fighting_brothers_right_opened", "Now I should probably talk to him…"),
                ("fighting_brothers_left", "Did you come from my brothers room?\n\nAre we cool?"),
                ("maiden_crystal_1", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nI have a pretty red dress.\n{SPEED2}\nJust thought I would tell you."),
                ("maiden_crystal_2", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nI have a pretty blue dress.\n{SPEED2}\nJust thought I would tell you."),
                ("maiden_crystal_3", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nI have a pretty gold dress.\n{SPEED2}\nJust thought I would tell you."),
                ("maiden_crystal_4", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nI have a pretty redder dress.\n{SPEED2}\nJust thought I would tell you."),
                ("maiden_crystal_5", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nI have a pretty green dress.\n{SPEED2}\nJust thought I would tell you."),
                ("maiden_crystal_6", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nI have a pretty green dress.\n{SPEED2}\nJust thought I would tell you."),
                ("maiden_crystal_7", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nIt's about friggin time.\n{SPEED2}\nDo you know how long I've been waiting?!"),
                ("maiden_ending", "May the way of the hero lead to the Triforce"),
                ("maiden_confirm_undersood", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nCapisce?\n  ≥ Yes\n    No\n{CHOICE}"),
                ("barrier_breaking", "What did the seven crystals say to Ganon's Tower?"),
                ("maiden_crystal_7_again", "{SPEED2}\n{BOTTOM}\n{NOBORDER}\nIt's about friggin time.\n{SPEED2}\nDo you know how long I've been waiting?!"),
                ("agahnim_zelda_teleport", "I am a magician, and this is my act. Watch as I make this girl disappear"),
                ("agahnim_magic_running_away", "And now, the end is near\nAnd so I face the final curtain\nMy friend, I'll say it clear\nI'll state my case, of which I'm certain\nI've lived a life that's full\nI've traveled each and every highway\nBut more, much more than this\nI did it my way"),
                ("agahnim_hide_and_seek_found", "Peek-a-boo!"),
                // 0x140
                ("agahnim_defeated", "Arrrgggghhh. Well you're coming with me!"),
                ("agahnim_final_meeting", "You have done well to come this far. Now, die!"),
                // 0x142
                ("zora_meeting", "What do you want?\n  ≥ Flippers\n    Nothin'\n{CHOICE}"),
                ("zora_tells_cost", "Fine! But they aren't cheap. You got 500 rupees?\n  ≥ Duh\n    Oh carp\n{CHOICE}"),
                ("zora_get_flippers", "Here's some Flippers for you! Swim little fish, swim."),
                ("zora_no_cash", "Fine!\nGo get some more money first."),
                ("zora_no_buy_item", "Wah hoo! Well, whenever you want to see these gills, stop on by."),
                ("kakariko_sahasrahla_grandson", "My grandpa is over in the East. I'm bad with directions. I'll mark your map. Best of luck!\n{HARP}"),
                ("kakariko_sahasrahla_grandson_next", "Someday I'll be in a high school band!"),
                ("dark_palace_tree_dude", "Did you know…\n\n\nA tree typically has many secondary branches supported clear of the ground by the trunk. This trunk typically contains woody tissue for strength, and vascular tissue to carry materials from one part of the tree to another."),
                ("fairy_wishing_ponds", "\n-wishing pond-\n\nThrow item in?\n  ≥ Yesh\n    No\n{CHOICE}"),
                ("fairy_wishing_ponds_no", "\n   stop it!"),
                ("pond_of_wishing_no", "\n  fine then!"),
                ("pond_of_wishing_return_item", "Okay. Here's your item back, cause I can't use it. I'm stuck in this fountain."),
                ("pond_of_wishing_throw", "How many?\n  ≥ᚌᚋ rupees\n   ᚎᚍ rupees\n{CHOICE}"),
                ("pond_pre_item_silvers", "I like you, so here's a thing you can use to beat up Ganon."),
                // 0x150
                ("pond_of_wishing_great_luck", "\nis great luck"),
                ("pond_of_wishing_good_luck", "\n is good luck"),
                ("pond_of_wishing_meh_luck", "\n is meh luck"),
                // Repurposed to no items in Randomizer
                ("pond_of_wishing_bad_luck", "Why you come in here and pretend like you have something this fountain wants? Come back with bottles!"),
                ("pond_of_wishing_fortune", "by the way, your fortune,"),
                ("item_get_14_heart", "3 more to go\n      ¼\nYay!"),
                ("item_get_24_heart", "2 more to go\n      ½\nWhee!"),
                ("item_get_34_heart", "1 more to go\n      ¾\nGood job!"),
                ("item_get_whole_heart", "You got a whole ♥!!\nGo you!"),
                ("item_get_sanc_heart", "You got a whole ♥!\nGo you!"),
                ("fairy_fountain_refill", "Well done, lettuce have a cup of tea…"),
                ("death_mountain_bullied_no_pearl", "I wrote a word. Just one. On a stone and threw it into the ocean. It was my word. It was what would save me. I hope someday someone finds that word and brings it to me. The word is the beginning of my song."),
                ("death_mountain_bullied_with_pearl", "I wrote a song. Just one. On a guitar and threw it into the sky. It was my song. It could tame beasts and free minds. It flitters on the wind and lurks in our minds. It is the song of nature, of humanity, of dreams and dreamers."),
                ("death_mountain_bully_no_pearl", "Add garlic, ginger and apple and cook for 2 minutes. Add carrots, potatoes, garam masala and curry powder and stir well. Add tomato paste, stir well and slowly add red wine and bring to a boil. Add sugar, soy sauce and water, stir and bring to a boil again."),
                ("death_mountain_bully_with_pearl", "I think I forgot how to smile…"),
                ("shop_darkworld_enter", "It's dangerous outside, buy my crap for safety."),
                // 0x160
                ("game_chest_village_of_outcasts", "Pay 30 rupees, open 2 chests. Are you lucky?\nSo, Play game?\n  ≥ play\n    never!\n{CHOICE}"),
                ("game_chest_no_cash", "So, like, you need 30 rupees.\nSilly!"),
                ("game_chest_not_played", "You want to play a game?\nTalk to me."),
                ("game_chest_played", "You've opened the chests!\nTime to go."),
                ("game_chest_village_of_outcasts_play", "Alright, brother!\nGo play!"),
                ("shop_first_time", "Welcome to my shop! Select stuff with A.\nDO IT NOW!"),
                ("shop_already_have", "So, like, you already have one of those."),
                ("shop_buy_shield", "Thanks! Now you can block fire balls."),
                ("shop_buy_red_potion", "Red goo, so good! It's like a fairy in a bottle, except you have to activate it yourself."),
                ("shop_buy_arrows", "Arrows! Cause you were too lazy to look under some pots!"),
                ("shop_buy_bombs", "You bought bombs. What, couldn't find any under bushes?"),
                ("shop_buy_bee", "He's my best friend. Please take care of him, and never lose him."),
                ("shop_buy_heart", "You really just bought this?"),
                ("shop_first_no_bottle_buy", "Why does no one own bottles? Go find one first!"),
                ("shop_buy_no_space", "You are carrying to much crap, go use some of it first!"),
                ("ganon_fall_in", "You drove\naway my other\nself, Agahnim,\ntwo times…\nBut, I won't\ngive you the\nTriforce.\nI'll defeat\nyou!"),
                // 0x170
                ("ganon_phase_3", "Can you beat\nmy darkness\ntechnique?"),
                ("lost_woods_thief", "Have you seen Andy?\n\nHe was out looking for our prized Ether medallion.\nI wonder when he will be back?"),
                ("blinds_hut_dude", "I'm just some dude. This is Blind's hut."),
                ("end_triforce", "{SPEED2}\n{MENU}\n{NOBORDER}\n     G G"),
                // 0x174
                ("toppi_fallen", "Ouch!\n\nYou Jerk!"),
                ("kakariko_tavern_fisherman", "Don't argue\nwith a frozen\nDeadrock.\nHe'll never\nchange his\nposition!"),
                ("thief_money", "It's a secret to everyone."),
                ("thief_desert_rupee_cave", "So you, like, busted down my door, and are being a jerk by talking to me? Normally I would be angry and make you pay for it, but I bet you're just going to break all my pots and steal my 50 rupees."),
                ("thief_ice_rupee_cave", "I'm a rupee pot farmer. One day I will take over the world with my skillz. Have you met my brother in the desert? He's way richer than I am."),
                ("telepathic_tile_south_east_darkworld_cave", "~~ dev cave ~~\n  no farming\n   required"),
                // 0x17A
                ("cukeman", "Did you hear that Veetorp beat ajneb174 in a 1 on 1 race at AGDQ?"),
                ("cukeman_2", "You found Shabadoo, huh?\nNiiiiice."),
                ("potion_shop_no_cash", "Yo! I'm not running a charity here."),
                ("kakariko_powdered_chicken", "Smallhacker…\n\n\nWas hiding, you found me!\n\n\nOkay, you can leave now."),
                ("game_chest_south_of_kakariko", "Pay 20 rupees, open 1 chest. Are you lucky?\nSo, Play game?\n  ≥ play\n    never!\n{CHOICE}"),
                ("game_chest_play_yes", "Good luck then"),
                // 0x180
                ("game_chest_play_no", "Well fine, I didn't want your rupees."),
                ("game_chest_lost_woods", "Pay 100 rupees, open 1 chest. Are you lucky?\nSo, Play game?\n  ≥ play\n    never!\n{CHOICE}"),
                ("kakariko_flophouse_man_no_flippers", "I sure do have a lot of beds.\n\nZora is a cheapskate and will try to sell you his trash for 500 rupees…"),
                ("kakariko_flophouse_man", "I sure do have a lot of beds.\n\nDid you know if you played the flute in the center of town things could happen?"),
                ("menu_start_2", new NoPause("{MENU}\n{SPEED0}\n≥@'s house\n Sanctuary\n{CHOICE3}")),
                ("menu_start_3", new NoPause("{MENU}\n{SPEED0}\n≥@'s house\n Sanctuary\n Mountain Cave\n{CHOICE2}")),
                ("menu_pause", new NoPause("{SPEED0}\n≥continue\n save and quit\n{CHOICE3}")),
                ("game_digging_choice", "Have 80 Rupees? Want to play digging game?\n  ≥yes\n   no\n{CHOICE}"),
                ("game_digging_start", "Okay, use the shovel with Y!"),
                ("game_digging_no_cash", "Shovel rental is 80 rupees.\nI have all day"),
                ("game_digging_end_time", "Time's up!\nTime for you to go."),
                ("game_digging_come_back_later", "Come back later, I have to bury things."),
                ("game_digging_no_follower", "Something is following you. I don't like."),
                ("menu_start_4", new NoPause("{MENU}\n{SPEED0}\n≥@'s house\n Mountain Cave\n{CHOICE3}")),
                ("ganon_fall_in_alt", "You think you\nare ready to\nface me?\n\nI will not die\n\nunless you\ncomplete your\ngoals. Dingus!"),
                ("ganon_phase_3_alt", "Got wax in your ears? I cannot die!"),
                // 0x190
                ("sign_east_death_mountain_bridge", "How did you get up here?"),
                ("fish_money", "It's a secret to everyone."),
                ("end_pad_data", ""),
            });
        }

    }

}