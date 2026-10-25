using System.Runtime.Serialization;

namespace TraktAPI.DataModels
{
  [DataContract]
  public class TraktLimits
  {
    [DataMember( Name = "list" )]
    public TraktListLimits List { get; set; }

    [DataMember( Name = "watchlist" )]
    public TraktItemCount Watchlist { get; set; }

    [DataMember( Name = "favorites" )]
    public TraktItemCount Favourites { get; set; }

    [DataMember( Name = "search" )]
    public TraktRecentLimits Search { get; set; }

    [DataMember( Name = "collection" )]
    public TraktItemCount Collection { get; set; }

    [DataMember( Name = "notes" )]
    public TraktItemCount Notes { get; set; }

    [DataContract]
    public class TraktListLimits : TraktItemCount
    {
      [DataMember( Name = "count" )]
      public uint Count { get; set; }
    }

    [DataContract]
    public class TraktRecentLimits
    {
      [DataMember( Name = "recent_count" )]
      public uint RecentCount { get; set; }
    }
  }
}
