using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes.Persistence;
using AutoMapper;
using Notes.Application.Interfaces;
using Notes.Application.Common.Mappings;

namespace Notes.Tests.Common
{
    public class QueryTestFixture : IDisposable
    {
        public NotesDbContext Context;
        public IMapper Mapper;

        public QueryTestFixture()
        {
            Context = NotesContextFactory.Create();
            var configurationBuilder = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AssemblyMappingProfile(typeof(IMapWith<>).Assembly)));
            Mapper = configurationBuilder.CreateMapper();
        }

        public void Dispose()
        {
            NotesContextFactory.Destroy(Context);
        }

        [CollectionDefinition("QueryCollection")]
        public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
    }
}
