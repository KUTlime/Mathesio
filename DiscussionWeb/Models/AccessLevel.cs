using System;

namespace DiscussionWeb.Models
{
	[Flags]
	public enum AccessLevel : byte
	{
		BasicUser = 0,
		Admin = 1
	}
}