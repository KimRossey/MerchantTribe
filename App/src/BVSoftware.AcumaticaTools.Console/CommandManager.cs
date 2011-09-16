using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole
{
    class CommandManager
    {
        List<ICommand> availableCommands = new List<ICommand>();
        private ServiceContext _Context = new ServiceContext();

        public string Username
        {
            get
            {
                return Properties.Settings.Default.Username;
            }
            set
            {
                _Context.Username = value;
                _Context.HasLoggedIn = false;
                Properties.Settings.Default.Username = value;
                Properties.Settings.Default.Save();
            }
        }
        public string Password
        {
            get
            {
                return Properties.Settings.Default.Password;                
            }
            set
            {
                _Context.Password = value;
                _Context.HasLoggedIn = false;   
                Properties.Settings.Default.Password = value;
                Properties.Settings.Default.Save();
            }
        }
        public string SiteAddress
        {
            get { return Properties.Settings.Default.SiteAddress;}
            set
            {
                _Context.SiteAddress = value;
                _Context.HasLoggedIn = false;
                Properties.Settings.Default.SiteAddress = value.ToString();
                Properties.Settings.Default.Save();
            }
        }
        public string NewItemTaxCategoryId
        {
            get { return Properties.Settings.Default.NewItemTaxCategoryId; }
            set
            {
                Properties.Settings.Default.NewItemTaxCategoryId = value;
                Properties.Settings.Default.Save();
            }
        }
        public CommandManager()
        {
            PopulateCommands();
            _Context.NewItemTaxAccountId = this.NewItemTaxCategoryId;
        }

        private void PopulateCommands()
        {
            availableCommands.Add(new Commands.Help());
            availableCommands.Add(new Commands.TestConnection());
            availableCommands.Add(new Commands.SimpleTest());
            availableCommands.Add(new Commands.CustomerSearch());
            availableCommands.Add(new Commands.Login());
            availableCommands.Add(new Commands.CreateCustomer());
            availableCommands.Add(new Commands.ItemSearch());
            availableCommands.Add(new Commands.CreateItem());
            availableCommands.Add(new Commands.List());
            availableCommands.Add(new Commands.SampleOrder());
        }

        public bool ParseCommand(string[] args)
        {
            if (args.Length < 1)
            {
                args = new string[] { "help" };
            }

            foreach (ICommand cmd in availableCommands)
            {
                if (cmd.NameMatches(args[0].ToLowerInvariant()))
                {
                    if (!this._Context.HasLoggedIn)
                    {
                        _Context = Connections.Login(this.Username, this.Password, this.SiteAddress);
                        _Context.NewItemTaxAccountId = this.NewItemTaxCategoryId;
                    }
                    return cmd.Execute(args, this._Context);
                }
            }

            Console.WriteLine("The requested command wasn't found.");
            Console.WriteLine("Try 'ac help' for a list of available commands");
            return false;
        }

        internal List<ICommand> AvailableCommands()
        {
            return availableCommands;
        }

       
    }
}
