using System;

namespace DependencyImporter.Application.Entities
{
    public class Activity : IEquatable<Activity>
    {
        public Activity(string projectId, string activityId, string name, string type)
        {
            ProjectId = projectId.Trim().ToUpper();
            ActivityId = activityId.Trim().ToUpper();
            Name = name;
            Type = type;
        }

        public Activity() { }

    public string ProjectId { get; set; }
        public string ActivityId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public bool Equals(Activity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ProjectId, other.ProjectId) && string.Equals(ActivityId, other.ActivityId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Activity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ProjectId?.GetHashCode() ?? 0)*397) ^ (ActivityId?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(Activity left, Activity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Activity left, Activity right)
        {
            return !Equals(left, right);
        }
    }
}