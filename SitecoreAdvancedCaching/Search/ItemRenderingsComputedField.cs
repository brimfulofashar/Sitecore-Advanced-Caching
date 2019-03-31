using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;

namespace SitecoreAdvancedCaching.Search
{
    public class ItemRenderingsComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item != null)
            {
                var layoutField = new LayoutField(item.Fields[FieldIDs.FinalLayoutField]);
                LayoutDefinition layout = LayoutDefinition.Parse(layoutField.Value);

                if (layout != null)
                {
                    StringBuilder sb = new StringBuilder();

                    // loop over devices in the rendering
                    for (int deviceIndex = layout.Devices.Count - 1; deviceIndex >= 0; deviceIndex--)
                    {
                        var device = layout.Devices[deviceIndex] as DeviceDefinition;

                        if (device == null) return null;

                        // loop over renderings within the device
                        for (int renderingIndex = device.Renderings.Count - 1; renderingIndex >= 0; renderingIndex--)
                        {
                            var rendering = device.Renderings[renderingIndex] as RenderingDefinition;

                            if (rendering == null) return null;

                            sb.Append(rendering.ItemID + "|");
                        }
                    }

                    return sb.ToString().Trim('|');
                }
            }

            return null;
        }
    }
}