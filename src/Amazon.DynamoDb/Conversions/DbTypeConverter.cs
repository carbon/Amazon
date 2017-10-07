using Carbon.Data;

namespace Amazon.DynamoDb
{
    public abstract class DbTypeConverter<T> : IDbValueConverter
    {
        public abstract T Parse(DbValue dbValue);

        public abstract DbValue ToDbValue(T value);

        #region IDbValueConverter

        DbValue IDbValueConverter.FromObject(object value, IMember meta) => ToDbValue((T)value);

        object IDbValueConverter.ToObject(DbValue item, IMember meta) => Parse(item);

        #endregion
    }   
}