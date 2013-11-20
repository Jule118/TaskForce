using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace TaskForce.ClientService
{
	/// <summary>
	/// Installer class for the TaskForce service
	/// </summary>
	[RunInstaller(true)]
	public partial class TaskForceServiceInstaller : System.Configuration.Install.Installer
	{
		public TaskForceServiceInstaller()
		{
			InitializeComponent();
			this.TaskForceServiveProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
		}
	}
}