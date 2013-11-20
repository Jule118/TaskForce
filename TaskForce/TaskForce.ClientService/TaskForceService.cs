using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace TaskForce.ClientService
{
	/// <summary>
	/// Windows service for controlling running processes on the maschine
	/// </summary>
	public partial class TaskForceService : ServiceBase
	{
		public TaskForceService()
		{
			InitializeComponent();
			ServiceName = "TaskForce Service";
			CanShutdown = true;
		}

		/// <summary>
		/// Will be executed when the service shutdowns
		/// </summary>
		protected override void OnShutdown()
		{
		}

		/// <summary>
		/// Will be executed when the service starts
		/// </summary>
		/// <param name="args"></param>
		protected override void OnStart(string[] args)
		{
		}
	}
}