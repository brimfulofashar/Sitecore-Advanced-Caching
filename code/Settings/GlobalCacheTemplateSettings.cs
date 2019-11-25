using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Foundation.HtmlCache.Settings
{
    public class GlobalCacheTemplateSettings
    {
        private static readonly Lazy<GlobalCacheTemplateSettings>
            lazy =
                new Lazy<GlobalCacheTemplateSettings>
                    (() => new GlobalCacheTemplateSettings());

        public readonly HashSet<Guid> GlobalCacheableTemplateIDs;

        public GlobalCacheTemplateSettings()
        {
            List<ID> globalCacheableTemplateIDsTemp = Sitecore.Configuration.Settings.GetSetting("GlobalCacheableTemplateIDs").Split('|')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => ID.Parse(x)).ToList();
            GlobalCacheableTemplateIDs = new HashSet<Guid>();
            foreach (ID g in globalCacheableTemplateIDsTemp)
            {
                GlobalCacheableTemplateIDs.Add(g.Guid);
            }
        }

        public static GlobalCacheTemplateSettings Instance => lazy.Value;
    }
}