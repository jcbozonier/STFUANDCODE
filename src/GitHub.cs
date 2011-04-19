using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace STFUANDCODE
{
  public class GitHub
  {
    public static string CreateGist(string codeToShare)
    {
      var gist = Gist.Create("", "I SHARED CODE WITH STFU and CODE!", ".cs", codeToShare);
      return gist.Url;
    }
  }

  public class Gist
  {
    public string Id { get; private set; }
    public string Url { get; private set; }
    public string Version { get; private set; }

    private const string GistUriString = "https://gist.github.com/gists";

    public static string LastResponse;

    public string Name { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public string TypeExt { get; set; }

    private Gist()
    {
    }

    public static Gist Create(string name, string description, string type, string content)
    {
      LastResponse = null;

      var gist = new Gist
      {
        Name = name,
        Description = description,
        TypeExt = type,
        Content = content
      };

      var request = WebRequest.Create(GistUriString);

      var ext = gist.TypeExt;

      var body =
          "authenticity_key=" + Uri.EscapeDataString("fcca8a96f57b3f7ba763403626d031f7074dc15d") +
          "&description=" + Uri.EscapeDataString(description) +
          "&file_contents[gistfile1]=" + Uri.EscapeDataString(content) +
          "&file_ext[gistfile1]=" + ext +
          "&file_name[gistfile1]=" + Uri.EscapeDataString(name);

      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = body.Length;

      var requestBytes = Encoding.UTF8.GetBytes(body);

      var requestStream = request.GetRequestStream();
      requestStream.Write(requestBytes, 0, requestBytes.Length);

      var response = request.GetResponse();
      var responseStream = response.GetResponseStream();
      var responseText = string.Empty;
      using (var tr = new StreamReader(responseStream))
        responseText = tr.ReadToEnd();

      LastResponse = responseText;

      gist.ParseMetadata(responseText);

      if (string.IsNullOrWhiteSpace(gist.Id))
        throw new GistCreationException(string.Format("Couldn't parse Id from {0}", responseText));

      return gist;
    }

    private void ParseMetadata(string responseText)
    {
      var regex = new Regex(@"<a href=""/raw/(?<Id>[0-9]+)/(?<Version>[0-9a-f]+)/(?<Name>[^""]*)"">raw</a>");
      var match = regex.Match(responseText);
      this.Id = match.Groups["Id"].Value;
      this.Version = match.Groups["Version"].Value;
      this.Name = Uri.UnescapeDataString(match.Groups["Name"].Value);
      this.Url = @"https://gist.github.com/" + this.Id;
    }
  }

  public class GistCreationException : Exception
  {
    public GistCreationException(string message)
      : base(message)
    {
    }

    public GistCreationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
