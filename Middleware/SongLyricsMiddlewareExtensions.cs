using Microsoft.AspNetCore.Builder;

namespace ASPNetCoreStreamingExample.Middleware
{
  public static class SongLyricsMiddlewareExtensions
  {
    public static IApplicationBuilder UseSongLyrics(this IApplicationBuilder builder)
      => builder.UseMiddleware<SongLyricsMiddleware>();
  }
}
