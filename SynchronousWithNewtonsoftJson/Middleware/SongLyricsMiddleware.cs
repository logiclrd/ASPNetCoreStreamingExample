﻿using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Model;

namespace ASPNetCoreStreamingExample.SynchronousWithNewtonsoftJson.Middleware
{
  public class SongLyricsMiddleware
  {
    RequestDelegate _next;
    ILyricsSource _lyricsSource;
    JsonSerializer _serializer;

    public SongLyricsMiddleware(RequestDelegate next, ILyricsSource lyricsSource, JsonSerializer serializer)
    {
      _next = next;
      _lyricsSource = lyricsSource;
      _serializer = serializer;
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
          using (var streamWriter = new StreamWriter(context.Response.Body))
          using (var jsonWriter = new JsonTextWriter(streamWriter))
            _serializer.Serialize(jsonWriter, line);
          
          await context.Response.Body.WriteAsync(JSONArraySeparator, context.RequestAborted);

          await context.Response.Body.FlushAsync(context.RequestAborted);

          await Task.Delay(200);
        }
      }

      // No JSONArrayEnd needs to be written, because the above loop will never exit.
    }
  }
}
