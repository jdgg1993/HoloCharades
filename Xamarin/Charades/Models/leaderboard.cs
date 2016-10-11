using System;
using Newtonsoft.Json;

namespace Charades
{
	public class leaderboard
	{
		string id;
		string schoolName;
		string totalTime;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		[JsonProperty(PropertyName = "schoolName")]
		public string SchoolName
		{
			get { return schoolName; }
			set { schoolName = value; }
		}

		[JsonProperty(PropertyName = "totalTime")]
		public string TotalTime
		{
			get { return totalTime; }
			set { totalTime = value; }
		}
	}
}
