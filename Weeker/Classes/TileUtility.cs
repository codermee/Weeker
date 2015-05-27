using System;
using System.Windows;
using Microsoft.Phone.Scheduler;
using ResourceLib;

namespace Weeker.Classes
{
    public class TileUtility
    {

        #region Singleton

        private static TileUtility instance;
        public static TileUtility Instance
        {
            get { return instance ?? (instance = new TileUtility()); }
        }

        #endregion

        public void CreatePeriodicTask(string name)
        {
            var task = new PeriodicTask(name)
                    {
                        Description = "Live tile updater for Weeker",
						ExpirationTime = DateTime.Now.AddDays(14)
                    };

            // If the agent is already registered, remove it and then add it again
            RemovePeriodicTask(task.Name);

            try
            {
                // Can only be called when application is running in foreground
                ScheduledActionService.Add(task);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show(Resource.BackgroundAgentsDisabled);
                }
                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // No user action required.
                    // The system prompts the user when the hard limit of periodic tasks has been reached.
                }
            }
			#if DEBUG_AGENT
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
			#endif
		}

        public void RemovePeriodicTask(string name)
        {
            if (ScheduledActionService.Find(name) != null)
			{
                ScheduledActionService.Remove(name);
            }
        }
    }
}