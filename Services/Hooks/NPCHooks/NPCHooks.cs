﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace ModLibsCore.Services.Hooks.NPCHooks {
	/// <summary>
	/// Allows defining tModLoader-like, delegate-based hooks for a variety of NPC-related functions.
	/// </summary>
	public class NPCHooks : ModSystem {
		/// <summary>
		/// General-use NPC spawn hooking.
		/// </summary>
		/// <param name="hook"></param>
		public static void AddSpawnNPCHook( OnSpawnNPCHook hook ) {
			ModContent.GetInstance<NPCHooks>()
				.OnSpawnNPCHooks.Add( hook );
		}



		////////////////

		private bool IsSpawningNPC = false;



		////////////////
		
		/// <summary>
		/// Called when an NPC is spawned.
		/// </summary>
		/// <param name="npcWho">`Main.npc` index.</param>
		public delegate void OnSpawnNPCHook( int npcWho );



		////////////////

		private IList<OnSpawnNPCHook> OnSpawnNPCHooks = new List<OnSpawnNPCHook>();



		////////////////
		
		public override void Load() {
			On.Terraria.NPC.SpawnNPC += this.NPC_SpawnNPC;
			On.Terraria.NPC.NewNPC += this.NPC_NewNPC;
		}

		////////////////

		private void NPC_SpawnNPC( On.Terraria.NPC.orig_SpawnNPC orig ) {
			this.IsSpawningNPC = true;

			orig.Invoke();

			this.IsSpawningNPC = false;
		}

		private int NPC_NewNPC(
					On.Terraria.NPC.orig_NewNPC orig,
					IEntitySource src,
					int X,
					int Y,
					int Type,
					int Start,
					float ai0,
					float ai1,
					float ai2,
					float ai3,
					int Target ) {
			int npcWho = orig.Invoke( src, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target );

			if( this.IsSpawningNPC ) {
				foreach( OnSpawnNPCHook hook in this.OnSpawnNPCHooks ) {
					hook.Invoke( npcWho );
				}
			}

			return npcWho;
		}
	}
}
