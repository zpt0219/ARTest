using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokid
{
	public class GoogleNewsProtocol
	{
		public string status;
		public int totalResults;
		public List<SingleGoogleArticle> articles;
	}

	public class SingleGoogleArticle
	{
		public SingleGoogleArtcileSource source;
		public string author;
		public string title;
		public string description;
		public string url;
		public string urlToImage;
		public string publishedAt;
	}

	public class SingleGoogleArtcileSource
	{
		public string id;
		public string name;
	}

}

