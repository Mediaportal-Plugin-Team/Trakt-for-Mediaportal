using System.Runtime.Serialization;

namespace TraktAPI.DataModels
{
  [DataContract]
  public class TraktSharingText
  {
    [DataMember( Name = "watching" )]
    public string WatchingFormat { get; set; }

    [DataMember( Name = "watched" )]
    public string WatchedFormat { get; set; }

    [DataMember( Name = "rated" )]
    public string RatedFormat { get; set; }
  }
}
