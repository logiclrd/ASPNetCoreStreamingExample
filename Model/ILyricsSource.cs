using System.Collections.Generic;

namespace ASPNetCoreStreamingExample.Model
{
  public interface ILyricsSource
  {
    IEnumerable<string> GetSongLyrics();
  }
}
