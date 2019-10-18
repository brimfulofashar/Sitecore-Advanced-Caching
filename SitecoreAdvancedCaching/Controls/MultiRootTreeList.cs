using System;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls;

namespace SitecoreAdvancedCaching.Controls
{
    public class MultiRootTreeList : TreeList
    {
        protected override void OnLoad(EventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            base.OnLoad(args);

            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                // find the existing TreeviewEx that the base OnLoad added, get a ref to its parent, and remove it from controls
                var existingTreeView = (TreeviewEx) WebUtil.FindControlOfType(this, typeof(TreeviewEx));
                var treeviewParent = existingTreeView.Parent;

                existingTreeView.Parent.Controls.Clear(); // remove stock treeviewex, we replace with multiroot

                // find the existing DataContext that the base OnLoad added, get a ref to its parent, and remove it from controls
                var dataContext = (DataContext) WebUtil.FindControlOfType(this, typeof(DataContext));
                var dataContextParent = dataContext.Parent;

                dataContextParent.Controls.Remove(dataContext); // remove stock datacontext, we parse our own

                // create our MultiRootTreeview to replace the TreeviewEx
                var impostor = new MultiRootTreeview();
                impostor.ID = existingTreeView.ID;
                impostor.DblClick = existingTreeView.DblClick;
                impostor.Enabled = existingTreeView.Enabled;
                impostor.DisplayFieldName = existingTreeView.DisplayFieldName;

                // parse the data source and create appropriate data contexts out of it
                var dataContexts = ParseDataContexts(dataContext);

                impostor.DataContext = string.Join("|", dataContexts.Select(x => x.ID));
                foreach (var context in dataContexts) dataContextParent.Controls.Add(context);

                // inject our replaced control where the TreeviewEx originally was
                treeviewParent.Controls.Add(impostor);
            }
        }

        /// <summary>
        ///     Parses multiple source roots into discrete data context controls (e.g.
        ///     'dataSource=/sitecore/content|/sitecore/media library')
        /// </summary>
        /// <param name="originalDataContext">
        ///     The original data context the base control generated. We reuse some of its property
        ///     values.
        /// </param>
        /// <returns></returns>
        protected virtual DataContext[] ParseDataContexts(DataContext originalDataContext)
        {
            return new ListString(DataSource).Select(x => CreateDataContext(originalDataContext, x)).ToArray();
        }

        /// <summary>
        ///     Creates a DataContext control for a given Sitecore path data source
        /// </summary>
        protected virtual DataContext CreateDataContext(DataContext baseDataContext, string dataSource)
        {
            var dataContext = new DataContext();
            dataContext.ID = GetUniqueID("D");
            dataContext.Filter = baseDataContext.Filter;
            dataContext.DataViewName = "Master";
            if (!string.IsNullOrEmpty(DatabaseName)) dataContext.Parameters = "databasename=" + DatabaseName;
            dataContext.Root = dataSource;
            dataContext.Language = Language.Parse(ItemLanguage);

            return dataContext;
        }
    }
}