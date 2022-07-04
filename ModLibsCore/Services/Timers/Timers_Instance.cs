using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
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
	public partial class Timers : ILoadable {
		private static object MyLock = new object();



		////////////////

		private IDictionary<string, TimerEntry> Running = new Dictionary<string, TimerEntry>();
		private IDictionary<string, int> Elapsed = new Dictionary<string, int>();

		private ISet<string> Expired = new HashSet<string>();

		private Func<bool> OnTickGet;



		////////////////

		internal Timers() { }

		void ILoadable.Load(Mod mod) {
			ModContent.GetInstance<ModLibsCoreModSystem>()
				.TickUpdates.Add( Timers._ConditionalLoad );
		}


		private static void _ConditionalLoad() {
			if( ContentInstance<LoadHooks>.Instance == null ) {
				return;
			}

			var modsys = ModContent.GetInstance<ModLibsCoreModSystem>();

			modsys.TickUpdates.Remove( Timers._ConditionalLoad );

			modsys.TickUpdates.Add( Timers._Update );

			//

			var timers = ModContent.GetInstance<Timers>();

			timers.OnTickGet = Timers.MainOnTickGet();

			//

			LoadHooks.AddWorldUnloadEachHook( () => {
				var timers = ModContent.GetInstance<Timers>();

				foreach ( (string timerName, TimerEntry timer) in timers.Running ) {
					LogLibraries.Log( "Aborted timer " + timerName );
				}

				timers.Running.Clear();
				timers.Elapsed.Clear();
				timers.Expired.Clear();
			} );
		}

		void ILoadable.Unload() {
			ModContent.GetInstance<ModLibsCoreModSystem>()
				.TickUpdates.Remove( Timers._Update );
		}


		////////////////

		private static void _Update() {  // <- Just in case references are doing something funky...
			var timers = ModContent.GetInstance<Timers>();
			if( timers?.OnTickGet() ?? false ) {
				timers.Update();
			}
		}

		private void Update() {
			int currElapsed;
			KeyValuePair<string, TimerEntry>[] timers = null;
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
								runsWhilePaused: timer.RunsWhilePaused,
								callback: timer.Callback,
								duration: duration
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
