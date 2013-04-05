﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Vixen.Sys;
using Vixen.Module.Preview;
using Vixen.Sys.Attribute;

namespace VixenModules.Preview.VixenPreview
{
    public class VixenPreviewDescriptor : PreviewModuleDescriptorBase
	{
        static VixenPreviewDescriptor()
        {
            ModulePath = "VixenDisplayPreview";            
        }

        private static Guid _typeId = new Guid("{f43c31bc-cfcd-4dbc-adb7-d349c9ec623d}");

        [ModuleDataPath]
        public static string ModulePath { get; set; }
        
        public override Guid TypeId { get { return _typeId; } }

        public override Type ModuleClass { get { return typeof(VixenPreviewModuleInstance); } }

		public override Type ModuleDataClass { get { return typeof(VixenPreviewData); } }

		public override string Author { get { return "Vixen Team"; } }

		public override string TypeName { get { return "Vixen Display Preview"; } }

		public override string Description { get { return "Vixen Display Preview"; } }

		public override string Version { get { return "0.1"; } }

        public override int UpdateInterval
        {
            get
            {
                return 50;
            }
        }

	}
}