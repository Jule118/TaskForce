using System;
using System.Collections.Generic;
using System.Text;

namespace TaskForce.Network
{
	/// <summary>
	/// List of all possible network packages
	/// </summary>
	[Serializable]
	public enum Command
	{
		/// <summary>
		/// Forces the client to refresh his ProtectedFilterList with the given filter list
		/// </summary>
		RefreshProtectedFilterList,

		/// <summary>
		/// Forces the client to refresh his Forbbiden FilterList with the given filter list
		/// </summary>
		RefreshForbiddenFilterList,

		/// <summary>
		/// Sends an Protocol object to the server
		/// </summary>
		SaveProtocol
	}
}