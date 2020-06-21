using System;
using System.Collections.Generic;
using System.Text;

using DiscussionWeb.Models;

namespace DiscussionWeb.ResourceParameters
{
	public class AuthorsResourceParameters : BaseResourceParameter
	{
		public AccessLevel AccessLevel { get; set; }
	}
}
