using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ASPNetCoreStreamingExample.Model;

namespace ASPNetCoreStreamingExample.Results
{
  public class SongLyricsResult : IActionResult
  {
    ILyricsSource _lyricsSource;

    public SongLyricsResult(ILyricsSource lyricsSource)
    {
      _lyricsSource = lyricsSource;
    }

    static readonly byte[] JSONArrayStart = new byte[] { (byte)'[' };
    static readonly byte[] JSONArraySeparator = new byte[] { (byte)',' };

    public async Task ExecuteResultAsync(ActionContext context)
    {
      await context.HttpContext.Response.Body.WriteAsync(JSONArrayStart, context.HttpContext.RequestAborted);
  
      while (true)
      {
        foreach (string line in _lyricsSource.GetSongLyrics())
        {
          await JsonSerializer.SerializeAsync(context.HttpContext.Response.Body, line, cancellationToken: context.HttpContext.RequestAborted);
          await context.HttpContext.Response.Body.WriteAsync(JSONArraySeparator, context.HttpContext.RequestAborted);

          await context.HttpContext.Response.Body.FlushAsync(context.HttpContext.RequestAborted);

          await Task.Delay(200);
        }
      }

      // No JSONArrayEnd needs to be written, because the above loop will never exit.
    }
  }
}
