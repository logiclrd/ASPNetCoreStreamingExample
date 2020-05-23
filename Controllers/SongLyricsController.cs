using Microsoft.AspNetCore.Mvc;

using ASPNetCoreStreamingExample.Model;
using ASPNetCoreStreamingExample.Results;

namespace ASPNetCoreStreamingExample.Controllers
{
  [Route("/v1")]
  public class SongLyricsController : Controller
  {
    ILyricsSource _lyricsSource;

    public SongLyricsController(ILyricsSource lyricsSource)
    {
      _lyricsSource = lyricsSource;
    }

    [HttpGet("sing")]
    public IActionResult PerformSong()
    {
      return new SongLyricsResult(_lyricsSource);
    }
  }
}
