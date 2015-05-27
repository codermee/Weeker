using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;
using LiveTileUpdater;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ResourceLib;
using Weeker.Classes;

namespace Weeker
{
	public partial class MainPage
	{
		private bool IsReloaded { get; set; }

		#region Constructor

		public MainPage()
		{
			InitializeComponent();
			SetupApplicationBar();
		}

		#endregion

		#region Private methods

		private void SetupApplicationBar()
		{
			var settingsMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
			settingsMenuItem.Text = Resource.Settings;
			var aboutMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[1];
			aboutMenuItem.Text = Resource.About;
		}

		private static void NavigateToUrl(string url)
		{
			var uri = new Uri(url, UriKind.Relative);
			((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(uri);
		}

		private static void SetupBackgroundAgent()
		{
			var isLowMemoryDevice = (bool)IsolatedStorageSettings.ApplicationSettings[Globals.IsLowMemoryDevice];
			if (!isLowMemoryDevice)
			{
				TileUtility.Instance.CreatePeriodicTask(Globals.LiveTileUpdaterPeriodicTaskName);
			}
		}

		#endregion

		#region Events

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			SetupBackgroundAgent();
			MainPagePivot.Title = Resource.ApplicationName.ToUpper();

			WeekNumber.Text = Convert.ToString(Helper.GetWeekNumber(DateTime.Now));
			DayOfWeekTextBlock.Text = Helper.GetDayOfWeek(DateTime.Now);
			DateTextBlock.Text = Helper.GetFriendlyDate(DateTime.Now);
			if (!IsReloaded)
			{
				TransformedWeekNumber.Text = Convert.ToString(Helper.GetWeekNumber(DateTime.Now));
				DateRange.Text = Helper.GetFriendlyDateRange(DateTime.Now);
				TileManager.Instance.CreateTile(WeekNumber.Text);
			}
			base.OnNavigatedTo(e);
		}

		private void OnDatePickerValueChanged(object sender, DateTimeValueChangedEventArgs e)
		{
			if (DatePicker.Value.HasValue)
			{
				var givenDate = DatePicker.Value.Value;
				TransformedWeekNumber.Text = Convert.ToString(Helper.GetWeekNumber(givenDate));
				DateRange.Text = Helper.GetFriendlyDateRange(givenDate);
				IsReloaded = true;
			}
		}

		private void OnAppBarSettingsMenuItemClick(object sender, EventArgs e)
		{
			NavigateToUrl(Globals.SettingsPageUri);
		}

		private void OnAppBarAboutMenuItemClick(object sender, EventArgs e)
		{
			NavigateToUrl(Globals.AboutPageUri);
		}

		#endregion

	}
}