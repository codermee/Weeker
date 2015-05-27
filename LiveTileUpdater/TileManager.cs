using System;
using System.IO.IsolatedStorage;
using System.Linq;
using Microsoft.Phone.Shell;
using ResourceLib;

namespace LiveTileUpdater
{
    public class TileManager
    {

        #region Singleton

        private static TileManager instance;
        public static TileManager Instance
        {
            get { return instance ?? (instance = new TileManager()); }
        }

        #endregion

        public bool GetLiveTileSetting()
        {
            bool isEnabled;
            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains(Globals.LiveTileSettingKey))
            {
                isEnabled = (bool)settings[Globals.LiveTileSettingKey];
            }
            else
            {
                isEnabled = true;
            }

            return isEnabled;
        }

        public void CreateTile(string item)
        {
            if (!String.IsNullOrWhiteSpace(item))
            {
	            var tile = new IconicTileData
		            {
			            Count = Convert.ToInt32(item),
						Title = Resource.ApplicationName,
			            SmallIconImage = new Uri("/Assets/IconImageSmall.png", UriKind.Relative),
			            IconImage = new Uri("/Assets/IconImage.png", UriKind.Relative)
		            };
				ShellTile.ActiveTiles.First().Update(tile);
            }
        }
	}
}