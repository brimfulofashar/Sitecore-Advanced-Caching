using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;

namespace SitecoreAdvancedCaching.Search
{
    public class ItemDatasourcesComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        private Regex IDRegex = new Regex(@"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}");

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item != null)
            {
                var layoutField = new LayoutField(item.Fields[FieldIDs.FinalLayoutField]);
                LayoutDefinition layout = LayoutDefinition.Parse(layoutField.Value);

                StringBuilder sb = new StringBuilder();

                if (layout != null)
                {
                    

                    // loop over devices in the rendering
                    for (int deviceIndex = layout.Devices.Count - 1; deviceIndex >= 0; deviceIndex--)
                    {
                        var device = layout.Devices[deviceIndex] as DeviceDefinition;

                        if (device == null) return null;

                        // loop over renderings within the device
                        for (int renderingIndex = device.Renderings.Count - 1; renderingIndex >= 0; renderingIndex--)
                        {
                            var rendering = device.Renderings[renderingIndex] as RenderingDefinition;

                            if (rendering?.Datasource == null) return null;

                            foreach (var match in IDRegex.Matches(rendering.Datasource))
                            {
                                sb.Append("{" + match + "}" + "|");
                            }
                        }
                    }

                    foreach (Field field in item.Fields)
                    {
                        foreach (var match in IDRegex.Matches(field.Value))
                        {
                            sb.Append("{" + match + "}" + "|");
                        }
                    }
                }

                return sb.ToString().Trim('|').Replace("{{", "{").Replace("}}", "}");
            }

            return null;
        }
    }
}