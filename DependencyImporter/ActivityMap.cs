using System;
using CsvHelper.Configuration;
using DependencyImporter.Application.Entities;

namespace DependencyImporter
{
    public class ActivityMap : ClassMap<Activity>
    {
        public ActivityMap()
        {
            this.Map(x => x.ProjectId);
            this.Map(x => x.ActivityId);
            this.Map(x => x.Name);
            this.Map(x => x.Type);
            this.Map(x => x.Critical).ConvertUsing(x => x.GetField<string>("Critical").Equals("Yes", StringComparison.OrdinalIgnoreCase));
        }
    }
}