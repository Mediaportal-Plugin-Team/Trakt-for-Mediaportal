using System.Runtime.Serialization;

namespace TraktAPI.DataModels
{
  [DataContract]
  public class TraktItemCount
  {
    [DataMember( Name = "item_count" )]
    public uint ItemCount { get; set; }
  }
}
