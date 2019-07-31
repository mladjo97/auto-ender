using System.Collections.Generic;
using System.Linq;

namespace AutoEnder
{
    public class AutoEnderBehaviour : ModBehaviour
    {
        private bool _isActive = false;

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public override void OnActivate()
        {
            _isActive = true;
        }

        public override void OnDeactivate()
        {
            _isActive = false;
        }

        public void Update()
        {
            if (_isActive
                && GameSettings.Instance != null
                && GameSettings.Instance.MyCompany != null
                && GameSettings.Instance.MyCompany.WorkItems != null)
            {
                /*
                 * Continue when design phase is done
                 */
                IEnumerable<DesignDocument> designPhase = GameSettings.Instance.MyCompany.WorkItems.OfType<DesignDocument>();
                foreach (DesignDocument document in designPhase)
                {
                    if (document.HasFinished && !document.Done)
                    {
                        document.PromoteAction();
                    }
                }

                /*
                 * Continue when alpha phase is done
                 */
                IEnumerable<SoftwareAlpha> alphaPhase = GameSettings.Instance.MyCompany.WorkItems.OfType<SoftwareAlpha>();

                foreach (SoftwareAlpha alpha in alphaPhase)
                {
                    if (alpha.HasFinished && !alpha.Done)
                    {
                        alpha.PromoteAction();
                    }
                }
            }

        }

    }
}
