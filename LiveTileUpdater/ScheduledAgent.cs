using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System;
using Microsoft.Phone.Shell;
using System.Linq;
using ResourceLib;

namespace LiveTileUpdater
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            var tile = (from t in ShellTile.ActiveTiles
                        select t).FirstOrDefault();

            if (tile != null)
            {
				var newTile = new IconicTileData
                        {
							Count = Helper.GetWeekNumber(DateTime.Now),
							Title = Resource.ApplicationName,
							SmallIconImage = new Uri("/Assets/IconImageSmall.png", UriKind.Relative),
							IconImage = new Uri("/Assets/IconImage.png", UriKind.Relative)
                        };

                tile.Update(newTile);
            }

            // Call NotifyComplete to let the system know the agent is done working.
            NotifyComplete();
        }
    }
}