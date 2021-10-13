using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace vector_unitech_core.Utils
{
    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, object> ToDictionary( this object source )
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>( this object source )
        {
            if ( source == null ) ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach ( PropertyDescriptor property in TypeDescriptor.GetProperties( source ) )
            {
                object value = property.GetValue( source );
                if ( IsOfType<T>( value ) )
                {
                    dictionary.Add( property.Name, ( T ) value );
                }
            }
            return dictionary;
        }

        public static string ToQueryString( this object source )
        {
            var parameters = ToDictionary<string>( source );
            //HttpUtility.UrlEncode
            return ( string.Join( "&",
                parameters.Select( kvp =>
                    string.Format( "{0}={1}", kvp.Key, kvp.Value ) ) ) );
        }


        private static bool IsOfType<T>( object value )
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new NullReferenceException( "Unable to convert anonymous object to a dictionary. The source anonymous object is null." );
        }
    }
}
