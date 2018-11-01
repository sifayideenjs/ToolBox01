using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.Model
{
    public static class BackboneType
    {
        public static Type BackboneStringType { get { return typeof(BackboneString); } }
        public static Type BackboneDoubleType { get { return typeof(BackboneDouble); } }
        public static Type BackboneBrowseFileType { get { return typeof(BackboneBrowseFile); } }
        public static Type BackboneBrowseFolderType { get { return typeof(BackboneBrowseFolder); } }
        public static Type BackboneButtonType { get { return typeof(BackboneButton); } }
    }
}
