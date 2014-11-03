﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using MediaPortal.GUI.Library;
using TraktPlugin.TraktAPI.v1.DataStructures;

namespace TraktPlugin.GUI
{
    public class GUISeasonListItem : GUIListItem
    {
        /// <summary>
        /// The id of the window that contains the gui list items (facade)
        /// </summary>
        private int WindowID { get; set; }

        public GUISeasonListItem(string strLabel, int windowID) : base(strLabel)
        {
            this.WindowID = windowID;
        }

        /// <summary>
        /// Images attached to a gui list item
        /// </summary>
        public TraktImage Images
        {
            get { return _Images; }
            set
            {
                _Images = value;
                var notifier = value as INotifyPropertyChanged;
                if (notifier != null) notifier.PropertyChanged += (s, e) =>
                {
                    if (s is TraktImage && e.PropertyName == "Poster")
                        SetImageToGui((s as TraktImage).SeasonImages.Poster.LocalImageFilename(ArtworkType.SeasonPoster));
                    if (s is TraktImage && e.PropertyName == "Fanart")
                        this.UpdateItemIfSelected(WindowID, ItemId);
                };
            }
        } protected TraktImage _Images;

        /// <summary>
        /// Set this to true to stop downloading any images
        /// e.g. when exiting the window
        /// </summary>
        internal static bool StopDownload { get; set; }

        /// <summary>
        /// Download all images attached to the GUI List Control
        /// TODO: Make part of a GUI Base Window
        /// </summary>
        /// <param name="itemsWithThumbs">List of images to get</param>
        internal static void GetImages(List<TraktImage> itemsWithThumbs)
        {
            StopDownload = false;

            // split the downloads in 5+ groups and do multithreaded downloading
            int groupSize = (int)Math.Max(1, Math.Floor((double)itemsWithThumbs.Count / 5));
            int groups = (int)Math.Ceiling((double)itemsWithThumbs.Count() / groupSize);

            for (int i = 0; i < groups; i++)
            {
                var groupList = new List<TraktImage>();
                for (int j = groupSize * i; j < groupSize * i + (groupSize * (i + 1) > itemsWithThumbs.Count ? itemsWithThumbs.Count - groupSize * i : groupSize); j++)
                {
                    groupList.Add(itemsWithThumbs[j]);
                }

                // sort images so that images that already exist are displayed first
                groupList.Sort((s1, s2) =>
                {
                    int x = Convert.ToInt32(File.Exists(s1.SeasonImages.Poster.LocalImageFilename(ArtworkType.SeasonPoster)));
                    int y = Convert.ToInt32(File.Exists(s2.SeasonImages.Poster.LocalImageFilename(ArtworkType.SeasonPoster)));
                    return y.CompareTo(x);
                });

                new Thread(delegate(object o)
                {
                    var items = (List<TraktImage>)o;
                    foreach (var item in items)
                    {
                        #region Season Poster
                        // stop download if we have exited window
                        if (StopDownload) break;

                        string remoteThumb = item.SeasonImages.Poster;
                        string localThumb = item.SeasonImages.Poster.LocalImageFilename(ArtworkType.SeasonPoster);

                        if (!string.IsNullOrEmpty(remoteThumb) && !string.IsNullOrEmpty(localThumb))
                        {
                            if (GUIImageHandler.DownloadImage(remoteThumb, localThumb))
                            {
                                // notify that image has been downloaded
                                item.NotifyPropertyChanged("Poster");
                            }
                        }
                        #endregion

                        #region Fanart
                        // stop download if we have exited window
                        if (StopDownload) break;
                        if (!TraktSettings.DownloadFanart) continue;

                        string remoteFanart = item.ShowImages.Fanart.ToSmallFanart();
                        string localFanart = item.ShowImages.Fanart.LocalImageFilename(ArtworkType.ShowFanart);

                        if (!string.IsNullOrEmpty(remoteFanart) && !string.IsNullOrEmpty(localFanart))
                        {
                            if (GUIImageHandler.DownloadImage(remoteFanart, localFanart))
                            {
                                // notify that image has been downloaded
                                item.NotifyPropertyChanged("Fanart");
                            }
                        }
                        #endregion
                    }
                })
                {
                    IsBackground = true,
                    Name = "ImageDownloader" + i.ToString()
                }.Start(groupList);
            }
        }

        /// <summary>
        /// Loads an Image from memory into a facade item
        /// </summary>
        /// <param name="imageFilePath">Filename of image</param>
        protected void SetImageToGui(string imageFilePath)
        {
            if (string.IsNullOrEmpty(imageFilePath)) return;

            ThumbnailImage = imageFilePath;
            IconImage = imageFilePath;
            IconImageBig = imageFilePath;

            // if selected and is current window force an update of thumbnail
            this.UpdateItemIfSelected(WindowID, ItemId);
        }
    }
}
