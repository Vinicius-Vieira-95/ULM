namespace UlmApi.Domain.Entities
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseEntity<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode()) + Id.GetHashCode();
        }
    }
}
