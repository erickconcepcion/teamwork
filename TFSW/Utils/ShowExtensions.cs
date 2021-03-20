using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSW.Data;

namespace TFSW.Utils
{
    static class ShowExtensions
    {
        public static string ToStringReferenceTable(this IEnumerable<WorkReference> refs)
            => refs.ToStringTable(new string[] { "Name", "ReferenceName" }, r => r.Name, r => r.ReferenceName);
        public static string ToStringRelTable(this IEnumerable<WorkItemRelationType> rels) 
            => rels.ToStringTable(new string[] { "Name", "ReferenceName", "Url" },r => r.Name, r => r.ReferenceName,
                r=> r.Url.ShortString(50));

        public static string ToStringTypeTable(this IEnumerable<WorkItemType> types)
            => types.ToStringTable(new string[] { "Name", "ReferenceName", "Description", "Is Disabled" }, t => t.Name,
                t => t.ReferenceName, t=> t.Description.ShortString(23), t=> t.IsDisabled);

        public static string ToStringProjectTable(this IEnumerable<TeamProjectReference> projects)
            =>projects.ToStringTable(new string[] { "Id", "Name", "Description" }, 
                p => p.Id, p => p.Name, p=> p.Description.ShortString(50));

        public static string ShortString(this string str, int max) 
            => str?.Length > max ? $"{str.Substring(0, max)}..." : str;
    }
}
