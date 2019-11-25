using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Marr.Data;
using Marr.Data.Mapping;
using NzbDrone.Common.Reflection;
using NzbDrone.Core.Datastore;
using NzbDrone.Core.Datastore.Converters;
using NzbDrone.Core.MediaFiles;
using NzbDrone.Core.Movies;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            int testCount = 2;
            string sql = "SELECT * FROM Movies e LEFT JOIN MovieFiles ef ON e.MovieFileId = ef.Id";

            var cs = new SQLiteConnectionStringBuilder();
            cs.DataSource = @"/home/tom/git_working/Radarr/radarr.db.40kbackup2";

            TableMapping.MapDapper();

            using (var connection = new SQLiteConnection(cs.ToString()))
            {
                for (int i = 0; i < testCount; i++)
                {
                    var watch1 = System.Diagnostics.Stopwatch.StartNew();
                    var orderDetails1 = connection.Query<Movie, MovieFile, Movie>(sql, (e, ef) => {
                            e.MovieFile = ef;
                            return e;
                        }
                        ).ToList();
                    watch1.Stop();
                    Console.WriteLine($"Dapper Join of {orderDetails1.Count} in {watch1.Elapsed}");
                }
            }

            TableMapping.Map();

            var mapper = new Marr.Data.DataMapper(new SQLiteFactory(), cs.ToString());
            {
                mapper.SqlMode = SqlModes.Text;

                for (int i = 0; i < testCount; i++)
                {
                    var watch1 = System.Diagnostics.Stopwatch.StartNew();
                    var movies1 = mapper.Query<Movie>()
                        .Join<Movie, MovieFile>(Marr.Data.QGen.JoinType.Left, v => v.MovieFile, (l,r) => l.MovieFileId == r.Id)
                        .ToList();
                    watch1.Stop();
                    Console.WriteLine($"Marr Join of {movies1.Count} in {watch1.Elapsed}");
                }
            }

            Console.ReadLine();
        }
    }
}
