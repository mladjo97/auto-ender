using System;
using System.Linq;
using System.Timers;
using System.Diagnostics;
using System.Collections.Generic;


namespace AutoEnder
{
    public class AutoEnderBehaviour : ModBehaviour
    {
        private bool _isActive = false;
        private readonly Timer _timer = new Timer(250);

        public override void OnActivate()
        {
            Debug.WriteLine("Activating");
            _isActive = true;

            _timer.Elapsed += UpdateProjects;
            _timer.AutoReset = true;
            _timer.Start();
        }

        public override void OnDeactivate()
        {
            _isActive = false;
            _timer.Stop();
        }

        private void UpdateProjects(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Updating projects");

            if (!_isActive
                || GameSettings.Instance == null
                || GameSettings.Instance.MyCompany == null
                || GameSettings.Instance.MyCompany.WorkItems == null)
            {
                return;
            }

            SHashSet<WorkItem> workItems = GameSettings.Instance.MyCompany.WorkItems;
            IEnumerable<DesignDocument> designDocuments = workItems.OfType<DesignDocument>().ToList();

            foreach (var designDocument in designDocuments)
            {
                if (designDocument.HasFinished && !designDocument.Done)
                {
                    Debug.WriteLine("Finishing design phase");
                    designDocument.PromoteAction();
                }
            }

        }

    }
}
