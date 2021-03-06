﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace RestfulRouting
{
	public class RouteConfiguration
	{
		static RouteConfiguration()
		{
			Default = () => new RouteConfiguration();
		}

		public RouteConfiguration()
		{
			InitializeDefaults();
		}

		private void InitializeDefaults()
		{
			IndexName = "index";
			ShowName = "show";
			NewName = "new";
			CreateName = "create";
			EditName = "edit";
			UpdateName = "update";
			DeleteName = "delete";
			DestroyName = "destroy";

			MemberRoutes = new Dictionary<string, HttpVerbs[]>();
			CollectionRoutes = new Dictionary<string, HttpVerbs[]>();
		}

		public string[] Namespaces { get; set; }

		public string IdValidationRegEx { get; set; }

		public RouteValueDictionary Constraints { get; set; }

		public string As { get; set; }

		public string PathPrefix { get; set; }

		public bool Shallow { get; set; }

		public static Func<RouteConfiguration> Default { get; set; }

		public string IndexName { get; set; }

		public string ShowName { get; set; }

		public string CreateName { get; set; }

		public string UpdateName { get; set; }

		public string DestroyName { get; set; }

		public string NewName { get; set; }

		public string EditName { get; set; }

		public string DeleteName { get; set; }

		private static string GetActionName<TController>(Expression<Func<TController, object>> actionExpression)
		{
			var body = ((MethodCallExpression)actionExpression.Body);
			var actionName = body.Method.Name.ToLowerInvariant();
			return actionName;
		}

		public void AddMemberRoute<TController>(Expression<Func<TController, object>> actionExpression, params HttpVerbs[] verbs)
		{
			if (verbs.Count() == 0)
				verbs = new[] { HttpVerbs.Get };
			AddMemberRoute(GetActionName(actionExpression), verbs);
		}

		public void AddCollectionRoute<TController>(Expression<Func<TController, object>> actionExpression, params HttpVerbs[] verbs)
		{
			if (verbs.Count() == 0)
				verbs = new[] { HttpVerbs.Get };
			AddCollectionRoute(GetActionName(actionExpression), verbs);
		}

		public void AddMemberRoute(string name, params HttpVerbs[] verbs)
		{
			MemberRoutes[name] = verbs;
		}

		public void AddCollectionRoute(string name, params HttpVerbs[] verbs)
		{
			CollectionRoutes[name] = verbs;
		}

		public IDictionary<string, HttpVerbs[]> MemberRoutes { get; private set; }

		public IDictionary<string, HttpVerbs[]> CollectionRoutes { get; private set; }

		public string[] GetCollectionVerbArray(string member)
		{
			return CollectionRoutes[member].Select(x => x.ToString().ToUpperInvariant()).ToArray();
		}

		public string[] GetMemberVerbArray(string member)
		{
			return MemberRoutes[member].Select(x => x.ToString().ToUpperInvariant()).ToArray();
		}

		private IList<string> IncludedActions { get; set; }

		public bool Includes(string action)
		{
			EnsureIncludedActionsIsInitialized();

			return IncludedActions.Contains(action);
		}

		public void Except(params string[] actions)
		{
			EnsureIncludedActionsIsInitialized();

			foreach (var action in actions)
			{
				IncludedActions.Remove(action);				
			}
		}

		private void EnsureIncludedActionsIsInitialized()
		{
			if (IncludedActions == null)
				IncludedActions = new List<string> { IndexName, ShowName, NewName, CreateName, EditName, UpdateName, DeleteName, DestroyName };
		}

		public void Only(params string[] actions)
		{
			IncludedActions = new List<string>(actions);
		}
	}
}
