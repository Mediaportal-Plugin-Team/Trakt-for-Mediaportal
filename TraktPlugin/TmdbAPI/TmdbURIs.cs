
namespace TraktPlugin.TmdbAPI
{
  public static class TmdbURIs
  {
    private const string apiUrl = "http://api.themoviedb.org/3/";

    public static string apiConfig = string.Concat( apiUrl, "configuration?api_key=", TmdbCredentials.ApiKey );
    public static string apiGetMovieImages = string.Concat( apiUrl, "movie/{0}/images?api_key=", TmdbCredentials.ApiKey );
    public static string apiGetShowImages = string.Concat( apiUrl, "tv/{0}/images?api_key=", TmdbCredentials.ApiKey );
    public static string apiGetSeasonImages = string.Concat( apiUrl, "tv/{0}/season/{1}/images?api_key=", TmdbCredentials.ApiKey );
    public static string apiGetEpisodeImages = string.Concat( apiUrl, "tv/{0}/season/{1}/episode/{2}/images?api_key=", TmdbCredentials.ApiKey );
    public static string apiGetPersonImages = string.Concat( apiUrl, "person/{0}/images?api_key=", TmdbCredentials.ApiKey );
  }
}
