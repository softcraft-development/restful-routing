using System;
using System.Web;
using System.Web.Routing;

namespace RestfulRouting
{
	public class PostOverrideConstraint : HttpMethodConstraint
	{
		public PostOverrideConstraint()
			: base("POST")
		{
		}

		protected override bool Match(
			HttpContextBase httpContext,
			Route route,
			string parameterName,
			RouteValueDictionary values,
			RouteDirection routeDirection)
		{
			bool matches;
			if (routeDirection.Equals(RouteDirection.IncomingRequest))
			{
				var formMethod = httpContext.Request.Form["_method"];
				if (formMethod == null)
				{
					matches = false;
				}
				else
				{
					matches = base.Match(httpContext, route, parameterName, values, routeDirection);
				}
			}
			else
			{
				// Don't try to generate any URLs for this route; you need to use 
				// a form value instead
				matches = false;
			}
			return matches;
		}
	}
}
