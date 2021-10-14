using Bogus;
using System.Collections.Generic;
using vector_unitech_core.Entities;

namespace Vector_unitech_test
{
    internal static class MockApiFaker
    {

        internal static List<TestEntity> FakerTestApi()
        {
            var list = new Faker<TestEntity>()
                    .RuleFor( x => x.CreatedAt, f => f.Date.Past() )
                    .RuleFor( x => x.Avatar, f => f.Internet.Avatar() )
                    .RuleFor( x => x.Id, f => f.Random.Int( min: 0, max: 30 ) )
                    .RuleFor( x => x.Mail, f => f.Internet.Email() )
                    .RuleFor( x => x.Name, f => f.Person.FullName )
                ;

            var result = list.Generate( 15 );

            return result;
        }
    }
}
