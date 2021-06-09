using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Extensions;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Services.Timers {
	class TimerEntry {
		public bool RunsWhilePaused;
		public Func<int> Callback;
		public int Duration;



		public TimerEntry( bool runsWhilePaused, Func<int> callback, int duration ) {
			this.RunsWhilePaused = runsWhilePaused;
			this.Callback = callback;
			this.Duration = duration;
		}
	}




	/// <summary>
	/// Provides a way to delay the onset of a given action by a set amount of ticks. As a secondary function,
	/// MainOnTickGet() provides a way to use Main.OnTick for running background tasks at 60FPS.
	/// </summary>
	public partial class Timers : ISequencedLoadable {
		private static object MyLock = new object();



		////////////////

		private IDictionary<string, TimerEntry> Running = new Dictionary<string, TimerEntry>();
		private IDictionary<string, int> Elapsed = new Dictionary<string, int>();

		private ISet<string> Expired = new HashSet<string>();

		private Func<bool> OnTickGet;



		////////////////

		internal Timers() { }

		bool ISequencedLoadable.OnModsLoad( ISet<object> alreadyLoaded ) {
			if( !alreadyLoaded.Any(o=>o.GetType() == typeof(LoadHooks)) ) {
				return false;
			}

			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += Timers._Update;
//TICKSTART = DateTime.Now.Ticks;

			LoadHooks.AddWorldUnloadEachHook( () => {
				foreach( (string timerName, TimerEntry timer) in this.Running ) {
					LogLibraries.Log( "Aborted timer " + timerName );
				}

				this.Running.Clear();
				this.Elapsed.Clear();
				this.Expired.Clear();
			} );

			return true;
		}

		bool ISequencedLoadable.OnPostModsLoad( ISet<object> alreadyPostLoaded ) {
			return true;
		}

		bool ISequencedLoadable.OnModsUnload( ISet<object> alreadyUnloaded ) {
			try {
				Main.OnTick -= Timers._Update;
			} catch { }

			return true;
		}


		////////////////

		//private static long TICKSTART=0;
		//private static int TICKCOUNT=0;
		private static void _Update() {  // <- Just in case references are doing something funky...
			var timers = ModContent.GetInstance<Timers>();
			if( timers?.OnTickGet() ?? false ) {
//long NOW = DateTime.Now.Ticks;
//TICKCOUNT++;
//if( (NOW - TICKSTART) > 10000000 ) { 
//	DebugLibraries.Print("blah", ""+TICKCOUNT,20);
//	TICKSTART = NOW;
//	TICKCOUNT = 0;
//}
				timers.Update();
			}
		}

		private void Update() {
			KeyValuePair<string, TimerEntry>[] timers = null;
			int currElapsed;
			lock( Timers.MyLock ) {
				timers = this.Running.ToArray();
			}

			foreach( (string timerName, TimerEntry timer) in timers ) {
				if( Main.gamePaused && !timer.RunsWhilePaused ) {
					continue;
				}

				lock( Timers.MyLock ) {
					currElapsed = ++this.Elapsed[timerName];
				}

				if( currElapsed >= timer.Duration ) {
					this.Expired.Add( timerName );

					int duration = timer.Callback();

					if( duration > 0 ) {
						lock( Timers.MyLock ) {
							this.Running[timerName] = new TimerEntry(
								timer.RunsWhilePaused,
								timer.Callback,
								duration
							);
							this.Elapsed[timerName] = 0;
						}

						this.Expired.Remove( timerName );
					}
				}
			}

			if( this.Expired.Count > 0 ) {
				lock( Timers.MyLock ) {
					foreach( string name in this.Expired ) {
						this.Running.Remove( name );
						this.Elapsed.Remove( name );
					}
				}

				this.Expired.Clear();
			}
		}
	}
}
