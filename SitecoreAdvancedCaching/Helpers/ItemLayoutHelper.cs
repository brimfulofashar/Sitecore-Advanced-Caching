using System.Collections.Generic;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;

namespace SitecoreAdvancedCaching.Helpers
{
    public class ItemLayoutHelper
    {
        public static bool ItemHasLayout(Item item)
        {
            var layoutField = new LayoutField(item.Fields[FieldIDs.FinalLayoutField]);
            var layout = LayoutDefinition.Parse(layoutField.Value);

            return layout != null && !string.IsNullOrEmpty(layoutField.Value);
        }

        public static List<RenderingDefinition> GetDeviceDefinition(Item item)
        {
            var layoutField = new LayoutField(item.Fields[FieldIDs.FinalLayoutField]);
            var layout = LayoutDefinition.Parse(layoutField.Value);

            var renderingDefinitions = new List<RenderingDefinition>();

            if (layout != null && !string.IsNullOrEmpty(layoutField.Value))
                for (var deviceIndex = layout.Devices.Count - 1; deviceIndex >= 0; deviceIndex--)
                {
                    var device = layout.Devices[deviceIndex] as DeviceDefinition;

                    if (device != null)
                        for (var renderingIndex = device.Renderings.Count - 1; renderingIndex >= 0; renderingIndex--)
                        {
                            var rendering = device.Renderings[renderingIndex] as RenderingDefinition;

                            if (rendering != null) renderingDefinitions.Add(rendering);
                        }
                }

            return renderingDefinitions;
        }
    }
}