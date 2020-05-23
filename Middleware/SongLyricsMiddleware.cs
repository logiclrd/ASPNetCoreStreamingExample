using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using ASPNetCoreStreamingExample.Model;

namespace ASPNetCoreStreamingExample.Middleware
{
  public class SongLyricsMiddleware
  {
    RequestDelegate _next;
    ILyricsSource _lyricsSource;

    public SongLyricsMiddleware(RequestDelegate next, ILyricsSource lyricsSource)
    {
      _next = next;
      _lyricsSource = lyricsSource;
    }

    public Task InvokeAsync(HttpContext context)
    {
      if (!context.Request.Path.StartsWithSegments(new PathString("/middleware/sing")))
        return _next(context);
      else
        return InvokeAsyncImplementation(context);
    }

    static readonly byte[] JSONArrayStart = new byte[] { (byte)'[' };
    static readonly byte[] JSONArraySeparator = new byte[] { (byte)',' };

    public async Task InvokeAsyncImplementation(HttpContext context)
    {
      await context.Response.Body.WriteAsync(JSONArrayStart, context.RequestAborted);

      while (true)
      {
        foreach (string line in _lyricsSource.GetSongLyrics())
        {
          await JsonSerializer.SerializeAsync(context.Response.Body, line, cancellationToken: context.RequestAborted);
          await context.Response.Body.WriteAsync(JSONArraySeparator, context.RequestAborted);

          await context.Response.Body.FlushAsync(context.RequestAborted);

          await Task.Delay(200);
        }
      }

      // No JSONArrayEnd needs to be written, because the above loop will never exit.
    }
  }
}
