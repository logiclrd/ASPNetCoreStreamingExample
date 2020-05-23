using System;
using System.Collections.Generic;
using System.Linq;

namespace ASPNetCoreStreamingExample.Model
{
  public class LyricsSource : ILyricsSource
  {
    public IEnumerable<string> GetSongLyrics() => Lyrics;

    const string Song =
@"Row, row, row your boat
Gently down the stream
Merrily, merrily, merrily, merrily
Life is but a dream

Row, row, row your boat
Gently up the creek
If you see a little mouse
Don't forget to squeak!

Row, row, row your boat
Gently down the stream
If you see a crocodile
Don't forget to scream!

Row, row, row your boat
Gently to the shore
If you see a lion there
Don’t forget to roar!";

    readonly string[] Lyrics = Song.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
  }
}
