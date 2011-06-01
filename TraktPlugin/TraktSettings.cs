﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MediaPortal.Profile;
using TraktPlugin.TraktAPI;
using TraktPlugin.TraktAPI.DataStructures;

namespace TraktPlugin
{
    public static class TraktSettings
    {
        #region Settings
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static int MovingPictures { get; set; }
        public static int TVSeries { get; set; }
        public static int MyVideos { get; set; }
        public static bool KeepTraktLibraryClean { get; set; }
        public static List<String> BlockedFilenames { get; set; }
        public static List<String> BlockedFolders { get; set; }
        public static int LogLevel { get; set; }
        public static int SyncTimerLength { get; set; }
        public static int TrendingMoviesDefaultLayout { get; set; }
        public static int TrendingShowsDefaultLayout { get; set; }
        public static int RecommendedMoviesDefaultLayout { get; set; }
        public static int RecommendedShowsDefaultLayout { get; set; }
        public static int WatchListMoviesDefaultLayout { get; set; }
        public static int WatchListShowsDefaultLayout { get; set; }
        public static int WatchListEpisodesDefaultLayout { get; set; }
        #endregion

        #region Constants
        private const string cTrakt = "Trakt";
        private const string cUsername = "Username";
        private const string cPassword = "Password";
        private const string cMovingPictures = "MovingPictures";
        private const string cTVSeries = "TVSeries";
        private const string cMyVideos = "MyVideos";
        private const string cKeepTraktLibraryClean = "KeepLibraryClean";
        private const string cBlockedFilenames = "BlockedFilenames";
        private const string cBlockedFolders = "BlockedFolders";
        private const string cSyncTimerLength = "SyncTimerLength";
        private const string cTrendingMoviesDefaultLayout = "TrendingMoviesDefaultLayout";
        private const string cTrendingShowsDefaultLayout = "TrendingShowsDefaultLayout";
        private const string cRecommendedMoviesDefaultLayout = "RecommendedMoviesDefaultLayout";
        private const string cRecommendedShowsDefaultLayout = "RecommendedShowsDefaultLayout";
        private const string cWatchListMoviesDefaultLayout = "WatchListMoviesDefaultLayout";
        private const string cWatchListShowsDefaultLayout = "WatchListShowsDefaultLayout";
        private const string cWatchListEpisodesDefaultLayout = "WatchListEpisodesDefaultLayout";
        #endregion

        /// <summary>
        /// Get Movie Plugin Count
        /// </summary>
        public static int MoviePluginCount
        {
            get
            {
                int count = 0;
                if (MovingPictures >= 0) count++;
                if (MyVideos >= 0) count++;
                return count;
            }
        }

