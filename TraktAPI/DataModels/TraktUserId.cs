using System.Runtime.Serialization;

namespace TraktAPI.DataModels
{
  [DataContract]
  public class TraktUserId
  {
    [DataMember( Name = "slug" )]
    public string Slug { get; set; }

    [DataMember( Name = "uuid" )]
    public string Uuid { get; set; }
  }
}
