using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Globalization;
using System.ComponentModel;

namespace GWF.Controls
{
    public class ModalUpdateProgress : UpdateProgress
    {
        private string _backgroundCssClass;
        private string _cancelControlID;

        /// <summary>
        /// Gets or sets the background CSS class.
        /// </summary>
        /// <value>The background CSS class.</value>
        [Category("Appearance"), DefaultValue(""), Description("The CSS class to apply to the background when the ModalUpdateProgress is displayed.")]
        public string BackgroundCssClass
        {
            get
            {
                if (this._backgroundCssClass == null)
                {
                    return string.Empty;
                }

                return this._backgroundCssClass;
            }

            set
            {
                this._backgroundCssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets the cancel control ID.
        /// </summary>
        /// <value>The cancel control ID.</value>
        [Category("Behavior"), DefaultValue(""), Description("The ID of the element that cancels the ModalUpdateProgress.")]
        public string CancelControlID
        {
            get
            {
                if (this._cancelControlID == null)
                {
                    return string.Empty;
                }

                return this._cancelControlID;
            }

            set
            {
                this._cancelControlID = value;
            }
        }

        /// <summary>
        /// Gets the script manager.
        /// </summary>
        /// <value>The script manager.</value>
        private ScriptManager ScriptManager
        {
            get
            {
                ScriptManager manager = ScriptManager.GetCurrent(this.Page);
                if (manager == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The control with ID '{0}' requires a ScriptManager on the page. The ScriptManager must appear before any controls that need it.", new object[] { this.ID }));
                }

                return manager;
            }
        }

        /// <summary>
        /// Gets the script descriptors.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            if (((this.Page != null) && this.ScriptManager.SupportsPartialRendering) && this.Visible)
            {
                ScriptControlDescriptor desc = new ScriptControlDescriptor("Sys.UI._ModalUpdateProgress", this.ClientID);
                string updatePanelClientID = null;
                if (!string.IsNullOrEmpty(this.AssociatedUpdatePanelID))
                {
                    UpdatePanel panel = this.NamingContainer.FindControl(this.AssociatedUpdatePanelID) as UpdatePanel;
                    if (panel == null)
                    {
                        panel = this.Page.FindControl(this.AssociatedUpdatePanelID) as UpdatePanel;
                    }

                    if (panel == null)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No UpdatePanel found for AssociatedUpdatePanelID '{0}'.", new object[] { this.AssociatedUpdatePanelID }));
                    }

                    updatePanelClientID = panel.ClientID;
                }

                string backgroundCssClass = null;
                string cancelControlID = null;
                if (!string.IsNullOrEmpty(this._cancelControlID))
                {
                    Control control = this.NamingContainer.FindControl(this._cancelControlID);
                    if (control == null)
                    {
                        control = this.Page.FindControl(this._cancelControlID);
                    }

                    if (control == null)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No control found for CancelControlID '{0}'.", new object[] { this._cancelControlID }));
                    }

                    cancelControlID = control.ClientID;
                }

                if (!string.IsNullOrEmpty(this._backgroundCssClass))
                {
                    backgroundCssClass = this._backgroundCssClass;
                }

                desc.AddProperty("associatedUpdatePanelId", updatePanelClientID);
                desc.AddProperty("dynamicLayout", this.DynamicLayout);
                desc.AddProperty("displayAfter", this.DisplayAfter);
                desc.AddProperty("backgroundCssClass", backgroundCssClass);
                desc.AddProperty("cancelControlID", cancelControlID);
                yield return desc;
            }
        }

        /// <summary>
        /// Gets the script references.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("GSS.Controls.ModalUpdateProgress.ModalUpdateProgress.js", "GSS");
        }
    }
}