        public static string Version
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.ToString();
            }
        }

        public static string UserAgent
        {
            get
            {
                return string.Format("TraktForMediaPortal/{0}", Version);
            }
        }

        public static ConnectionState AccountStatus
        {
            get 
            {
                if (_AccountStatus == ConnectionState.Pending)
                {
                    if (string.IsNullOrEmpty(TraktSettings.Username) || string.IsNullOrEmpty(TraktSettings.Password))
                        return ConnectionState.Disconnected;

                    // test connection
                    TraktAccount account = new TraktAccount
                    {
                        Username = TraktSettings.Username,
                        Password = TraktSettings.Password
                    };

                    TraktResponse response = TraktAPI.TraktAPI.TestAccount(account);
                    if (response.Status == "success")
                        _AccountStatus = ConnectionState.Connected;
                    else
                        _AccountStatus = ConnectionState.Invalid;
                }
                return _AccountStatus;
            }
            set
            {
                _AccountStatus = value;
            }
        }
        static ConnectionState _AccountStatus = ConnectionState.Pending;

        /// <summary>
        /// Loads the Settings
        /// </summary>
        public static void loadSettings()
        {
            TraktLogger.Info("Loading Settings");
            using (Settings xmlreader = new MPSettings())
            {
                Username = xmlreader.GetValueAsString(cTrakt, cUsername, "");
                Password = xmlreader.GetValueAsString(cTrakt, cPassword, "");
                MovingPictures = xmlreader.GetValueAsInt(cTrakt, cMovingPictures, -1);
                TVSeries = xmlreader.GetValueAsInt(cTrakt, cTVSeries, -1);
                MyVideos = xmlreader.GetValueAsInt(cTrakt, cMyVideos, -1);
                KeepTraktLibraryClean = xmlreader.GetValueAsBool(cTrakt, cKeepTraktLibraryClean, false);
                BlockedFilenames = xmlreader.GetValueAsString(cTrakt, cBlockedFilenames, "").FromJSONArray<string>().ToList();
                BlockedFolders = xmlreader.GetValueAsString(cTrakt, cBlockedFolders, "").FromJSONArray<string>().ToList();
                LogLevel = xmlreader.GetValueAsInt("general", "loglevel", 1);
                SyncTimerLength = xmlreader.GetValueAsInt(cTrakt, cSyncTimerLength, 86400000);
                TrendingMoviesDefaultLayout = xmlreader.GetValueAsInt(cTrakt, cTrendingMoviesDefaultLayout, 0);
                TrendingShowsDefaultLayout = xmlreader.GetValueAsInt(cTrakt, cTrendingShowsDefaultLayout, 0);
                RecommendedMoviesDefaultLayout = xmlreader.GetValueAsInt(cTrakt, cRecommendedMoviesDefaultLayout, 0);
                RecommendedShowsDefaultLayout = xmlreader.GetValueAsInt(cTrakt, cRecommendedShowsDefaultLayout, 0);
                WatchListMoviesDefaultLayout = xmlreader.GetValueAsInt(cTrakt, cWatchListMoviesDefaultLayout, 0);
                WatchListShowsDefaultLayout = xmlreader.GetValueAsInt(cTrakt, cWatchListShowsDefaultLayout, 0);
                WatchListEpisodesDefaultLayout = xmlreader.GetValueAsInt(cTrakt, cWatchListEpisodesDefaultLayout, 0);
            }
        }

        /// <summary>
        /// Saves the Settings
        /// </summary>
        public static void saveSettings()
        {
            TraktLogger.Info("Saving Settings");
            using (Settings xmlwriter = new MPSettings())
            {
                xmlwriter.SetValue(cTrakt, cUsername, Username);
                xmlwriter.SetValue(cTrakt, cPassword, Password);
                xmlwriter.SetValue(cTrakt, cMovingPictures, MovingPictures);
                xmlwriter.SetValue(cTrakt, cTVSeries, TVSeries);
                xmlwriter.SetValue(cTrakt, cMyVideos, MyVideos);
                xmlwriter.SetValueAsBool(cTrakt, cKeepTraktLibraryClean, KeepTraktLibraryClean);
                xmlwriter.SetValue(cTrakt, cBlockedFilenames, BlockedFilenames.ToJSON());
                xmlwriter.SetValue(cTrakt, cBlockedFolders, BlockedFolders.ToJSON());
                xmlwriter.SetValue(cTrakt, cSyncTimerLength, SyncTimerLength);
                xmlwriter.SetValue(cTrakt, cTrendingMoviesDefaultLayout, TrendingMoviesDefaultLayout);
                xmlwriter.SetValue(cTrakt, cTrendingShowsDefaultLayout, TrendingShowsDefaultLayout);
                xmlwriter.SetValue(cTrakt, cRecommendedMoviesDefaultLayout, RecommendedMoviesDefaultLayout);
                xmlwriter.SetValue(cTrakt, cRecommendedShowsDefaultLayout, RecommendedShowsDefaultLayout);
                xmlwriter.SetValue(cTrakt, cWatchListMoviesDefaultLayout, WatchListMoviesDefaultLayout);
                xmlwriter.SetValue(cTrakt, cWatchListShowsDefaultLayout, WatchListShowsDefaultLayout);
                xmlwriter.SetValue(cTrakt, cWatchListEpisodesDefaultLayout, WatchListEpisodesDefaultLayout);
            }

            Settings.SaveCache();
        }
    }
}
