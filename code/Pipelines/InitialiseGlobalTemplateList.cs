using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitialiseGlobalTemplateList
    {
        public static Dictionary<ID, Item> TemplateIds;

        public void Initialize(PipelineArgs args)
        {
            if (TemplateIds == null)
            {
                TemplateIds = new Dictionary<ID, Item>();

                var customCacheableTemplatePaths = Settings.GetSetting("CustomCacheableTemplatePaths").Split('|');

                foreach (var path in customCacheableTemplatePaths)
                {
                    var templates = Database.GetDatabase("web").GetItem(path).Axes.GetDescendants();

                    foreach (var templateItem in templates)
                        if (templateItem.TemplateID == new ID("{AB86861A-6030-46C5-B394-E8F99E8B87DB}") || templateItem
                                .Template.BaseTemplates.Select(x => x.ID)
                                .Contains(new ID("{AB86861A-6030-46C5-B394-E8F99E8B87DB}")))
                            TemplateIds.Add(templateItem.ID, templateItem);
                }
            }
        }
    }
}