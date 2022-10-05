using System.Drawing;

namespace Common
{
    public class Academy
    {
        public string Name { get; set; }    
        public List<Path> Paths { get; set; }
        public decimal? MinDistanceToStart { get; set; }
        public bool Visited { get; set; }
        public Academy NearestToStart { get; set; }
        public override string ToString() => Name;
        public override bool Equals(object obj)
        {
            if(obj is null)
            {
                return false;
            }
            var item = obj as Academy;

            if (item is null)
            {
                return false;
            }

            return this.Name == item.Name;
        }

        public static bool operator ==(Academy arg1, Academy arg2)
        {
            if (arg1 is null && arg2 is null)
                return true;
            if (arg1 is null || arg2 is null)
                return false;
            return arg1.Equals(arg2);
        }

        public static bool operator !=(Academy arg1, Academy arg2)
        {
            if (arg1 is null && arg2 is null)
                return false;
            if (arg1 is null || arg2 is null)
                return true;
            return !arg1.Equals(arg2);
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }

}