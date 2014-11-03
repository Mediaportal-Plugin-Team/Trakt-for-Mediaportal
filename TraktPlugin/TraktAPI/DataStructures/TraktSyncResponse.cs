﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktPlugin.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktSyncResponse
    {
        [DataMember(Name = "added")]
        public Items Added { get; set; }

        [DataMember(Name = "deleted")]
        public Items Deleted { get; set; }

        [DataMember(Name = "existing")]
        public Items Existing { get; set; }

        [DataContract]
        public class Items
        {
            [DataMember(Name = "movies")]
            public int Movies { get; set; }

            [DataMember(Name = "shows")]
            public int Shows { get; set; }

            [DataMember(Name = "seasons")]
            public int Seasons { get; set; }

            [DataMember(Name = "episodes")]
            public int Episodes { get; set; }
        }

        [DataMember(Name = "not_found")]
        public NotFoundObjects NotFound { get; set; }

        [DataContract]
        public class NotFoundObjects
        {
            [DataMember(Name = "movies")]
            public List<TraktMovie> Movies { get; set; }

            [DataMember(Name = "shows")]
            public List<TraktShow> Shows { get; set; }

            [DataMember(Name = "episodes")]
            public List<TraktEpisode> Episodes { get; set; }
        }
    }
}
