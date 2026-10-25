using System.Runtime.Serialization;

namespace TraktAPI.DataModels
{
  [DataContract]
  public class TraktPermissions
  {
    [DataMember( Name = "commenting" )]
    public bool Commenting { get; set; }

    [DataMember( Name = "liking" )]
    public bool Liking { get; set; }

    [DataMember( Name = "following" )]
    public bool Following { get; set; }
  }
}
