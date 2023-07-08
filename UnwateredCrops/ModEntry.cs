using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace TestMod
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Player.Warped += this.OnPlayerWarped;
        }
        
        /*********
        ** Private methods
        *********/
        private void OnPlayerWarped(object sender, WarpedEventArgs e)
        {
            if (e.OldLocation.Name != "Farm")
                return;

            Farm farm = (Farm) e.OldLocation;
            int unwateredTiles = 0;

            foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in farm.terrainFeatures.Pairs)
            {
                TerrainFeature feature = terrainFeature.Value;
                if (feature is HoeDirt hoeDirt)
                {
                    // Check if the HoeDirt has a crop and is not watered
                    if (hoeDirt.crop != null && hoeDirt.state.Value != HoeDirt.watered)
                        unwateredTiles++;
                }
            }

            if (unwateredTiles == 0)
                return;

            Game1.addHUDMessage(new HUDMessage($"You have {unwateredTiles} unwatered crops!", HUDMessage.newQuest_type));
        }
    }
}