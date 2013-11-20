namespace TaskForce.ClientService
{
	partial class TaskForceServiceInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.TaskForceServiveProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.TaskForceServiceInstaller1 = new System.ServiceProcess.ServiceInstaller();
			// 
			// UpdaterServiveProcessInstaller
			// 
			this.TaskForceServiveProcessInstaller.Password = null;
			this.TaskForceServiveProcessInstaller.Username = null;
			// 
			// UpdaterServiceInstaller1
			// 
			this.TaskForceServiceInstaller1.Description = "Controlls all running processes on the maschine";
			this.TaskForceServiceInstaller1.DisplayName = "TaskForce Service";
			this.TaskForceServiceInstaller1.ServiceName = "TaskForceService";
			this.TaskForceServiceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// UpdaterServiceInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.TaskForceServiveProcessInstaller,
            this.TaskForceServiceInstaller1});
		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller TaskForceServiveProcessInstaller;
		private System.ServiceProcess.ServiceInstaller TaskForceServiceInstaller1;
	}
}