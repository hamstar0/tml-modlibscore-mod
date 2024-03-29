﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;


namespace ModLibsCore.Services.Timers {
	/// <summary>
	/// Provides a way to delay the onset of a given action by a set amount of ticks. As a secondary function,
	/// MainOnTickGet() provides a way to use Main.OnTick for running background tasks at 60FPS.
	/// </summary>
	public partial class Timers {
		/// <summary>
		/// Returns a delegate that returns true when at least 1/60th second (1 60FPS 'tick') has fully
		/// elapsed since the last time the delegate returned true (seemed necessary?).
		/// </summary>
		/// <returns></returns>
		public static Func<bool> MainOnTickGet() {
			long then = DateTime.Now.Ticks;
			int frames = 0, subFrames;
//int FRAMES=0;

			return () => {
				long now = DateTime.Now.Ticks;
				int span = (int)( now - then );

				frames += span / (10000000 / 90);
				subFrames = span % (10000000 / 90);

				then = now - subFrames;
//DebugLibraries.Print("blahh", "frames: "+FRAMES, 20);
				
				if( frames > 0 ) {
//FRAMES+=frames;
					frames = 0;//frames--;
					return true;
				}
				return false;
			};
		}


		////////////////

		/// <summary>
		/// Convenience method to run a given action 'now' (AKA a 0 delay timer).
		/// </summary>
		/// <param name="action"></param>
		public static void RunNow( Action action ) {
			string ctx = TmlLibraries.SafelyGetRand().NextDouble() + "_" + action.GetHashCode();

			Timers.SetTimer( ctx, 1, true, () => {
				action();
				return false;
			} );
		}

		/// <summary>
		/// Convenience method to repeatedly run a given action 'now' (AKA a 0 delay timer) until indicated otherwise.
		/// </summary>
		/// <param name="func">Return `true` to repeat timer.</param>
		/// <param name="runsWhilePaused"></param>
		public static void RunUntil( Func<bool> func, bool runsWhilePaused ) {
			string ctx = TmlLibraries.SafelyGetRand().NextDouble() + "_" + func.GetHashCode();

			Timers.SetTimer( ctx, 1, runsWhilePaused, () => {
				return func();
			} );
		}

		/// <summary>
		/// Convenience method to repeatedly run a given action 'now' (AKA a 0 delay timer) the indicated number of times, or until indicated otherwise.
		/// </summary>
		/// <param name="func">Return `true` to repeat timer.</param>
		/// <param name="times"></param>
		/// <param name="runsWhilePaused"></param>
		public static void RunUntil( Func<bool> func, int times, bool runsWhilePaused ) {
			string ctx = TmlLibraries.SafelyGetRand().NextDouble() + "_" + func.GetHashCode();

			Timers.SetTimer( ctx, 1, runsWhilePaused, () => {
				return func() && times-- > 0;
			} );
		}


		////////////////

		/// <summary>
		/// Creates a 'timer' that waits the given amount of ticks before running the given action. Not multi-threaded,
		/// but does not obstruct current thread.
		/// </summary>
		/// <param name="tickDuration"></param>
		/// <param name="runsWhilePaused"></param>
		/// <param name="func">Action to run. Returns `true` to make the action repeat after another period of the
		/// given tick duration.</param>
		public static void SetTimer( int tickDuration, bool runsWhilePaused, Func<bool> func ) {
			string ctx = TmlLibraries.SafelyGetRand().NextDouble() + "_" + func.GetHashCode();

			Timers.SetTimer( ctx, tickDuration, runsWhilePaused, func );
		}

		/// <summary>
		/// Creates a 'timer' that waits the given amount of ticks before running the given action. Not multi-threaded,
		/// but does not obstruct current thread.
		/// </summary>
		/// <param name="name">Identifier of timer. Re-assigning with this identifier replaces any existing timer.</param>
		/// <param name="tickDuration"></param>
		/// <param name="runsWhilePaused"></param>
		/// <param name="func">Action to run. Returns `true` to make the action repeat after another period of the
		/// given tick duration.</param>
		public static void SetTimer( string name, int tickDuration, bool runsWhilePaused, Func<bool> func ) {
			Timers.SetTimer( name, tickDuration, runsWhilePaused, () => {
				if( func() ) {
					return tickDuration;
				} else {
					return 0;
				}
			} );
		}

		/// <summary>
		/// Creates a 'timer' that waits the given amount of ticks before running the given action. Not multi-threaded,
		/// but does not obstruct current thread.
		/// </summary>
		/// <param name="name">Identifier of timer. Re-assigning with this identifier replaces any existing timer.</param>
		/// <param name="tickDuration"></param>
		/// <param name="runsWhilePaused"></param>
		/// <param name="func">Action to run. Returns a value of ticks to indicate the next duration of the next timer
		/// loop. Return `0` to terminate loop.</param>
		public static void SetTimer( string name, int tickDuration, bool runsWhilePaused, Func<int> func ) {
			var timers = ModContent.GetInstance<Timers>();
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				timers.Running[name] = new TimerEntry( runsWhilePaused, func, tickDuration );
				timers.Elapsed[name] = 0;
				timers.Expired.Remove( name );
			}
		}


		////

		/// <summary>
		/// Indicates time remaining for the given 'timer'.
		/// </summary>
		/// <param name="name">Identifier of timer.</param>
		/// <returns></returns>
		public static int GetTimerTickDuration( string name ) {
			var timers = ModContent.GetInstance<Timers>();
			if( timers == null ) { return 0; }

			lock( Timers.MyLock ) {
				if( timers.Running.ContainsKey( name ) ) {
					return timers.Running[name].Duration - timers.Elapsed[name];
				}
			}

			return 0;
		}


		/// <summary>
		/// Ends a given 'timer'.
		/// </summary>
		/// <param name="name">Identifier of timer.</param>
		public static void UnsetTimer( string name ) {
			var timers = ModContent.GetInstance<Timers>();
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				if( timers.Running.ContainsKey( name ) ) {
					timers.Running.Remove( name );
					timers.Elapsed.Remove( name );
					timers.Expired.Remove( name );
				}
			}
		}


		/// <summary>
		/// Ends all 'timers'.
		/// </summary>
		public static void ResetAll() {
			var timers = ModContent.GetInstance<Timers>();
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				timers.Running = new Dictionary<string, TimerEntry>();
				timers.Elapsed = new Dictionary<string, int>();
				timers.Expired = new HashSet<string>();
			}
		}
	}
}
