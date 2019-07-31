using DevConsole;
using System.Linq;
using System.Timers;
using System.Collections.Generic;

namespace AutoEnder
{
    public class AutoEnderBehaviour : ModBehaviour
    {
        private bool _isActive = false;
        private readonly Timer _timer = new Timer(250);

        public override void OnActivate()
        {
            Console.LogInfo("Activated Auto-Ender by github.com/mladjo97");
            _isActive = true;

            /*
             *  Set the timer to be invoked for project update
             */
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
            if (!_isActive
                || GameSettings.Instance == null
                || GameSettings.Instance.MyCompany == null
                || GameSettings.Instance.MyCompany.WorkItems == null)
            {
                return;
            }

            /*
             *  Auto-end (skip) the design phase for all projects
             */
            SHashSet<WorkItem> workItems = GameSettings.Instance.MyCompany.WorkItems;
            IEnumerable<DesignDocument> designDocuments = workItems.OfType<DesignDocument>().ToList();

            foreach (var designDocument in designDocuments)
            {
                if (designDocument.HasFinished && !designDocument.Done)
                {
                    designDocument.PromoteAction();
                }
            }
            
            /*
             *  Auto-end (skip) the alpha phase for contract projects
             */
            IEnumerable<SoftwareAlpha> alphaPhase = workItems.OfType<SoftwareAlpha>().ToList();

            foreach (var alpha in alphaPhase)
            {
                if (!alpha.InBeta && !alpha.InDelay && alpha.contract != null)
                {
                    /*
                    *   There are different multipliers applied on more complex projects
                    *   so there are different calculations for unit progress
                    */
                    float actualCodeProgress = alpha.CodeProgress * alpha.CodeDevTime;
                    float actualArtProgress = alpha.ArtProgress * alpha.ArtDevTime;

                    if (actualCodeProgress >= alpha.contract.CodeUnits && actualArtProgress >= alpha.contract.ArtUnits)
                    {
                        alpha.PromoteAction();
                    }

                }
            }

        }

    }
}
