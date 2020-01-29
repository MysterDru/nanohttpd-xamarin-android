using System;
using System.Collections.Generic;
using System.Text;
using FI.Iki.Elonen;
using Java.Interop;
using nanohttpd_sample;
namespace nanohttpd_sample
{
	public class HelloServer : NanoHTTPD
	{
		public HelloServer()
			: base(8080)
		{
		}

		public override Response Serve(IHTTPSession session)
		{
			var decodedQueryParameters = NanoHTTPD.DecodeParameters(session.QueryParameterString);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<html>");
			sb.AppendLine("<head><title>Debug Server</title></head>");
			sb.AppendLine("<body>");
			sb.AppendLine("<h1>Debug Server</h1>");

			sb.AppendLine("<p><blockquote><b>URI</b> = ")
				.AppendLine(session.Uri)
				.AppendLine("<br />");

			sb.AppendLine("<b>Method</b> = ")
				.AppendLine(session.Method.ToString())
				.AppendLine("</blockquote></p>");

			sb.AppendLine("<h3>Headers</h3><p><blockquote>")
				.AppendLine(toString(session.Headers))
				.AppendLine("</blockquote></p>");

			sb.AppendLine("<h3>Parms</h3><p><blockquote>")
				.AppendLine(toString(session.Parms))
				.AppendLine("</blockquote></p>");

			//sb.AppendLine("<h3>Parms (multi values?)</h3><p><blockquote>")
			//	.AppendLine(toString(decodedQueryParameters))
			//	.AppendLine("</blockquote></p>");

			try
			{
				var files = new Dictionary<string, string>();
				session.ParseBody(files);
				sb.AppendLine("<h3>Files</h3><p><blockquote>").AppendLine(toString(files)).AppendLine("</blockquote></p>");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}

			sb.AppendLine("</body>");
			sb.AppendLine("</html>");

			return NewFixedLengthResponse(sb.ToString());
			//return base.Serve(session);
		}

		private string toString(IDictionary<string, string> map)
		{
			if (map.Count == 0)
			{
				return "";
			}
			return unsortedList(map);
		}

		string unsortedList(IDictionary<string, string> map)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<ul>");
			foreach(var kvp in map)
			{
				listItem(sb, kvp);
			}
			sb.AppendLine("</ul>");
			return sb.ToString();
		}

		void listItem(StringBuilder sb, KeyValuePair<string, string> entry)
		{
			sb.AppendLine("<li><code><b>")
				.AppendLine(entry.Key)
				.AppendLine("</b> = ")
				.AppendLine(entry.Value)
				.AppendLine("</code></li>");
		}
	}
}
